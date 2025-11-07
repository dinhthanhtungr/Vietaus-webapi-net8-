using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Employees;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs.ResultDtos;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static QuestPDF.Helpers.Colors;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerFeatures
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _CurrentUser;

        public CustomerService (IUnitOfWork unitOfWork, IMapper mapper, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _CurrentUser = currentUser;
        }

        private static string NormalizeTaxCode(string? tax)
        {
            if (string.IsNullOrWhiteSpace(tax)) return string.Empty;
            // chuẩn hoá để so sánh/uniqueness: bỏ khoảng trắng, gạch, viết hoa
            return new string(tax.Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();
        }

        /// <summary>
        /// Thêm một khách hàng mới
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<OperationResult<AddCustomerResultDto>> AddNewCustomer(PostCustomer customer)
        {

            customer.CompanyId = _CurrentUser.CompanyId;
            // 0) ExternalId (chưa cần transaction)
            if (string.IsNullOrWhiteSpace(customer.ExternalId))
            {
                customer.ExternalId = await ExternalIdGenerator.GenerateCode(
                    "KH",
                    prefix => _unitOfWork.CustomerRepository.GetLatestExternalIdStartsWithAsync(prefix)
                );
            }

            // 1) Validate input tối thiểu
            if (customer.CustomerAssignment is null)
                return OperationResult<AddCustomerResultDto>.Fail("CustomerAssignment không được null.");

            // 2) Check MST (dùng Replace/Upper để EF translate được)
            var taxNorm = NormalizeTaxCode(customer.TaxNumber); // bỏ ký tự không chữ số/chữ, Upper
            if (!string.IsNullOrEmpty(taxNorm))
            {
                var existedInfo = await _unitOfWork.CustomerRepository.Query()
                    .Where(c =>
                        ((c.TaxNumber ?? "")
                            .Replace("-", "")
                            .Replace(".", "")
                            .Replace(" ", "")
                            .ToUpper()) == taxNorm
                        && c.CompanyId == customer.CompanyId)
                    .Select(c => new
                    {
                        c.ExternalId,
                        c.CustomerId,
                        c.CustomerName,
                        c.TaxNumber,
                        Assignment = c.CustomerAssignments
                            .Where(a => a.IsActive)
                            .OrderByDescending(a => a.CreatedDate)
                            .Select(a => new
                            {
                                a.EmployeeId,
                                EmployeeName = a.Employee.FullName,
                                a.GroupId,
                                GroupName = a.Group.Name
                            })
                            .FirstOrDefault()
                    })
                    .FirstOrDefaultAsync();

                if (existedInfo != null)
                {
                    var dto = new AddCustomerResultDto(
                        existedInfo.CustomerId,
                        existedInfo.ExternalId,
                        existedInfo.CustomerName,
                        existedInfo.TaxNumber ?? string.Empty,
                        existedInfo.Assignment?.EmployeeId ?? Guid.Empty,
                        existedInfo.Assignment?.EmployeeName ?? string.Empty,
                        existedInfo.Assignment?.GroupId ?? Guid.Empty,
                        existedInfo.Assignment?.GroupName ?? string.Empty
                    );

                    return OperationResult<AddCustomerResultDto>.Fail(dto,
                           $"Mã số thuế {customer.TaxNumber} đã tồn tại cho khách hàng \"{dto.Name}\" " +
                           $"và hiện đang do {(dto.EmployeeName ?? "chưa gán")} quản lý" +
                           $"{(dto.GroupName is null ? "" : $" ({dto.GroupName})")}."
                       );
                }
            }

            // 3) Tìm Group theo SALE (khuyến nghị dùng EmployeeId của assignment)
            var saleId = customer.CustomerAssignment.EmployeeId;
            var groupId = await _unitOfWork.MemberInGroupRepository.Query()
                .Where(m => m.Profile == saleId && m.IsActive == true)
                .Select(m => (Guid?)m.GroupId)
                .FirstOrDefaultAsync();

            if (groupId is null)
                return OperationResult<AddCustomerResultDto>.Fail("Nhân viên sale chưa thuộc nhóm nào (Group).");

            // Gán thông tin assignment
            customer.CustomerAssignment.GroupId = groupId.Value;
            customer.CustomerAssignment.IsActive = true;
            customer.CustomerAssignment.CreatedDate = DateTime.Now;
            customer.CustomerAssignment.UpdatedDate = DateTime.Now;

            customer.CustomerAssignment.CompanyId = _CurrentUser.CompanyId;
            customer.CustomerAssignment.CreatedBy = _CurrentUser.EmployeeId;
            customer.CustomerAssignment.UpdatedBy = _CurrentUser.EmployeeId;

            customer.CustomerAssignment.EmployeeId = _CurrentUser.EmployeeId;

            // 4) Transaction chỉ bắt đầu khi chuẩn bị ghi
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var customerEntity = _mapper.Map<Customer>(customer);

                customerEntity.CreatedBy = _CurrentUser.EmployeeId;
                customerEntity.CreatedDate = DateTime.Now;
                customerEntity.CompanyId = _CurrentUser.CompanyId;
                customerEntity.UpdatedBy = _CurrentUser.EmployeeId;
                customerEntity.UpdatedDate = DateTime.Now;


                await _unitOfWork.CustomerRepository.AddNewCustomer(customerEntity);

                var affected = await _unitOfWork.SaveChangesAsync();

                if (affected <= 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return OperationResult<AddCustomerResultDto>.Fail("Thất bại.");
                }

                await _unitOfWork.CommitTransactionAsync();

                return OperationResult<AddCustomerResultDto>.Ok("Tạo khách hàng thành công");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult<AddCustomerResultDto>.Fail($"Lỗi khi tạo khách hàng: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa mềm dữ liệu khách hàng này
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteCustomerByIdAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _unitOfWork.CustomerRepository.DeleteCustomerByIdAsync(id);
                var affected = await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return affected > 0
                    ? OperationResult.Ok("Xóa khách hàng thành công")
                    : OperationResult.Fail("Không tìm thấy khách hàng hoặc xóa không thành công.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi xóa khách hàng: {ex.Message}");

            }
        }

        /// <summary>
        /// Xem danh sách tất cả khách hàng có phân trang
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PagedResult<GetReviewCustomer>> GetAllAsync(CustomerQuery? query)
        {
            var pagedResult = await _unitOfWork.CustomerRepository.GetAllAsync(query);
            try
            {
                var pagedResultMapped = _mapper.Map<PagedResult<GetReviewCustomer>>(pagedResult);
                return pagedResultMapped;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhân viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách khách hàng được phân công cho nhân viên cụ thể và yêu cầu đặt biệt của khách hàng ở đơn hàng gần nhất
        /// </summary>
        /// <param name="isAdmin"></param>
        /// <param name="employeeId"></param>
        /// <param name="customerId"></param>
        /// <param name="keyword"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception> 
        public async Task<PagedResult<GetReviewCustomer>> GetCustomerByEmployeeAssignment(CustomerQuery? query, CancellationToken ct = default)
        {
            try
            {
                // Đặt giá trị mặc định cho PageNumber và PageSize
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                // Lấy groupId của leader (nếu có)
                var groupId = await _unitOfWork.MemberInGroupRepository.Query()
                    .Where(g => g.Profile == _CurrentUser.EmployeeId && g.IsAdmin == true && g.IsActive == true)
                    .Select(g => g.GroupId)
                    .FirstOrDefaultAsync(ct);

                // Bắt đầu xây dựng query cho CustomerAssignment
                var customerQuery = _unitOfWork.CustomerAssignmentRepository.Query()
                    .Where(ca => ca.IsActive)
                    .Select(ca => new
                    {
                        ca.CustomerId,
                        ca.Customer.CustomerName,
                        ca.Customer.RegistrationNumber,
                        ca.Customer.ExternalId,
                        ca.Customer.CustomerGroup,
                        ca.Customer.CreatedDate,
                        ca.EmployeeId,
                        ca.Employee.FullName,
                        ca.Customer.MerchandiseOrders,
                        ca.Customer.Contacts,
                        ca.Customer.Addresses,
                        ca.GroupId
                    });

                // Lọc theo groupId hoặc EmployeeId
                if (groupId != Guid.Empty)
                {
                    customerQuery = customerQuery.Where(c => c.GroupId == groupId);
                }
                else
                {
                    customerQuery = customerQuery.Where(c => c.EmployeeId == _CurrentUser.EmployeeId);
                }

                // Các lọc bổ sung từ query
                if (query.CompanyId.HasValue)
                {
                    customerQuery = customerQuery.Where(c => c.CustomerGroup != null && c.CustomerGroup.Contains(query.CompanyId.Value.ToString()));
                    // OR, if CustomerGroup is not related to CompanyId, you may need to join with the Customer entity to filter by CompanyId.
                    // Example:
                    // customerQuery = customerQuery.Where(c => c.Customer.CompanyId == query.CompanyId.Value);
                }

                if (query.EmployeeId.HasValue)
                {
                    customerQuery = customerQuery.Where(c => c.EmployeeId == query.EmployeeId.Value);
                }

                if (!string.IsNullOrWhiteSpace(query.keyword))
                {
                    customerQuery = customerQuery.Where(c =>
                        c.CustomerName.Contains(query.keyword) ||
                        c.ExternalId.Contains(query.keyword)
                    );
                }

                if (query.From.HasValue)
                {
                    customerQuery = customerQuery.Where(c => c.CreatedDate >= query.From.Value);
                }

                if (query.To.HasValue)
                {
                    customerQuery = customerQuery.Where(c => c.CreatedDate <= query.To.Value);
                }

                // Tính tổng số bản ghi
                var totalItems = await customerQuery.CountAsync(ct);

                // Truy vấn dữ liệu và phân trang
                var customers = await customerQuery
                    .OrderByDescending(c => c.CreatedDate)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(ts => new GetReviewCustomer
                    {
                        CustomerId = ts.CustomerId,
                        ExternalId = ts.ExternalId,
                        Name = ts.CustomerName,
                        RegNo = ts.RegistrationNumber,
                        Phone = ts.MerchandiseOrders
                            .OrderByDescending(o => o.CreateDate)
                            .Select(o => o.PhoneSnapshot)
                            .FirstOrDefault() ?? ts.Contacts
                            .Where(a => a.IsPrimary == true)
                            .Select(a => a.Phone)
                            .FirstOrDefault(),
                        Group = ts.CustomerGroup,
                        Address = ts.MerchandiseOrders
                            .OrderByDescending(o => o.CreateDate)
                            .Select(o => o.DeliveryAddress)
                            .FirstOrDefault() ?? ts.Addresses
                            .Where(a => a.IsPrimary == true)
                            .Select(a => a.AddressLine)
                            .FirstOrDefault(),
                        DeliveryName = ts.MerchandiseOrders
                            .OrderByDescending(o => o.CreateDate)
                            .Select(o => o.Receiver)
                            .FirstOrDefault() ?? ts.Contacts
                            .OrderByDescending(co => co.IsPrimary)
                            .Select(contact => $"{contact.FirstName} {contact.LastName}".Trim())
                            .FirstOrDefault(),
                        EmployeeId = ts.EmployeeId,
                        EmployeeName = ts.FullName,
                        CustomerSpectialRequirement = ts.MerchandiseOrders
                            .OrderByDescending(o => o.CreateDate)
                            .Select(o => o.Note)
                            .FirstOrDefault() ?? string.Empty,
                        paymentType = ts.MerchandiseOrders
                            .Select(o => o.PaymentType)
                            .FirstOrDefault() ?? string.Empty,
                        delivieryType = ts.MerchandiseOrders
                            .Select(o => o.ShippingMethod)
                            .FirstOrDefault() ?? string.Empty
                    })
                    .ToListAsync(ct);

                // Trả về kết quả phân trang
                return new PagedResult<GetReviewCustomer>(customers, totalItems, query.PageNumber, query.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách khách hàng: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết khách hàng theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<GetCustomer?> GetCustomerByIdAsync(Guid id)
        {
            return _unitOfWork.CustomerRepository.GetCustomerByIdAsync(id);
        }

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateCustomerAsync(PatchCustomer customer)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _unitOfWork.CustomerRepository.UpdateCustomerAsync(customer);
                var affected = await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
                return affected > 0
                    ? OperationResult.Ok("Cập nhật khách hàng thành công")
                    : OperationResult.Fail("Cập nhật khách hàng không thành công.");
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi cập nhật khách hàng: {ex.Message}");
            }

        }

        
    }
}

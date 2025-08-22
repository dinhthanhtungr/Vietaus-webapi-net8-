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
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Shared.Helper;
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

        public CustomerService (IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OperationResult> AddNewCustomer(PostCustomer customer)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(customer.ExternalId))
                {
                    customer.ExternalId = await ExternalIdGenerator.GenerateCode(
                        "KH",
                        prefix => _unitOfWork.CustomerRepository.GetLatestExternalIdStartsWithAsync(prefix)
                    );
                }

                if (customer.CustomerAssignment == null)
                {
                    throw new ArgumentNullException(nameof(customer.CustomerAssignment), "CustomerAssignment cannot be null.");
                }

                var groupId = await _unitOfWork.MemberInGroupRepository.Query()
                    .Where(m => m.Profile == customer.CustomerAssignment.CreatedBy
                             && m.IsActive == true)   // so sánh an toàn cho nullable
                    .Select(m => (Guid?)m.GroupId)
                    .FirstOrDefaultAsync();


                customer.CustomerAssignment.GroupId = groupId;

                var customerEntity = _mapper.Map<Customer>(customer);

                await _unitOfWork.CustomerRepository.AddNewCustomer(customerEntity);

                var affected = await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return affected > 0
                    ? OperationResult.Ok("Tạo khách hàng thành công")
                    : OperationResult.Fail("Thất bại.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi tạo khách hàng: {ex.Message}");
            }
        }

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

        public async Task<PagedResult<GetReviewCustomer>> GetCustomerByEmployeeAssignment(
            bool isAdmin,
            Guid employeeId,
            string? keyword = null,
            int pageNumber = 1,
            int pageSize = 15,
            CancellationToken ct = default)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 15;

            // Base query: khách hàng có ít nhất 1 assignment thuộc employeeId
            IQueryable<Customer> q = _unitOfWork.CustomerRepository.Query(); // nhớ AsNoTracking() trong repo cho nhẹ

            // Nếu là admin, lấy tất cả khách hàng thuộc group của admin


            //Tạm thời tắt chức năng này sau này có gì có thể sửa lại nha
            //isAdmin = false;

            if (isAdmin)
            {
                // Lấy groupId từ employeeId nếu là nhân viên
                var groupId = await _unitOfWork.MemberInGroupRepository.Query()
                    .Where(a => a.Profile == employeeId && a.IsActive == true)
                    .Select(m => (Guid?)m.GroupId)
                    .FirstOrDefaultAsync();

                if (groupId == null && isAdmin)
                    throw new Exception("Không tìm thấy GroupId của admin.");



                q = q.Where(c => c.IsActive == true &&
                    c.CustomerAssignments.Any(a => a.IsActive && a.GroupId == groupId));
            }
            else
            {
                q = q.Where(c => c.IsActive == true &&
                    c.CustomerAssignments.Any(a => a.IsActive && a.EmployeeId == employeeId));
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword.Trim(); // SQL Server thường CI (case-insensitive) nên không cần ToLower()

                q = q.Where(c =>
                    // Các field của Customer
                    (c.CustomerName ?? "").Contains(kw) ||
                    (c.RegistrationNumber ?? "").Contains(kw) ||
                    (c.Phone ?? "").Contains(kw) ||
                    (c.ExternalId ?? "").Contains(kw) ||

                    // Tìm theo nhân viên được assign (quản lý)
                    c.CustomerAssignments.Any(a => a.IsActive &&
                        (
                            (a.Employee.FullName ?? "").Contains(kw) ||
                            (a.Employee.ExternalId ?? "").Contains(kw)         // Nếu mã NV lưu ở ExternalId
                        )
                    )
                );
            }
            // Tổng trước khi paging
            int total = await q.CountAsync(ct);

            // Page + select ra DTO
            List<GetReviewCustomer> items = await q
                .OrderBy(c => c.CustomerName) // sort theo tên KH
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new GetReviewCustomer
                {
                    CustomerId = c.CustomerId,
                    ExternalId = c.ExternalId,
                    Name = c.CustomerName,
                    RegNo = c.RegistrationNumber,
                    Phone = c.Phone,
                    Group = c.CustomerGroup,

                    EmployeeId = c.CustomerAssignments
                        .Where(a => a.IsActive)
                        .Select(a => (Guid?)a.EmployeeId)
                        .FirstOrDefault(),
                    EmployeeName = c.CustomerAssignments
                        .Where(a => a.IsActive)
                        .Select(a => a.Employee.FullName)
                        .FirstOrDefault()
                })
                .ToListAsync(ct);

            return new PagedResult<GetReviewCustomer>(items, total, pageNumber, pageSize);
        }


        public Task<GetCustomer?> GetCustomerByIdAsync(Guid id)
        {
            return _unitOfWork.CustomerRepository.GetCustomerByIdAsync(id);
        }

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

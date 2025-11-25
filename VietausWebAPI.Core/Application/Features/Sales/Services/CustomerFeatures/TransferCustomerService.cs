using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerFeatures
{
    public class TransferCustomerService : ITransferCustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _CurrentUser;

        public TransferCustomerService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _CurrentUser = currentUser;
        }

        /// <summary>
        /// Tạo mới một lần chuyển khách hàng từ nhân viên này sang nhân viên khác
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<TransferCustomerDTO> CreateTransferAsync(TransferCustomersRequest req, CancellationToken ct)
        {
            req.CompanyId = _CurrentUser.CompanyId;
            req.CreatedBy = _CurrentUser.EmployeeId;

            if (req == null) throw new ArgumentNullException(nameof(req));
            if (req.CustomerIds == null || req.CustomerIds.Count == 0)
                throw new ArgumentException("CustomerIds is empty.", nameof(req.CustomerIds));
            if (req.FromEmployeeId == Guid.Empty || req.ToEmployeeId == Guid.Empty
                || req.FromGroupId == Guid.Empty || req.ToGroupId == Guid.Empty
                || req.CompanyId == Guid.Empty || req.CreatedBy == Guid.Empty)
                throw new ArgumentException("Some required Id is empty (Guid.Empty).");
            if (req.FromEmployeeId == req.ToEmployeeId && req.FromGroupId == req.ToGroupId)
                throw new ArgumentException("Source and destination are the same.");

            var customerIds = req.CustomerIds.Distinct().ToList();
            var nowUtc = DateTime.Now;

            await using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 0) Validate: tất cả KH đều đang thuộc from-employee + from-group (active)
                var invalidIds = await _unitOfWork.CustomerAssignmentRepository.Query()
                    .Where(a => customerIds.Contains(a.CustomerId) && a.IsActive == true)
                    .Where(a => !(a.EmployeeId == req.FromEmployeeId && a.GroupId == req.FromGroupId))
                    .Select(a => a.CustomerId)
                    .Distinct()
                    .ToListAsync(ct);

                if (invalidIds.Count > 0)
                    throw new InvalidOperationException($"Một số khách hàng không thuộc nguồn hiện tại (from): {string.Join(", ", invalidIds)}");

                // 1) Ghi header + items
                var log = new CustomerTransferLog
                {
                    Id = Guid.CreateVersion7(),
                    FromEmployeeId = req.FromEmployeeId,
                    ToEmployeeId = req.ToEmployeeId,
                    FromGroupId = req.FromGroupId,
                    ToGroupId = req.ToGroupId,
                    Note = req.Note,
                    CompanyId = req.CompanyId,
                    CreatedBy = req.CreatedBy,
                    CreatedDate = nowUtc,
                    DetailCustomerTransfers = customerIds.Select(id => new DetailCustomerTransfer { CustomerId = id }).ToList()
                };
                await _unitOfWork.CustomerTransferLogRepository.AddAsync(log, ct);

                // 2) Chuyển quyền: đóng bản ghi active cũ + thêm bản ghi active mới
                //await _unitOfWork.CustomerAssignmentRepository.BulkUpdateEmployeeGroupAsync(
                //     customerIds, req.ToEmployeeId, req.ToGroupId, req.CreatedBy, nowUtc, ct);


                await _unitOfWork.CustomerAssignmentRepository.BulkTransferWithHistoryAsync(
                        customerIds,
                        req.FromEmployeeId, req.FromGroupId,
                        req.ToEmployeeId, req.ToGroupId,
                        req.CreatedBy, req.CompanyId, nowUtc, ct);
                await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync(ct);

                // 3) Projection → DTO
                var dto = await _unitOfWork.CustomerTransferLogRepository.Query()
                    .Where(x => x.Id == log.Id)
                    .Select(x => new TransferCustomerDTO
                    {
                        Id = x.Id,
                        CreatedDate = x.CreatedDate,
                        FromEmployee = new EmpLiteDto
                        {
                            Id = x.FromEmployeeId,
                            FullName = x.FromEmployee.FullName,
                            Code = x.FromEmployee.ExternalId
                        },
                        ToEmployee = new EmpLiteDto
                        {
                            Id = x.ToEmployeeId,
                            FullName = x.ToEmployee.FullName,
                            Code = x.ToEmployee.ExternalId
                        },
                        Customers = x.DetailCustomerTransfers
                            .Select(d => new CustomerLiteDto
                            {
                                Id = d.CustomerId,
                                ExternalId = d.Customer.ExternalId,
                                Name = d.Customer.CustomerName
                            })
                            .ToList()
                    })
                    .AsNoTracking()
                    .FirstAsync(ct);

                return dto;
            }
            catch (DbUpdateException ex)
            {
                await tx.RollbackAsync(ct);
                // log ex if needed
                throw; // giữ stack trace gốc
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }

        /// <summary>
        /// Lấy thông tin một lần chuyển khách hàng theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<TransferCustomerDTO?> GetTransferByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _unitOfWork.CustomerTransferLogRepository.Query()
                .Where(x => x.Id == id)
                .Select(x => new TransferCustomerDTO
                {
                    Id = x.Id,
                    CreatedDate = x.CreatedDate,
                    FromEmployee = new EmpLiteDto
                    {
                        Id = x.FromEmployeeId,
                        FullName = x.FromEmployee.FullName,
                        Code = x.FromEmployee.ExternalId
                    },
                    ToEmployee = new EmpLiteDto
                    {
                        Id = x.ToEmployeeId,
                        FullName = x.ToEmployee.FullName,
                        Code = x.ToEmployee.ExternalId
                    },
                    FromGroup = x.FromGroup != null ? new GroupLiteDto
                    {
                        Id = x.FromGroup.GroupId,
                        Name = x.FromGroup.Name,
                        Code = x.FromGroup.ExternalId // nếu có mã nhóm
                    } : null,
                    ToGroup = x.ToGroup != null ? new GroupLiteDto
                    {
                        Id = x.ToGroup.GroupId,
                        Name = x.ToGroup.Name,
                        Code = x.ToGroup.ExternalId // nếu có mã nhóm
                    } : null,
                    Note = x.Note,
                    Customers = x.DetailCustomerTransfers
                        .Select(d => new CustomerLiteDto
                        {
                            Id = d.CustomerId,
                            ExternalId = d.Customer.ExternalId,
                            Name = d.Customer.CustomerName
                        })
                        .ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// Lấy danh sách các lần chuyển khách hàng theo bộ lọc trong query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PagedResult<TransferCustomerDTO>> GetTransfersAsync(CustomerTransferQuery query, CancellationToken ct = default)
        {
            try
            {
                // Nếu là leader, lấy tất cả khách hàng thuộc group của leader

                var groupId = await _unitOfWork.MemberInGroupRepository.Query()
                    .Where(g => g.Profile == _CurrentUser.EmployeeId && g.IsAdmin == true && g.IsActive == true)
                    .Select(g => g.GroupId)
                    .FirstOrDefaultAsync(ct);

                IQueryable<CustomerTransferLog> CustomerTransferLog = _unitOfWork.TransferCustomerRepository.Query();
                
                if (groupId != Guid.Empty)
                {
                    CustomerTransferLog = CustomerTransferLog
                        .Where(t => t.FromGroupId == groupId || t.ToGroupId == groupId);
                }

                // Áp dụng lọc từ query
                if (query.From.HasValue)
                {
                    CustomerTransferLog = CustomerTransferLog
                        .Where(t => t.CreatedDate >= query.From.Value);
                }

                if (query.To.HasValue)
                {
                    CustomerTransferLog = CustomerTransferLog
                        .Where(t => t.CreatedDate <= query.To.Value);
                }

                var totalItems = await CustomerTransferLog.CountAsync(ct);
                var items = await CustomerTransferLog
                    .OrderByDescending(t => t.CreatedDate)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(x => new TransferCustomerDTO
                    {
                        Id = x.Id,
                        CreatedDate = x.CreatedDate,
                        FromEmployee = new EmpLiteDto
                        {
                            Id = x.FromEmployeeId,
                            FullName = x.FromEmployee.FullName,
                            Code = x.FromEmployee.ExternalId
                        },
                        ToEmployee = new EmpLiteDto
                        {
                            Id = x.ToEmployeeId,
                            FullName = x.ToEmployee.FullName,
                            Code = x.ToEmployee.ExternalId
                        },
                        FromGroup = x.FromGroup != null ? new GroupLiteDto
                        {
                            Id = x.FromGroup.GroupId,
                            Name = x.FromGroup.Name,
                            Code = x.FromGroup.ExternalId // nếu có mã nhóm
                        } : null,
                        ToGroup = x.ToGroup != null ? new GroupLiteDto
                        {
                            Id = x.ToGroup.GroupId,
                            Name = x.ToGroup.Name,
                            Code = x.ToGroup.ExternalId // nếu có mã nhóm
                        } : null,
                        Note = x.Note,
                        Customers = x.DetailCustomerTransfers
                            .Select(d => new CustomerLiteDto
                            {
                                Id = d.CustomerId,
                                ExternalId = d.Customer.ExternalId,
                                Name = d.Customer.CustomerName
                            })
                            .ToList()
                    })
                    .AsNoTracking()
                    .ToListAsync(ct);

                return new PagedResult<TransferCustomerDTO>(items, totalItems, query.PageNumber, query.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách khách hàng: {ex.Message}", ex);
            }
        }
    }
}

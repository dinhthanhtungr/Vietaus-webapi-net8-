using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerFeatures
{
    public class TransferCustomerService : ITransferCustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransferCustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TransferCustomerDTO> CreateTransferAsync(TransferCustomersRequest req, CancellationToken ct)
        {
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
                    Id = Guid.NewGuid(),
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

        public async Task<PagedResult<TransferCustomerDTO>> GetTransfersAsync(CustomerTransferQuery query, CancellationToken ct = default)
        {
            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 15;

            // 1) Base query
            IQueryable<CustomerTransferLog> q = _unitOfWork.TransferCustomerRepository.Query();

            // 2) Optional filter
            if (query.CompanyId.HasValue)
                q = q.Where(x => x.CompanyId == query.CompanyId.Value);

            if (query.From.HasValue)
                q = q.Where(x => x.CreatedDate >= query.From.Value);

            if (query.To.HasValue)
                q = q.Where(x => x.CreatedDate <= query.To.Value);

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                // Tìm theo tên/mã NV hoặc tên/mã khách trong batch
                q = q.Where(x =>
                    x.FromEmployee.FullName.Contains(query.Keyword) ||
                    x.ToEmployee.FullName.Contains(query.Keyword) ||
                    x.FromEmployee.ExternalId.Contains(query.Keyword) ||
                    x.ToEmployee.ExternalId.Contains(query.Keyword) ||
                    x.DetailCustomerTransfers.Any(d =>
                        d.Customer.CustomerName.Contains(query.Keyword) ||
                        d.Customer.ExternalId.Contains(query.Keyword)
                    )
                );
            }

            // 3) Sort
            q = q.OrderByDescending(x => x.CreatedDate);

            // 4) Total trước khi paging
            int total = await q.CountAsync(ct);

            // Remove the .AsSplitQuery() call, as it is not valid on IQueryable<TransferCustomerDTO>
            // The correct usage is on the Entity Framework Core query before projection to DTOs.
            // So, simply delete the following line from your code:

            // .AsSplitQuery()   // tránh join nổ khi có collection

            // The corrected code block should look like this:
            List<TransferCustomerDTO> items = await q
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new TransferCustomerDTO
                {
                    Id = x.Id,
                    CreatedDate = x.CreatedDate,
                    FromEmployee = new EmpLiteDto
                    {
                        Id = x.FromEmployee.EmployeeId,
                        FullName = x.FromEmployee.FullName,
                        Code = x.FromEmployee.ExternalId // đổi sang cột mã NV bạn dùng
                    },
                    ToEmployee = new EmpLiteDto
                    {
                        Id = x.ToEmployee.EmployeeId,
                        FullName = x.ToEmployee.FullName,
                        Code = x.ToEmployee.ExternalId
                    },

                    FromGroup = new GroupLiteDto
                    {
                        Id = x.FromGroup.GroupId,
                        Name = x.FromGroup.Name,
                        Code = x.FromGroup.ExternalId // nếu có mã nhóm
                    },

                    ToGroup = new GroupLiteDto
                    {
                        Id = x.ToGroup.GroupId,
                        Name = x.ToGroup.Name,
                        Code = x.ToGroup.ExternalId // nếu có mã nhóm
                    },
                    Note = x.Note,

                    Customers = x.DetailCustomerTransfers
                        .Select(d => new CustomerLiteDto
                        {
                            Id = d.CustomerId,
                            ExternalId = d.Customer.ExternalId,   // mã KH
                            Name = d.Customer.CustomerName
                        })
                        .ToList()
                })
                .ToListAsync(ct);

            // 6) Kết quả chuẩn theo model của bạn
            return new PagedResult<TransferCustomerDTO>(items, total, query.PageNumber, query.PageSize);
        }
    }
}

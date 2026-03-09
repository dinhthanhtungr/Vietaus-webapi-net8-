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
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

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



        public async Task<OperationResult> CreateAssignLeadRequestAsync(AssignLeadRequest req, CancellationToken ct)
        {
            var now = DateTime.Now;
            var userId = _CurrentUser.EmployeeId;
            var companyId = _CurrentUser.CompanyId;

            using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // ------------------- 1) Kiểm tra Leader -------------------
                var leaderGroupId = await _unitOfWork.MemberInGroupRepository.Query()
                    .Where(m => m.Profile == userId
                             && m.IsAdmin == true
                             && m.IsActive == true)
                    .Select(m => (Guid?)m.GroupId)
                    .FirstOrDefaultAsync(ct);

                if (!leaderGroupId.HasValue)
                    return OperationResult<AssignLeadRequestNoice>.Fail("Bạn không phải Leader.");

                // customer exist + is lead
                var customer = await _unitOfWork.CustomerRepository.Query()
                    .Where(c => c.CustomerId == req.CustomerId && c.CompanyId == companyId)
                    .FirstOrDefaultAsync(ct);

                if (customer is null)
                    return OperationResult<AssignLeadRequestNoice>.Fail("Không tìm thấy khách hàng.");

                // ------------------- 2) Tạo yêu cầu -------------------
                //var saleInGroup = await _unitOfWork.MemberInGroupRepository.Query()
                //    .AnyAsync(m => m.GroupId == leaderGroupId.Value
                //                && m.Profile == req.SalesEmployeeId
                //                && m.IsActive == true, ct);

                //if (!saleInGroup)
                //    return OperationResult<AssignLeadRequestNoice>.Fail("Nhân viên nhận không thuộc nhóm của bạn.");

                // Tạo Work-Claim mới (nhiều sale cùng follow OK)
                var newClaim = new CustomerClaim
                {
                    Id = Guid.CreateVersion7(),
                    CustomerId = req.CustomerId,
                    EmployeeId = req.SalesEmployeeId,
                    GroupId = leaderGroupId.Value,
                    Type = ClaimType.Work,
                    ExpiresAt = now.AddDays(req.ExpiredInDays),
                    IsActive = true,
                    CompanyId = companyId
                };

                await _unitOfWork.CustomerClaimRepository.AddAsync(newClaim, ct);

                // Ghi log chuyển lead
                var logId = Guid.CreateVersion7();
                var log = new CustomerTransferLog
                {
                    Id = logId,
                    FromEmployeeId = userId,
                    ToEmployeeId = req.SalesEmployeeId,
                    FromGroupId = leaderGroupId.Value,
                    ToGroupId = leaderGroupId.Value,
                    TransferType = TransferType.Lead,
                    Note = req.note,
                    CreatedDate = now,
                    CreatedBy = userId,
                    CompanyId = companyId,
                    DetailCustomerTransfers = new List<DetailCustomerTransfer>
                    {
                        new DetailCustomerTransfer { CustomerId = req.CustomerId }
                    }
                };
                await _unitOfWork.CustomerTransferLogRepository.AddAsync(log, ct);

                await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync(ct);

                return OperationResult.Ok("Đã giao lead thành công cho sale.");
            }

            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                return OperationResult.Fail($"Lỗi khi giao Lead: {ex.Message}");
            }
        }

        /// <summary>
        /// Tạo mới một lần chuyển khách hàng từ nhân viên này sang nhân viên khác
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<OperationResult<TransferCustomerDTO>> CreateTransferAsync(TransferCustomersRequest req, CancellationToken ct)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            req.CompanyId = _CurrentUser.CompanyId;
            req.CreatedBy = _CurrentUser.EmployeeId;

            if (req.FromEmployeeId == Guid.Empty || req.ToEmployeeId == Guid.Empty
                || req.FromGroupId == Guid.Empty || req.ToGroupId == Guid.Empty
                || req.CompanyId == Guid.Empty || req.CreatedBy == Guid.Empty)
                return OperationResult<TransferCustomerDTO>.Fail("Some required Id is empty (Guid.Empty).");

            if (req.FromEmployeeId == req.ToEmployeeId && req.FromGroupId == req.ToGroupId)
                return OperationResult<TransferCustomerDTO>.Fail("Source and destination are the same.");

            var nowUtc = DateTime.Now;

            List<Guid> customerIds;

            if (req.TransferAllCustomers)
            {
                customerIds = await _unitOfWork.CustomerAssignmentRepository.Query()
                    .Where(a => a.IsActive == true
                             && a.EmployeeId == req.FromEmployeeId
                             && a.GroupId == req.FromGroupId)
                    .Select(a => a.CustomerId)
                    .Distinct()
                    .ToListAsync(ct);
            }
            else
            {
                customerIds = req.CustomerIds?.Distinct().ToList() ?? new List<Guid>();
            }

            if (customerIds.Count == 0)
                return OperationResult<TransferCustomerDTO>.Fail("No customers found to transfer.");

            await using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Validate: tất cả KH đều đang thuộc from-employee + from-group
                var invalidIds = await _unitOfWork.CustomerAssignmentRepository.Query()
                    .Where(a => customerIds.Contains(a.CustomerId) && a.IsActive == true)
                    .Where(a => !(a.EmployeeId == req.FromEmployeeId && a.GroupId == req.FromGroupId))
                    .Select(a => a.CustomerId)
                    .Distinct()
                    .ToListAsync(ct);

                if (invalidIds.Count > 0)
                    throw new InvalidOperationException($"Một số khách hàng không thuộc nguồn hiện tại (from): {string.Join(", ", invalidIds)}");

                var log = new CustomerTransferLog
                {
                    Id = Guid.CreateVersion7(),
                    FromEmployeeId = req.FromEmployeeId,
                    ToEmployeeId = req.ToEmployeeId,
                    FromGroupId = req.FromGroupId,
                    ToGroupId = req.ToGroupId,
                    Note = req.Note,
                    TransferType = TransferType.Saled,
                    CompanyId = req.CompanyId,
                    CreatedBy = req.CreatedBy,
                    CreatedDate = nowUtc,
                    DetailCustomerTransfers = customerIds
                        .Select(id => new DetailCustomerTransfer { CustomerId = id })
                        .ToList()
                };

                await _unitOfWork.CustomerTransferLogRepository.AddAsync(log, ct);

                await _unitOfWork.CustomerAssignmentRepository.BulkTransferWithHistoryAsync(
                    customerIds,
                    req.FromEmployeeId, req.FromGroupId,
                    req.ToEmployeeId, req.ToGroupId,
                    req.CreatedBy, req.CompanyId, nowUtc, ct);

                await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync(ct);

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

                return OperationResult<TransferCustomerDTO>.Ok(dto);
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
        public async Task<PagedResult<TransferCustomerDTO>> GetTransfersAsync( CustomerTransferQuery query, CancellationToken ct = default)
        {
            try
            {
                var groupId = await _unitOfWork.MemberInGroupRepository.Query()
                    .Where(g => g.Profile == _CurrentUser.EmployeeId && g.IsAdmin == true && g.IsActive == true)
                    .Select(g => g.GroupId)
                    .FirstOrDefaultAsync(ct);

                var q = _unitOfWork.TransferCustomerRepository.Query();

                // scope theo leader group (nếu có)
                if (groupId != Guid.Empty)
                {
                    q = q.Where(t => t.FromGroupId == groupId || t.ToGroupId == groupId);
                }

                // lọc thời gian
                if (query.From.HasValue) q = q.Where(t => t.CreatedDate >= query.From.Value);
                if (query.To.HasValue) q = q.Where(t => t.CreatedDate <= query.To.Value);

                // lọc theo TransferType (nếu bạn có cột này)
                if (query.TransferType.HasValue)
                {
                    q = q.Where(t => t.TransferType == query.TransferType.Value);
                }

                // lọc theo keyword (citext nên không cần ILike)
                string? kw = string.IsNullOrWhiteSpace(query.Keyword) ? null : query.Keyword!.Trim();

                if (kw is not null)
                {
                    q = q.Where(t =>
                           t.FromEmployee.FullName.Contains(kw)
                        || t.FromEmployee.ExternalId.Contains(kw)
                        || t.ToEmployee.FullName.Contains(kw)
                        || t.ToEmployee.ExternalId.Contains(kw)
                        || t.DetailCustomerTransfers.Any(d =>
                               d.Customer.ExternalId.Contains(kw) || d.Customer.CustomerName.Contains(kw))
                    );
                }

                var totalItems = await q.CountAsync(ct);

                var items = await q
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
                            Code = x.FromGroup.ExternalId
                        } : null,
                        ToGroup = x.ToGroup != null ? new GroupLiteDto
                        {
                            Id = x.ToGroup.GroupId,
                            Name = x.ToGroup.Name,
                            Code = x.ToGroup.ExternalId
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
                    .OrderByDescending(x => x.CreatedDate)
                    .ToListAsync(ct);

                return new PagedResult<TransferCustomerDTO>(items, totalItems, query.PageNumber, query.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách chuyển khách: {ex.Message}", ex);
            }
        }

    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Sales
{
    public class CustomerAssignmentRepository : ICustomerAssignmentRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerAssignmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public IQueryable<CustomerAssignment> Query(bool track = false)
        {
            var db = _context.CustomerAssignments.AsQueryable();
            return track ? db : db.AsNoTracking();
        }

        public async Task BulkTransferWithHistoryAsync(
                IEnumerable<Guid> customerIds,
                Guid fromEmployeeId,
                Guid fromGroupId,
                Guid toEmployeeId,
                Guid toGroupId,
                Guid actorId,
                Guid CompanyId,
                DateTime nowUtc,

                CancellationToken ct = default)
        {
            var ids = customerIds?.Distinct().ToList();
            if (ids == null || ids.Count == 0) return;

            // 1) Close các assignment đang active đúng nguồn (from)
            var closedByExecuteUpdate = false;
            try
            {
                await _context.CustomerAssignments
                    .Where(a => ids.Contains(a.CustomerId)
                             && a.IsActive == true
                             && a.EmployeeId == fromEmployeeId
                             && a.GroupId == fromGroupId)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(a => a.IsActive, false)
                        .SetProperty(a => a.UpdatedBy, actorId)      // đảm bảo actorId là EmployeeId hợp lệ nếu có FK
                        .SetProperty(a => a.UpdatedDate, nowUtc)
                    // .SetProperty(a => a.EffectiveTo, nowUtc)     // nếu có cột thời gian kết thúc
                    , ct);
                closedByExecuteUpdate = true;
            }
            catch (MissingMethodException) { }
            catch (NotSupportedException) { }

            if (!closedByExecuteUpdate)
            {
                var closing = await _context.CustomerAssignments
                    .Where(a => ids.Contains(a.CustomerId)
                             && a.IsActive == true
                             && a.EmployeeId == fromEmployeeId
                             && a.GroupId == fromGroupId)
                    .ToListAsync(ct);

                foreach (var a in closing)
                {
                    a.IsActive = false;
                    a.UpdatedBy = actorId;      // nếu FK không hợp lệ → set null hoặc bỏ
                    a.UpdatedDate = nowUtc;
                    // a.EffectiveTo = nowUtc;     // nếu có cột
                }
            }

            // 2) Thêm assignment mới (trạng thái hiện tại)
            var newRows = ids.Select(cid => new CustomerAssignment
            {
                CustomerId = cid,
                EmployeeId = toEmployeeId,
                GroupId = toGroupId,
                IsActive = true,
                CreatedBy = actorId,
                CreatedDate = nowUtc,
                UpdatedBy = actorId,
                CompanyId = CompanyId,
                UpdatedDate = nowUtc,
                // EffectiveFrom = nowUtc,        // nếu có cột
            }).ToList();

            await _context.CustomerAssignments.AddRangeAsync(newRows, ct);
            // SaveChangesAsync do UnitOfWork gọi
        }

        public async Task PostCustomerAssignment(CustomerAssignment customerAssignment)
        {
            await _context.CustomerAssignments.AddAsync(customerAssignment);
        }
    }
}

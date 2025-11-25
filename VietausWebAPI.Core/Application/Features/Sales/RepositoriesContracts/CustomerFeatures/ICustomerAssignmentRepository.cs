using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;

namespace VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures
{
    public interface ICustomerAssignmentRepository
    {
        // Nếu cần đọc assignment
        IQueryable<CustomerAssignment> Query(bool track = false);

        // Cập nhật hàng loạt Employee/Group cho danh sách khách
        Task BulkTransferWithHistoryAsync(
            IEnumerable<Guid> customerIds,
            Guid fromEmployeeId,
            Guid fromGroupId,
            Guid toEmployeeId,
            Guid toGroupId,
            Guid actorId,
            Guid ComponyId,
            DateTime nowUtc,
            CancellationToken ct = default);

        Task PostCustomerAssignment(CustomerAssignment customerAssignment);
    }
}

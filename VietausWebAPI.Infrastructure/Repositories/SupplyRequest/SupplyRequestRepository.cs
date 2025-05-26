using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.DTOs.Approval;
using VietausWebAPI.Core.Application.DTOs.SupplyRequests;
using VietausWebAPI.Core.Application.Usecases.SupplyRequests.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Infrastructure.Utilities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.SupplyRequest
{
    public class SupplyRequestRepository : ISupplyRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public SupplyRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SupplyRequestsMaterialDatum> CreateRequestAsync(SupplyRequestsMaterialDatum request)
        {
            await _context.SupplyRequestsMaterialData.AddRangeAsync(request);
            return request;
        }

        public async Task<PagedResult<SupplyRequestsMaterialDatum>> GetSupplyRequestRepository(SupplyRequestsQuery query)
        {
            var queryable = _context.SupplyRequestsMaterialData
                .AsNoTracking()
                .Include(x => x.Employee)
                .Include(x => x.Employee.Part)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.KeyWord))
            {
                string keyword = query.KeyWord.ToLower();

                queryable = queryable.Where(x =>
                    (x.Employee != null && EF.Functions.Collate(x.Employee.FullName, "Latin1_General_CI_AI").ToLower().Contains(keyword)) ||
                    (x.EmployeeId != null && x.EmployeeId.ToLower().Contains(keyword)) ||
                    (x.RequestId != null && x.RequestId.ToLower().Contains(keyword))
                );
            }

            if (query.RequestDateFrom != null && query.RequestDateTo != null)
            {
                queryable = queryable.Where(x => x.RequestDate >= query.RequestDateFrom && x.RequestDate <= query.RequestDateTo);
            }
            else if (query.RequestDateFrom.HasValue)
            {
                queryable = queryable.Where(x => x.RequestDate.Date >= query.RequestDateFrom.Value);
            }
            else if (query.RequestDateTo.HasValue)
            {
                queryable = queryable.Where(x => x.RequestDate.Date <= query.RequestDateTo.Value);
            }

            if (query.requestStatus != null)
            {
                queryable = queryable.Where(x => x.RequestStatus == query.requestStatus);
            }

            if (query.PartId != null)
            {
                queryable = queryable.Where(x => x.Employee.PartId == query.PartId);
            }

            if (query.StatusFilter != null && query.StatusFilter.Count > 0)
            {
                queryable = queryable.Where(x => query.StatusFilter.Contains(x.RequestStatus));
            }

            switch (query.sortBy)
            {
                case "RequestDate":
                    queryable = query.SortAscending
                        ? queryable.OrderBy(x => x.RequestDate)
                        : queryable.OrderByDescending(x => x.RequestDate);
                    break;
                default:
                    queryable = queryable.OrderByDescending(x => x.RequestDate);
                    break;
            }

            return await QueryableExtensions.GetPagedAsync(queryable, query);

        }
    }
}

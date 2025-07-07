using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Infrastructure.Utilities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories
{
    public class SupplyRequestsMaterialDatumRepository : ISupplyRequestsMaterialDatumRepository
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public SupplyRequestsMaterialDatumRepository (ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Thêm đề xuất mới
        /// </summary>
        /// <param name="supplyRequestsMaterialData"></param>
        /// <returns></returns>
        public async Task AddSupplyRequestsMaterialDatumRepository(List<SupplyRequestsMaterialDatum> supplyRequestsMaterialData)
        {
            await _context.SupplyRequestsMaterialData.AddRangeAsync(supplyRequestsMaterialData);
        }

        /// <summary>
        /// Lấy đề xuất theo id
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public async Task<SupplyRequestsMaterialDatum> GetWithId(string requestId)
        {
            return await _context.SupplyRequestsMaterialData.FirstOrDefaultAsync(x => x.RequestId == requestId);         
        }

        /// <summary>
        /// Lấy tất cả đề xuất
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SupplyRequestsMaterialDatum>> GetAllSupplyRequestsMaterialDatumRepository()
        {
            return await _context.SupplyRequestsMaterialData.ToListAsync();
        }
        /// <summary>
        /// Cập nhật trạng thái đề xuất
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="requestStatus"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdateRequestStatusAsyncRepository(string requestId, string requestStatus)
        {
            var request = await _context.SupplyRequestsMaterialData
                .FirstOrDefaultAsync(x => x.RequestId == requestId);

            if (request == null)
            {
                throw new Exception($"Request with ID {requestId} not found");
            }
            else
            {
                request.RequestStatus = requestStatus;
            }
        }

        /// <summary>
        /// Cập nhật trạng thái đề xuất thành công
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SuccessRequestStatusAsyncRepository(string requestId, string note, string status)
        {
            var request = await _context.SupplyRequestsMaterialData
                .FirstOrDefaultAsync(x => x.RequestId == requestId);

            if (request == null)
            {
                throw new Exception($"Request with ID {requestId} not found");
            }
            else
            {
                request.NoteCancel = note;
                request.RequestStatus = status;
            }
        }

        public async Task<PagedResult<SupplyRequestsMaterialDatum>> GetSearchSupplyRequestsMaterialDatumRepository(SupplyRequestQuery query)
        {
            var queryable = _context.SupplyRequestsMaterialData
                .Include(x => x.Employee)
                .ThenInclude(x => x.Part)
                .Include(x => x.InventoryReceiptsMaterialData)
                .ThenInclude(x => x.Material.MaterialGroup)

            .AsQueryable();

            if (query.PartId != null && query.PartId.Any())
            {
                queryable = queryable.Where(x => query.PartId.Contains(x.Employee.PartId));
            }
            // Lọc theo khoản thời gian
            if (query.RequestDateFrom != null && query.RequestDateTo != null)
            {
                queryable = queryable.Where(x => x.RequestDate >= query.RequestDateFrom && x.RequestDate <= query.RequestDateTo);
            }

            if (query.RequestDateFrom != null)
            {
                queryable = queryable.Where(x => x.RequestDate >= query.RequestDateFrom);
            }

            if (query.RequestDateTo != null)
            {
                queryable = queryable.Where(x => x.RequestDate <= query.RequestDateTo);
            }

            if (query.materialGroupId != null)
            {
                queryable = queryable.Where(x =>
                    x.InventoryReceiptsMaterialData.Any(r => r.Material.MaterialGroupId == query.materialGroupId)
                );
            }

            if (query.materialName != null)
            {
                var keyword = query.materialName.ToLower();

                queryable = queryable.Where(x =>
                    x.InventoryReceiptsMaterialData.Any(r =>
                        EF.Functions.Collate(r.Material.Name, "Latin1_General_CI_AI").Contains(keyword)
                    )
                );
            }

            // Lọc theo nhiều trạng thái (ví dụ: Đang mua, Hoàn thành)
            if (query.StatusFilter != null && query.StatusFilter.Any())
            {
                queryable = queryable.Where(x => query.StatusFilter.Contains(x.RequestStatus));
            }

            switch (query.SortBy)
            {
                case "RequestId":
                    queryable = query.SortAscending
                        ? queryable.OrderBy(x => x.RequestId)
                        : queryable.OrderByDescending(x => x.RequestId);
                    break;
                case "RequestDate":
                    queryable = query.SortAscending
                        ? queryable.OrderBy(x => x.RequestDate)
                        : queryable.OrderByDescending(x => x.RequestDate);
                    break;
                case "RequestStatus":
                    queryable = query.SortAscending
                        ? queryable.OrderBy(x => x.RequestStatus)
                        : queryable.OrderByDescending(x => x.RequestStatus);
                    break;
                default:
                    queryable = queryable.OrderBy(x => x.RequestDate);
                    break;
            }

            return await QueryableExtensions.GetPagedAsync(queryable, query);
        }
    }
}

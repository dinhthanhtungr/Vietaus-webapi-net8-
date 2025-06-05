using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.DTOs.MaterialRequestDetails.Query;
using VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Infrastructure.Utilities;
using VietausWebAPI.WebAPI.DatabaseContext;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VietausWebAPI.Infrastructure.Repositories.MaterialRequestDetail
{
    public class MaterialRequestDetailRepository : IMaterialRequestDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public MaterialRequestDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRequetMaterialRepository(IEnumerable<RequestDetailMaterialDatum> requestDetailMaterialDatum)
        {
            await _context.RequestDetailMaterialData.AddRangeAsync(requestDetailMaterialDatum);
        }

        /// <summary>
        /// Lấy danh sách các yêu cầu mở (chưa được mua) theo ID vật liệu.
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RequestDetailMaterialDatum>> GetOpenRequestsByMaterialIdRepository(Guid materialId)
        {
            return await _context.RequestDetailMaterialData
                .AsNoTracking()
                .Include(x => x.Request)
                .Where(x => x.MaterialId == materialId && x.RequestedQuantity > x.PurchasedQuantity)
                .OrderBy(x => x.Request.RequestDate)
                .ToListAsync();
        }

        /// <summary>
        /// Cập nhật số lượng đã mua cho một dòng chi tiết đề xuất.
        /// </summary>
        /// <param name="detailId"></param>
        /// <param name="quantityToAdd"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdatePurchasedQuantityAsync(int detailId, int quantityToAdd)
        {
            var detail = await _context.RequestDetailMaterialData.FirstOrDefaultAsync(x => x.DetailId == detailId);

            if (detail == null)
                throw new Exception($"Không tìm thấy dòng đề xuất DetailID = {detailId}");

            detail.PurchasedQuantity += quantityToAdd;
        }


        public async Task<IEnumerable<RequestDetailMaterialDatum>> GetRequestMaterialRepository(string requestId)
        {
            var result = await _context.RequestDetailMaterialData
                .Include(x => x.Material)   
                .Where(x => x.RequestId == requestId)
                .ToListAsync();

            return result;
        }

        public async Task<PagedResult<RequestDetailMaterialDatum>> GetRequestMaterialStatusPayRepository(CreatePOQuery createPOQuery)
        {
            var queryable = _context.RequestDetailMaterialData
                .AsNoTracking()
                .Include(x => x.Material)
                .Include(x => x.Request)
                .Include(x => x.Material.MaterialGroup)
                .Where(x => x.RequestedQuantity > x.PurchasedQuantity)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(createPOQuery.requestStatus))
            {
                queryable = queryable.Where(x => x.Request.RequestStatus == createPOQuery.requestStatus);
            }

            if (createPOQuery.RequestDate.HasValue)
            {
                queryable = queryable.Where(x => x.Request.RequestDate.Date == createPOQuery.RequestDate.Value.Date);
            }

            if ( !string.IsNullOrWhiteSpace(createPOQuery.name))
            {
                string keyword = createPOQuery.name.ToLower();
                queryable = queryable.Where(x => x.Material.Name != null &&
                                                 EF.Functions.Collate(x.Material.Name, "Latin1_General_CI_AI").ToLower().Contains(keyword));
            }

            if (!string.IsNullOrWhiteSpace(createPOQuery.materialGroupName))
            {
                string keyword = createPOQuery.materialGroupName.ToLower();
                queryable = queryable.Where(x => x.Material.MaterialGroup.MaterialGroupName != null &&
                                                 EF.Functions.Collate(x.Material.MaterialGroup.MaterialGroupName, "Latin1_General_CI_AI").ToLower().Contains(keyword));
            }

            switch (createPOQuery.SortBy)
            {
                case "RequestId":
                    queryable = createPOQuery.SortAscending
                        ? queryable.OrderBy(x => x.RequestId)
                        : queryable.OrderByDescending(x => x.RequestId);
                    break;
                case "RequestStatus":
                    queryable = createPOQuery.SortAscending
                        ? queryable.OrderBy(x => x.Request.RequestStatus)
                        : queryable.OrderByDescending(x => x.Request.RequestStatus);
                    break;
                default:
                    queryable = queryable.OrderBy(x => x.Material.Name);
                    break;
            }
            createPOQuery.PageSize = 50;

            return await QueryableExtensions.GetPagedAsync(queryable, createPOQuery);
        }

        public async Task RollbackPurchasedQuantityAsync(Guid materialId, int quantityToRollback)
        {
            var requestDetails = await _context.RequestDetailMaterialData
                .Where(x => x.MaterialId == materialId && x.PurchasedQuantity > 0)
                .OrderByDescending(x => x.Request.RequestDate)
                .ToListAsync();

            int totalPurchased = requestDetails.Sum(x => x.PurchasedQuantity.GetValueOrDefault());
            int maxRollback = Math.Min(quantityToRollback, totalPurchased);

            foreach (var rd in requestDetails)
            {
                if (maxRollback <= 0)
                    break;

                int rollbackAmount = Math.Min(rd.PurchasedQuantity ?? 0, maxRollback);
                rd.PurchasedQuantity -= rollbackAmount;
                maxRollback -= rollbackAmount;
            }
        }
    }
}

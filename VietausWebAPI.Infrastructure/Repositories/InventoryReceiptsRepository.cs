using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

using VietausWebAPI.WebAPI.DatabaseContext;
using VietausWebAPI.Infrastructure.Utilities;
using VietausWebAPI.Core.DTO.PostDTO;

namespace VietausWebAPI.Infrastructure.Repositories
{
    public class InventoryReceiptsRepository : IInventoryReceiptsRepository
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Khởi tạo đối tượng InventoryReceiptsRepository
        /// </summary>
        /// <param name="context"></param>
        public InventoryReceiptsRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Thêm mới danh sách phiếu nhập kho
        /// </summary>
        /// <param name="inventoryReceiptsMaterialDatum"></param>
        /// <returns></returns>
        public async Task AddInventoryReceiptsRepositoryAsync(List<InventoryReceiptsMaterialDatum> inventoryReceiptsMaterialDatum)
        {
            await _context.InventoryReceiptsMaterialData.AddRangeAsync(inventoryReceiptsMaterialDatum);
            //await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Lấy tất cả danh sách phiếu nhập kho
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<InventoryReceiptsMaterialDatum>> GetAllInventoryReceiptsRepositoryAsync()
        {
            return await _context.InventoryReceiptsMaterialData.ToListAsync();
        }

        /// <summary>
        /// Tìm kiếm danh sách phiếu nhập kho theo các tiêu chí tìm kiếm và trả về kết quả phân trang
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedResult<InventoryReceiptsMaterialDatum>> SearchInventoryReceiptsRepositoryAsync(InventoryReceiptsQuery query)
        {
            // Lấy dữ liệu từ bảng InventoryReceiptsMaterialData
            var queryable = _context.InventoryReceiptsMaterialData
                .AsNoTracking()
                .Include(r => r.Request)
                .Include(r => r.Request.Employee)
                .AsQueryable();

            // Lọc theo RequestId
            if (!string.IsNullOrEmpty(query.RequestId))
            {
                queryable = queryable.Where(x => x.RequestId == query.RequestId);
            }

            // Lọc theo RequestDate
            if (!string.IsNullOrEmpty(query.EmployeeName))
            {
                queryable = queryable.Where(x => x.Request.Employee.FullName == query.EmployeeName);
            }

            // Lọc theo ReceiptDate
            else if (query.ReceiptDate != null)
            {
                queryable = queryable.Where(x => x.ReceiptDate == query.ReceiptDate);
            }
            // Lọc theo RequestStatus
            if (!string.IsNullOrEmpty(query.RequestStatus))
            {
                queryable = queryable.Where(x => x.Request.RequestStatus == query.RequestStatus);
            }
            // Lọc theo Static
            if (query.Static != null)
            {
                queryable = queryable.Where(x => x.Status == query.Static);
            }   

            // Lọc theo MaterialName
            if (!string.IsNullOrEmpty(query.MaterialName))
            {
                queryable = queryable.Where(x => x.MaterialName == query.MaterialName);
            }

            // Sắp xếp
            switch (query.SortBy?.ToLower())
            {
                case "RequestId":
                    queryable = query.SortAscending
                        ? queryable.OrderBy(x => x.RequestId)
                        : queryable.OrderByDescending(x => x.RequestId);
                    break;
                case "RequestDate":
                    queryable = query.SortAscending
                        ? queryable.OrderBy(x => x.ReceiptDate)
                        : queryable.OrderByDescending(x => x.ReceiptDate);
                    break;
                default:
                    queryable = queryable.OrderBy(x => x.ReceiptDate);
                    break;
            }

            var temp = new PaginationQuery();

            return await QueryableExtensions.GetPagedAsync(queryable, query);
        }
        /// <summary>
        /// Cập nhật danh sách phiếu nhập kho
        /// </summary>
        /// <param name="inventoryReceiptsMaterialDatum"></param>
        /// <returns></returns>
        public async Task UpdateInventoryReceiptsRepositoryAsync(InventoryReceiptsUpdatePriceDTO inventoryReceiptsMaterialDatum)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                //var receiptIds = inventoryReceiptsMaterialDatum.Select(x => x.ReceiptId).ToList();

                var receipts = await _context.InventoryReceiptsMaterialData
                    .FirstOrDefaultAsync(x => x.ReceiptId == inventoryReceiptsMaterialDatum.ReceiptId);
                //.Where(x => receiptIds.Contains(x.ReceiptId))
                //.ToListAsync();

                //foreach (var receipt in receipts)
                //{
                //    var newReceipt = inventoryReceiptsMaterialDatum.FirstOrDefault(x => x.ReceiptId == receipt.ReceiptId);
                //    if (newReceipt != null)
                //    {
                //        receipt.UnitPrice = newReceipt.UnitPrice;
                //        receipt.TotalPrice = newReceipt.TotalPrice;
                //    }
                //}
                if (receipts == null)
                {
                    throw new Exception($"Receipt with ID {inventoryReceiptsMaterialDatum.ReceiptId} not found");
                }
                else
                {
                    receipts.UnitPrice = inventoryReceiptsMaterialDatum.UnitPrice;
                    receipts.TotalPrice = inventoryReceiptsMaterialDatum.TotalPrice;
                    receipts.Status = inventoryReceiptsMaterialDatum.Status;
                }
            }

            catch (Exception ex)
            {
                transaction.Rollback();
            }
        }
    }
}

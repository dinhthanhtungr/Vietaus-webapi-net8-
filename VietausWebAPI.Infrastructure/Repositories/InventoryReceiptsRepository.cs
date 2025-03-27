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

namespace VietausWebAPI.Infrastructure.Repositories
{
    public class InventoryReceiptsRepository : IInventoryReceiptsRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryReceiptsRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddInventoryReceiptsRepositoryAsync(List<InventoryReceiptsMaterialDatum> inventoryReceiptsMaterialDatum)
        {
            await _context.InventoryReceiptsMaterialData.AddRangeAsync(inventoryReceiptsMaterialDatum);
            //await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<InventoryReceiptsMaterialDatum>> GetAllInventoryReceiptsRepositoryAsync()
        {
            return await _context.InventoryReceiptsMaterialData.ToListAsync();
        }

        //public async Task<IEnumerable<InventoryReceiptsMaterialDatum>> SearchInventoryReceiptsRepositoryAsync(InventoryReceiptsQuery query)
        //{
        //    var queryable = _context.InventoryReceiptsMaterialData
        //        .AsNoTracking()
        //        .Include(r => r.Request)
        //        .Include(r => r.Request.Employee)
        //        //.Include(r => r.SupplierId)
        //        //.Include(r => r.)
        //        .AsQueryable();

        //    if (!string.IsNullOrEmpty(query.RequestId))
        //    {
        //        queryable = queryable.Where(x => x.RequestId == query.RequestId);
        //    }

        //    if (!string.IsNullOrEmpty(query.EmployeeName))
        //    {
        //        queryable = queryable.Where(x => x.Request.Employee.FullName == query.EmployeeName);    
        //    }

        //    else if (query.ReceiptDate != null)
        //    {
        //        queryable = queryable.Where(x => x.ReceiptDate == query.ReceiptDate);
        //    }
        //    // Lọc theo RequestStatus
        //    if (!string.IsNullOrEmpty(query.RequestStatus))
        //    {
        //        queryable = queryable.Where(x => x.Request.RequestStatus == query.RequestStatus);
        //    }

        //    // Lọc theo MaterialName
        //    if (!string.IsNullOrEmpty(query.MaterialName))
        //    {
        //        queryable = queryable.Where(x => x.MaterialName == query.MaterialName);
        //    }

        //    // Sắp xếp
        //    switch (query.SortBy?.ToLower())
        //    {
        //        case "RequestId":
        //            queryable = query.SortAscending
        //                ? queryable.OrderBy(x => x.RequestId)
        //                : queryable.OrderByDescending(x => x.RequestId);
        //            break;
        //        case "RequestDate":
        //            queryable = query.SortAscending
        //                ? queryable.OrderBy(x => x.ReceiptDate)
        //                : queryable.OrderByDescending(x => x.ReceiptDate);
        //            break;
        //        //case "RequestStatus":
        //        //    queryable = query.SortAscending
        //        //        ? queryable.OrderBy(x => x.RequestStatus)
        //        //        : queryable.OrderByDescending(x => x.RequestStatus);
        //        //    break;
        //        default:
        //            queryable = queryable.OrderBy(x => x.ReceiptDate);
        //            break;
        //    }


        //    // Tính tổng số trang trước khi nhận trang
        //    int totalItems = await queryable.CountAsync();

        //    int pageNumber = Math.Max(1, query.PageNumber);
        //    int pageSize = Math.Max(1, query.PageSize);
        //    queryable = queryable
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize);

        //    return await queryable.ToListAsync();
        //}

        public async Task<PagedResult<InventoryReceiptsMaterialDatum>> SearchInventoryReceiptsRepositoryAsync(InventoryReceiptsQuery query)
        {
            var queryable = _context.InventoryReceiptsMaterialData
                .AsNoTracking()
                .Include(r => r.Request)
                .Include(r => r.Request.Employee)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.RequestId))
            {
                queryable = queryable.Where(x => x.RequestId == query.RequestId);
            }

            if (!string.IsNullOrEmpty(query.EmployeeName))
            {
                queryable = queryable.Where(x => x.Request.Employee.FullName == query.EmployeeName);
            }

            else if (query.ReceiptDate != null)
            {
                queryable = queryable.Where(x => x.ReceiptDate == query.ReceiptDate);
            }
            // Lọc theo RequestStatus
            if (!string.IsNullOrEmpty(query.RequestStatus))
            {
                queryable = queryable.Where(x => x.Request.RequestStatus == query.RequestStatus);
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


            // Tính tổng số trang trước khi nhận trang
            //int totalItems = await queryable.CountAsync();

            //int pageNumber = Math.Max(1, query.PageNumber);
            //int pageSize = Math.Max(1, query.PageSize);
            //queryable = queryable
            //    .Skip((pageNumber - 1) * pageSize)
            //    .Take(pageSize);

            //return await queryable.ToListAsync();
            return await QueryableExtensions.GetPagedAsync(queryable, query);
        }

        public async Task<bool> UpdateInventoryReceiptsRepositoryAsync(List<InventoryReceiptsMaterialDatum> inventoryReceiptsMaterialDatum)
        {
            //using var transaction = _context.Database.BeginTransaction();
            //try
            //{
            //    var receiptIds = inventoryReceiptsMaterialDatum.Select(x => x.ReceiptId).ToList();

            //    var receipts = await _context.InventoryReceiptsMaterialData
            //        .Where(x => receiptIds.Contains(x.ReceiptId))
            //        .ToListAsync();

            //    foreach (var receipt in receipts)
            //    {
            //        var newReceipt = inventoryReceiptsMaterialDatum.FirstOrDefault(x => x.ReceiptId == receipt.ReceiptId);
            //        if (newReceipt != null)
            //        {
            //            receipt.MaterialGroupId = newReceipt.MaterialGroupId;
            //            receipt.RequestId = newReceipt.RequestId;
            //            receipt.ReceiptDate = newReceipt.ReceiptDate;
            //            receipt.MaterialName = newReceipt.MaterialName;
            //            receipt.Unit = newReceipt.Unit;
            //            receipt.ReceivedQuantity = newReceipt.ReceivedQuantity;
            //            receipt.UnitPrice = newReceipt.UnitPrice;
            //            receipt.TotalPrice = newReceipt.TotalPrice;
            //            receipt.Note = newReceipt.Note;
            //            receipt.Status = newReceipt.Status;
            //            receipt.SupplierId = newReceipt.SupplierId;
            //        }
            //    }

            //    await _un

            //}

            return true;
        }
    }
}

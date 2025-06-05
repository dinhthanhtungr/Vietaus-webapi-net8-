using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.DTOs.PurchaseOrders;
using VietausWebAPI.Core.Application.DTOs.PurchaseOrders.Query;
using VietausWebAPI.Core.Application.Usecases.PurchaseOrders.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Infrastructure.Migrations;
using VietausWebAPI.Infrastructure.Utilities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.PurchaseOrders
{
    public class PurchaseOrdersRepository : IPurchaseOrdersRepository
    {
        private readonly ApplicationDbContext _context;
        public PurchaseOrdersRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreatePurchaseOrdersRepositoryAsync(PurchaseOrdersMaterialDatum purchaseOrder)
        {
            await _context.PurchaseOrdersMaterialData.AddAsync(purchaseOrder);
        }

        public async Task<SequencePoMaterialDatum?> GetLastPurchaseOrderIdRepositoryAsync(int year)
        {
            return await _context.SequencePoMaterialData.FirstOrDefaultAsync(s => s.Year == year);
        }

        public async Task AddNewNumber(SequencePoMaterialDatum sequencePoMaterialDatum)
        {
            await _context.SequencePoMaterialData.AddAsync(sequencePoMaterialDatum);
        }

        public async Task<PagedResult<PurchaseOrdersMaterialDatum>> GetPurchaseOrdersRepositoryAsync(GetPOQuery query)
        {
            var queryable = _context.PurchaseOrdersMaterialData
                .AsNoTracking()
                .Include(po => po.Supplier)
                .Include(po => po.Employee)
           
                //.Include(po => po.PurchaseOrderDetailsMaterialData)
                //.Include(po => po.PurchaseOrderStatusHistoryMaterialData)
                .AsQueryable();

            // Lọc theo khoản thời gian
            if (query.RequestDateFrom != null && query.RequestDateTo != null)
            {
                queryable = queryable.Where(x => x.OrderDate >= query.RequestDateFrom && x.OrderDate <= query.RequestDateTo);
            }

            else if (query.RequestDateFrom.HasValue)
            {
                queryable = queryable.Where(x => x.OrderDate >= query.RequestDateFrom.Value);
            }

            else if (query.RequestDateTo.HasValue)
            {
                queryable = queryable.Where(x => x.OrderDate <= query.RequestDateTo.Value);
            }

            if (!string.IsNullOrEmpty(query.Pocode))
            {
                queryable = queryable.Where(x => x.Pocode.Contains(query.Pocode));
            }

            if (!string.IsNullOrEmpty(query.Status))
            {
                queryable = queryable.Where(x => x.Status == query.Status);
            }

            if (query.SupplierId.HasValue)
            {
                queryable = queryable.Where(x => x.SupplierId == query.SupplierId.Value);
            }

            queryable = queryable.OrderByDescending(x => x.OrderDate);

            return await QueryableExtensions.GetPagedAsync(queryable, query);
        }

        public async Task<PurchaseOrdersMaterialDatum> GetByIdAsync(Guid poid)
        {
            var result = await _context.PurchaseOrdersMaterialData.FirstOrDefaultAsync(x => x.Poid == poid);
            if (result == null)
            {
                throw new InvalidOperationException($"Purchase order with ID {poid} not found.");
            }
            return result;
        }

        public async Task DeleteAsync(PurchaseOrdersMaterialDatum po)
        {
            var existingPo = await _context.PurchaseOrdersMaterialData
                .FirstOrDefaultAsync(x => x.Poid == po.Poid);
            if (existingPo == null)
            {
                throw new InvalidOperationException($"Purchase order with ID {po.Poid} not found.");
            }
            _context.PurchaseOrdersMaterialData.Remove(existingPo);
        }

        public async Task SuccessPunchaseOrderRepository(Guid guid)
        {
            var item = await _context.PurchaseOrdersMaterialData
                .Where(x => x.Poid == guid)
                .FirstOrDefaultAsync();

            if (item != null)
            {
                item.Status = "ĐÃ HOÀN THÀNH";
            }
        }
    }
}

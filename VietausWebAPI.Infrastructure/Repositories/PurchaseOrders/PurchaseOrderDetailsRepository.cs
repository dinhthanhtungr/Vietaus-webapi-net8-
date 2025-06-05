using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Usecases.PurchaseOrders.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.PurchaseOrders
{
    public class PurchaseOrderDetailsRepository : IPurchaseOrderDetailsRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteByPOIDAsync(Guid guid)
        {
            var query = await _context.PurchaseOrderDetailsMaterialData
                .Where(detail => detail.Poid == guid)
                .ToListAsync();
            _context.PurchaseOrderDetailsMaterialData.RemoveRange(query);
        }

        public async Task<IEnumerable<PurchaseOrderDetailsMaterialDatum>> GetByPOIDAsync(Guid poid)
        {
            var query = await _context.PurchaseOrderDetailsMaterialData
                .Where(detail => detail.Poid == poid)
                .ToListAsync();

            return query;
        }

        public async Task GetPurchaseOrderDetail(IEnumerable<PurchaseOrderDetailsMaterialDatum> purchaseOrderDetailsMaterialDatum)
        {
            await _context.PurchaseOrderDetailsMaterialData.AddRangeAsync(purchaseOrderDetailsMaterialDatum);

        }

        public async Task<IEnumerable<PurchaseOrderDetailsMaterialDatum>> PostPurchaseOrderDetail(Guid POID)
        {
            return await _context.PurchaseOrderDetailsMaterialData

                .Where(detail => detail.Poid == POID)
                .Include(r => r.Material)
                .Include(r => r.Material.MaterialGroup)
                .ToListAsync();
        }
    }
}

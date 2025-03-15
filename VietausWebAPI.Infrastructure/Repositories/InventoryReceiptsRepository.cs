using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

using VietausWebAPI.WebAPI.DatabaseContext;

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
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<InventoryReceiptsMaterialDatum>> GetAllInventoryReceiptsRepositoryAsync()
        {
            return await _context.InventoryReceiptsMaterialData.ToListAsync();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Infrastructure.Models;
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

        public async Task AddInventoryReceipts(InventoryReceiptsMaterialDatum inventoryReceiptsMaterialDatum)
        {
            _context.InventoryReceiptsMaterialData.Add(inventoryReceiptsMaterialDatum);
            await _context.SaveChangesAsync();
        }

    }
}

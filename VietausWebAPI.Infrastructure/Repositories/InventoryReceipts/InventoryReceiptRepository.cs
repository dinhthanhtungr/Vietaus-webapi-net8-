using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Usecases.InventoryReceipts.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.InventoryReceipts
{
    public class InventoryReceiptRepository : IInventoryReceiptRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryReceiptRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddInventoryReceiptsRepositoryAsync(List<InventoryReceiptsMaterialDatum> inventoryReceiptsMaterialDatum)
        {
            await _context.InventoryReceiptsMaterialData.AddRangeAsync(inventoryReceiptsMaterialDatum);
        }
    }
}

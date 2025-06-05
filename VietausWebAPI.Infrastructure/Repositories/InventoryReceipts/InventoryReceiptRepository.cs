using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.DTOs.InventoryReceipts;
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

        public async Task<List<InventoryReceiptsMaterialDatum>> GetInventoryReceiptsByIdAsync(string id)
        {
            return await _context.InventoryReceiptsMaterialData
                .Where(x => x.RequestId == id)
                .Include(x => x.Material)
                .ThenInclude(x => x.MaterialGroup)
                .ToListAsync();
        }
        public async Task UpdateFieldChangeRepository(int id, FieldUpdateDTO field)
        {
            var item = await _context.InventoryReceiptsMaterialData
                .FirstOrDefaultAsync(x => x.DetailId == id);

            if (item == null)
                throw new Exception("Item not found");

            switch (field.FieldName)
            {
                case "ReceiptQty":
                    if (field.ReceiptQty.HasValue)
                        item.ReceiptQty = field.ReceiptQty.Value;
                    else
                        throw new Exception("IntValue is required for ReceiptQty");
                    break;

                case "UnitPrice":
                    if (field.UnitPrice.HasValue)
                        item.UnitPrice = field.UnitPrice.Value;
                    else
                        throw new Exception("DecimalValue is required for UnitPrice");
                    break;

                default:
                    throw new ArgumentException("Invalid field name");
            }

            await _context.SaveChangesAsync();
        }

    }
}

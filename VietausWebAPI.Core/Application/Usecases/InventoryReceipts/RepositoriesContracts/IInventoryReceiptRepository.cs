using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.InventoryReceipts;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Usecases.InventoryReceipts.RepositoriesContracts
{
    public interface IInventoryReceiptRepository
    {
        Task AddInventoryReceiptsRepositoryAsync(List<InventoryReceiptsMaterialDatum> inventoryReceiptsMaterialDatum);

        Task<List<InventoryReceiptsMaterialDatum>> GetInventoryReceiptsByIdAsync(string id);
        Task UpdateFieldChangeRepository(int id, FieldUpdateDTO field);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Infrastructure.Models;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IInventoryReceiptsRepository
    {
        Task AddInventoryReceipts(InventoryReceiptsMaterialDatum inventoryReceiptsMaterialDatum);
    }
}

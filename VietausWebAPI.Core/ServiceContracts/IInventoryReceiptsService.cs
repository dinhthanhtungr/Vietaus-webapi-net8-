using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO;

namespace VietausWebAPI.Core.ServiceContracts
{
    public interface IInventoryReceiptsService
    {
        Task AddInventoryReceiptsAsync(InventoryReceiptsDTO inventoryReceiptsDTO);
    }
}

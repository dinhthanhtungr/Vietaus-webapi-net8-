using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.InventoryReceipts;

namespace VietausWebAPI.Core.Application.Usecases.InventoryReceipts.ServiceContracts
{
    public interface IInventoryReceiptService
    {
        Task<List<InventoryDetailMaterialDTO>> GetMaterialReceiptIdService(string id);
        Task UpdateFieldChangeService(int id, FieldUpdateDTO field);    
    }
}

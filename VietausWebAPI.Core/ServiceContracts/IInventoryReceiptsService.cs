using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.DTO.QueryObject;


namespace VietausWebAPI.Core.ServiceContracts
{
    public interface IInventoryReceiptsService
    {
        Task AddInventoryReceiptsServiceAsync(InventoryReceiptsDTO inventoryReceiptsDTO);
        Task<IEnumerable<InventoryReceiptsGetDTO>> GetAllInventoryReceiptsServiceAsync();
        Task<PagedResult<InventoryReceiptsGetDTO>> AddInventoryReceiptsServiceAsync(InventoryReceiptsQuery inventoryReceiptsQuery);
    }
}

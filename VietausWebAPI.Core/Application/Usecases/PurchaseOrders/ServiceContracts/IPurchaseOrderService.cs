using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.PurchaseOrders;
using VietausWebAPI.Core.Application.DTOs.PurchaseOrders.Query;
using VietausWebAPI.Core.DTO.QueryObject;

namespace VietausWebAPI.Core.Application.Usecases.PurchaseOrders.ServiceContracts
{
    public interface IPurchaseOrderService
    {
        Task<PagedResult<PurchaseOrderDTO>> GetPurchaseOrdersServiceAsync(GetPOQuery query);

        Task<string> GeneratePONumberAsync();
        Task CreatePurchaseOrderAsync(CreatePurchaseOrderDTO purchaseOrder);
        Task DeletePurchaseOrderAsync(Guid poid);
        Task SuccessPunchaseOrderAsync(Guid guid);
    }
}

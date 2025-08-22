using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.PurchaseOrders;
using VietausWebAPI.Core.Application.DTOs.PurchaseOrders.Query;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Usecases.PurchaseOrders.RepositoriesContracts
{
    public interface IPurchaseOrdersRepository
    {
        Task<PagedResult<PurchaseOrdersMaterialDatum>> GetPurchaseOrdersRepositoryAsync(GetPOQuery query);
        Task CreatePurchaseOrdersRepositoryAsync(PurchaseOrdersMaterialDatum purchaseOrder);
        Task<PurchaseOrdersMaterialDatum> GetByIdAsync(Guid poid);
        Task DeleteAsync(PurchaseOrdersMaterialDatum po);

        Task SuccessPunchaseOrderRepository(Guid guid);
    }
}

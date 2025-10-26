using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Queries;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs.Material_warehouse;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.Queries;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.ServiceContracts
{
    public interface IPurchaseOrderService
    {
        Task<OperationResult> CreateAsync (PostPurchaseOrder postPurchaseOrder, CancellationToken ct = default);
        Task<PagedResult<GetSamplePurchaseOrder>> GetAllAsync (PurchaseOrderQuery query, CancellationToken ct = default);
        Task<PagedResult<GetPOPurchaseOrder>> GetSelectableLinesAsync(DeliveryOrderQuery query, CancellationToken ct = default);
        Task<GetPurchaseOrder> GetPurchaseOrderByIdAsync (Guid purchaseOrderId, CancellationToken ct = default);
        Task<MaterialStock> GetMaterialStockAsync(Guid materialId, CancellationToken ct = default); 
        Task<OperationResult> UpdateAsync (PatchPurchaseOrder patchPurchaseOrder, CancellationToken ct = default);
    }
}

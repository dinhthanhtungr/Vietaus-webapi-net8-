using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Queries;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.ServiceContracts
{
    public interface IDeliveryOrderService
    {


        // ======================================================================== Get ======================================================================== 
        Task<PagedResult<GetSampleDelivery>> GetAllAsync(DeliveryOrderQuery query, CancellationToken ct = default);
        //Tạm thời cmt lại để thử chức năng mới
        //Task<PagedResult<GetPODeliveryOrder>> GetSelectableLinesAsync(DeliveryOrderQuery query, CancellationToken ct = default);
        Task<OperationResult<PagedResult<GetPODeliveryOrder>>> GetSelectableLinesAsync(DeliveryOrderQuery query, CancellationToken ct = default);
        Task<PagedResult<GetDeliverer>> GetAllDelivererAsync(DelivererQuery query, CancellationToken ct = default);
        Task<GetDeliveryOrder> GetAsync(Guid query, CancellationToken ct = default);

        // ======================================================================== Post ======================================================================== 
        //Task<OperationResult> CreateAsync(PostDeliveryOrder postDeliveryOrder, CancellationToken ct = default);
        Task<OperationResult<GetDeliveryOrder>> CreateAsync(PostDeliveryOrder request, CancellationToken ct = default);

        // ======================================================================== Patch ======================================================================== 
        Task<OperationResult> UpdateAsync(PatchDeliveryOrder putDeliveryOrder, CancellationToken ct = default);
        Task<OperationResult> SoftDeleteAsync(Guid id, CancellationToken ct = default);
        //Task<OperationResult> AssignDeliverersAsync(AssignDeliverersCommand commkand, CancellationToken ct = default);

        // ======================================================================== Excel ======================================================================== 

        Task<List<DeliveryPlanRow>> BuildRowsAsync(DateTime from, DateTime to, CancellationToken ct);
    
    }
}

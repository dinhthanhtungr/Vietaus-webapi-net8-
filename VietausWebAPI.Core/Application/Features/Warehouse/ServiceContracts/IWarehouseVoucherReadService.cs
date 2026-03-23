using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices;
using VietausWebAPI.Core.Application.Features.Warehouse.Queries;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts
{
    public interface IWarehouseVoucherReadService
    {
        Task<OperationResult<PagedResult<WarehouseVoucherDto>>> GetWarehouseVouchersAsync(WarehouseVoucherReadQuery query);
        Task<OperationResult<WarehouseVoucherDto>> GetWarehouseVoucherByIdAsync(long voucherId);
    }
}

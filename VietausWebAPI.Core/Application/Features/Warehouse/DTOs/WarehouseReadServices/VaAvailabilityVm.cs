using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices
{
    public sealed record VaAvailabilityVm(
        string Code,
        //decimal SnapshotQty,        // ảnh chụp lúc lập công thức
        decimal OnHandKg,           // tồn thực tế hiện tại
        decimal ReservedOpenAllKg,  // tổng đang giữ chỗ (OPEN) toàn hệ thống
        decimal AvailableKg         // = OnHandKg - ReservedOpenAllKg
    );

}

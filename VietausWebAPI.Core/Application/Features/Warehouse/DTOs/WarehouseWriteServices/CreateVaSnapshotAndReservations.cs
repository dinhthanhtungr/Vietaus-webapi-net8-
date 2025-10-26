using Microsoft.EntityFrameworkCore.Design.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices;

namespace VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseWriteServices
{
    public class CreateVaSnapshotAndReservations
    {
        public Guid companyId { get; set; }
        public Guid manufacturingFormulaId { get; set; }
        public Guid createdBy { get; set; }
        public bool cancelPreviousOpen { get; set; } = true;        // huỷ các giữ chỗ OPEN cũ của VA (nếu có)
        public List<MfgFormulaMaterialWarehouse> reservations { get; set; } = new List<MfgFormulaMaterialWarehouse>();
        public List<VaAvailabilityVm> expected { get; set; } = new List<VaAvailabilityVm>(); // <= mới (optional)
    }
}

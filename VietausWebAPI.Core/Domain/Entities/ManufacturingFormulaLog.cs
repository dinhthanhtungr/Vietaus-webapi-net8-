using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums;

namespace VietausWebAPI.Core.Domain.Entities
{
    public class ManufacturingFormulaLog
    {
        public Guid Id { get; set; }
        public Guid ManufacturingFormulaId { get; set; }
        public ManufacturingFormulaLogAction Action { get; set; }     // Activate, Deactivate, SetStandard, UnsetStandard
        public Guid PerformedBy { get; set; }
        public string? PerformedByNameSnapshot { get; set; }
        public DateTime PerformedDate { get; set; }
        public string? Comment { get; set; }

        public ManufacturingFormula ManufacturingFormula { get; set; } = default!;
        public Employee PerformedByNavigation { get; set; } = default!; // <--- thêm navigation
    }
}

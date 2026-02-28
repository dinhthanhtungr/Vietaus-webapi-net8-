using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures
{

    public sealed class FormulaExportRow
    {
        public string TP { get; set; } = "";          // TP_15679
        public string SoMe { get; set; } = "";        // VU221100052
        public string Code { get; set; } = "";        // WP21042
        public string Ten { get; set; } = "";         // HẠT MÀU
        public string NVL { get; set; } = "";         // NVL_BM_...
        public string Detail { get; set; } = "";      // Bột màu... / Hạt nhựa...
        public decimal DinhMuc { get; set; }          // 0.0062500000
        public string? KhachHang { get; set; }        // optional
    }
}

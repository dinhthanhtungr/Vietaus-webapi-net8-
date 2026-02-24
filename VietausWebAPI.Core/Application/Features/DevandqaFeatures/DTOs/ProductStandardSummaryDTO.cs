using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs
{
    public class ProductStandardSummaryDTO
    {
        public string ColourCode { get; set; } = string.Empty;      // Ví dụ: LP71034C
        public string Status { get; set; } = string.Empty;          // Ví dụ: Hoàn thành
        public string Density { get; set; } = string.Empty;         // Ví dụ: 0.9–1.0
        public string MeltIndex { get; set; } = string.Empty;       // Ví dụ: 3.0–5.0
        public string packed { get; set; } = string.Empty;           // Ví dụ: Bao jumbo
        public int? weight { get; set; }
        public DateTime CreatedDate { get; set; }                   // Ví dụ: 14/11/2024
        public Guid Id { get; set; } = default;              // Dùng cho hành động
    }
}

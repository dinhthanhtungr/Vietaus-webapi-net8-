using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.ProductInspectionFeature
{
    public class PDFColumn
    {
        public string? TestItem { get; set; }
        public string? Specification { get; set; }
        public string? Method { get; set; }
        public string? Unit { get; set; }
        public string? Result { get; set; }
    }
}

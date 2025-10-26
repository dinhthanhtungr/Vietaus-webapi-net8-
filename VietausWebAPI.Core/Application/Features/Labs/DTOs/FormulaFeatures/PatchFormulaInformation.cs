using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures
{
    public class PatchFormulaInformation
    {
        public Guid FormulaId { get; set; }
        public Guid SampleRequestId { get; set; }
        public string? Status { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? CheckBy { get; set; }          // UNIQUEIDENTIFIER
        public Guid? SentBy { get; set; }          // UNIQUEIDENTIFIER
        public string? CheckNameSnapshot { get; set; }  // NVARCHAR
        public string? SentByNameSnapshot { get; set; }  // NVARCHAR

    }
}

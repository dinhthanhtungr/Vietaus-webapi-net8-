using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.QCOutputFeature
{
    public class QCDetailDTO
    {

        public string? BatchExternalId { get; set; }

        public Guid? BatchId { get; set; }

        public string MachineExternalId { get; set; } = default!;
    }
}

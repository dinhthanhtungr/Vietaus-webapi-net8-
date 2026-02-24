using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Devandqa;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.QCInputByQCFeatures
{
    public class PatchQCInputByQC
    {
        public string? InspectionMethod { get; set; }
        public bool? IsCOAProvided { get; set; }
        public bool? IsMSDSTDSProvided { get; set; }
        public bool? IsMetalDetectionRequired { get; set; }
        public bool? IsSuccessQuality { get; set; }
        public QcDecision? ImportWarehouseType { get; set; }
        public string? Note { get; set; }

        // nếu muốn cho đổi attachment collection
        public Guid? AttachmentCollectionId { get; set; }
        public bool? HasNewAttachments { get; set; }
    }

}

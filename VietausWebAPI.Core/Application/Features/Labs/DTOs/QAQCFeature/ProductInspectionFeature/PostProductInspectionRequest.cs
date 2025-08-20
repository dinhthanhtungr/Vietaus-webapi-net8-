using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.QCOutputFeature;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.ProductInspectionFeature
{
    public class PostProductInspectionRequest
    {
        public ProductInspectionInformation ProductInspection { get; set; }
        public QCDetailDTO? QCDetail { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ManufacturingVUFormulaFeatures;

namespace VietausWebAPI.Core.Application.Features.Labs.Helpers.FormulaFeatures
{
    public interface IFormulaPDF
    {
        byte[] Render(ManufacturingVUPDF data);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.MfgGetInformationDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.MfgGetInformationInforDtos;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.MFGProductionOrderFeatures
{
    public interface IMfgGetInformationService
    {
        Task<GetMfgProductionOrderNoteInfor> GetAsync(Guid? mfgProductionOrderId, Guid? formulaId, FormulaType? formulaType, CancellationToken ct = default);

        Task<GetMfgProductionOrderInform?> GetByIdAsync(Guid mfgProductionOrderId);
    }
}

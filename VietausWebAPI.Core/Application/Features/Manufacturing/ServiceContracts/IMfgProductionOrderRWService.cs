using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts
{
    public interface IMfgProductionOrderRWService
    {
        Task<GetMfgProductionOrderRWs?> GetByIdAsync(Guid mfgProductionOrderId);
    }
}

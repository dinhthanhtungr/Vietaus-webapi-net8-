using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.SupplyRequests;
using VietausWebAPI.Core.DTO.PostDTO;

namespace VietausWebAPI.Core.Application.Usecases.SupplyRequests.ServiceContracts
{
    public interface ISupplyRequestService
    {
        Task<string> CreateRequestMaterial(SupplyRequestData request);
    }
}

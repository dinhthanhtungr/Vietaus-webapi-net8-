using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.Materials;
using VietausWebAPI.Core.Application.DTOs.SupplyRequests;

namespace VietausWebAPI.Core.Application.Usecases.SupplyRequests.ServiceContracts
{
    public interface IMaterialsService
    {
        Task<List<MaterialSearchResultDto>> materialSearcheServiceAsync(string name, Guid materialGroupId);
        Task CreateMaterialAsync(List<MaterialsDTO> material);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.Entities;

namespace VietausWebAPI.Core.ServiceContracts
{
    public interface IRequestMaterialService
    {
        Task<string> CreateRequestMaterial(RequestMaterialDTO request);
        Task<RequestIdDTO> GetLastRequestIdService();
        Task<IEnumerable<RequestMaterialDTO>> GetMaterialAsyncService(RequestMaterialQuery query);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.PostDTO;

namespace VietausWebAPI.Core.ServiceContracts
{
    public interface IRequestDetailMaterialService
    {
        Task AddRequestDetailServiceAsync(RequestDetailMaterialDatumPostDTO requestDetailMaterialDatumPostDTO);
        Task<IEnumerable<RequestDetailMaterialDatumPostDTO>> GetAllRequestDetailServiceAsync();
    }
}

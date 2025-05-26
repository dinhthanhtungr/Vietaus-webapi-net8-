using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.Approval;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.PostDTO;

namespace VietausWebAPI.Core.ServiceContracts
{
    public interface IRequestDetailMaterialService
    {
        /// <summary>
        /// Thêm mới danh sách các vật tư đề xuất đi chung với lại request
        /// </summary>
        /// <param name="requestDetailMaterialDatumPostDTO"></param>
        /// <returns></returns>
        Task AddRequestDetailServiceAsync(RequestDetailMaterialDatumPostDTO requestDetailMaterialDatumPostDTO);
        /// <summary>
        /// Lấy tất cả danh sách vật tư đề xuất
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<RequestDetailMaterialDatumPostDTO>> GetAllRequestDetailServiceAsync();

        //Task<IEnumerable<RequestDetailResponseGetDto>> GetSearchRequestDetailServiceAsync(string requestId);
         
    }
}

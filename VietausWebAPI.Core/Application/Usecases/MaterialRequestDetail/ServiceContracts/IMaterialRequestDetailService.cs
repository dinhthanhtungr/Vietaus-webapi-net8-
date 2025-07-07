using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.MaterialRequestDetails;
using VietausWebAPI.Core.Application.DTOs.MaterialRequestDetails.Query;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail.ServiceContracts
{
    public interface IMaterialRequestDetailService
    {
        /// <summary>
        /// Thêm mới danh sách các vật tư đề xuất đi chung với lại request
        /// </summary>
        /// <param name="MaterialRequestDetailGetDTO"></param>
        /// <returns></returns>
        Task AddRequestDetailServiceAsync(MaterialRequestDetailGetDTO MaterialRequestDetailGetDTO );

        Task<IEnumerable<MaterialRequestDetailPostDTO>> GetRequestDetailServieAsync(string requestId);

        Task<PagedResult<POMaterialRequestDetailPostDTO>> GetRequestMaterialStatusPayService(CreatePOQuery createPOQuery);

    }
}

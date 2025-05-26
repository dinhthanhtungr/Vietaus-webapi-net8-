using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.Core.Service
{
    public class RequestMaterialService : IRequestMaterialService
    {
        //private readonly IRequestMaterialRepository _requestRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public RequestMaterialService (IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <summary>
        /// Tạo một đề xuất mua vật tư với đầy đủ các thông số
        /// </summary>
        /// <param name="requestDTO"></param>
        /// <returns></returns>
        //public async Task<string> CreateRequestMaterial(RequestMaterialDTO requestDTO)
        //{
        //    await _unitOfWork.BeginTransactionAsync();
        //    try
        //    {
        //        var request = _mapper.Map<SupplyRequestsMaterialDatum>(requestDTO);


        //        await _unitOfWork.RequestMaterialRepository.CreateRequestAsync(request);

        //        var requestDetails = requestDTO.RequestDetails.Select(detailDTO =>
        //        {
        //            var detail = _mapper.Map<RequestDetailMaterialDatum>(detailDTO);
        //            detail.RequestId = request.RequestId;
        //            return detail;
        //        }).ToList();

        //        await _unitOfWork.RequestMaterialRepository.AddRequestDetailMaterialAsync(requestDetails);
        //        await _unitOfWork.SaveChangesAsync();
        //        await _unitOfWork.CommitTransactionAsync();



        //        return request.RequestId;
        //    }
        //    catch (Exception ex) 
        //    {
        //        //await _requestRepository.RollbackAsync(transaction);
        //        await _unitOfWork.RollbackTransactionAsync();
        //        return ex.ToString();
        //    }
        //}
        /// <summary>
        /// Lấy ra mã đề xuất cuối cùng
        /// </summary>
        /// <returns></returns>
        //public async Task<RequestIdDTO> GetLastRequestIdService()
        //{
        //    var supplyRequestMaterialDatum = await _unitOfWork.RequestMaterialRepository.GetLastRequestIdRepository();
        //    var result = _mapper.Map<RequestIdDTO>(supplyRequestMaterialDatum);

        //    return result;
        //}
        public async Task<string> GetLastRequestIdService()
        {
            var supplyRequestMaterialDatum = await _unitOfWork.RequestMaterialRepository.GetLastRequestIdRepository();
            //var result = _mapper.Map<RequestIdDTO>(supplyRequestMaterialDatum);

            return supplyRequestMaterialDatum;
        }
        /// <summary>
        /// Lấy ra danh sách vật tư theo các điều kiện tìm kiếm
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedResult<RequestMaterialDTO>> GetMaterialAsyncService(RequestMaterialQuery query)
        {
            var materials = await _unitOfWork.RequestMaterialRepository.GetRequestRepository(query);
            var result = _mapper.Map<PagedResult<RequestMaterialDTO>>(materials);
            return result;
        }

        public async Task<PagedResult<FlatRequestMaterialDto>> FlatRequestMaterialService(RequestMaterialQuery query)
        {
            var result = await _unitOfWork.RequestMaterialRepository.FlatRequestMaterialRepository(query);
            return result;
            //throw new NotImplementedException();
        }
    }
}

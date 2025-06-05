using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.Application.DTOs.MaterialRequestDetails;
using VietausWebAPI.Core.Application.DTOs.MaterialRequestDetails.Query;
using VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail.ServiceContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail.Services
{
    public class MaterialRequestDetailService : IMaterialRequestDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MaterialRequestDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddRequestDetailServiceAsync(MaterialRequestDetailGetDTO MaterialRequestDetailGetDTO)
        {
            var result = _mapper.Map<IEnumerable<RequestDetailMaterialDatum>>(MaterialRequestDetailGetDTO);

            await _unitOfWork.MaterialRequestDetailRepository.AddRequetMaterialRepository(result);
        }

        //public async Task<IEnumerable<RequestDetailMaterialDatumPostDTO>> GetRequestDetailServieAsync(string requestId)
        //{
        //    var result = await _unitOfWork.MaterialRequestDetailRepository.GetRequestMaterialRepository(requestId);
        //    var resultMap = _mapper.Map<IEnumerable<RequestDetailMaterialDatumPostDTO>>(result);
        //    return resultMap;
        //}

        public async  Task<IEnumerable<MaterialRequestDetailPostDTO>> GetRequestDetailServieAsync(string requestId)
        {
            var result = await _unitOfWork.MaterialRequestDetailRepository.GetRequestMaterialRepository(requestId);
            var resultMap = _mapper.Map<IEnumerable<MaterialRequestDetailPostDTO>>(result);
            return resultMap;
        }

        public async Task<PagedResult<POMaterialRequestDetailPostDTO>> GetRequestMaterialStatusPayService(CreatePOQuery createPOQuery)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _unitOfWork.MaterialRequestDetailRepository.GetRequestMaterialStatusPayRepository(createPOQuery);

                var resultMap = _mapper.Map<PagedResult<POMaterialRequestDetailPostDTO>>(result);

                var summary = result.Items
                    .GroupBy(x => new { x.MaterialId, x.Material.Name })
                    .Select(g =>
                    {
                        var first = g.FirstOrDefault();
                        return new POMaterialRequestDetailPostDTO
                        {
                            materialId = g.Key.MaterialId,
                            externalId = first?.Material.ExternalId ?? string.Empty,
                            Unit = first?.Material.Unit ?? string.Empty,
                            MaterialGroupName = first?.Material.MaterialGroup?.MaterialGroupName ?? string.Empty,
                            Name = g.Key.Name,
                            //RequestedQuantity = g.Sum(x => x.RequestedQuantity.GetValueOrDefault() - x.PurchasedQuantity)

                            RequestedQuantity = g.Sum(x =>
                                    x.RequestedQuantity.GetValueOrDefault() - x.PurchasedQuantity.GetValueOrDefault())

                        };
                    })
                    .ToList();
                await _unitOfWork.CommitTransactionAsync();
                resultMap.Items = summary;
                return resultMap;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Error while getting request material status pay", ex);
            }
        }
    }

}

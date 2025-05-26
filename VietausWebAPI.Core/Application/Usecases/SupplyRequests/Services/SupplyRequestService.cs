using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.Application.DTOs.SupplyRequests;
using VietausWebAPI.Core.Application.Usecases.SupplyRequests.ServiceContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Usecases.SupplyRequests.Services
{
    public class SupplyRequestService : ISupplyRequestService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SupplyRequestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<string> CreateRequestMaterial(SupplyRequestData requestDTO)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var request = _mapper.Map<SupplyRequestsMaterialDatum>(requestDTO);

                await _unitOfWork.SupplyRequestRepository.CreateRequestAsync(request);

                var requestDetails = requestDTO.RequestDetails.Select(detailDTO =>
                {
                    var detail = _mapper.Map<RequestDetailMaterialDatum>(detailDTO);
                    detail.RequestId = request.RequestId;
                    return detail;
                }).ToList();

                await _unitOfWork.MaterialRequestDetailRepository.AddRequetMaterialRepository(requestDetails);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return request.RequestId;
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("An error occurred while creating the request", ex);
            }
        }
    }
}

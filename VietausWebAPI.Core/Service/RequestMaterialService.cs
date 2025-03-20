using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.Core.Service
{
    public class RequestMaterialService : IRequestMaterialService
    {
        private readonly IRequestMaterialRepository _requestRepository;
        private readonly IMapper _mapper;

        public RequestMaterialService (IRequestMaterialRepository repository, IMapper mapper)
        {
            _requestRepository = repository;
            _mapper = mapper;
        }

        public async Task<string> CreateRequestMaterial(RequestMaterialDTO requestDTO)
        {
            using var transaction = await _requestRepository.BeginTransactionAsync();
            try
            {
                var request = _mapper.Map<SupplyRequestsMaterialDatum>(requestDTO);

                await _requestRepository.CreateRequestAsync(request);

                var requestDetails = requestDTO.RequestDetails.Select(detailDTO =>
                {
                    var detail = _mapper.Map<RequestDetailMaterialDatum>(detailDTO);
                    detail.RequestId = request.RequestId;
                    return detail;
                }).ToList();


                Console.WriteLine("Saving request details...");
                await _requestRepository.AddRequestDetailMaterialAsync(requestDetails);
                Console.WriteLine("Request details saved.");

                Console.WriteLine("Committing transaction...");
                await _requestRepository.CommitTransactionAsync(transaction);
                Console.WriteLine("Transaction committed.");



                return request.RequestId;
            }
            catch (Exception ex) 
            {
                await _requestRepository.RollbackAsync(transaction);
                return ex.ToString();
            }
        }
        public async Task<RequestIdDTO> GetLastRequestIdService()
        {
            var supplyRequestMaterialDatum = await _requestRepository.GetLastRequestIdRepository();
            var result = _mapper.Map<RequestIdDTO>(supplyRequestMaterialDatum);

            return result;
        }

        public async Task<IEnumerable<RequestMaterialDTO>> GetMaterialAsyncService(RequestMaterialQuery query)
        {
            var materials = await _requestRepository.GetRequestRepository(query);
            var result = _mapper.Map<IEnumerable<RequestMaterialDTO>>(materials);
            return result;
        }
    }
}

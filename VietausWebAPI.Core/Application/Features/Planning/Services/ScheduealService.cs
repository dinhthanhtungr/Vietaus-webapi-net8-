using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Planning.DTOs.SchedualFeatures;
using VietausWebAPI.Core.Application.Features.Planning.Queries.SchedualFeatures;
using VietausWebAPI.Core.Application.Features.Planning.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Planning.Services
{
    public class ScheduealService : IScheduealService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScheduealService (IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //public async Task<PagedResult<SchedualMfgDTO>> GetSchedualPageAsync(SchedualQuery scheduealQuery)
        //{
        //    var repository = await _unitOfWork.ScheduealRepository.GetSchedualPageAsync(scheduealQuery);
        //    var pageResult = _mapper.Map<PagedResult<SchedualMfgDTO>>(repository);


        //    if (repository == null)
        //    {
        //        throw new InvalidOperationException("SchedualMfg is not registered in the unit of work.");
        //    }

        //    return pageResult;
        //}

        //public async Task<SchedualMfgDTO> GetScheduealByIdAsync(string externalId)
        //{
        //    var repository = await _unitOfWork.ScheduealRepository.GetScheduealByIdAsync(externalId);
        //    var result = _mapper.Map<SchedualMfgDTO>(repository);

        //    if(result == null)
        //    { 
        //        throw new InvalidOperationException($"Schedual with  ID {externalId} not found.");
        //    }

        //    return result;
        //}
    }
}

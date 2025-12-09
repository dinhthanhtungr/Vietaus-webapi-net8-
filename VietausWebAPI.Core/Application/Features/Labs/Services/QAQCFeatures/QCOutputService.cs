//using AutoMapper;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.QCOutputFeature;
//using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.QAQCFeatures;
//using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrdersPlanRepository;
//using VietausWebAPI.Core.Application.Shared.Models.PageModels;
//using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

//namespace VietausWebAPI.Core.Application.Features.Labs.Services.QAQCFeatures
//{
//    public class QCOutputService : IQCOutputService
//    {
//        private readonly IMapper _mapper;
//        private readonly IUnitOfWork _unitOfWork;

//        public QCOutputService (IMapper mapper, IUnitOfWork unitOfWork)
//        {
//            _mapper = mapper;
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<PagedResult<MfgProductDTOs>> GetPageAsync(MfgPOLQuery query)
//        {
//            var repository = await _unitOfWork.IMfgProductionOrdersPlanRepository.GetPagedAsync(query);
//            var pageResult = _mapper.Map<PagedResult<MfgProductDTOs>>(repository);

//            if (pageResult == null)
//            {
//                throw new InvalidOperationException("MfgProductOrder repository is not registered in the unit of work.");
//            }

//            return pageResult;
//        }

//        public async Task UpdateProductNameInPlansAsync(Guid productId, string newProductName)
//        {
//            await _unitOfWork.IMfgProductionOrdersPlanRepository.UpdateProductNameInPlansAsync(productId, newProductName);
//        }
//    }
//}

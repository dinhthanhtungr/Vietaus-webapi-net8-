using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductTestFeature;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductTestFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Labs.Services
{
    public class ProductTestService : IProductTestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductTestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProductTestDTO>> GetAllAsync(ProductTestQuery productTest)
        {
            var repository = await _unitOfWork.ProductTestRepository.GetAllAsync(productTest);
            var pageResult = _mapper.Map<PagedResult<ProductTestDTO>>(repository);

            //var mfgProduct
            if (repository == null)
            {
                throw new InvalidOperationException("ProductTest repository is not registered in the unit of work.");
            }
            return pageResult;
        }

        public async Task<ProductTestDTO> GetPagedByIdAsync(string ExternalId)
        {
            var repository = await _unitOfWork.ProductTestRepository.GetPagedByIdAsync(ExternalId);
            var result = _mapper.Map<ProductTestDTO>(repository);

            if (result == null)
            {
                throw new InvalidOperationException($"ProductTest with ID {ExternalId} not found.");
            }

            return result;
        }
    }
}

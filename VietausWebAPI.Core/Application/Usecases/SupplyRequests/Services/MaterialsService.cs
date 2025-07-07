using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.Application.DTOs.Materials;
using VietausWebAPI.Core.Application.DTOs.SupplyRequests;
using VietausWebAPI.Core.Application.Usecases.SupplyRequests.ServiceContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Usecases.SupplyRequests.Services
{
    public class MaterialsService :     IMaterialsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaterialsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<MaterialSearchResultDto>> materialSearcheServiceAsync(string name, Guid materialGroupId)
        {
            var materials = await _unitOfWork.MaterialsRepository.SearchByNameAsync(name, materialGroupId);
            var result =  _mapper.Map<List<MaterialSearchResultDto>>(materials);

            return result;
        }

        public async Task CreateMaterialAsync(List<MaterialsDTO> material)
        {
            await _unitOfWork.BeginTransactionAsync();
            var materialData = material.Select(m => new MaterialsMaterialDatum
            {
                Name = m.Name,
                Unit = m.Unit,
                CreateDate = m.CreateDate,
                EmployeeId = m.EmployeeId,
                MaterialGroupId = m.MaterialGroupId,
            }).ToList();

            await _unitOfWork.MaterialsRepository.CreateMaterialAsync(materialData);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
    }
}

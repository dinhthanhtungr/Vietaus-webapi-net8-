using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.Core.Service
{
    public class MaterialGroupsService : IMaterialGroupsService
    {
        private readonly IMaterialGroupsRepository _materialGroupsRepository;
        private readonly IMapper _mapper;
        public MaterialGroupsService(IMaterialGroupsRepository materialGroupsRepository, IMapper mapper)
        {
            _materialGroupsRepository = materialGroupsRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Thêm mới nhóm vật liệu
        /// </summary>
        /// <param name="materialGroupDTO"></param>
        /// <returns></returns>
        public async Task AddMaterialGroupServiceAsync(MaterialGroupsDTO materialGroupDTO)
        {
            var materialGroup = _mapper.Map<MaterialsMaterialGroupsDatum>(materialGroupDTO);
            await _materialGroupsRepository.AddMaterialGroupRepositoryAsync(materialGroup);
        }

        /// <summary>
        /// Lấy tất cả nhóm vật liệu
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MaterialGroupsDTO>> GetAllMaterialGroupsServiceAsync()
        {
            var materialGroups = await _materialGroupsRepository.GetAllMaterialGroupsRepositoryAsync();
            return _mapper.Map<IEnumerable<MaterialGroupsDTO>>(materialGroups);
        }
    }
}

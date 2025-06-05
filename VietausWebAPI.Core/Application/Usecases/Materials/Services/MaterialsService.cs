using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.Materials;
using VietausWebAPI.Core.Application.Usecases.Materials.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.Materials.ServiceContracts;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Usecases.Materials.Services
{
    public class MaterialsService : IMaterialsService
    {
        private readonly IMaterialsRepository _materialsRepository;
        public MaterialsService(IMaterialsRepository materialsRepository)
        {
            _materialsRepository = materialsRepository;
        }
        //public async Task CreateMaterialAsync(List<MaterialsDTO> material)
        //{
        //    var materialData = material.Select(m => new MaterialsMaterialDatum
        //    {
        //        Name = m.Name,
        //        Unit = m.Unit,
        //        CreateDate = m.CreateDate,
        //        EmployeeId = m.EmployeeId
        //    }).ToList();

        //    await _materialsRepository.CreateMaterialAsync(materialData);
        //}
    }
}

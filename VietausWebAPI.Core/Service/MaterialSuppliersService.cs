using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.Core.Service
{
    public class MaterialSuppliersService : IMaterialSupplierService
    {
        private readonly IMaterialSuppliersRepository _materialSuppliersRepository;
        private readonly IMapper _mapper;

        public MaterialSuppliersService(IMaterialSuppliersRepository materialSuppliersRepository, IMapper mapper)
        {
            _materialSuppliersRepository = materialSuppliersRepository;
            _mapper = mapper;
        }

        public async Task AddMaterialSuppliersServiceAsync(MaterialSuppliersDTO materialSuppliersDTO)
        {
            var materialSuppliers = _mapper.Map<MaterialSuppliersMaterialDatum>(materialSuppliersDTO);
            await _materialSuppliersRepository.AddMaterialSupplierRepositoryAsync(materialSuppliers);
        }

        public async Task<IEnumerable<MaterialSuppliersDTO>> GetAllMaterialSuppliersServiceAsync()
        {
            var materialSuppliers = await _materialSuppliersRepository.GetAllMaterialSuppliersRepositoryAsync();
            return _mapper.Map<IEnumerable<MaterialSuppliersDTO>>(materialSuppliers);
        }

    }
}

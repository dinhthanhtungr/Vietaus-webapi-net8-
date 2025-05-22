using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.Core.Service
{
    public class MaterialSuppliersService : IMaterialSupplierService
    {
        private readonly IMaterialSuppliersRepository _materialSuppliersRepository;
        private readonly IMapper _mapper;
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="materialSuppliersRepository"></param>
        /// <param name="mapper"></param>
        public MaterialSuppliersService(IMaterialSuppliersRepository materialSuppliersRepository, IMapper mapper)
        {
            _materialSuppliersRepository = materialSuppliersRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Thêm mới nhà cung cấp vật liệu
        /// </summary>
        /// <param name="materialSuppliersDTO"></param>
        /// <returns></returns>
        public async Task AddMaterialSuppliersServiceAsync(MaterialSuppliersDTO materialSuppliersDTO)
        {
            var materialSuppliers = _mapper.Map<MaterialsSuppliersMaterialDatum>(materialSuppliersDTO);
            await _materialSuppliersRepository.AddMaterialSupplierRepositoryAsync(materialSuppliers);
        }
        /// <summary>
        /// Lấy tất cả nhà cung cấp vật liệu
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MaterialSuppliersDTO>> GetAllMaterialSuppliersServiceAsync()
        {
            var materialSuppliers = await _materialSuppliersRepository.GetAllMaterialSuppliersRepositoryAsync();
            return _mapper.Map<IEnumerable<MaterialSuppliersDTO>>(materialSuppliers);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.Application.DTOs.Suppliers;
using VietausWebAPI.Core.Application.Usecases.Suppliers.ServiceContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Usecases.Suppliers.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SupplierInformationsDTO>> GetAllSupplierName()
        {
            var suppliers = await _unitOfWork.SupplierRepository.GetAllNameSupplierRepository();
            var result = _mapper.Map<List<SupplierInformationsDTO>>(suppliers);
            return result;
        }

        public async Task<IEnumerable<SupplierAddressDTO>> GetSupplierAddress(Guid supplierId)
        {
            var supplierAddresses = await _unitOfWork.SupplierRepository.GetSupplierAddress(supplierId);
            var result = _mapper.Map<IEnumerable<SupplierAddressDTO>>(supplierAddresses);
            return result;
        }
    }
}

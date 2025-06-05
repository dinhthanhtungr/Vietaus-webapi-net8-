using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.Application.DTOs.PurchaseOrders;
using VietausWebAPI.Core.Application.Usecases.PurchaseOrders.ServiceContracts;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Usecases.PurchaseOrders.Services
{
    public class PurchaseOrderDetailsService : IPurchaseOrderDetailsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PurchaseOrderDetailsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ShowPurchaseOrderDetailsDTO>> ShowPurchaseOrderDetailServiceAsync(Guid POID)
        {
            try
            {
                var purchaseOrderDetails = await _unitOfWork.PurchaseOrderDetailsRepository.PostPurchaseOrderDetail(POID);
                if (purchaseOrderDetails == null)
                {
                    throw new Exception("Purchase order details not found.");
                }
                var purchaseOrderDetailsDTO = _mapper.Map<IEnumerable<ShowPurchaseOrderDetailsDTO>>(purchaseOrderDetails);
                return purchaseOrderDetailsDTO;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception($"An error occurred while retrieving purchase order details: {ex.Message}");
            }
        }
    }
}

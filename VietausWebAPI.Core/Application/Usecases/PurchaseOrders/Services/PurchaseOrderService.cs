using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.Application.DTOs.MaterialRequestDetails;
using VietausWebAPI.Core.Application.DTOs.PurchaseOrders;
using VietausWebAPI.Core.Application.DTOs.PurchaseOrders.Query;
using VietausWebAPI.Core.Application.Usecases.PurchaseOrders.ServiceContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Usecases.PurchaseOrders.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PurchaseOrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreatePurchaseOrderAsync(CreatePurchaseOrderDTO purchaseOrder)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // MAP PO
                var poEntity = _mapper.Map<PurchaseOrdersMaterialDatum>(purchaseOrder);
                
                await _unitOfWork.PurchaseOrdersRepository.CreatePurchaseOrdersRepositoryAsync(poEntity);
                await _unitOfWork.SaveChangesAsync();

                // MAP CHI TIẾT PO
                var poDetails = purchaseOrder.PurchaseOrderDetails.Select(detailDTO =>
                {
                    var detail = _mapper.Map<PurchaseOrderDetailsMaterialDatum>(detailDTO);
                    detail.Poid = poEntity.Poid;
                    return detail;
                }).ToList();

                await _unitOfWork.PurchaseOrderDetailsRepository.GetPurchaseOrderDetail(poDetails);
                await _unitOfWork.SaveChangesAsync();

                // Phân bổ số lượng mua vào các đề xuất
                foreach (var group in poDetails.GroupBy(x => x.MaterialId))
                {
                    int totalQuantity = group.Sum(x => x.Quantity); // tổng số lượng bạn mua cho từng loại vật tư

                    if (group.Key.HasValue)
                    {
                        // Phân bổ số lượng này cho các request chưa đủ của vật tư đó
                        var allocations = await AllocateToRequests(group.Key.Value, totalQuantity);

                        foreach (var alloc in allocations)
                        {
                            // Cập nhật số lượng đã mua cho các dòng chi tiết của request
                            await _unitOfWork.MaterialRequestDetailRepository
                                  .UpdatePurchasedQuantityAsync(alloc.DetailID, alloc.QuantityAllocated);
                        }
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();


            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Error creating purchase order", ex);
            }

        }

        /// <summary>
        /// Phân bổ số lượng mua vào các yêu cầu mở (requests) dựa trên ID vật liệu và số lượng cần phân bổ.
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="quantityToAllocate"></param>
        /// <returns></returns>
        private async Task<List<AllocationResult>> AllocateToRequests(Guid materialId, int quantityToAllocate)
        {
            // Lấy danh sách các dòng request còn thiếu vật tư
            var requestDetails = await _unitOfWork.MaterialRequestDetailRepository
                                .GetOpenRequestsByMaterialIdRepository(materialId); // lấy những dòng còn thiếu

            var result = new List<AllocationResult>();

            // Duyệt theo ngày yêu cầu tăng dần để phân bổ theo thứ tự ưu tiên
            foreach (var r in requestDetails.OrderBy(x => x.Request.RequestDate))
            {
                int? remaining = r.RequestedQuantity - r.PurchasedQuantity;

                if (remaining <= 0)
                    continue;

                // Phân bổ số lượng tối đa là min(đang còn lại, số lượng cần phân bổ)
                int allocate = Math.Min(quantityToAllocate, remaining.GetValueOrDefault());

                result.Add(new AllocationResult
                {
                    DetailID = r.DetailId,
                    QuantityAllocated = allocate
                });

                quantityToAllocate -= allocate;

                if (quantityToAllocate <= 0)
                    break;
            }

            return result;
        }


        public async Task<string> GeneratePONumberAsync()
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var currentYear = DateTime.Now.Year;
                var seq = await _unitOfWork.PurchaseOrdersRepository.GetLastPurchaseOrderIdRepositoryAsync(currentYear);

                if (seq == null)
                {
                    seq = new SequencePoMaterialDatum
                    {
                        Year = currentYear,
                        LastNumber = 1
                    };

                    await _unitOfWork.PurchaseOrdersRepository.AddNewNumber(seq);
                }

                else
                {
                    seq.LastNumber++;
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return $"PO-{currentYear}-{seq.LastNumber:D5}";
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Error generating PO number", ex);
            }
        }

        public  async Task<PagedResult<PurchaseOrderDTO>> GetPurchaseOrdersServiceAsync(GetPOQuery query)
        {
            var POValue = await _unitOfWork.PurchaseOrdersRepository.GetPurchaseOrdersRepositoryAsync(query);
            var result = _mapper.Map<PagedResult<PurchaseOrderDTO>>(POValue);

            return result;
        }

        public async Task DeletePurchaseOrderAsync(Guid poid)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {

                //1 Lấy PurchaseOrder theo poid
                var purchaseOrder = await _unitOfWork.PurchaseOrdersRepository.GetByIdAsync(poid);
                if (purchaseOrder == null)
                {
                    throw new Exception("Purchase order not found for the given PO ID.");
                }

                //2 Lấy PurchaseOrderDetails theo poid
                var purchaseOrderDetails = await _unitOfWork.PurchaseOrderDetailsRepository.GetByPOIDAsync(poid);

                //3 Rollback số lượng đã mua trong các dòng chi tiết của request
                foreach (var detail in purchaseOrderDetails)
                {
                    await _unitOfWork.MaterialRequestDetailRepository
                        .RollbackPurchasedQuantityAsync(detail.MaterialId.GetValueOrDefault(), detail.Quantity);
                }

                //4 Xóa PurchaseOrderDetail
                await _unitOfWork.PurchaseOrderDetailsRepository.DeleteByPOIDAsync(poid);

                //5 Xóa PurchaseOrder
                await _unitOfWork.PurchaseOrdersRepository.DeleteAsync(purchaseOrder);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Error deleting purchase order", ex);
            }
        }

        public async Task SuccessPunchaseOrderAsync(Guid guid)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.PurchaseOrdersRepository.SuccessPunchaseOrderRepository(guid);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Error updating purchase order status", ex);
            }
        }
    }
}

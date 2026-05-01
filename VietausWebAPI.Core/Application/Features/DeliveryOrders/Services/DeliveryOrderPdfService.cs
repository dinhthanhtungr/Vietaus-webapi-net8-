using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Helpers;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Labs.Helpers.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.EventLogDtos;
using VietausWebAPI.Core.Application.Features.TimelineFeature.ServiceContracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.Services;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;
using VietausWebAPI.Core.Domain.Enums.Logs;
using VietausWebAPI.Core.Domain.Enums.Merchadises;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.Services
{
    public class DeliveryOrderPdfService : IDeliveryOrderPdfService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDeliveryOrderPdfRenderHelper _pdfRenderHelper;
        private readonly ITimelineService _timeLineService;
        private readonly ICurrentUser _currentUser;

        private readonly IWarehouseReservationService _warehouseReservationService;
        private readonly IProductFormulaRuleHelper _productFormulaRuleHelper;

        public DeliveryOrderPdfService(
            IUnitOfWork unitOfWork,
            IDeliveryOrderPdfRenderHelper pdfRenderHelper,
            ITimelineService timelineService,
            ICurrentUser currentUser,
            IWarehouseReservationService warehouseReservationService,
            IProductFormulaRuleHelper productFormulaRuleHelper)
        {
            _unitOfWork = unitOfWork;
            _pdfRenderHelper = pdfRenderHelper;
            _timeLineService = timelineService;
            _currentUser = currentUser;
            _warehouseReservationService = warehouseReservationService;
            _productFormulaRuleHelper = productFormulaRuleHelper;
        }

        public async Task<byte[]> GenerateAsync(Guid deliveryOrderId, CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;
            using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var vm = await _unitOfWork.DeliveryOrderRepository.Query(track: false)
                .Where(x => x.Id == deliveryOrderId)
                .Select(x => new PdfPrinterDeliveryOrder
                {
                    Id = x.Id,
                    ExternalId = x.ExternalId,
                    CustomerExternalIdSnapShot = x.CustomerExternalIdSnapShot,
                    DeliveryAddress = x.DeliveryAddress,

                    CustomerAddress = x.Customer.Addresses.Select(a => a.AddressLine).FirstOrDefault(),
                    CustomerName = x.Customer.CustomerName,

                    TaxNumber = x.TaxNumber,
                    PhoneSnapshot = x.PhoneSnapshot,
                    Receiver = x.Receiver,
                    PaymentType = x.PaymentType,
                    PaymentDeadline = x.PaymentDeadline,
                    Note = x.Note,

                    HasPrinted = x.HasPrinted,
                    CreateBy = userId,
                    CreateDate = x.CreatedDate,

                    Details = x.Details.Select(d => new PdfPrinterDeliveryOrderDetail
                    {
                        ProductId = d.ProductId,
                        ProductCode = d.ProductExternalIdSnapShot ?? string.Empty,
                        ProductName = d.ProductNameSnapShot ?? string.Empty,
                        LotNumber = d.LotNoList ?? string.Empty,
                        WeightKg = d.Quantity,
                        BagNumber = d.NumOfBags,
                        StockStatus = d.MerchandiseOrderDetail != null ? d.MerchandiseOrderDetail.Status : string.Empty,
                        PONoNumber = d.PONo ?? string.Empty
                    }).ToList(),


                    DeliveryOrderDetails = x.Details.Select(d => new GetDeliveryOrderDetail
                    {
                        // Map properties according to GetDeliveryOrderDetail signature
                        ProductId = d.ProductId,
                        ProductExternalIdSnapShot = d.ProductExternalIdSnapShot,
                        ProductNameSnapShot = d.ProductNameSnapShot,
                        LotNoList = d.LotNoList,
                        PONo = d.PONo,
                        Quantity = d.Quantity,
                        NumOfBags = d.NumOfBags
                    }).ToList(),

                    Deliverers = x.Deliverers.Select(del => new GetDeliverer
                    {
                        Id = del.Id,
                        Name = del.DelivererInfor != null ? del.DelivererInfor.Name : null,
                    }).ToList(),

                    // Gom chuỗi ExtternalId các PO đã link
                    MerchandiseOrderExternalIdList = string.Join(", ",
                    x.DeliveryOrderPOs
                        .Where(dop => dop.MerchandiseOrder != null)
                        .Select(dop => dop.MerchandiseOrder!.ExternalId)
                        .Distinct()
                    ),
                })
                .FirstOrDefaultAsync(ct);


                if (vm == null)
                    throw new Exception("Delivery Order not found.");


                var productIds = vm.Details
                    .Where(x => x.ProductId.HasValue)
                    .Select(x => x.ProductId!.Value);

                var singleMaterialProductIds =
                    await _productFormulaRuleHelper.GetProductIdsWithSingleMaterialFormulaAsync(productIds, ct);

                foreach (var row in vm.Details)
                {
                    row.IsSingleMaterialFormula =
                        row.ProductId.HasValue &&
                        singleMaterialProductIds.Contains(row.ProductId.Value);
                }

                // ====== (B4) Cập nhật RealQuantity cho MerchandiseOrderDetail ======
                // Gom tổng số lượng DO theo MerchandiseOrderDetailId (chỉ lấy dòng có map vào PO detail, bỏ attach)

                var moDetailIds = await _unitOfWork.DeliveryOrderDetailRepository.Query(track: false)
                    .Where(dod => dod.DeliveryOrderId == deliveryOrderId
                                  && dod.MerchandiseOrderDetailId != null)
                    .Select(dod => dod.MerchandiseOrderDetailId!.Value)
                    .Distinct()
                    .ToListAsync(ct);

                var poIds = await _unitOfWork.MerchandiseOrderRepository.QueryDetail(track: false)
                    .Where(d => moDetailIds.Contains(d.MerchandiseOrderDetailId))
                    .Select(d => d.MerchandiseOrderId)
                    .Distinct()
                    .ToListAsync(ct);

                if (!vm.HasPrinted)
                {
                    var sumByMoDetailId = await _unitOfWork.DeliveryOrderDetailRepository.Query(track: false)
                        .Where(dod => dod.DeliveryOrderId == deliveryOrderId
                                      && dod.MerchandiseOrderDetailId != null)
                        .GroupBy(dod => dod.MerchandiseOrderDetailId!.Value)
                        .Select(g => new
                        {
                            MerchandiseOrderDetailId = g.Key,
                            TotalQuantity = g.Sum(dod => dod.Quantity)
                        })
                        .ToDictionaryAsync(x => x.MerchandiseOrderDetailId, x => x.TotalQuantity, ct);

                    if (sumByMoDetailId.Count > 0)
                    {
                        var detailsToUpdate = await _unitOfWork.MerchandiseOrderRepository.QueryDetail(track: true)
                            .Where(d => sumByMoDetailId.Keys.Contains(d.MerchandiseOrderDetailId))
                            .ToListAsync(ct);

                        foreach (var detail in detailsToUpdate)
                        {
                            var add = sumByMoDetailId[detail.MerchandiseOrderDetailId];
                            var current = detail.RealQuantity ?? 0m;

                            detail.RealQuantity = current + add;
                            detail.Status = detail.ExpectedQuantity > 0m &&
                                            (detail.RealQuantity ?? 0m) >= detail.ExpectedQuantity
                                ? MerchadiseStatus.Delivered.ToString()
                                : MerchadiseStatus.Delivering.ToString();

                        }
                    }

                    await _unitOfWork.DeliveryOrderRepository.Query(track: false)
                        .Where(x => x.Id == deliveryOrderId && x.HasPrinted == false)
                        .ExecuteUpdateAsync(s => s.SetProperty(p => p.HasPrinted, _ => true), ct);

                    await _unitOfWork.SaveChangesAsync();
                }


                if (poIds.Count > 0)
                {
                    var poDetails = await _unitOfWork.MerchandiseOrderRepository.QueryDetail(track: false)
                        .Where(d => poIds.Contains(d.MerchandiseOrderId))
                        .Select(d => new
                        {
                            d.MerchandiseOrderId,
                            d.ExpectedQuantity,
                            d.RealQuantity
                        })
                        .ToListAsync(ct);

                    var deliveredPoIds = poDetails
                        .GroupBy(x => x.MerchandiseOrderId)
                        .Where(g => g.All(x =>
                            x.ExpectedQuantity > 0m &&
                            (x.RealQuantity ?? 0m) >= x.ExpectedQuantity))
                        .Select(g => g.Key)
                        .ToList();

                    var statusDelivered = MerchadiseStatus.Delivered.ToString();
                    var statusDelivering = MerchadiseStatus.Delivering.ToString();

                    if (deliveredPoIds.Count > 0)
                    {
                        await _unitOfWork.MerchandiseOrderRepository.Query(track: false)
                            .Where(po => deliveredPoIds.Contains(po.MerchandiseOrderId)
                                         && po.Status != statusDelivered)
                            .ExecuteUpdateAsync(s => s
                                .SetProperty(p => p.Status, _ => statusDelivered)
                                .SetProperty(p => p.UpdatedBy, _ => vm.CreateBy)
                                .SetProperty(p => p.UpdatedDate, _ => now), ct);
                    }

                    var deliveringPoIds = poIds.Except(deliveredPoIds).ToList();

                    if (deliveringPoIds.Count > 0)
                    {
                        await _unitOfWork.MerchandiseOrderRepository.Query(track: false)
                            .Where(po => deliveringPoIds.Contains(po.MerchandiseOrderId)
                                         && po.Status != statusDelivered
                                         && po.Status != statusDelivering)
                            .ExecuteUpdateAsync(s => s
                                .SetProperty(p => p.Status, _ => statusDelivering)
                                .SetProperty(p => p.UpdatedBy, _ => vm.CreateBy)
                                .SetProperty(p => p.UpdatedDate, _ => now), ct);
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync(ct);

                return _pdfRenderHelper.Render(vm, false);
                //return _pdfRenderHelper.RenderTemplate();
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                throw new Exception($"Lỗi khi lấy danh sách đơn giao hàng: {ex.Message}", ex);
            }


        }
    }
}



using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Helpers;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs.PdfPrinter;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.Helpers;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.ServiceContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.Services
{
    public class PurchaseOrderPdfService : IPurchaseOrderPdfService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPurchaseOrderPdfRenderHelper _pdfRenderHelper;

        public PurchaseOrderPdfService(IUnitOfWork unitOfWork, IPurchaseOrderPdfRenderHelper pdfRenderHelper)
        {
            _unitOfWork = unitOfWork;
            _pdfRenderHelper = pdfRenderHelper;
        }

        public async Task<byte[]> GenerateAsync(Guid PurchaseOrderId, CancellationToken ct = default)
        {
            var now = DateTime.Now;

            using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {



                var vm = await _unitOfWork.PurchaseOrderRepository.Query(track: false)
                        .Where(x => x.PurchaseOrderId == PurchaseOrderId)
                        .Select(x => new PdfPrinterPurchaseOrder
                        {
                            PurchaseOrderSnapshotId = x.PurchaseOrderSnapshotId ?? Guid.Empty,
                            PurchaseOrderExternalIdSnapshot = x.ExternalId ?? string.Empty,

                            // Snapshot thông tin người tạo đơn
                            EmployeeExternalIdSnapshot = x.PurchaseOrderSnapshot != null
                                                             ? x.PurchaseOrderSnapshot.EmployeeExternalIdSnapshot
                                                             : x.CreatedByNavigation!.ExternalId,
                            EmployeeFullNameSnapshot = x.PurchaseOrderSnapshot != null
                                                             ? x.PurchaseOrderSnapshot.EmployeeFullNameSnapshot
                                                             : x.CreatedByNavigation!.FullName,
                            PhoneNumberSnapshot = x.PurchaseOrderSnapshot != null
                                                             ? x.PurchaseOrderSnapshot.PhoneNumberSnapshot
                                                             : x.CreatedByNavigation!.PhoneNumber,
                            EmailSnapshot = x.PurchaseOrderSnapshot != null
                                                             ? x.PurchaseOrderSnapshot.EmailSnapshot
                                                             : x.CreatedByNavigation!.Email,

                            // Snapshot nhà cung cấp
                            SupplierExternalIdSnapshot = x.PurchaseOrderSnapshot != null
                                                             ? x.PurchaseOrderSnapshot.SupplierExternalIdSnapshot
                                                             : x.Supplier!.ExternalId,
                            SupplierNameSnapshot = x.PurchaseOrderSnapshot != null
                                                             ? x.PurchaseOrderSnapshot.SupplierNameSnapshot
                                                             : x.Supplier!.SupplierName,
                            SupplierContactSnapshot = x.PurchaseOrderSnapshot != null
                                                             ? x.PurchaseOrderSnapshot.SupplierContactSnapshot
                                                             : x.Supplier!.SupplierContacts.Where(c => c.IsPrimary == true).Select(c => c.LastName).FirstOrDefault(),
                            SupplierPhoneNumberSnapshot = x.PurchaseOrderSnapshot != null
                                                             ? x.PurchaseOrderSnapshot.SupplierPhoneNumberSnapshot
                                                             : x.Supplier!.Phone,
                            SupplierAddressSnapshot = x.PurchaseOrderSnapshot != null
                                                             ? x.PurchaseOrderSnapshot.SupplierAddressSnapshot
                                                             : x.Supplier!.SupplierAddresses.Where(c => c.IsPrimary == true).Select(c => c.AddressLine).FirstOrDefault(),

                            Note = x.Comment,

                            // Thông tin tổng quan đơn
                            TotalPrice = x.PurchaseOrderSnapshot != null
                                                             ? x.PurchaseOrderSnapshot.TotalPrice
                                                             : x.PurchaseOrderDetails.Sum(d => d.TotalPriceAgreed) ?? 0,
                            DeliveryAddress = x.PurchaseOrderSnapshot != null ? x.PurchaseOrderSnapshot.DeliveryAddress : null,

                            PaymentTypes = x.PurchaseOrderSnapshot != null
                                                             ? x.PurchaseOrderSnapshot.PaymentTypes
                                                             : null,
                            Vat = x.PurchaseOrderSnapshot != null
                                                             ? x.PurchaseOrderSnapshot.Vat
                                                             : 0,

                            CreatedDate = x.CreateDate,
                            RequestDeliveryDate = x.RequestDeliveryDate,

                            // Chi tiết
                            Details = x.PurchaseOrderDetails
                                .Where(d => d.PurchaseOrderId == x.PurchaseOrderId)
                                .Select(d => new PdfPrinterPurchaseOrderDetail
                                {
                                    MaterialId = d.MaterialId,
                                    MaterialExternalIDSnapshot = d.MaterialExternalIDSnapshot,
                                    MaterialNameSnapshot = d.MaterialNameSnapshot,

                                    Package = d.Package,

                                    BaseCostSnapshot = d.BaseCostSnapshot,
                                    BaseDateSnapshot = d.BaseDateSnapshot,

                                    RequestQuantity = d.RequestQuantity,
                                    UnitPriceAgreed = d.UnitPriceAgreed,

                                    TotalUnitPriceAgreed = d.TotalPriceAgreed
                                })
                                .ToList()
                        })
                        .FirstOrDefaultAsync(ct);

                if (vm == null)
                    throw new Exception("Delivery Order not found.");

                await tx.CommitAsync();
                return _pdfRenderHelper.Render(vm);
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
    
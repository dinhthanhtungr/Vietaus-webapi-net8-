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
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;
using VietausWebAPI.Core.Domain.Enums.Category;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.Services
{
    public class PurchaseOrderPdfService : IPurchaseOrderPdfService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPurchaseOrderPdfRenderHelper _pdfRenderHelper;
        private readonly ICurrentUser _currentUserService;
        private readonly IExternalIdService _idService;

        public PurchaseOrderPdfService(IUnitOfWork unitOfWork, IPurchaseOrderPdfRenderHelper pdfRenderHelper, ICurrentUser currentUserService, IExternalIdService idService)
        {
            _unitOfWork = unitOfWork;
            _pdfRenderHelper = pdfRenderHelper;
            _currentUserService = currentUserService;   
            _idService = idService;
        }

        public async Task<byte[]> GenerateAsync(Guid PurchaseOrderId, CancellationToken ct = default)
        {
            var now = DateTime.Now;

            using var tx = await _unitOfWork.BeginTransactionAsync();
            try
            {
                // ✅ Tạo WarehouseRequest lần đầu (nếu chưa có)
                await EnsureWarehouseRequestCreatedFirstTimeAsync(PurchaseOrderId, now, ct);

                // --- phần build VM như bạn đang làm ---
                var vm = await _unitOfWork.PurchaseOrderRepository.Query(track: false)
                    .Where(x => x.PurchaseOrderId == PurchaseOrderId)
                    .Select(x => new PdfPrinterPurchaseOrder
                    {
                        PurchaseOrderSnapshotId = x.PurchaseOrderSnapshotId ?? Guid.Empty,
                        PurchaseOrderExternalIdSnapshot = x.ExternalId ?? string.Empty,
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

                        TotalPrice = x.PurchaseOrderSnapshot != null
                            ? x.PurchaseOrderSnapshot.TotalPrice
                            : x.PurchaseOrderDetails.Sum(d => d.TotalPriceAgreed) ?? 0,

                        DeliveryAddress = x.PurchaseOrderSnapshot != null ? x.PurchaseOrderSnapshot.DeliveryAddress : null,
                        PaymentTypes = x.PurchaseOrderSnapshot != null ? x.PurchaseOrderSnapshot.PaymentTypes : null,
                        Vat = x.PurchaseOrderSnapshot != null ? x.PurchaseOrderSnapshot.Vat : 0,

                        CreatedDate = x.CreateDate,
                        RequestDeliveryDate = x.RequestDeliveryDate,

                        Details = x.PurchaseOrderDetails
                            .Where(d => d.PurchaseOrderId == x.PurchaseOrderId)
                            .OrderBy(d => d.LineNo)
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
                    throw new Exception("Purchase Order not found.");

                await tx.CommitAsync();
                return _pdfRenderHelper.Render(vm, false);
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        private async Task EnsureWarehouseRequestCreatedFirstTimeAsync(Guid purchaseOrderId, DateTime now, CancellationToken ct)
        {
            var user = _currentUserService.EmployeeId;

            var po = await _unitOfWork.PurchaseOrderRepository
                .Query(track: true)
                .Include(x => x.PurchaseOrderDetails)
                    .ThenInclude(d => d.Material)
                .FirstOrDefaultAsync(x => x.PurchaseOrderId == purchaseOrderId, ct);

            if (po == null) throw new Exception("Purchase Order not found (for warehouse request).");
            if (po.CompanyId == null) throw new Exception("Purchase Order missing CompanyId.");
            if (po.CreatedBy == null) throw new Exception("Purchase Order missing CreatedBy.");

            var existed = await _unitOfWork.WarehouseRequestRepository
                .Query(track: false)
                .AnyAsync(x => x.IsActive
                            && x.ReqType == WareHouseRequestType.ImportOther
                            && x.codeFromRequest == po.ExternalId, ct);

            if (existed) return;

            // 3) Tạo header
            var materialCategoryId = Guid.Parse("3bed94ed-da05-4e5f-ac04-c7647aaa63d6");

            var hasMaterialCategory = po.PurchaseOrderDetails
                .Where(d => d.IsActive)
                .Any(d => d.Material != null && d.Material.CategoryId == materialCategoryId);

            var req = new WarehouseRequest
            {
                RequestCode = await _idService.NextAsync(DocumentPrefix.PRQ.ToString()),
                ReqStatus = WarehouseRequestStatus.Pending,
                RequestName = $"Nhập kho NCC - PO {(po.ExternalId ?? po.PurchaseOrderId.ToString())}",
                IsActive = true,

                ReqType = hasMaterialCategory
                    ? WareHouseRequestType.ImportMaterial
                    : WareHouseRequestType.ImportOther,

                codeFromRequest = po.ExternalId ?? "Error no externalId",

                CompanyId = po.CompanyId.Value,
                CreatedBy = user,
                CreatedDate = now,
                UpdatedBy = user,
                UpdatedDate = now,

                WarehouseRequestDetails = po.PurchaseOrderDetails
                    .Where(d => d.IsActive)
                    .OrderBy(d => d.LineNo)
                    .Select(d => new WarehouseRequestDetail
                    {
                        ProductCode = d.MaterialExternalIDSnapshot ?? string.Empty,
                        ProductName = d.MaterialNameSnapshot ?? string.Empty,
                        WeightKg = d.RequestQuantity ?? 0m,
                        BagNumber = TryParseBagNumber(d.Package),
                        StockStatus = VoucherDetailType.Waiter.ToString(),
                        LotNumber = null,
                        IsActive = true
                    })
                    .ToList()
            };

            // 4) Save
            await _unitOfWork.WarehouseRequestRepository.AddAsync(req, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        private static int TryParseBagNumber(string? package)
        {
            // Nếu package lưu kiểu "10 bags" hoặc "10" thì parse được
            if (string.IsNullOrWhiteSpace(package)) return 0;

            // Lấy số đầu tiên trong chuỗi
            var digits = new string(package.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out var n) ? n : 0;
        }
    }

}

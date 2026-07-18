using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.QuotationDTOs;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Enums.Category;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.QuotationFeatures
{
    public sealed partial class QuotationService
    {
        public async Task<OperationResult<Guid>> CreateAsync(CreateQuotationRequest request, CancellationToken ct = default)
        {
            if (!_currentUser.IsAuthenticated)
                return OperationResult<Guid>.Fail("Bạn chưa đăng nhập.");

            if (request.CustomerId == Guid.Empty)
                return OperationResult<Guid>.Fail("CustomerId không hợp lệ.");

            if (request.Lines == null || request.Lines.Count == 0)
                return OperationResult<Guid>.Fail("Báo giá phải có ít nhất một dòng sản phẩm.");

            if (request.ExchangeRate <= 0)
                return OperationResult<Guid>.Fail("Tỷ giá phải lớn hơn 0.");

            var viewer = await _visibilityHelper.BuildViewerScopeAsync(ct);
            var customer = await _visibilityHelper
                .ApplyCustomer(_unitOfWork.CustomerRepository.Query(), viewer)
                .Include(x => x.Contacts)
                .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId && x.CompanyId == viewer.CompanyId, ct);

            if (customer == null)
                return OperationResult<Guid>.Fail("Không tìm thấy khách hàng hoặc bạn không có quyền tạo báo giá cho khách hàng này.");

            var productIds = request.Lines
                .Select(x => x.ProductId)
                .Where(x => x != Guid.Empty)
                .Distinct()
                .ToList();

            if (productIds.Count != request.Lines.Count)
                return OperationResult<Guid>.Fail("Có dòng báo giá thiếu ProductId hoặc ProductId bị trùng dữ liệu không hợp lệ.");

            var products = await _productRepository.Query(track: false)
                .Where(x => productIds.Contains(x.ProductId) && x.IsActive)
                .Select(x => new
                {
                    x.ProductId,
                    x.Code,
                    x.ColourCode,
                    x.Name
                })
                .ToDictionaryAsync(x => x.ProductId, ct);

            if (products.Count != productIds.Count)
                return OperationResult<Guid>.Fail("Có sản phẩm không tồn tại hoặc đã ngưng hoạt động.");

            var now = DateTime.Now;
            var quotationId = Guid.CreateVersion7();
            var lines = new List<QuotationLine>();
            decimal subTotal = 0m;

            foreach (var line in request.Lines.Select((value, index) => new { value, index }))
            {
                if (line.value.Quantity <= 0)
                    return OperationResult<Guid>.Fail("Số lượng báo giá phải lớn hơn 0.");

                if (line.value.UnitPrice < 0)
                    return OperationResult<Guid>.Fail("Đơn giá không được âm.");

                var product = products[line.value.ProductId];
                var lineTotal = CalculateLineTotal(line.value);
                subTotal += lineTotal;

                lines.Add(new QuotationLine
                {
                    QuotationLineId = Guid.CreateVersion7(),
                    QuotationId = quotationId,
                    ProductId = line.value.ProductId,
                    ProductExternalIdSnapshot = BuildProductCode(line.value.ProductExternalIdSnapshot, product.ColourCode, product.Code),
                    ProductNameSnapshot = string.IsNullOrWhiteSpace(line.value.ProductNameSnapshot)
                        ? product.Name ?? string.Empty
                        : line.value.ProductNameSnapshot.Trim(),
                    Quantity = line.value.Quantity,
                    Unit = string.IsNullOrWhiteSpace(line.value.Unit) ? "kg" : line.value.Unit.Trim(),
                    UnitPrice = line.value.UnitPrice,
                    DiscountPercent = line.value.DiscountPercent,
                    TaxPercent = line.value.TaxPercent,
                    LineTotal = lineTotal,
                    Note = BuildLineNotePayload(line.value.Note, line.value.PriceTiers),
                    SortOrder = line.value.SortOrder > 0 ? line.value.SortOrder : line.index + 1
                });
            }

            var totalAmount = subTotal - request.DiscountAmount + request.TaxAmount;
            if (totalAmount < 0) totalAmount = 0m;

            var quotation = new Quotation
            {
                QuotationId = quotationId,
                ExternalId = await _externalIdService.NextAsync(DocumentPrefix.BBG.ToString(), ct),
                CustomerId = request.CustomerId,
                ContactId = request.ContactId,
                ContactName = ResolveContactName(customer, request.ContactId, request.ContactName),
                CompanyId = viewer.CompanyId,
                SaleEmployeeId = viewer.EmployeeId,
                Status = QuotationStatus.Draft,
                Currency = string.IsNullOrWhiteSpace(request.Currency) ? "VND" : request.Currency.Trim().ToUpperInvariant(),
                ExchangeRate = request.ExchangeRate,
                SubTotal = subTotal,
                DiscountAmount = request.DiscountAmount,
                TaxAmount = request.TaxAmount,
                TotalAmount = totalAmount,
                QuotationDate = request.QuotationDate?.Date ?? now.Date,
                ValidUntil = request.ValidUntil,
                PaymentTerms = request.PaymentTerms,
                DeliveryTerms = request.DeliveryTerms,
                Note = request.Note,
                Version = 1,
                IsActive = true,
                CreatedBy = viewer.EmployeeId,
                CreatedDate = now,
                UpdatedBy = viewer.EmployeeId,
                UpdatedDate = now
            };

            var history = new QuotationStatusHistory
            {
                Id = Guid.CreateVersion7(),
                QuotationId = quotationId,
                FromStatus = QuotationStatus.Draft,
                ToStatus = QuotationStatus.Draft,
                Note = "Tạo yêu cầu báo giá.",
                ChangedBy = viewer.EmployeeId,
                ChangedDate = now
            };

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _quotationRepository.AddAsync(quotation, ct);
                await _quotationLineRepository.AddRangeAsync(lines, ct);
                await _statusHistoryRepository.AddAsync(history, ct);
                await _unitOfWork.SaveChangesAsync(ct);
                await _unitOfWork.CommitTransactionAsync();

                return OperationResult<Guid>.Ok(quotationId, "Tạo yêu cầu báo giá thành công.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult<Guid>.Fail($"Lỗi khi tạo yêu cầu báo giá: {ex.Message}");
            }
        }
    }
}

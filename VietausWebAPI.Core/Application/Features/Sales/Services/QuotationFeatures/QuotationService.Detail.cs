using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.QuotationDTOs;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.QuotationFeatures
{
    public sealed partial class QuotationService
    {
        public async Task<OperationResult<QuotationDetailDto>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var item = await BuildDetailAsync(id, ct);
            return item == null
                ? OperationResult<QuotationDetailDto>.Fail("Không tìm thấy báo giá hoặc bạn không có quyền xem.")
                : OperationResult<QuotationDetailDto>.Ok(item);
        }

        public async Task<OperationResult<byte[]>> PrintPdfAsync(Guid id, CancellationToken ct = default)
        {
            var detailResult = await GetByIdAsync(id, ct);
            if (!detailResult.Success || detailResult.Data == null)
                return OperationResult<byte[]>.Fail(detailResult.Message ?? "Không tìm thấy báo giá.");

            var pdf = _quotationPdf.Render(detailResult.Data);
            return OperationResult<byte[]>.Ok(pdf);
        }

        private async Task<QuotationDetailDto?> BuildDetailAsync(Guid id, CancellationToken ct)
        {
            var viewer = await _visibilityHelper.BuildViewerScopeAsync(ct);
            var entity = await ApplyQuotationVisibility(_quotationRepository.Query(), viewer)
                .Where(x => x.QuotationId == id)
                .Include(x => x.Customer)
                .Include(x => x.SaleEmployee)
                .Include(x => x.Lines)
                .Include(x => x.StatusHistories)
                    .ThenInclude(x => x.ChangedByNavigation)
                .FirstOrDefaultAsync(ct);

            if (entity == null)
                return null;

            var lines = entity.Lines
                .OrderBy(x => x.SortOrder)
                .Select(x => ParseLineNotePayload(new QuotationLineDto
                {
                    QuotationLineId = x.QuotationLineId,
                    ProductId = x.ProductId,
                    ProductExternalIdSnapshot = x.ProductExternalIdSnapshot,
                    ProductNameSnapshot = x.ProductNameSnapshot,
                    Quantity = x.Quantity,
                    Unit = x.Unit,
                    UnitPrice = x.UnitPrice,
                    DiscountPercent = x.DiscountPercent,
                    TaxPercent = x.TaxPercent,
                    LineTotal = x.LineTotal,
                    Note = x.Note,
                    SortOrder = x.SortOrder
                }))
                .ToList();

            return new QuotationDetailDto
            {
                QuotationId = entity.QuotationId,
                ExternalId = entity.ExternalId,
                CustomerId = entity.CustomerId,
                CustomerCode = entity.Customer.ExternalId,
                CustomerName = entity.Customer.CustomerName,
                ContactId = entity.ContactId,
                ContactName = entity.ContactName,
                SaleEmployeeId = entity.SaleEmployeeId,
                SaleEmployeeCode = entity.SaleEmployee.ExternalId,
                SaleEmployeeName = entity.SaleEmployee.FullName,
                Status = entity.Status,
                Currency = entity.Currency,
                ExchangeRate = entity.ExchangeRate,
                SubTotal = entity.SubTotal,
                DiscountAmount = entity.DiscountAmount,
                TaxAmount = entity.TaxAmount,
                TotalAmount = entity.TotalAmount,
                QuotationDate = entity.QuotationDate,
                ValidUntil = entity.ValidUntil,
                PaymentTerms = entity.PaymentTerms,
                DeliveryTerms = entity.DeliveryTerms,
                Note = entity.Note,
                Version = entity.Version,
                PreviousQuotationId = entity.PreviousQuotationId,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedBy,
                UpdatedDate = entity.UpdatedDate,
                UpdatedBy = entity.UpdatedBy,
                IsActive = entity.IsActive,
                LineCount = lines.Count,
                Lines = lines,
                StatusHistories = entity.StatusHistories
                    .OrderByDescending(x => x.ChangedDate)
                    .Select(x => new QuotationStatusHistoryDto
                    {
                        Id = x.Id,
                        FromStatus = x.FromStatus,
                        ToStatus = x.ToStatus,
                        Note = x.Note,
                        ChangedBy = x.ChangedBy,
                        ChangedByName = x.ChangedByNavigation.FullName,
                        ChangedDate = x.ChangedDate
                    })
                    .ToList()
            };
        }
    }
}

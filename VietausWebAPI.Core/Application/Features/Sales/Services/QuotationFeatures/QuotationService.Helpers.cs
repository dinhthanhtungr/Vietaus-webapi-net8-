using System.Text.Json;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.QuotationDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Helpers.QuotationFeatures;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.QuotationFeatures
{
    public sealed partial class QuotationService
    {
        private static QuotationLineDto ParseLineNotePayload(QuotationLineDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Note))
                return dto;

            try
            {
                var payload = JsonSerializer.Deserialize<QuotationLineNotePayload>(dto.Note, JsonOptions);
                if (payload == null) return dto;

                dto.Note = payload.Note;
                dto.PriceTiers = payload.PriceTiers
                    .OrderBy(x => x.SortOrder)
                    .Select(x => new QuotationLinePriceTierDto
                    {
                        MinQuantity = x.MinQuantity,
                        MaxQuantity = x.MaxQuantity,
                        Price = x.Price,
                        PriceAdjustment = x.PriceAdjustment,
                        Label = x.Label,
                        SortOrder = x.SortOrder
                    })
                    .ToList();
            }
            catch
            {
            }

            return dto;
        }

        private static string BuildLineNotePayload(string? note, IReadOnlyList<QuotationLinePriceTierDto>? tiers)
        {
            var payload = new QuotationLineNotePayload
            {
                Note = note,
                PriceTiers = (tiers ?? Array.Empty<QuotationLinePriceTierDto>())
                    .OrderBy(x => x.SortOrder)
                    .Select(x => new QuotationLineTierPayload
                    {
                        MinQuantity = x.MinQuantity,
                        MaxQuantity = x.MaxQuantity,
                        Price = x.Price,
                        PriceAdjustment = x.PriceAdjustment,
                        Label = x.Label,
                        SortOrder = x.SortOrder
                    })
                    .ToList()
            };

            return JsonSerializer.Serialize(payload, JsonOptions);
        }

        private static decimal CalculateLineTotal(CreateQuotationLineRequest line)
        {
            var gross = line.Quantity * line.UnitPrice;
            var discount = gross * Math.Clamp(line.DiscountPercent, 0m, 100m) / 100m;
            var beforeTax = gross - discount;
            var tax = beforeTax * Math.Clamp(line.TaxPercent, 0m, 100m) / 100m;
            return beforeTax + tax;
        }

        private static string BuildProductCode(string? requestCode, string? colourCode, string? productCode)
        {
            if (!string.IsNullOrWhiteSpace(requestCode))
                return requestCode.Trim();

            if (!string.IsNullOrWhiteSpace(colourCode))
                return colourCode.Trim();

            return productCode?.Trim() ?? string.Empty;
        }

        private static string? ResolveContactName(Customer customer, Guid? contactId, string? requestName)
        {
            if (!string.IsNullOrWhiteSpace(requestName))
                return requestName.Trim();

            if (!contactId.HasValue)
                return null;

            var contact = customer.Contacts.FirstOrDefault(x => x.ContactId == contactId.Value);
            if (contact == null)
                return null;

            return string.Join(" ", new[] { contact.FirstName, contact.LastName }
                .Where(x => !string.IsNullOrWhiteSpace(x)));
        }
    }
}

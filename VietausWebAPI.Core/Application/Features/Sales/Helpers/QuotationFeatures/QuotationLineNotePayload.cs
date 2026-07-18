namespace VietausWebAPI.Core.Application.Features.Sales.Helpers.QuotationFeatures
{
    internal sealed class QuotationLineNotePayload
    {
        public string? Note { get; set; }
        public List<QuotationLineTierPayload> PriceTiers { get; set; } = new();
    }

    internal sealed class QuotationLineTierPayload
    {
        public decimal MinQuantity { get; set; }
        public decimal? MaxQuantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceAdjustment { get; set; }
        public string? Label { get; set; }
        public int SortOrder { get; set; }
    }
}

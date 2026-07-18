using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.QuotationDTOs;

namespace VietausWebAPI.Core.Application.Features.Sales.Helpers.QuotationFeatures
{
    public sealed class QuotationPdf : IQuotationPdf
    {
        private static readonly CultureInfo ViCulture = CultureInfo.GetCultureInfo("vi-VN");

        public byte[] Render(QuotationDetailDto quotation)
        {
            quotation ??= new QuotationDetailDto();

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(14);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(8));
                    page.Content().Element(c => BuildContent(c, quotation));
                });
            });

            return doc.GeneratePdf();
        }

        private static void BuildContent(IContainer container, QuotationDetailDto q)
        {
            container.Column(col =>
            {
                col.Spacing(4);
                col.Item().Element(c => BuildHeader(c, q));
                col.Item().LineHorizontal(2).LineColor(Colors.Green.Darken2);
                col.Item().Text("Xin cảm ơn Quý khách hàng đã quan tâm và sử dụng sản phẩm của Công ty chúng tôi. Theo yêu cầu, chúng tôi xin gửi tới quý vị báo giá tốt nhất của chúng tôi cho các sản phẩm sau:")
                    .FontSize(8);
                col.Item().Text("Thanks for customer`s support. As request, we are very pleased to submit to you our best price for the following product(s):")
                    .Italic().FontSize(8);
                col.Item().Element(c => BuildPriceTable(c, q));
                col.Item().Element(BuildNotes);
                col.Item().Element(BuildTerms);
                col.Item().PaddingTop(5).Text("Chúng tôi rất mong nhận được sự quan tâm, hồi đáp sớm của Quý vị").FontSize(8);
                col.Item().Text("We look to your kind attention, favorable reply.").Italic().FontSize(8);
                col.Item().Text("Trân trọng kính chào / Best regards,").FontSize(8);
                col.Item().PaddingTop(14).Text(q.SaleEmployeeName).FontSize(9).Bold().FontColor(Colors.Blue.Medium);
                col.Item().Text($"Sales Rep. (HP: ; Email: )").FontSize(8);
                col.Item().Text("VIETAUS POLYMER CO., LTD.").FontSize(8).Bold().FontColor(Colors.Blue.Medium);
                col.Item().Element(BuildFooter);
            });
        }

        private static void BuildHeader(IContainer container, QuotationDetailDto q)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.RelativeColumn(1.4f);
                    cols.RelativeColumn(1.2f);
                    cols.RelativeColumn(0.9f);
                });

                table.Cell().ColumnSpan(2).AlignCenter().PaddingTop(34)
                    .Text("BẢNG BÁO GIÁ/QUOTATION").FontSize(18).Bold().FontColor(Colors.Red.Medium);

                table.Cell().AlignRight().Column(col =>
                {
                    col.Item().AlignCenter().Text("VIETAUS").Bold().FontSize(12).FontColor(Colors.Red.Medium);
                    col.Item().AlignCenter().Text("POLYMER").Bold().FontSize(8).FontColor(Colors.Blue.Darken2);
                    col.Item().AlignCenter().Border(1).Width(58).Height(58).AlignMiddle().Text("QR").FontSize(16).Bold();
                });

                table.Cell().Element(HeaderCell).Text("Kính Gửi/To:");
                table.Cell().Element(HeaderCell).Text(q.CustomerName);
                table.Cell().Element(HeaderCell).Text($"Số/No: {q.ExternalId}");

                table.Cell().Element(HeaderCell).Text("Ông/Bà (Mr/Ms):");
                table.Cell().Element(HeaderCell).Text(q.ContactName ?? string.Empty);
                table.Cell().Element(HeaderCell).Text($"Ngày/Date: {q.QuotationDate:dd/MM/yyyy}");

                table.Cell().Element(HeaderCell).Text("Đơn Gửi (Cc):");
                table.Cell().Element(HeaderCell).Text(string.Empty);
                table.Cell().Element(HeaderCell).Text($"Từ/From: {q.SaleEmployeeName}");

                table.Cell().Element(HeaderCell).Text("Địa Chỉ/Address:");
                table.Cell().Element(HeaderCell).Text(string.Empty);
                table.Cell().Element(HeaderCell).Text("Đ/T/Tel:");

                table.Cell().Element(HeaderCell).Text("Điện Thoại/Tel:");
                table.Cell().Element(HeaderCell).Text(string.Empty);
                table.Cell().Element(HeaderCell).Text("Fax:\nTrang/Page: 1 trang");
            });
        }

        private static void BuildPriceTable(IContainer container, QuotationDetailDto q)
        {
            var tierLabels = new[] { "50-100", "125-300", "325-600", "625-1000" };

            container.Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.RelativeColumn(1.1f);
                    cols.RelativeColumn(2.5f);
                    cols.RelativeColumn(0.9f);
                    cols.RelativeColumn(0.9f);
                    cols.RelativeColumn(0.9f);
                    cols.RelativeColumn(0.9f);
                    cols.RelativeColumn(0.8f);
                });

                table.Cell().RowSpan(2).Element(YellowHeader).Text("Mã số/Code").Bold();
                table.Cell().RowSpan(2).Element(YellowHeader).Text("Tên hàng/Name").Bold();
                table.Cell().ColumnSpan(4).Element(YellowHeader).AlignCenter().Text("Số lượng/Quantity(kg)").Bold();
                table.Cell().RowSpan(2).Element(YellowHeader).Text("Ghi chú\nNote").Bold();

                foreach (var label in tierLabels)
                    table.Cell().Element(YellowHeader).AlignCenter().Text(label).Bold();

                foreach (var line in q.Lines.OrderBy(x => x.SortOrder))
                {
                    table.Cell().Element(BodyCell).Text(line.ProductExternalIdSnapshot);
                    table.Cell().Element(BodyCell).Text(line.ProductNameSnapshot);

                    for (var tierIndex = 0; tierIndex < tierLabels.Length; tierIndex++)
                    {
                        var tier = FindTier(line.PriceTiers, tierLabels[tierIndex], tierIndex);
                        table.Cell().Element(BodyCell).AlignCenter().Text(FormatTierPrice(tier, line.UnitPrice)).Bold();
                    }

                    table.Cell().Element(BodyCell).Text(line.Note ?? string.Empty);
                }

                var emptyRows = Math.Max(0, 4 - q.Lines.Count);
                for (var i = 0; i < emptyRows; i++)
                {
                    for (var c = 0; c < 7; c++)
                        table.Cell().Element(BodyCell).Text(string.Empty);
                }
            });
        }

        private static void BuildNotes(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Text("Ghi chú/Note : Giá trên không bao gồm thuế GTGT 10% - Note: The prices above are excluded 10% VAT")
                    .FontColor(Colors.Blue.Medium).FontSize(8);
                col.Item().Text("Phí giao hàng chỉ hỗ trợ cho đơn hàng số lượng từ 1000kg trở lên - Transport fee just supports for PO over 1000kg")
                    .FontColor(Colors.Blue.Medium).FontSize(8);
                col.Item().Text("KHÁCH HÀNG KHÔNG LẤY HÓA ĐƠN VUI LÒNG CHỊU THÊM 5% TỔNG GIÁ TRỊ ĐƠN HÀNG")
                    .FontColor(Colors.Red.Medium).Bold().FontSize(8);
            });
        }

        private static void BuildTerms(IContainer container)
        {
            string[] rows =
            {
                "1. Thời hạn giao hàng/Delivery date (Ngày nhận đơn hàng/The receive PO date): 7 ngày/date",
                "2. Địa điểm giao hàng/Place to delivery: Kho khách hàng/Your warehouse",
                "3. Đóng gói/Packaging: 25kg/bao dệt PP/25 kg/PP woven bag",
                "4. Số lượng tối thiểu cho đơn hàng/Minimum quantity for order: 1000kg",
                "5. Thanh toán/payment term: Thanh toán ngay/TT in 0 day",
                "6. Thời hạn hiệu lực của báo giá/Validity: 20/06/2026"
            };

            container.Column(col =>
            {
                col.Item().Text("Các điều khoản khác:").Bold().Underline().FontSize(10);
                foreach (var row in rows)
                    col.Item().Text(row).FontSize(8);
            });
        }

        private static void BuildFooter(IContainer container)
        {
            container.PaddingTop(4).Column(col =>
            {
                col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                col.Item().Table(t =>
                {
                    t.ColumnsDefinition(cols =>
                    {
                        cols.RelativeColumn();
                        cols.RelativeColumn();
                    });

                    AddOffice(t, "Head office:", "No 266, Street 12, Tam Binh ward, Thu Duc Dist, HCMC, Vietnam.");
                    AddOffice(t, "U.S Representative Office:", "12651 Tash Avenue Garden Grove, CA 92843 USA");
                    AddOffice(t, "Head Factory:", "No 296, Trung Thang, Binh Thang ward, Di An Dist, Binh Duong Pro.");
                    AddOffice(t, "Japan Branch:", "333-0835 Saitama ken Kawaguchi shi michiai 11-15.");
                    AddOffice(t, "Branch Ha Noi:", "No 78, Lane 106, Hoang Quoc Viet st., Ha Noi, Vietnam.");
                    AddOffice(t, "Myanmar factory:", "No 59, Yangon, Myanmar.");
                    AddOffice(t, "Branch Da Nang:", "No 26, Ong Ich Khiem st., Hai Chau Dist, Da Nang City.");
                    AddOffice(t, "Email:", "sale01@vietaus.com");
                });

                col.Item().AlignCenter().Text("Web: www.vietaus.com – hotline : (84). 28. 73 09 39 69")
                    .FontColor(Colors.Blue.Medium).Bold();
                col.Item().AlignCenter().Text("COLOURING YOUR FUTURE WITH SERVICE AT YOUR DOORSTEP")
                    .FontColor(Colors.Red.Medium).Bold();
            });
        }

        private static void AddOffice(TableDescriptor table, string title, string text)
        {
            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(2).Text(t =>
            {
                t.Span(title).Bold();
                t.Span(" " + text);
            });
        }

        private static QuotationLinePriceTierDto? FindTier(IReadOnlyList<QuotationLinePriceTierDto> tiers, string label, int index)
        {
            return tiers.FirstOrDefault(x => string.Equals(x.Label, label, StringComparison.OrdinalIgnoreCase))
                ?? tiers.OrderBy(x => x.SortOrder).FirstOrDefault(x =>
                    label.StartsWith(x.MinQuantity.ToString("0", CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))
                ?? tiers.OrderBy(x => x.SortOrder).Skip(index).FirstOrDefault();
        }

        private static string FormatTierPrice(QuotationLinePriceTierDto? tier, decimal fallback)
        {
            if (tier?.Price is decimal price)
                return price.ToString("#,##0.##", ViCulture);

            if (tier?.PriceAdjustment is decimal adjustment)
                return adjustment.ToString("#,##0.##", ViCulture);

            return fallback.ToString("#,##0.##", ViCulture);
        }

        private static IContainer HeaderCell(IContainer c) =>
            c.Border(0.5f).BorderColor(Colors.Grey.Lighten2).PaddingVertical(2).PaddingHorizontal(3).AlignMiddle();

        private static IContainer YellowHeader(IContainer c) =>
            c.Border(1).BorderColor(Colors.Black).Background(Colors.Yellow.Medium).Padding(2).AlignMiddle();

        private static IContainer BodyCell(IContainer c) =>
            c.Border(1).BorderColor(Colors.Black).Padding(2).MinHeight(18).AlignMiddle();
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;
using VietausWebAPI.Core.Application.Shared.Helper;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.Helpers
{
    public class DeliveryOrderPdfRenderHelper : IDeliveryOrderPdfRenderHelper
    {
        private List<PdfPrinterDeliveryOrderDetail> _deliveryOrders = new List<PdfPrinterDeliveryOrderDetail>();
        private List<GetDeliveryOrderDetail> _DeliveryOrderDetails = new List<GetDeliveryOrderDetail>();

        public byte[] Render(PdfPrinterDeliveryOrder d)
        {

            // Null-safe
            _deliveryOrders = d.Details.ToList() ?? new List<PdfPrinterDeliveryOrderDetail>();          // Dòng hiển thị (từ WR)
            _DeliveryOrderDetails = d.DeliveryOrderDetails.ToList() ?? new List<GetDeliveryOrderDetail>();    // Dùng để tính tổng


            var doc = Document.Create(c =>
            {
                c.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(t => t.FontFamily("Open Sans").FontSize(9));

                    page.Header().Component(new HeaderComponent());
                    page.Content().Element(x => BuildContent(x, d));
                    page.Footer().Component(new FooterComponent());
                });
            });

            return doc.GeneratePdf();
        }

        private void BuildContent(IContainer c, PdfPrinterDeliveryOrder d)
        {
            c.Column(col =>
            {
                // Tiêu đề
                col.Item().AlignCenter().PaddingTop(5).Text("PHIẾU GIAO HÀNG / DELIVERYORDER").FontSize(14).Bold();


                // No và Ngày tạo đơn giao hàng
                col.Item().PaddingTop(5).AlignRight().Row(row =>
                {
                    row.Spacing(25); // khoảng cách giữa 2 phần

                    row.AutoItem().Text($"No: {d.ExternalId}");

                    row.AutoItem().Text($"Create Date: {DateTime.Now:dd/MM/yyyy}");
                });


                col.Item().PaddingBottom(5).Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.ConstantColumn(180); // Label
                        cols.RelativeColumn();    // Value
                    });

                    void AddRow(string label, string? value)
                    {
                        table.Cell().Element(CellStyle).Text(label).FontSize(9).SemiBold();
                        table.Cell().Element(CellStyle).Text(value ?? "-").FontSize(9);
                    }

                    static IContainer CellStyle(IContainer container) =>
                        container.BorderBottom(1)
                                 .BorderColor(Colors.Grey.Lighten2)
                                 .PaddingVertical(3);

                    AddRow("Đơn hàng (Product order):", d.MerchandiseOrderExternalIdList);
                    AddRow("Họ tên người mua (Customer name):", d.Receiver);
                    AddRow("Đơn vị (Company name):", d.CustomerName);
                    AddRow("Địa chỉ (Address):", d.CustomerAddress);
                    AddRow("Hình thức thanh toán (Payment term):", d.PaymentType);
                    AddRow("Mã số thuế (VAT code):", d.TaxNumber);
                    AddRow("Ngày giao (Delivery date):", $"{DateTime.Now:dd/MM/yyyy}");
                    AddRow("Người giao (Deliverer):", d.Deliverers != null && d.Deliverers.Any() ? string.Join(", ", d.Deliverers.Select(x => x.Name)) : "-");
                });

                col.Item().PaddingTop(10).PaddingBottom(10).Table(table =>
                {
                    // === Kích thước cột giống mẫu ===
                    table.ColumnsDefinition(cd =>
                    {
                        cd.ConstantColumn(70);   // Mã SP Code (hẹp lại)
                        cd.RelativeColumn(2.4f); // Tên SP Product Name (mở rộng)
                        cd.RelativeColumn(1.2f); // Số lô Batch No.
                        cd.ConstantColumn(45);   // Đơn vị Unit (hẹp)
                        cd.RelativeColumn(1.1f); // Số lượng Quantity
                        //cd.ConstantColumn(65);   // Số bao Bag number
                        cd.RelativeColumn(1.1f); // Số PO PO No.
                    });


                    // === Header 2 dòng (song ngữ) nền xám đậm, chữ trắng ===
                    var borderStyle = new Func<IContainer, IContainer>(c => c.Border(1).BorderColor(Colors.Black));

                    // ==== local helpers ====
                    static IContainer HeaderCell(IContainer x) => x
                        .PaddingVertical(4).PaddingHorizontal(6)
                        .Background(Colors.Grey.Darken2)
                        .Border(0.2f).BorderColor(Colors.Grey.Darken3);

                    static IContainer Cell(IContainer x) => x
                        .PaddingVertical(4).PaddingHorizontal(6)
                        .Border(0.2f).BorderColor(Colors.Grey.Darken3);

                    // Header
                    table.Header(row =>
                    {
                        var cellStyle = new Func<string, string, Action<IContainer>>((vi, en) => x =>
                        {
                            borderStyle(x)
                                .Background(Colors.Grey.Lighten1)
                                .AlignCenter()
                                //.PaddingVertical(4)
                                .Text($"{vi}\n{en}")
                                .FontSize(9)
                                .Bold();
                        });

                        cellStyle("Mã SP", "Code")(row.Cell());
                        cellStyle("Tên SP", "Product Name")(row.Cell());
                        cellStyle("Số lô", "Batch No")(row.Cell());
                        cellStyle("Đơn vị", "Unit")(row.Cell());
                        cellStyle("Số lượng", "Quantity")(row.Cell());
                        //cellStyle("Số bao", "Bag number")(row.Cell());
                        cellStyle("Số PO", "PO No")(row.Cell());
                    });

                    string? lastKey = null;
                    int index = 1;

                    // Hàm xác định “họ mã” = prefix chữ cái của mã (VD: LA từ LA31001C). Không có thì "OTHER"
                    string FamilyFromCode(string? code)
                    {
                        code ??= string.Empty;
                        var m = Regex.Match(code, @"^[A-Za-z]+");
                        return m.Success ? m.Value.ToUpperInvariant() : "OTHER";
                    }

                    // ===== Nhóm WR rows theo họ mã để in theo nhóm =====
                    var wrGroups = _deliveryOrders
                        .GroupBy(r => r.ProductCode)
                        // Ưu tiên nhóm "LA" lên đầu, rồi các nhóm khác theo alpha
                        .OrderByDescending(g => g.Key)
                        .ToList();

                    // ===== Tính subtotal theo nhóm TỪ DeliveryOrderDetails (DO) =====
                    // Quantity/NumOfBags trong GetDeliveryOrderDetail có thể là null → về 0
                    var doGroupTotals = _DeliveryOrderDetails
                        .GroupBy(r => r.ProductExternalIdSnapShot ?? string.Empty)
                        .ToDictionary(
                            g => g.Key,
                            g => new {
                                Weight = g.Sum(x => (decimal)(x?.Quantity ?? 0m)),
                                Bags = g.Sum(x => (int)(x?.NumOfBags ?? 0))
                            }
                        );

                    // Grand total TỪ DeliveryOrderDetails (DO)
                    var grandWeight = _DeliveryOrderDetails.Sum(x => (decimal)(x?.Quantity ?? 0m));
                    var grandBags = _DeliveryOrderDetails.Sum(x => (int)(x?.NumOfBags ?? 0));

                    foreach (var g in wrGroups)
                    {
                        // (tuỳ chọn) Header của nhóm
                        //borderStyle(table.Cell().ColumnSpan(6))
                        //    .PaddingVertical(4).PaddingHorizontal(6)
                        //    .Text($"Nhóm {g.Key}").Bold();

                        // In các dòng chi tiết trong nhóm (theo WR)
                        foreach (var r in g)
                        {
                            // Mã SP Code
                            borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(4).AlignLeft()
                                .Text(r?.ProductCode ?? "");

                            // Tên SP
                            borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).AlignLeft()
                                .Text(r?.ProductName ?? "");

                            // Số lô
                            borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).AlignLeft()
                                .Text(r?.LotNumber ?? "");

                            // Đơn vị
                            borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).AlignLeft()
                                .Text("Kg");

                            // Số lượng (theo WR để hiển thị từng dòng)
                            borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).AlignRight()
                                .Text($"{(r?.WeightKg ?? 0m):0.00}");

                            // (Nếu cần cột BagNumber theo WR thì mở; hiện anh giữ dữ liệu cũ → thường không in cột này ở từng dòng)
                            // borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).AlignRight()
                            //     .Text($"{(r?.BagNumber ?? 0)}");

                            // PO No
                            borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).AlignLeft()
                                .Text(r?.PONoNumber ?? "");
                        }

                        // ===== Subtotal của NHÓM g – TÍNH THEO DO =====
                        // Lấy totals của nhóm tương ứng từ doGroupTotals; không có thì 0
                        var key = g.Key;
                        var gw = doGroupTotals.TryGetValue(key, out var t) ? t.Weight : 0m;
                        var gb = doGroupTotals.TryGetValue(key, out t) ? t.Bags : 0;

                        // Gộp 4 cột đầu
                        borderStyle(table.Cell().ColumnSpan(4))
                            .PaddingVertical(4).PaddingHorizontal(6).AlignCenter()
                            .Text(t2 => { t2.Span("Tổng nhóm").Bold(); t2.Span($" {key}").Bold(); });

                        // 2 cột số (Weight + Bag)
                        borderStyle(table.Cell().ColumnSpan(2))
                            .PaddingVertical(4).PaddingHorizontal(6).AlignCenter()
                            .Text(t2 =>
                            {
                                t2.Span($"{gw:0.00} Kg - {gb} Bao").Bold();
                                t2.Span(" (Subtotal)").Bold();
                            });

                        // Cột PO trống
                        //borderStyle(table.Cell())
                        //    .PaddingVertical(4).PaddingHorizontal(6).AlignLeft()
                        //    .Text("");
                    }

                    // ===== GRAND TOTAL – TÍNH THEO DO =====
                    borderStyle(table.Cell().ColumnSpan(6))
                        .PaddingVertical(4).PaddingHorizontal(6).AlignRight()
                        .Text(t => { t.Span($"Tổng cộng: {grandWeight:0.00} Kg - {grandBags} Bao").Bold(); t.Span(" (Total)").Bold(); });


                });

                // Ghi chú dưới bảng
                col.Item().PaddingTop(6).Text(txt =>
                {
                    txt.Span("Ghi chú (Note): ").Bold();
                    txt.Span(d.Note ?? "");
                });

                // ==== Địa chỉ giao hàng & Liên hệ (song ngữ) ====
                col.Item().PaddingTop(6).Text(t =>
                {
                    t.Span("Địa chỉ giao hàng (Delivery Address): ").Bold();
                    t.Span(d.DeliveryAddress ?? "-");
                });

                col.Item().Text(t =>
                {
                    t.Span("Liên hệ nhận hàng (Contact person): ").Bold();
                    // Nếu bạn tách riêng tên & số ĐT:
                    // t.Span($"{d.ContactName ?? ""} - {d.ContactPhone ?? ""}");
                    // Còn nếu bạn đang có 1 chuỗi chung:
                    t.Span(d.Receiver ?? "-");
                });

                // ==== Khối chữ ký 3 cột ====
                col.Item().PaddingTop(14).Row(r =>
                {
                    // helper cho 1 cột chữ ký
                    void SignCol(RowDescriptor r, string vi, string en)
                    {
                        r.RelativeItem().Column(c2 =>
                        {
                            // Tiêu đề (song ngữ)
                            c2.Item().AlignCenter().Text(t =>
                            {
                                t.Span(vi).SemiBold();
                                t.Span(" (").SemiBold();
                                t.Span(en).SemiBold();
                                t.Span(")").SemiBold();
                            });
                            // Gợi ý ở dưới (chữ nhỏ, nghiêng)
                            c2.Item().AlignCenter().Text("(Tên, chữ ký/Fullname, signature)")
                                .Italic().FontSize(9);

                            // Vùng ký tên: chừa khoảng trống + kẻ line riêng cột
                            c2.Item().MinHeight(60);


                        });
                    }

                    SignCol(r, "Người lập phiếu", "Preparer");
                    SignCol(r, "Thủ kho", "Storekeeper");
                    SignCol(r, "Người nhận hàng", "Receiver");
                });
            });
        }
    }
}

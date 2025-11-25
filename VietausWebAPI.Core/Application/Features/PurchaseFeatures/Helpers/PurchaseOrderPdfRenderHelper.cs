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
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs.PdfPrinter;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.Pdfs;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.Helpers
{
    public class PurchaseOrderPdfRenderHelper : IPurchaseOrderPdfRenderHelper
    {
        private PdfPrinterPurchaseOrder _purchaseOrder = new PdfPrinterPurchaseOrder();
        private List<PdfPrinterPurchaseOrderDetail> _purchaseOrderDetails = new List<PdfPrinterPurchaseOrderDetail>();

        public byte[] Render(PdfPrinterPurchaseOrder d)
        {

            // Null-safe
            _purchaseOrder = new PdfPrinterPurchaseOrder();          // Dòng hiển thị (từ WR)
            _purchaseOrderDetails = d.Details.ToList() ?? new List<PdfPrinterPurchaseOrderDetail>();    // Dùng để tính tổng


            var doc = Document.Create(c =>
            {
                c.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(t => t.FontFamily("Open Sans").FontSize(8));

                    page.Header().Component(new HeaderComponent());
                    page.Content().Element(x => BuildContent(x, d));
                    page.Footer().Component(new FooterComponent());
                });
            });

            return doc.GeneratePdf();
        }
        private void BuildContent(IContainer c, PdfPrinterPurchaseOrder d)
        {
            c.Column(col =>
            {
                // Tiêu đề
                col.Item().AlignCenter().PaddingTop(5).Text("ĐƠN ĐẶT HÀNG / PURCHASING ORDER").FontSize(10).Bold();


                // No và Ngày tạo đơn giao hàng
                col.Item().PaddingTop(5).AlignRight().Row(row =>
                {
                    row.Spacing(25); // khoảng cách giữa 2 phần

                    //row.AutoItem().Text($"No: {d.PurchaseOrderExternalIdSnapshot}");

                    row.AutoItem().Text($"Create Date: {DateTime.Now:dd/MM/yyyy}");
                });


                col.Item().Row(row =>
                {
                    // Cột trái
                    col.Item().PaddingBottom(5).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(90);  // Label trái
                            cols.RelativeColumn(2);    // Value trái
                            cols.ConstantColumn(80);  // Label phải
                            cols.RelativeColumn();    // Value phải
                        });

                        static IContainer CellStyle(IContainer container) =>
                            container.BorderBottom(1)
                                     .BorderColor(Colors.Grey.Lighten2)
                                     .PaddingVertical(2);

                        void AddRow(string leftLabel, string? leftValue, string rightLabel, string? rightValue)
                        {
                            table.Cell().Element(CellStyle).Text(leftLabel).SemiBold();
                            table.Cell().Element(CellStyle).Text(string.IsNullOrWhiteSpace(leftValue) ? "-" : leftValue);

                            table.Cell().Element(CellStyle).Text(rightLabel).SemiBold();
                            table.Cell().Element(CellStyle).Text(string.IsNullOrWhiteSpace(rightValue) ? "-" : rightValue);
                        }

                        var dateStr = (d.CreatedDate ?? DateTime.Now).ToString("dd/MM/yyyy");

                        AddRow("Kính gửi/To:", d.SupplierNameSnapshot, "ĐĐH SỐ/PO No:", d.PurchaseOrderExternalIdSnapshot);
                        AddRow("Ông/Bà (Mr/Ms):", d.SupplierContactSnapshot, "Ngày/Date:", dateStr);
                        AddRow("Đồng gửi/Cc:", null, "Từ/From:", d.EmployeeFullNameSnapshot);
                        AddRow("Địa chỉ/Address:", d.SupplierAddressSnapshot, "Điện thoại/Tel:", d.PhoneNumberSnapshot);
                        AddRow("Điện thoại/Tel:", d.SupplierPhoneNumberSnapshot, "Website:", "https://vietaus.com");
                    });
                });


                // Ghi chú dưới bảng
                col.Item().PaddingTop(2).Text(txt =>
                {
                    txt.Span("Công ty chúng tôi có nhu cầu đặt các nguyên liệu sau:/Our company has plan to order the raw material below:\r\n");
                });


                col.Item().PaddingTop(2).Table(table =>
                {
                    // ==== Cấu hình cột ====
                    table.ColumnsDefinition(cd =>
                    {
                        cd.RelativeColumn(3.5f); // Tên SP
                        cd.RelativeColumn(1.0f); // Số lượng (kg)
                        cd.RelativeColumn(1.0f); // Đơn giá (VND)
                        cd.RelativeColumn(1.0f); // Đóng bao
                        cd.RelativeColumn(1.0f); // Thành tiền (VND)
                    });

                    // ==== Helpers ====
                    static IContainer Th(IContainer x) => x
                        .Background(Colors.Grey.Lighten1)
                        .Border(0.2f).BorderColor(Colors.Grey.Darken3)
                        .PaddingVertical(2)   // ↓ giảm từ 4 xuống 2
                        .PaddingHorizontal(6)
                        .AlignCenter();

                    static IContainer Td(IContainer x) => x
                        .Border(0.2f).BorderColor(Colors.Grey.Darken3)
                        .PaddingVertical(2)   // ↓ giảm từ 4 xuống 2
                        .PaddingHorizontal(6);


                    static string Money(decimal? v) => (v ?? 0m).ToString("#,0.00");
                    static string Qty(decimal? v) => (v ?? 0m).ToString("0.00");

                    // ==== Header ====
                    table.Header(row =>
                    {
                        Th(row.Cell()).Text("Tên SP\nProduct Name").FontSize(9).Bold();
                        Th(row.Cell()).Text("Số lượng\nQuantity (kg)").FontSize(9).Bold();
                        Th(row.Cell()).Text("Đơn giá\nPrice (VND)").FontSize(9).Bold();
                        Th(row.Cell()).Text("Đóng bao\nPackage").FontSize(9).Bold();
                        Th(row.Cell()).Text("Thành tiền\nTotal (VND)").FontSize(9).Bold();
                    });

                    // ==== Dòng chi tiết ====
                    foreach (var it in d.Details) // d: PdfPrinterPurchaseOrder
                    {
                        var qty = it.RequestQuantity ?? 0m;
                        var price = it.UnitPriceAgreed ?? 0m;
                        var line = it.TotalUnitPriceAgreed ?? (qty * price);

                        Td(table.Cell()).AlignLeft().Text(it.MaterialNameSnapshot ?? "-").FontSize(9);
                        Td(table.Cell()).AlignRight().Text(Qty(qty)).FontSize(9);
                        Td(table.Cell()).AlignRight().Text(Money(price)).FontSize(9);
                        Td(table.Cell()).AlignRight().Text(it.Package ?? "-").FontSize(9);
                        Td(table.Cell()).AlignRight().Text(Money(line)).FontSize(9);
                    }

                    // ==== Cộng tiền ====
                    var subTotal = d.Details.Sum(x => x.TotalUnitPriceAgreed ?? ((x.RequestQuantity ?? 0m) * (x.UnitPriceAgreed ?? 0m)));
                    var vatRate = (d.Vat ?? 0) / 100m;
                    var vatAmt = decimal.Round(subTotal * vatRate, 2, MidpointRounding.AwayFromZero);
                    var grand = d.TotalPrice ?? (subTotal + vatAmt);  // ưu tiên TotalPrice nếu đã chốt

                    // Dòng: Thành tiền (Without VAT)
                    Td(table.Cell().ColumnSpan(4)).AlignCenter().Text(t =>
                    {
                        t.Span("Thành tiền/Total").Bold(); t.Span("\n(Chưa thuế GTGT/Without VAT)");
                    });
                    Td(table.Cell()).AlignRight().Text(Money(subTotal)).Bold();

                    // Dòng: Thuế GTGT (x%)
                    Td(table.Cell().ColumnSpan(4)).AlignCenter().Text($"Thuế GTGT/VAT ({d.Vat ?? 0:0}%)");
                    Td(table.Cell()).AlignRight().Text(Money(vatAmt)).Bold();

                    // Dòng: Tổng cộng
                    Td(table.Cell().ColumnSpan(4)).AlignCenter().Text("Tổng cộng/Total").Bold();
                    Td(table.Cell()).AlignRight().Text(Money(grand)).Bold();
                });

                // Ghi chú dưới bảng
                col.Item().PaddingTop(6).Text(txt =>
                {
                    txt.Span("Ghi chú (Note): ").Bold();
                    txt.Span(d.Note ?? "");
                });

                col.Item().PaddingTop(6).Table(t =>
                {
                    // 2 cột: số thứ tự (hẹp) + nội dung (rộng)
                    t.ColumnsDefinition(c =>
                    {
                        c.ConstantColumn(14); // STT "1.", "2."
                        c.RelativeColumn();   // Nội dung
                    });

                    // Helper style
                    static IContainer Num(IContainer x) => x
                        .PaddingRight(4)
                        .AlignRight();

                    static IContainer Txt(IContainer x) => x
                        .PaddingBottom(2); // hàng gọn

                    void Row(string no, Action<IContainer> content)
                    {
                        Num(t.Cell()).Text(no + ".");
                        content(t.Cell());
                    }

                    // ===== Tiêu đề =====
                    t.Cell().ColumnSpan(2)
                        .PaddingBottom(4)
                        .Text(txt =>
                        {
                            txt.Span("Các điều khoản khác/Other conditions:").SemiBold().Underline();
                        });

                    // 1) Ngày giao
                    Row("1", c => Txt(c).Text(txt =>
                    {
                        txt.Span("Thời hạn giao hàng/Delivery Date: ").SemiBold();
                        txt.Span(d.RequestDeliveryDate?.ToString("dd/MM/yyyy"));
                    }));

                    // 2) Địa điểm giao + khung giờ + liên hệ (song ngữ 1 dòng)
                    Row("2", c => Txt(c).Text(txt =>
                    {
                        txt.Span("Địa điểm giao/Delivery Address: ").SemiBold();
                        txt.Span(d.DeliveryAddress);
                    }));
                    // 4) Thanh toán
                    Row("3", c => Txt(c).Text(txt =>
                    {
                        txt.Span("Thanh toán/Payment Term: ").SemiBold();
                        txt.Span(d.PaymentTypes);
                    }));

                    // 5) Chứng từ bắt buộc – song ngữ
                    Row("4", c => Txt(c).Text(txt =>
                    {
                        txt.Span("Các chứng từ bắt buộc phải có: ").SemiBold();
                        txt.Span("Phiếu xuất kho (phiếu giao hàng); Hóa đơn (bản chính hoặc photo); COA; Đơn đặt hàng có xác nhận của nhà cung cấp. Nếu thiếu một trong những chứng từ trên, BP Kho có quyền không nhận hàng.\n");
                        txt.Span("The documents must have when you deliver: Delivery order; Invoice (original or photo); COA; PO’s confirm. If one of the above documents is missing, our warehouse has the right not to receive goods.");
                    }));
                });

                col.Item().PaddingTop(6).Text(txt =>
                {
                    txt.Span("Thông tin xuất hóa đơn/ Tax invoice information:\n").SemiBold().Underline();
                    txt.Span("CÔNG TY TNHH CƠ KHÍ NHỰA VIỆT ÚC\n").FontSize(10).Bold();
                    txt.Span("26/6 Đường 12, Phường Tam Bình, Thành Phố Hồ Chí Minh, Việt Nam\n").FontSize(10).Bold();
                    txt.Span("Mã số thuế/ Tax code: 0312889634\n").FontSize(10).Bold();
                    txt.Span("Điện thoại/ Tel: 028-7309.3969\n").FontSize(10).Bold();
                    txt.Span("Số tài khoản/ Account Number: 0371000473451 tại NH Vietcombank – CN Tân Định").FontSize(10).Bold();
                });

                col.Item().PaddingTop(14).ExtendHorizontal().Row(r =>
                {
                    r.Spacing(0);           // không chừa khe giữa 2 cột
                    r.RelativeItem(1).Column(c =>
                    {
                        // TIÊU ĐỀ TRÁI: căn trái
                        c.Item().AlignLeft().Text(t =>
                        {
                            t.Span("Trân trọng kính chào").SemiBold();
                            t.Span(" (").SemiBold();
                            t.Span("Best regards").SemiBold();
                            t.Span(")\n").SemiBold();

                            t.Span("Nguyễn Văn Phương\n").Bold();
                            t.Span("Director").SemiBold();
                        });

                        //c.Item().Height(60); // khoảng trống ký
                        //c.Item().AlignLeft().Text("").FontSize(9);
                    });

                    r.RelativeItem(1).Column(c =>
                    {
                        // TIÊU ĐỀ PHẢI: căn phải
                        c.Item().AlignRight().Text(t =>
                        {
                            t.Span("Xác nhận nhà cung cấp").SemiBold();
                            t.Span(" (").SemiBold();
                            t.Span("Supplier confirmation").SemiBold();
                            t.Span(")").SemiBold();
                        });

                        c.Item().Height(60); // khoảng trống ký
                        c.Item().AlignRight().Text("-----------------------------").FontSize(9);
                    });
                });

                //col.Item().PaddingTop(10).Row(r =>
                //{
                //    // === CỘT TRÁI ===
                //    r.RelativeItem(1.3f).Column(c =>
                //    {
                //        c.Item().Text("VIETAUS POLYMER CO., LTD.")
                //            .Bold()
                //            .FontSize(9);

                //        c.Item().Text(t =>
                //        {
                //            t.Span("Head office: ").Bold();
                //            t.Span("No 266, Street 12, Tan Binh ward, Thu Duc Dist., HCMC, Vietnam.\n");
                //            t.Span("T +84 28 73 09 39 69 | M +84 973 527 465 | F +84 274.3800 037\n");
                //            t.Span("Email: quyen.dang@vietaus.com");
                //        });

                //        c.Item().Text(t =>
                //        {
                //            t.Span("Head factory: ").Bold();
                //            t.Span("No 286, Trung Thang, Dong Hoa ward, Ho Chi Minh city, Vietnam.\n");
                //            t.Span("T +84 28 73 09 39 69 | M +84 907 606 637 | F +84 274 3 800 037\n");
                //            t.Span("Email: phuong.nguyen@vietaus.com");
                //        });

                //        c.Item().Text(t =>
                //        {
                //            t.Span("Branch Ha Noi: ").Bold();
                //            t.Span("No 2 Lane 1, Tran Quoc Hoan St., Cau Giay Dist., Ha Noi, Vietnam.\n");
                //            t.Span("T +84 24 32 191 132 | M +84 915 344 343 | F +84 24 32 191 132\n");
                //            t.Span("Email: lan.quach@vietaus.com");
                //        });

                //        c.Item().Text(t =>
                //        {
                //            t.Span("Branch Da Nang: ").Bold();
                //            t.Span("No 108, Cao Xuan Duc St., Hai Chau Dist., Da Nang city, Vietnam.\n");
                //            t.Span("T +84 236 35 30889 | M +84 905 458 758 | F +84 236 355 2869\n");
                //            t.Span("Email: ha.nguyen@vietaus.com");
                //        });
                //    });

                //    // === CỘT PHẢI ===
                //    r.RelativeItem(1).Column(c =>
                //    {
                //        c.Item().Text(t =>
                //        {
                //            t.Span("US Representative Office: ").Bold();
                //            t.Span("12651 Trash Avenue Garden Grove, CA 92843 USA\n");
                //            t.Span("T +1 714 262 9764 | Email: info@uspolimex.com\n");
                //            t.Span("Website: www.uspolimex.com");
                //        });

                //        c.Item().Text(t =>
                //        {
                //            t.Span("Japan Branch: ").Bold();
                //            t.Span("333-0835 saitama ken kawaguchishi midai 11-15.\n");
                //            t.Span("日本ブランチ: 333-0835 埼玉県川口市前代11-15.\n");
                //            t.Span("Email: tuanduy.nguyen@vietaus.com | Tel (+81) 080 5027 6878");
                //        });

                //        c.Item().Text(t =>
                //        {
                //            t.Span("Myanmar factory: ").Bold();
                //            t.Span("No 59, myoung Daiker steel IZ, Hmawbi, Yangon, Myanmar.\n");
                //            t.Span("Tel: (+95)9420 309 22 / +9420 209 559\n");
                //            t.Span("Email: sale01@vietaus.com");
                //        });
                //    });
                //});


            });
        }
    }
}

namespace VietausWebAPI.WebAPI.Helpers
{
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using QuestPDF.Fluent;
    using QuestPDF.Helpers;
    using QuestPDF.Infrastructure;
    using VietausWebAPI.Core.DTO;

    public class PurchaseOrderPdf : IDocument
    {
        private readonly PurchaseOrderModel _model;
        public PurchaseOrderPdf(PurchaseOrderModel model) => _model = model;

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(15); // Giảm lề trang
                page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(8)); // Giảm font toàn trang

                page.Header().Row(row =>
                {
                    // BÊN TRÁI: TIÊU ĐỀ + THÔNG TIN
                    row.RelativeItem().Column(left =>
                    {
                        left.Item().AlignLeft().Text("ĐƠN ĐẶT HÀNG / PURCHASING ORDER")
                            .FontSize(16).Bold();

                        left.Item().PaddingTop(10).Row(infoRow =>
                        {
                            // Thông tin vendor (trái)
                            infoRow.RelativeItem().Column(col =>
                            {
                                col.Item().AlignLeft().Text(t =>
                                {
                                    t.Span("Kính gửi/To: ").SemiBold();
                                    t.Span(_model.VendorName);
                                });

                                col.Item().AlignLeft().Text(t =>
                                {
                                    t.Span("Ông/Bà (Mr/Ms): ").SemiBold();
                                    t.Span(_model.ContactName);
                                });

                                col.Item().AlignLeft().Text("Đồng gửi/Cc:");

                                col.Item().AlignLeft().Text(t =>
                                {
                                    t.Span("Địa chỉ/Address: ").SemiBold();
                                    t.Span(_model.VendorAddress);
                                });

                                col.Item().AlignLeft().Text(t =>
                                {
                                    t.Span("Điện thoại/Tel: ").SemiBold();
                                    t.Span($"{_model.VendorPhone}     Fax");
                                });
                            });

                            infoRow.ConstantItem(50); // 👈 Khoảng cách 30px giữa 2 cột
                            // Thông tin PO (phải)
                            infoRow.RelativeItem().Column(col =>
                            {
                                col.Item().AlignLeft().Text(t =>
                                {
                                    t.Span("ĐĐH Số/PO No: ").SemiBold();
                                    t.Span(_model.POCode);
                                });

                                col.Item().AlignLeft().Text(t =>
                                {
                                    t.Span("Ngày/Date: ").SemiBold();
                                    t.Span($"{_model.OrderDate:dd/MM/yyyy}");
                                });

                                col.Item().AlignLeft().Text(t =>
                                {
                                    t.Span("Từ/From: ").SemiBold();
                                    t.Span(_model.FullName);
                                });

                                col.Item().AlignLeft().Text(t =>
                                {
                                    t.Span("Điện thoại/Tel: ").SemiBold();
                                    t.Span(_model.PhoneNumber);
                                });

                                col.Item().AlignLeft().Text(t =>
                                {
                                    t.Span("Website: ").SemiBold();
                                    t.Span("https://vietaus.com");
                                });
                            });

                        });
                    });

                    // BÊN PHẢI: LOGO
                    row.ConstantItem(100).AlignRight().Column(col =>
                    {
                        var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Logo.png");
                        if (File.Exists(logoPath))
                        {
                            var logoBytes = File.ReadAllBytes(logoPath);
                            col.Item().Height(60).Image(logoBytes).FitHeight(); // điều chỉnh size nếu cần
                        }
                    });

                });

                page.Content().Column(column =>
                {
                    column.Item().PaddingTop(6).PaddingBottom(2).AlignLeft().Text(
                        "Công ty chúng tôi có nhu cầu đặt các nguyên liệu sau:/Our company has plan to order the raw material below:");
                    column.Spacing(5);

                    // Table
                    // Bảng sản phẩm + tổng tiền

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(4); // Tên SP
                            cols.RelativeColumn(2); // Số lượng
                            cols.RelativeColumn(2); // Đơn giá
                            cols.RelativeColumn(2); // Thành tiền
                        });

                        var borderStyle = new Func<IContainer, IContainer>(c => c.Border(1).BorderColor(Colors.Black));

                        // Header
                        table.Header(h =>
                        {
                            borderStyle(h.Cell()).Background(Colors.Grey.Lighten1).Padding(5).Text("Tên SP\nProduct Name").Bold();
                            borderStyle(h.Cell()).Background(Colors.Grey.Lighten1).Padding(5).AlignCenter().Text("Số lượng\nQuantity (kg)").Bold();
                            borderStyle(h.Cell()).Background(Colors.Grey.Lighten1).Padding(5).AlignCenter().Text("Đơn giá\nPrice (VNĐ)").Bold();
                            borderStyle(h.Cell()).Background(Colors.Grey.Lighten1).Padding(5).AlignCenter().Text("Thành tiền\nTotal (VNĐ)").Bold();
                        });


                        // Dòng sản phẩm
                        foreach (var item in _model.Items)
                        {
                            borderStyle(table.Cell()).Padding(5).Text($"{item.ProductName} {item.Note}");
                            borderStyle(table.Cell()).Padding(5).AlignRight().Text($"{item.Quantity:N2}");
                            borderStyle(table.Cell()).Padding(5).AlignRight().Text($"{item.UnitPrice:N0}");
                            borderStyle(table.Cell()).Padding(5).AlignRight().Text($"{item.Total:N0}");
                        }

                        // Tổng cộng
                        borderStyle(table.Cell().ColumnSpan(3)).Padding(5).AlignRight().Text("Thành tiền (chưa VAT)").Bold();
                        borderStyle(table.Cell()).Padding(5).AlignRight().Text($"{_model.TotalAmount:N0}").Bold();

                        borderStyle(table.Cell().ColumnSpan(3)).Padding(5).AlignRight().Text($"Thuế GTGT ({_model.VAT}%)");
                        borderStyle(table.Cell()).Padding(5).AlignRight().Text($"{_model.VAT:N0}");

                        borderStyle(table.Cell().ColumnSpan(3)).Padding(5).AlignRight().Text("Tổng cộng");
                        borderStyle(table.Cell()).Padding(5).AlignRight().Text($"{_model.GrandTotal:N0}").Bold();
                    });

                    // Ghi chú
                    column.Item().Text(text =>
                    {
                        text.Span($"Ghi chú: {_model.Note}").SemiBold();
                        //text.Span($"\nGiá trên không bao gồm thuế GTGT - {_model.VAT}% VAT");
                        text.Span($"\nNote: {_model.Note}").SemiBold();
                        //text.Span($"\nThe prices above are excluded - {_model.VAT}% VAT");
                    });

                    //column.Item().Text("Phụ gia UV 791 250kg");

                    // Điều khoản
                    column.Item().PaddingTop(5).Text(text =>
                    {
                        text.Span("Các điều khoản khác/Other conditions: ").SemiBold();
                    });

                    column.Item().PaddingBottom(5).Text(text =>
                    {
                        // Cụm công nghiệp dốc 47, khu phố Long Khánh 2, Phường Tam Phước, Thành phố Biên Hoà, Tỉnh Đồng Nai, Việt Nam.\n    Liên hệ Ms Quỳnh 0356883820 ; Thời gian làm việc: Sáng đến chiều.
                        text.Line($"1. Thời hạn giao hàng/Delivery Date: {_model.DeliveryDate:dd/MM/yyyy}.");
                        text.Line($"2. Địa điểm giao hàng/Delivery Address: {_model.DeliveryAddress}.");
                        text.Line($"3. Đóng gói/Packaging: {_model.Packaging}.");
                        text.Line($"4. Thanh toán/Payment Term: {_model.PaymentTerm}.");
                        text.Line($"5. Các chứng từ bắt buộc phải có: Phiếu xuất kho/giao hàng); Hóa đơn (bản chính hoặc photo); COA; Đơn đặt hàng có xác nhận của nhà cung cấp.    {_model.RequiredDocuments}.\n");
                        text.Line($"    The documents must to have when delivery: Delivery order, Invoice(original or photo); COA; PO’s confirm. {_model.RequiredDocuments_Eng}.");
                    });


                    // Lưu ý
                    column.Item().Text(text =>
                    {
                        text.Span("Lưu ý/Note: ").SemiBold();
                        text.Span("Thông tin xuất hoá đơn/Tax invoice information:");
                    });

                    // Thông tin công ty
                    column.Item().Text(text =>
                    {
                        text.Line("CÔNG TY TNHH CƠ KHÍ NHỰA VIỆT ÚC").Bold();
                        text.Line("26/6 Đường 12, Phường Tam Bình, TP.Thủ Đức, TP.Hồ Chí Minh, Việt Nam").Bold();
                        text.Line("Mã số thuế/Tax code: 0312889634").Bold();
                        text.Line("Điện thoại/ Tel: 028-7309.3969").Bold();
                        text.Line("Số tài khoản/ Account Number: 0371000473451 tại NH Vietcombank – CN Tân Định").Bold();
                    });

                    // Lời kết
                    column.Item().PaddingBottom(30).Row(row =>
                    {
                        row.RelativeItem().Text("Trân trọng kính chào/Best regards");
                        row.ConstantItem(200).AlignRight().Text("Xác nhận nhà cung cấp").Bold();
                    });

                  

                });



                page.Footer().Column(column =>
                {

                    column.Item().PaddingBottom(10).Text("VIETAUS POLYMER CO., LTD.");

                    column.Item().DefaultTextStyle(x => x.FontSize(7)).Row(row =>
                    {
                        // Cột trái: trụ sở & chi nhánh VN
                        row.RelativeItem().Column(col =>
                        {

                            col.Item().Text(text =>
                            {
                                text.Span("Head office: ").Bold();
                                text.Span("No 26/6, Street 12, Tam Binh ward, Thu Duc Dist., HCMC, Vietnam\n");
                                text.Span("T +84 28 73 09 39 69 | M +84 973 527 465 | F +84.274.3800 037\n");
                                text.Span("Email: duy.nguyen@vietaus.com");
                            });

                            col.Item().Text(text =>
                            {
                                text.Span("Head factory: ").Bold();
                                text.Span("No 298, Trung Thang, Binh Thang ward, Di An Dist., Binh Duong, Vietnam\n");
                                text.Span("T +84 28 73 09 39 69 | M +84 907 606 837 | F +84 274 3 800 037\n");
                                text.Span("Email: phuong.nguyen@vietaus.com");
                            });

                            col.Item().Text(text =>
                            {
                                text.Span("Branch Ha Noi: ").Bold();
                                text.Span("No 22, Lane 1, Tran Quoc Hoan St., Cau Giay Dist., Ha Noi, Vietnam\n");
                                text.Span("T +84 24 32 191 132 | M +84 915 344 331 | F +84 24 32 191 132\n");
                                text.Span("Email: lam.quachdai@vietaus.com");
                            });

                            col.Item().Text(text =>
                            {
                                text.Span("Branch Da Nang: ").Bold();
                                text.Span("No 108, Cao Xuan Duc St., Hai Chau Dist., Da Nang city, Vietnam\n");
                                text.Span("T +84 236 35 0689 | M +84 905 458 758 | F +84 236 355 2869\n");
                                text.Span("Email: ha.nguyen@vietaus.com");
                            });
                        });

                        // Cột phải: các văn phòng đại diện
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text(text =>
                            {
                                text.Span("US Representative Office: ").Bold();
                                text.Span("12651 Trash Avenue Garden Grove, CA 92843 USA\n");
                                text.Span("T +1 714 262 9764 | Email: info@uspolimex.com | ");
                                text.Span("www.uspolimex.com");
                            });

                            col.Item().Text(text =>
                            {
                                text.Span("Japan Branch: ").Bold();
                                text.Span("333-0835 saitamaken kawaguchishi michiai 11-15\n");
                                text.Span("日本アカ: 333-0835 埼玉県川口市道合11-15\n");
                                text.Span("Email: tuan.nguyen@vietaus.com | Tel (81) 080 5027 6878");
                            });

                            col.Item().Text(text =>
                            {
                                text.Span("Myanmar factory: ").Bold();
                                text.Span("No 59, myanmar Daker steel IZ, Hmawbi, Yangon, Myanmar\n");
                                text.Span("Tel: (+95)920 309 222/9420 209 559\n");
                                text.Span("Email: sale01@vietaus.com");
                            });
                        });
                    });


                    // Dòng 1: Website + Hotline + Slogan tách dòng
                    column.Item().AlignCenter().PaddingTop(10).Text(text =>
                    {
                        text.Line("Website: https://vietaus.com - Hotline: (84). 8. 73 09 39 69");
                        text.Line("COLOURING YOUR FUTURE WITH SERVICE AT YOUR DOORSTEP").Bold();
                    });


                    // Dòng 2: ISO + Ngày + Mã biểu mẫu
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Text("ISO 9001 / ISO 14001 / ISO 45001 GRS").FontSize(9);
                        row.ConstantItem(100).AlignCenter().Text(DateTime.Now.ToString("dd-MM-yyyy")).FontSize(9);
                        row.ConstantItem(120).AlignRight().Text("VA-PL&PU-F01(03)").FontSize(9);
                    });
                });


            });
        }



    }

}

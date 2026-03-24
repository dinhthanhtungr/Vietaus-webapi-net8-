using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Features.Labs.Helpers;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Application.Shared.Helper.Pdfs;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.ProductInspectionFeature;

namespace VietausWebAPI.Core.Application.Features.Labs.Helpers
{
    public class COAPdf : IDocument
    {
        private readonly PDFResultValue _result;
        private readonly PDFSpecificationsValue? _specs;
        private bool _isLongGiangBag;

        // Constructor 1: chỉ có kết quả kiểm tra
        public COAPdf(PDFResultValue result)
        {
            _result = result;
            _specs = null;
        }

        // Constructor 2: có cả kết quả kiểm tra và tiêu chuẩn
        public COAPdf(PDFResultValue result, PDFSpecificationsValue? specs)
        {
            _result = result;
            _specs = specs;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);

                _isLongGiangBag = (_result.bagType ?? "")
                    .Replace("_", " ")
                    .Contains("long giang", StringComparison.OrdinalIgnoreCase);


                if (_isLongGiangBag)
                {
                    page.Header().Component(new LGHeaderComponent());
                }

                else
                {
                    page.Header().Component(new HeaderComponent());
                }

                page.DefaultTextStyle(t => t.FontFamily("Open Sans").FontSize(9));

                page.Content().Column(column =>
                {
                    // Tiêu đề
                    column.Item().AlignCenter().PaddingTop(5).Text("CERTIFICATE OF ANALYSIS").FontSize(14).Bold();

                    // No và Ngày tạo COA
                    column.Item().PaddingTop(5).AlignRight().Row(row =>
                    {
                        row.Spacing(25); // khoảng cách giữa 2 phần

                        row.AutoItem().Text($"No: {_result.ExternalId}");

                        row.AutoItem().Text($"Create Date: {_result.CreateDate:dd/MM/yyyy}");
                    });


                    // I. THÔNG TIN SẢN PHẨM


                    if (_result.bagType?.ToLower().Replace("_", " ").Contains("long giang") == true)
                    {
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().PaddingRight(10).Table(table =>
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

                                AddRow("Product name (Tên sản phẩm):", _result.ProductName);
                                AddRow("Date (Ngày sản xuất):", _result.ManufacturingDate?.ToString("dd/MM/yyyy"));
                                AddRow("Expiry Date (Ngày hết hạn):", _result.ExpiryDate?.ToString("dd/MM/yyyy"));
                                AddRow("Product Code:", _result.ProductCode);
                                AddRow("Lot No. (Lô sản xuất):", _result.BatchId);
                                AddRow("Quantity (Số lượng):", $"{_result.Weight} KG");
                                AddRow("Company (Tên công ty):", "LONG GIANG CHEMICAL CO.,LTD");
                            });

                            // Bên phải là ảnh
                            row.ConstantItem(90).AlignMiddle().AlignLeft().Element(e =>
                            {
                                var imagePath = "wwwroot/images/BagType/LongGiangBag.jpg";
                                if (File.Exists(imagePath))
                                    e.Image(Image.FromFile(imagePath)).FitHeight().FitWidth();
                                else
                                    e.Text("Logo").FontSize(10);
                            });
                        });
                    }

                    else
                    {
                        column.Item().PaddingBottom(5).Table(table =>
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

                            AddRow("Product name (Tên sản phẩm):", _result.ProductName);
                            AddRow("Date (Ngày sản xuất):", _result.ManufacturingDate?.ToString("dd/MM/yyyy"));
                            AddRow("Expiry Date (Ngày hết hạn):", _result.ExpiryDate?.ToString("dd/MM/yyyy"));
                            AddRow("Product Code:", _result.ProductCode);
                            AddRow("Lot No. (Lô sản xuất):", _result.BatchId);
                            AddRow("Quantity (Số lượng):", $"{_result.Weight} KG");
                            AddRow("Company (Tên công ty):", "VIETAUS POLYMER CO.,LTD");
                        });

                    }

                    column.Item().PaddingTop(5).AlignCenter().AlignMiddle().Text(text =>
                    {
                        text.Span("It is hereby certified that the product described below is manufactured by ");
                        text.Span("Vietaus Polymer Co.,Ltd").Bold();
                    });


                    var rows = ExtractTestRowsHelper.ExtractTestRows(_result, _specs, _isLongGiangBag);

                    column.Item().PaddingTop(10).PaddingBottom(10).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(20);     // No
                            cols.RelativeColumn(2);      // Test Item
                            cols.RelativeColumn(2);      // Specification
                            cols.RelativeColumn();       // Method
                            cols.ConstantColumn(40);     // Unit
                            cols.RelativeColumn();       // Result
                        });

                        var borderStyle = new Func<IContainer, IContainer>(c => c.Border(1).BorderColor(Colors.Black));

                        // Header
                        table.Header(row =>
                        {
                            borderStyle(row.Cell()).Background(Colors.Grey.Lighten1).AlignCenter().Text("No").Bold();
                            borderStyle(row.Cell()).Background(Colors.Grey.Lighten1).AlignCenter().Text("Test Item").Bold();
                            borderStyle(row.Cell()).Background(Colors.Grey.Lighten1).AlignCenter().Text("Specifications").Bold();
                            borderStyle(row.Cell()).Background(Colors.Grey.Lighten1).AlignCenter().Text("Test method").Bold();
                            borderStyle(row.Cell()).Background(Colors.Grey.Lighten1).AlignCenter().Text("Unit").Bold();
                            borderStyle(row.Cell()).Background(Colors.Grey.Lighten1).AlignCenter().Text("Result").Bold();
                        });

                        // Rows
                        int index = 1;
                        string? lastTestItem = null;

                        foreach (var r in rows)
                        {
                            bool showNo = !string.IsNullOrWhiteSpace(r.TestItem) && r.TestItem != lastTestItem;

                            // No
                            if (showNo)
                                borderStyle(table.Cell()).AlignCenter().Padding(2).Text(index++.ToString());
                            else
                                table.Cell().BorderTop(0).BorderBottom(0).BorderLeft(1).BorderRight(1); // ẩn No

                            // Test Item
                            if (showNo)
                            {
                                // Có viền đầy đủ
                                borderStyle(table.Cell()).AlignLeft().Padding(2).Text(r.TestItem ?? "");
                            }
                            else
                            {
                                // Bỏ gạch ngang → chỉ BorderLeft và BorderRight
                                table.Cell()
                                    .BorderLeft(1)
                                    .BorderRight(1)
                                    .BorderTop(0)
                                    .BorderBottom(0)
                                    .AlignCenter()
                                    .Text(r.TestItem ?? "");
                            }

                            // Các cột còn lại vẫn như cũ
                            borderStyle(table.Cell()).AlignCenter().Padding(2).Text(r.Specification);
                            borderStyle(table.Cell()).AlignCenter().Padding(2).Text(r.Method);
                            borderStyle(table.Cell()).AlignCenter().Padding(2).Text(r.Unit);
                            borderStyle(table.Cell()).AlignRight().Padding(2).Text(r.Result ?? "-");

                            if (showNo)
                                lastTestItem = r.TestItem;
                        }
                    });

                    // Dòng xác nhận
                    column.Item().AlignCenter().AlignMiddle().Element(e => e.Text("We certify that the quantity and quality of the above mentioned product are correct.")
                        .FontFamily("Open Sans").FontSize(9));

                });

                // Footer
                if (_isLongGiangBag)
                {
                    page.Footer().Component(new LGFooterComponent());
                }

                else
                {
                    page.Footer().Component(new FooterComponent());
                }

            });
        }
    }

}

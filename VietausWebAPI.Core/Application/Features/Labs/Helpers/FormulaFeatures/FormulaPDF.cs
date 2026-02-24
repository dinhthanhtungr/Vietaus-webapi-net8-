using Microsoft.Extensions.FileSystemGlobbing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ManufacturingVUFormulaFeatures;
using VietausWebAPI.Core.Application.Shared.Helper.Pdfs;
using static VietausWebAPI.Core.Application.Features.Labs.Helpers.FormulaFeatures.FormulaPDF;

namespace VietausWebAPI.Core.Application.Features.Labs.Helpers.FormulaFeatures
{
    public class FormulaPDF : IFormulaPDF
    {
        private static readonly Guid PIGMENT_ID = Guid.Parse("dbca1d46-109c-4219-be56-2e90ff2b6514");
        private static readonly Guid POLYMER_ID = Guid.Parse("7e8177c3-6827-4631-9f3f-72847e544872");
        private static readonly Guid ADDITIVE_ID = Guid.Parse("3904e221-1052-4a12-81e6-bd453108ff53");
        private static readonly Guid OTHER_ID = Guid.Parse("66284761-8fa8-4c75-9ae3-b01cb583c380");

        public byte[] Render(ManufacturingVUPDF data)
        {
            data ??= new ManufacturingVUPDF();

            var doc = Document.Create(c =>
            {
                c.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(t => t.FontFamily("Open Sans").FontSize(9));

                    page.Header().Component(new HeaderComponent());
                    page.Content().Element(x => BuildContent(x, data));
                    page.Footer().Component(new FooterComponent());
                });
            });

            return doc.GeneratePdf();
        }

        private void BuildContent(IContainer c, ManufacturingVUPDF d)
        {
            var s = d.getManufacturingVUFormula ?? new GetManufacturingVUFormula();

            c.Column(col =>
            {
                col.Item().AlignCenter().PaddingTop(5)
                    .Text("PHIẾU CÔNG THỨC (VU)").FontSize(14).Bold();

                col.Item().PaddingTop(5).AlignRight().Row(row =>
                {
                    row.Spacing(25);
                    row.AutoItem().Text($"VU No: {d.BatchNo}");
                    row.AutoItem().Text($"Ngày in: {DateTime.Now:dd/MM/yyyy}");
                });

                col.Item().PaddingBottom(5).Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.ConstantColumn(180);
                        cols.RelativeColumn();
                    });

                    void AddRow(string label, string? value)
                    {
                        table.Cell().Element(InfoCellStyle).Text(label).FontSize(9).SemiBold();
                        table.Cell().Element(InfoCellStyle).Text(string.IsNullOrWhiteSpace(value) ? "-" : value).FontSize(9);
                    }

                    static IContainer InfoCellStyle(IContainer container) =>
                        container.BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .PaddingVertical(3);

                    AddRow("Tên sản phẩm:", s.Name);
                    AddRow("Colour Code:", s.ColourCode);
                    AddRow("Khách hàng:", $"{s.CustomerCode} - {s.CustomerName}".Trim(' ', '-'));

                    AddRow("Tổng SL sản xuất:", s.TotalProductionQuantity?.ToString("N3"));
                    AddRow("Số mẻ (Batches):", s.NumOfBatches?.ToString());
                    AddRow("Kiểm tra:", s.QcCheck);

                    if (!string.IsNullOrWhiteSpace(s.LabNote))
                        AddRow("Ghi chú thực hiện:", s.LabNote);

                    if (!string.IsNullOrWhiteSpace(s.Requirement))
                        AddRow("Yêu cầu của khách:", s.Requirement);


                });

                col.Item().PaddingTop(10).Element(x => BuildMaterialsTable(x, d));
            });
        }

        private void BuildMaterialsTable(IContainer c, ManufacturingVUPDF d)
        {
            var items = (d.materials ?? new List<FormulaPDFMaterialDTOs>())
                .Where(x => x != null && x.Quantity > 0)
                .ToList();


            var s = d.getManufacturingVUFormula ?? new GetManufacturingVUFormula();

            var batchQty = s.TotalProductionQuantity ?? 0m; // 6 (kg) như ảnh UI
            var batches = s.NumOfBatches ?? 1;             // nếu null thì coi như 1
            if (batches < 1) batches = 1;


            if (!items.Any())
            {
                c.Text("Không có nguyên vật liệu.").Italic();
                return;
            }

            decimal unitQtyKg = 1.000000000m;

            var groups = items
                .GroupBy(x => x.CategoryId)
                .OrderBy(g => CategorySortKey(g.Key))
                .ToList();

            c.Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.ConstantColumn(70);   // Code
                    cols.RelativeColumn();     // Name
                    cols.ConstantColumn(110);  // Lot #
                    cols.ConstantColumn(70);   // Std
                    cols.ConstantColumn(90);   // Qty per batch
                    cols.ConstantColumn(90);   // Total qty
                });

                table.Header(h =>
                {
                    h.Cell().Element(HeaderCell).Text("Code").FontSize(8).SemiBold();
                    h.Cell().Element(HeaderCell).Text("Name").FontSize(8).SemiBold();
                    h.Cell().Element(HeaderCell).Text("Lot #").FontSize(8).SemiBold();
                    h.Cell().Element(HeaderCell).AlignRight().Text("Std").FontSize(8).SemiBold();
                    h.Cell().Element(HeaderCell).AlignRight().Text("SL / mẻ").FontSize(8).SemiBold();
                    h.Cell().Element(HeaderCell).AlignRight().Text("SL tổng").FontSize(8).SemiBold();
                });

                decimal sumStd = 0m;
                decimal sumPerBatch = 0m;
                decimal sumTotal = 0m;

                foreach (var g in groups)
                {
                    // ✅ ColumnSpan đúng = 6
                    table.Cell().ColumnSpan(6).Element(GroupTitleCell)
                        .AlignCenter().Text(CategoryTitle(g.Key)).SemiBold();

                    foreach (var m in g.OrderBy(x => x.ExternalId))
                    {
                        var std = m.Quantity;               // Std (kg) theo công thức (tổng ~ 1)
                        var perBatch = std * batchQty;      // ✅ SL mỗi mẻ
                        var total = perBatch * batches;     // ✅ SL tổng

                        sumStd += std;
                        sumPerBatch += perBatch;
                        sumTotal += total;


                        table.Cell().Element(BodyCell).Text(m.ExternalId).FontSize(8);
                        table.Cell().Element(BodyCell).Text(m.MaterialName).FontSize(8).ClampLines(2);
                        table.Cell().Element(BodyCell).Text("-").FontSize(8);

                        table.Cell().Element(BodyCell).AlignRight().Text(FormatKg(std)).FontSize(8);
                        table.Cell().Element(BodyCell).AlignRight().Text(FormatKg(perBatch)).FontSize(8);
                        table.Cell().Element(BodyCell).AlignRight().Text(FormatKg(total)).FontSize(8);

                    }
                }
                table.Cell().ColumnSpan(3).Element(TotalLabelCell)
                    .AlignCenter().Text("Tổng").SemiBold();

                table.Cell().Element(TotalCell).AlignRight().Text(FormatKg(sumStd)).SemiBold();
                table.Cell().Element(TotalCell).AlignRight().Text(FormatKg(sumPerBatch)).SemiBold();
                table.Cell().Element(TotalCell).AlignRight().Text(FormatKg(sumTotal)).SemiBold();

            });

            static IContainer HeaderCell(IContainer x) =>
                x.Border(1).BorderColor(Colors.Black)
                    .Background(Colors.Grey.Lighten2)
                    .PaddingVertical(2).PaddingHorizontal(2);

            static IContainer GroupTitleCell(IContainer x) =>
                x.BorderLeft(1).BorderRight(1).BorderBottom(1).BorderTop(1)
                    .BorderColor(Colors.Black)
                    .PaddingVertical(2).PaddingHorizontal(2);

            static IContainer BodyCell(IContainer x) =>
                x.BorderLeft(1).BorderRight(1).BorderBottom(1)
                    .BorderColor(Colors.Black)
                    .PaddingVertical(2).PaddingHorizontal(2);

            static IContainer TotalLabelCell(IContainer x) =>
                x.BorderLeft(1).BorderRight(1).BorderBottom(1)
                    .BorderColor(Colors.Black)
                    .PaddingVertical(2).PaddingHorizontal(2);

            static IContainer TotalCell(IContainer x) =>
                x.BorderLeft(1).BorderRight(1).BorderBottom(1)
                    .BorderColor(Colors.Black)
                    .PaddingVertical(2).PaddingHorizontal(2);


        }

        private static string FormatKg(decimal v) => v.ToString("G29");

        private static string FormatKg(decimal? v) => v.HasValue ? v.Value.ToString("G29") : "-";



        private int CategorySortKey(Guid categoryId)
        {
            if (categoryId == PIGMENT_ID) return 1;
            if (categoryId == ADDITIVE_ID) return 2;
            if (categoryId == POLYMER_ID) return 3;
            if (categoryId == OTHER_ID) return 4;
            return 99;
        }

        private string CategoryTitle(Guid categoryId)
        {
            if (categoryId == PIGMENT_ID) return "Pigments/Bột màu";
            if (categoryId == ADDITIVE_ID) return "Additives/Phụ gia";
            if (categoryId == POLYMER_ID) return "Plastic/Nhựa";
            return "Others/Khác";
        }
    }
}


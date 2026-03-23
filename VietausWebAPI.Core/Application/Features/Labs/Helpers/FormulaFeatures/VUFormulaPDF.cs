using Microsoft.Extensions.FileSystemGlobbing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ManufacturingVUFormulaFeatures;
using VietausWebAPI.Core.Application.Shared.Helper.Pdfs;

namespace VietausWebAPI.Core.Application.Features.Labs.Helpers.FormulaFeatures
{
    public class VUFormulaPDF : IVUFormulaPDF
    {
        private static readonly Guid PIGMENT_ID = Guid.Parse("d5fdf825-cca1-445c-9cdb-0bae7bd0d75b");
        private static readonly Guid POLYMER_ID = Guid.Parse("04fbe69b-dbd1-45ee-9361-50031955681b");
        private static readonly Guid ADDITIVE_ID = Guid.Parse("dacb8f0f-0152-474b-b719-761f784d6c78");
        private static readonly Guid OTHER_ID = Guid.Parse("da05ebd0-5cb2-4b23-993a-e11407b6734a");
        public byte[] Render(ManufacturingVUPDF data)
        {
            data ??= new ManufacturingVUPDF();

            var doc = Document.Create(c =>
            {
                c.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(10);
                    page.DefaultTextStyle(t => t.FontFamily("Open Sans").FontSize(9));

                    //page.Header().Component(new HeaderComponent());
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
                    .Text("PHIẾU CÔNG THỨC").FontSize(14).Bold();

                col.Item().PaddingTop(5).AlignRight().Row(row =>
                {
                    row.Spacing(25);
                    var batchNo = d.BatchNo ?? "";
                    var isVU = batchNo.StartsWith("VU", StringComparison.OrdinalIgnoreCase);
                    var prefix = isVU ? "VU" : "VA";

                    row.AutoItem().Text(tx =>
                    {
                        tx.Span($"{prefix} No: ").FontSize(9);
                        tx.Span($"{batchNo}").FontSize(11).Black();   // đậm + to hơn

                        tx.Span(" - Batch#: ").FontSize(9);
                        tx.Span($"{d.getManufacturingVUFormula?.FormulaExternalId ?? "-"}")
                          .FontSize(11).Black();
                    });

                    row.AutoItem().Text(tx =>
                    {
                        tx.Span("Ngày in: ").FontSize(9);
                        tx.Span($"{DateTime.Now:dd/MM/yyyy}")
                          .FontSize(9).Black();

                        //tx.Span(" - Ngày yêu cầu: ").FontSize(9);
                        //tx.Span($"{d.RequestDate:dd/MM/yyyy}")
                        //  .FontSize(11).Black();
                    });
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
                        table.Cell().Element(InfoCellStyle).Text(label).FontSize(8).SemiBold();
                        table.Cell().Element(InfoCellStyle).Text(string.IsNullOrWhiteSpace(value) ? "-" : value).FontSize(9);
                    }

                    static IContainer InfoCellStyle(IContainer container) =>
                        container.BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .PaddingVertical(3);


                    // ====== GRID 3 cột giống ảnh (FontSize 8) ======
                    table.Cell().ColumnSpan(2).Element(x =>
                    {
                        x.Table(t =>
                        {
                            t.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(2.2f); // trái
                                cols.RelativeColumn(1.6f); // giữa
                                cols.RelativeColumn(1.2f); // phải
                            });

                            // Tính khối lượng/mẻ
                            decimal? kgPerBatch = null;
                            if (s.NumOfBatches.HasValue && s.NumOfBatches.Value > 0 && s.TotalProductionQuantity.HasValue)
                                kgPerBatch = s.TotalProductionQuantity.Value / s.NumOfBatches.Value;

                            // gợi ý size
                            const float labelSize = 8f;
                            const float valueSize = 10f;   // tăng lên 1 chút cho nổi

                            // Row 1
                            t.Cell().Element(GridCell).Text(tx =>
                            {
                                tx.Span("Khách hàng: ").FontSize(labelSize);
                                tx.Span($"{($"{s.CustomerCode} ".Trim(' ', '-'))}") /*- { s.CustomerName}*/
                                  .FontSize(valueSize).Black();
                            });

                            t.Cell().Element(GridCell).Text(tx =>
                            {
                                tx.Span("Colour Code: ").FontSize(labelSize);
                                tx.Span($"{s.ColourCode ?? ""}")
                                  .FontSize(valueSize).Black();
                            });


                            t.Cell().Element(GridCell).Text(tx =>
                            {
                                tx.Span("Khối lượng/mẻ (Kg): ").FontSize(labelSize);
                                tx.Span(kgPerBatch.HasValue ? $"{FormatKgPretty(kgPerBatch, 2)}" : "-")
                                  .FontSize(valueSize).Black();
                            });


                            // Row 2
                            t.Cell().Element(GridCell).Text(tx =>
                            {
                                tx.Span("Tên sp: ").FontSize(labelSize);
                                tx.Span($"{s.Name ?? ""}")
                                  .FontSize(valueSize).Black();
                            });

                            t.Cell().Element(GridCell).Text(tx =>
                            {
                                tx.Span("Kiểm tra: ").FontSize(labelSize);
                                tx.Span($"{s.QcCheck ?? ""}")
                                  .FontSize(valueSize).Black();
                            });



                            t.Cell().Element(GridCell).Text(tx =>
                            {
                                tx.Span("Số mẻ: ").FontSize(labelSize);
                                tx.Span($"{s.NumOfBatches?.ToString() ?? ""}")
                                  .FontSize(valueSize).Black();
                            });

                            // Row 3 (mới): 1 ô nội dung + 2 ô trống
                            t.Cell().Element(GridCell).Text(tx =>
                            {
                                tx.Span("Ngày yêu cầu: ").FontSize(labelSize);
                                tx.Span($"{d.RequestDate:dd/MM/yyyy}")
                                  .FontSize(valueSize).Black();
                            });
                            t.Cell().Element(GridCell).Text(tx =>
                            {
                                tx.Span("Tỷ lệ sử dụng: ").FontSize(labelSize);
                                tx.Span($"{s.userRate?.ToString() ?? ""} %")
                                  .FontSize(valueSize).Black();
                            });
                            t.Cell().Element(GridCell).Text(tx =>
                            {
                                tx.Span("Tổng khối lượng (kg): ").FontSize(labelSize);
                                tx.Span($"{FormatKgPretty(s.TotalProductionQuantity, 2)}")
                                  .FontSize(valueSize).Black();
                            });

                        });

                        static IContainer GridCell(IContainer c) =>
                            c.Border(1).BorderColor(Colors.Black)
                             .PaddingVertical(2).PaddingHorizontal(4)
                             .AlignMiddle();
                    });


                    var LabNote = s.LabNote;
                    var Requirement = s.Requirement;

                    if (!string.IsNullOrWhiteSpace(LabNote) || !string.IsNullOrWhiteSpace(Requirement))
                    {
                        table.Cell().ColumnSpan(2)
                            .PaddingTop(6)
                            .Element(x => BuildNoteRequirementBox(x, LabNote, Requirement));
                    }

                });

                col.Item().PaddingTop(10).Element(x => BuildMaterialsTable(x, d));

                col.Item().PaddingTop(10).Element(x => BuildMachineSetupSection(x, d));

            });
        }

        private static void BuildNoteRequirementBox(IContainer c, string? LabNote, string? requirement)
        {
            c.Table(t =>
            {
                t.ColumnsDefinition(cols =>
                {
                    cols.RelativeColumn(1);  // Ghi chú thực hiện
                    cols.RelativeColumn(1);  // Yêu cầu ca SX và QC thực hiện
                });

                // Header row (xám)
                t.Cell().Element(BoxHeaderCell).Text("Ghi chú thực hiện:").SemiBold();
                t.Cell().Element(BoxHeaderCell).Text("Yêu cầu ca SX và QC thực hiện:").SemiBold();

                // Content row
                t.Cell().Element(BoxBodyCell).Text(string.IsNullOrWhiteSpace(LabNote) ? "-" : LabNote).FontSize(9);
                t.Cell().Element(BoxBodyCell).Text(string.IsNullOrWhiteSpace(requirement) ? "-" : requirement).FontSize(9);
            });

            static IContainer BoxHeaderCell(IContainer x) =>
                x.Border(1).BorderColor(Colors.Black)
                 .Background(Colors.Grey.Lighten2)
                 .PaddingVertical(4).PaddingHorizontal(6);

            static IContainer BoxBodyCell(IContainer x) =>
                x.Border(1).BorderColor(Colors.Black)
                 .MinHeight(35)              // tăng/giảm tuỳ bạn, giống khung ảnh
                 .PaddingVertical(4).PaddingHorizontal(6)
                 .AlignTop();
        }

        private void BuildMaterialsTable(IContainer c, ManufacturingVUPDF d)
        {
            var batchNo = d.BatchNo ?? "";
            var isVU = batchNo.StartsWith("VU", StringComparison.OrdinalIgnoreCase);

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
                    cols.RelativeColumn(1.50f); // Code
                    cols.RelativeColumn(3.40f); // Name (rộng hơn)
                    cols.RelativeColumn(1.10f); // Lot #
                    cols.RelativeColumn(1.20f); // Std (kg)
                    cols.RelativeColumn(1.35f); // SL / mẻ (kg)
                    cols.RelativeColumn(1.25f); // SL / mẻ (g)
                    cols.RelativeColumn(1.45f); // SL tổng (kg)
                });

                table.Header(h =>
                {
                    h.Cell().Element(HeaderCell).Text("Code").FontSize(9).Bold();
                    h.Cell().Element(HeaderCell).Text("Name").FontSize(9).Bold();
                    h.Cell().Element(HeaderCell).Text("Lot #").FontSize(9).Bold();
                    h.Cell().Element(HeaderCell).AlignMiddle().Text("Std").FontSize(9).Bold();

                    h.Cell().Element(HeaderCell).AlignMiddle().Text("SL / mẻ (kg)").FontSize(9).Bold();
                    h.Cell().Element(HeaderCell).AlignMiddle().Text("SL / mẻ (g)").FontSize(9).Bold();

                    h.Cell().Element(HeaderCell).AlignMiddle().Text("SL tổng (kg)").FontSize(9).Bold();
                });

                decimal sumStd = 0m;
                decimal sumPerBatch = 0m;
                decimal sumTotal = 0m;

                foreach (var g in groups)
                {
                    // ✅ ColumnSpan đúng = 7
                    table.Cell().ColumnSpan(7).Element(GroupTitleCell)
                        .AlignCenter().Text(CategoryTitle(g.Key)).SemiBold();

                    foreach (var m in g)
                    {
                        var std = m.Quantity;

                        decimal perBatchKg = 0m;
                        decimal totalKg = 0m;

                        perBatchKg = std * (batchQty / batches);
                        totalKg = perBatchKg * batches; // = std * batchQty

                        sumStd += std;
                        sumTotal += totalKg;
                        sumPerBatch += perBatchKg;

                        table.Cell().Element(BodyCell).Text(m.ExternalId).FontSize(10).Bold();
                        table.Cell().Element(BodyCell).Text(m.MaterialName).FontSize(10).ClampLines(2).Bold();
                        table.Cell().Element(BodyCell)
                            .Text(string.IsNullOrWhiteSpace(m.LotNo) ? "-" : m.LotNo)
                            .FontSize(8)
                            .Bold();

                        table.Cell().Element(BodyCell).AlignRight().Text(FormatKgPretty(std, 7)).FontSize(10).Bold();

                        table.Cell().Element(BodyCell).AlignRight().Text(FormatKgPretty(perBatchKg, 7)).FontSize(10).Bold();
                        table.Cell().Element(BodyCell).AlignRight().Text(FormatGPretty(perBatchKg, 4)).FontSize(10).Bold();

                        table.Cell().Element(BodyCell).AlignRight().Text(FormatKgPretty(totalKg, 7)).FontSize(10).Bold();
                    }
                }
                table.Cell().ColumnSpan(3).Element(TotalLabelCell)
                    .AlignCenter().Text("Tổng").SemiBold();

                table.Cell().Element(TotalCell).AlignRight().Text(FormatKgPretty(sumStd, 7)).FontSize(10).Bold();
                table.Cell().Element(TotalCell).AlignRight().Text(FormatKgPretty(sumPerBatch, 7)).FontSize(10).Bold();
                table.Cell().Element(TotalCell).AlignRight().Text(FormatGPretty(sumPerBatch, 4)).FontSize(10).Bold();
                table.Cell().Element(TotalCell).AlignRight().Text(FormatKgPretty(sumTotal, 7)).FontSize(10).Bold();

            });

            static IContainer HeaderCell(IContainer x) =>
                x.Border(0.5f).BorderColor(Colors.Black)
                    .Background(Colors.Grey.Lighten2)
                    .PaddingVertical(2).PaddingHorizontal(2);

            static IContainer GroupTitleCell(IContainer x) =>
                x.BorderLeft(0.5f).BorderRight(0.5f).BorderBottom(0.5f).BorderTop(0.5f)
                    .BorderColor(Colors.Black)
                    .PaddingVertical(2).PaddingHorizontal(2);

            static IContainer BodyCell(IContainer x) =>
                x.BorderLeft(0.5f).BorderRight(0.5f).BorderBottom(0.5f)
                 .BorderColor(Colors.Black)
                 .PaddingVertical(2).PaddingHorizontal(4); // tăng ngang

            static IContainer TotalCell(IContainer x) =>
                x.BorderLeft(0.5f).BorderRight(0.5f).BorderBottom(0.5f)
                 .BorderColor(Colors.Black)
                 .PaddingVertical(2).PaddingHorizontal(4);

            static IContainer TotalLabelCell(IContainer x) =>
                x.BorderLeft(0.5f).BorderRight(0.5f).BorderBottom(0.5f)
                    .BorderColor(Colors.Black)
                    .PaddingVertical(2).PaddingHorizontal(2);

        }
        private static readonly CultureInfo _inv = CultureInfo.InvariantCulture;

        // Không scientific notation (E), giữ tối đa N chữ số thập phân (mặc định 50)
        private static string NoSci(decimal v, int decimals = 50)
        {
            // 0.########.... (decimals lần) => không E, không trailing zero thừa
            var fmt = "0." + new string('#', decimals);
            return v.ToString(fmt, _inv);
        }

        private static string NoSci(decimal? v, int decimals = 50)
            => v.HasValue ? NoSci(v.Value, decimals) : "-";

        // FormatKg = wrapper để bạn không cần sửa các chỗ gọi Text(FormatKg(...))
        private static string FormatKg(decimal v) => FormatKgPretty(v, 6);
        private static string FormatKg(decimal? v) => FormatKgPretty(v, 2);

        private static string FormatG(decimal v) => FormatGPretty(v, 0);
        private static string FormatG(decimal? v) => FormatGPretty(v, 0);

        private static string Fmt(decimal v, int decimals)
        {
            // 1,234.567 (dấu , hàng nghìn; dấu . thập phân)
            var fmt = "#,##0" + (decimals > 0 ? "." + new string('0', decimals) : "");
            return v.ToString(fmt, CultureInfo.InvariantCulture);
        }

        private static string Fmt(decimal? v, int decimals) => v.HasValue ? Fmt(v.Value, decimals) : "-";

        private static string FormatKgPretty(decimal v, int decimals = 3) => Fmt(v, decimals);
        private static string FormatKgPretty(decimal? v, int decimals = 3) => Fmt(v, decimals);

        private static string FormatGPretty(decimal v, int decimals = 0) => Fmt(v * 1000m, decimals);
        private static string FormatGPretty(decimal? v, int decimals = 0) => v.HasValue ? FormatGPretty(v.Value, decimals) : "-";
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

        private void BuildMachineSetupSection(IContainer c, ManufacturingVUPDF d)
        {
            c.Table(t =>
            {
                t.ColumnsDefinition(cols =>
                {
                    cols.ConstantColumn(55);
                    cols.RelativeColumn();
                    cols.RelativeColumn();
                    cols.RelativeColumn();
                    cols.RelativeColumn();
                    cols.RelativeColumn();
                    cols.RelativeColumn();
                    cols.RelativeColumn();
                });

                t.Cell().ColumnSpan(8).Element(CellHeaderTitle)
                    .AlignCenter()
                    .Text("Thông số cài đặt (yêu cầu trưởng ca SX phải cập nhật tốc độ, nhiệt độ máy đùn thực tế lúc máy đang SX)")
                    .FontSize(9).SemiBold();

                t.Cell().Element(CellHeader).Text("");
                t.Cell().Element(CellHeader).Text("Operator").SemiBold();
                t.Cell().Element(CellHeader).Text("Machine").SemiBold();
                t.Cell().Element(CellHeader).Text("Mfg Date").SemiBold();
                t.Cell().Element(CellHeader).Text("Batch #").SemiBold();
                t.Cell().Element(CellHeader).Text("Total Qty (kg)").SemiBold();
                t.Cell().Element(CellHeader).Text("Time (h)").SemiBold();
                t.Cell().Element(CellHeader).Text("Prod (kgs/h)").SemiBold();

                t.Cell().Element(CellPrev).Text("Previous").SemiBold();
                t.Cell().Element(CellBody).Text("");
                t.Cell().Element(CellBody).Text("");
                t.Cell().Element(CellBody).Text("");
                t.Cell().Element(CellBody).Text(d.BatchNo ?? "");
                t.Cell().Element(CellBody).Text("");
                t.Cell().Element(CellBody).Text("");
                t.Cell().Element(CellBody).Text("");

                // Zone 1-6
                t.Cell().Element(CellPrev).Text("");
                t.Cell().Element(CellZoneHeader).Text("Rpm").SemiBold();
                t.Cell().Element(CellZoneHeader).Text("Zone 1(°C)").SemiBold();
                t.Cell().Element(CellZoneHeader).Text("Zone 2(°C)").SemiBold();
                t.Cell().Element(CellZoneHeader).Text("Zone 3(°C)").SemiBold();
                t.Cell().Element(CellZoneHeader).Text("Zone 4(°C)").SemiBold();
                t.Cell().Element(CellZoneHeader).Text("Zone 5(°C)").SemiBold();
                t.Cell().Element(CellZoneHeader).Text("Zone 6(°C)").SemiBold();

                t.Cell().Element(CellPrev).Text("Previous").SemiBold();
                for (int i = 0; i < 7; i++) t.Cell().Element(CellBody).Text("");

                // Zone 7-11 + Die
                t.Cell().Element(CellPrev).Text("");
                t.Cell().Element(CellZoneHeader).Text("Zone 7(°C)").SemiBold();
                t.Cell().Element(CellZoneHeader).Text("Zone 8(°C)").SemiBold();
                t.Cell().Element(CellZoneHeader).Text("Zone 9(°C)").SemiBold();
                t.Cell().Element(CellZoneHeader).Text("Zone 10(°C)").SemiBold();
                t.Cell().Element(CellZoneHeader).Text("Zone 11(°C)").SemiBold();
                t.Cell().Element(CellZoneHeader).Text("Die-head(°C)").SemiBold();
                t.Cell().Element(CellZoneHeader).Text("");

                t.Cell().Element(CellPrev).Text("Previous").SemiBold();
                for (int i = 0; i < 7; i++) t.Cell().Element(CellBody).Text("");

                t.Cell().ColumnSpan(8).Element(CellNote)
                    .Text("Note/Lưu ý: tolerance temperature permitted up - down 10°C/nhiệt độ cài đặt được phép tăng giảm trong khoảng 10°C")
                    .FontSize(8);

                // signature table
                t.Cell().ColumnSpan(8).Element(SignatureSection);
            });

            // styles (giữ y hệt bạn đang có)
            static IContainer CellHeaderTitle(IContainer x) => x.Border(1).BorderColor(Colors.Black)
                .Background(Colors.Grey.Lighten3).PaddingVertical(4).PaddingHorizontal(4);

            static IContainer CellHeader(IContainer x) => x.Border(1).BorderColor(Colors.Black)
                .Background(Colors.Grey.Lighten2).PaddingVertical(3).PaddingHorizontal(3);

            static IContainer CellZoneHeader(IContainer x) => x.Border(1).BorderColor(Colors.Black)
                .Background(Colors.Grey.Lighten2).PaddingVertical(3).PaddingHorizontal(3).AlignCenter();

            static IContainer CellPrev(IContainer x) => x.Border(1).BorderColor(Colors.Black)
                .Background(Colors.Grey.Lighten3).PaddingVertical(3).PaddingHorizontal(3);

            static IContainer CellBody(IContainer x) => x.Border(1).BorderColor(Colors.Black)
                .PaddingVertical(3).PaddingHorizontal(3);

            static IContainer CellNote(IContainer x) => x.Border(1).BorderColor(Colors.Black)
                .PaddingVertical(3).PaddingHorizontal(3);

            static void SigRow(TableDescriptor sig, string label, bool includeDateCells = false)
            {
                sig.Cell().Element(CellBody).Text(label);
                sig.Cell().Element(CellBody).Text("");

                if (includeDateCells)
                    sig.Cell().Element(CellBody).Row(r => { r.RelativeItem().Text(""); r.ConstantItem(40).Text("Date").SemiBold(); });
                else
                    sig.Cell().Element(CellBody).Text("");

                if (includeDateCells)
                    sig.Cell().Element(CellBody).Row(r => { r.RelativeItem().Text(""); r.ConstantItem(40).Text("Date").SemiBold(); });
                else
                    sig.Cell().Element(CellBody).Text("");
            }


        }

        private static void SignatureSection(IContainer c)
        {
            c.Border(1).BorderColor(Colors.Black).Padding(0).Column(col =>
            {
                // header row
                col.Item().Row(r =>
                {
                    r.RelativeItem().Element(HeaderBox).Text("Thành phẩm").SemiBold();
                    r.RelativeItem().Element(HeaderBox).Text("Người tổng kết").SemiBold();
                    r.RelativeItem().Element(HeaderBox).Text("Check by (Lab)").SemiBold();
                    r.RelativeItem().Element(HeaderBox).Text("Approved").SemiBold();
                });

                // body: 1 hàng cao để ký (nhìn là biết ký ở đâu)
                col.Item().Height(70).Row(r =>
                {
                    r.RelativeItem().Element(BodyBox).Column(c1 =>
                    {
                        c1.Item().Text("Hàng đạt / Không đạt / Hao hụt").FontSize(8);
                        c1.Item().PaddingTop(6).Text("☐ Hàng đạt    ☐ Hàng không đạt    ☐ Hao hụt").FontSize(8);
                    });

                    r.RelativeItem().Element(BodyBox).AlignCenter().AlignTop().Text("Ký & ghi rõ họ tên");
                    r.RelativeItem().Element(BodyBox).AlignCenter().AlignTop().Text("Ký & ghi rõ họ tên");
                    r.RelativeItem().Element(BodyBox).AlignCenter().AlignTop().Text("Ký & ghi rõ họ tên");
                });

                // date row (nhỏ thôi)
                col.Item().Row(r =>
                {
                    r.RelativeItem().Element(DateBox).Text("Ngày hoàn thành: .......... / .......... / ..........").FontSize(8);
                    r.RelativeItem().Element(DateBox).Text("Date: ....../....../......").FontSize(8);
                    r.RelativeItem().Element(DateBox).Text("Date: ....../....../......").FontSize(8);
                    r.RelativeItem().Element(DateBox).Text("Date: ....../....../......").FontSize(8);
                });
            });

            static IContainer HeaderBox(IContainer x) =>
                x.BorderRight(1).BorderColor(Colors.Black)
                 .Background(Colors.Grey.Lighten2)
                 .PaddingVertical(3).PaddingHorizontal(4);

            static IContainer BodyBox(IContainer x) =>
                x.BorderRight(1).BorderColor(Colors.Black)
                 .BorderTop(1).BorderColor(Colors.Black)
                 .Padding(6);

            static IContainer DateBox(IContainer x) =>
                x.BorderRight(1).BorderColor(Colors.Black)
                 .BorderTop(1).BorderColor(Colors.Black)
                 .PaddingVertical(3).PaddingHorizontal(4);
        }
    }
}

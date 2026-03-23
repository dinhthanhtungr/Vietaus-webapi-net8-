using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.QCInputByQCFeatures;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.Helpers.QCInputByQCFeatures
{
    public class ExportQCInputByQCExcel : IExportQCInputByQCExcel
    {
        public byte[] ExportExcel(QCInputByQCExcelExportData data)
        {
            data ??= new QCInputByQCExcelExportData();
            data.DetailRows ??= new List<QCInputByQCExportRow>();
            data.ReportRows ??= new List<GetSummaryQCInput>();

            using var wb = new XLWorkbook();

            BuildDetailSheet(wb, data.DetailRows);

            var categoryReport = BuildCategoryReport(data.ReportRows);
            var qcStatusReport = BuildQcStatusReport(data.ReportRows);

            BuildReportSheet(wb, categoryReport, qcStatusReport);

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        private void BuildDetailSheet(XLWorkbook wb, List<QCInputByQCExportRow> rows)   
        {
            var ws = wb.Worksheets.Add("QCInput");

            ws.Cell(1, 1).Value = "DANH SÁCH KIỂM QC NGUYÊN VẬT LIỆU";
            ws.Range(1, 1, 1, 12).Merge();
            ws.Range(1, 1, 1, 12).Style.Font.Bold = true;
            ws.Range(1, 1, 1, 12).Style.Font.FontSize = 14;
            ws.Range(1, 1, 1, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range(1, 1, 1, 12).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            var headerRow = 3;
            string[] headers =
            {
                "Mã số",
                "Tên",
                "Nhập kho",
                "Nhà cung cấp",
                "Kết quả",
                "Người kiểm",
                "Ngày kiểm",
                "Ngày tạo",
                "Lot #",
                "Khối lượng giao hàng (kg)",
                "Khối lượng thực nhận (kg)",
                "Ghi chú"
            };

            for (int c = 0; c < headers.Length; c++)
            {
                ws.Cell(headerRow, c + 1).Value = headers[c];
            }

            StyleHeader(ws.Range(headerRow, 1, headerRow, headers.Length));

            var r = headerRow + 1;
            foreach (var x in rows)
            {
                ws.Cell(r, 1).Value = x.MaterialExternalId;
                ws.Cell(r, 2).Value = x.MaterialName;
                ws.Cell(r, 3).Value = x.ImportWarehouseText;
                ws.Cell(r, 4).Value = x.SupplierName;
                ws.Cell(r, 5).Value = x.ResultText;
                ws.Cell(r, 6).Value = x.InspectorName;
                ws.Cell(r, 7).Value = x.InspectionDate;
                ws.Cell(r, 8).Value = x.CreatedDate;
                ws.Cell(r, 9).Value = x.LotNo;
                ws.Cell(r, 10).Value = x.ActualReceivedQuantityKg;
                ws.Cell(r, 11).Value = x.ActualReceivedQuantityKg;
                ws.Cell(r, 12).Value = x.Note;
                r++;
            }

            var lastDataRow = r - 1;

            ws.Column(7).Style.DateFormat.Format = "dd/MM/yyyy";
            ws.Column(8).Style.DateFormat.Format = "dd/MM/yyyy";
            ws.Column(10).Style.NumberFormat.Format = "#,##0.0";
            ws.Column(11).Style.NumberFormat.Format = "#,##0.0";

            if (lastDataRow >= headerRow + 1)
            {
                var dataRange = ws.Range(headerRow + 1, 1, lastDataRow, headers.Length);
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                dataRange.Style.Alignment.WrapText = true;
            }

            ws.Column(1).Width = 18;
            ws.Column(2).Width = 28;
            ws.Column(3).Width = 18;
            ws.Column(4).Width = 24;
            ws.Column(5).Width = 16;
            ws.Column(6).Width = 20;
            ws.Column(7).Width = 14;
            ws.Column(8).Width = 14;
            ws.Column(9).Width = 20;
            ws.Column(10).Width = 24;
            ws.Column(11).Width = 24;
            ws.Column(12).Width = 30;

            ws.SheetView.FreezeRows(headerRow);
            ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            ws.PageSetup.FitToPages(1, 0);
        }

        private void BuildReportSheet(
            XLWorkbook wb,
            List<QCInputCategoryReportRow> categoryRows,
            List<QCInputQcStatusReportRow> qcRows)
        {
            var ws = wb.Worksheets.Add("BaoCao");

            // title tổng
            ws.Cell(1, 1).Value = "BÁO CÁO QC NGUYÊN LIỆU";
            ws.Range(1, 1, 1, 10).Merge();
            ws.Range(1, 1, 1, 10).Style.Font.Bold = true;
            ws.Range(1, 1, 1, 10).Style.Font.FontSize = 16;
            ws.Range(1, 1, 1, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range(1, 1, 1, 10).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            // ===== block trái =====
            ws.Cell(3, 1).Value = "BẢNG THỐNG KÊ TỪNG LOẠI NGUYÊN LIỆU NHẬP VỀ";
            ws.Range(3, 1, 3, 4).Merge();
            StyleSectionTitle(ws.Range(3, 1, 3, 4));

            ws.Cell(5, 1).Value = "PHÂN LOẠI";
            ws.Cell(5, 2).Value = "SỐ MÃ";
            ws.Cell(5, 3).Value = "SỐ LƯỢNG (kg)";
            ws.Cell(5, 4).Value = "TỶ LỆ (%)";
            StyleHeader(ws.Range(5, 1, 5, 4));

            var leftStartRow = 6;
            var leftRow = leftStartRow;
            foreach (var row in categoryRows)
            {
                ws.Cell(leftRow, 1).Value = row.CategoryName;
                ws.Cell(leftRow, 2).Value = row.ItemCount;
                ws.Cell(leftRow, 3).Value = row.QtyKg;
                ws.Cell(leftRow, 4).Value = row.Percent;
                leftRow++;
            }

            var leftTotalRow = leftRow;
            ws.Cell(leftTotalRow, 1).Value = "TỔNG";
            ws.Cell(leftTotalRow, 2).Value = categoryRows.Sum(x => x.ItemCount);
            ws.Cell(leftTotalRow, 3).Value = categoryRows.Sum(x => x.QtyKg);
            ws.Cell(leftTotalRow, 4).Value = 100m;
            StyleTotalRow(ws.Range(leftTotalRow, 1, leftTotalRow, 4));

            if (leftTotalRow >= leftStartRow)
            {
                var leftRange = ws.Range(6, 1, leftTotalRow, 4);
                leftRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                leftRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                leftRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }

            // ===== block phải =====
            ws.Cell(3, 6).Value = "BẢNG THỂ HIỆN KẾT QUẢ QC NGUYÊN LIỆU";
            ws.Range(3, 6, 3, 9).Merge();
            StyleSectionTitle(ws.Range(3, 6, 3, 9));

            ws.Cell(5, 6).Value = "KẾT QUẢ";
            ws.Cell(5, 7).Value = "SỐ MÃ";
            ws.Cell(5, 8).Value = "SỐ LƯỢNG (kg)";
            ws.Cell(5, 9).Value = "TỶ LỆ (%)";
            StyleHeader(ws.Range(5, 6, 5, 9));

            var rightStartRow = 6;
            var rightRow = rightStartRow;
            foreach (var row in qcRows)
            {
                ws.Cell(rightRow, 6).Value = row.StatusName;
                ws.Cell(rightRow, 7).Value = row.ItemCount;
                ws.Cell(rightRow, 8).Value = row.QtyKg;
                ws.Cell(rightRow, 9).Value = row.Percent;
                rightRow++;
            }

            var rightTotalRow = rightRow;
            ws.Cell(rightTotalRow, 6).Value = "TỔNG";
            ws.Cell(rightTotalRow, 7).Value = qcRows.Sum(x => x.ItemCount);
            ws.Cell(rightTotalRow, 8).Value = qcRows.Sum(x => x.QtyKg);
            ws.Cell(rightTotalRow, 9).Value = 100m;
            StyleTotalRow(ws.Range(rightTotalRow, 6, rightTotalRow, 9));

            if (rightTotalRow >= rightStartRow)
            {
                var rightRange = ws.Range(6, 6, rightTotalRow, 9);
                rightRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                rightRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                rightRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }

            // format cột
            ws.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Column(3).Style.NumberFormat.Format = "#,##0.0";
            ws.Column(4).Style.NumberFormat.Format = "#,##0.0";

            ws.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Column(8).Style.NumberFormat.Format = "#,##0.0";
            ws.Column(9).Style.NumberFormat.Format = "#,##0.0";

            // width
            ws.Column(1).Width = 28;
            ws.Column(2).Width = 14;
            ws.Column(3).Width = 20;
            ws.Column(4).Width = 14;

            ws.Column(5).Width = 4; // cột trống ngăn cách

            ws.Column(6).Width = 24;
            ws.Column(7).Width = 14;
            ws.Column(8).Width = 20;
            ws.Column(9).Width = 14;

            ws.SheetView.FreezeRows(5);
            ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            ws.PageSetup.FitToPages(1, 0);
        }

        private List<QCInputCategoryReportRow> BuildCategoryReport(List<GetSummaryQCInput> reportRows)
        {
            var rows = reportRows
                .GroupBy(x => string.IsNullOrWhiteSpace(x.CategoryName) ? "KHÁC" : x.CategoryName.Trim().ToUpper())
                .Select(g => new QCInputCategoryReportRow
                {
                    CategoryName = g.Key,
                    ItemCount = g.Select(x => x.ExternalId).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().Count(),
                    QtyKg = g.Sum(x => x.QtyKg)
                })
                .OrderByDescending(x => x.ItemCount)
                .ToList();

            var total = rows.Sum(x => x.ItemCount);

            foreach (var row in rows)
            {
                row.Percent = total == 0
                    ? 0
                    : Math.Round(row.ItemCount * 100m / total, 1);
            }

            return rows;
        }

        private List<QCInputQcStatusReportRow> BuildQcStatusReport(List<GetSummaryQCInput> reportRows)
        {
            var qcTypes = new[]
            {
                VoucherDetailType.QCPass,
                VoucherDetailType.QCFail,
                VoucherDetailType.Waiter,
                VoucherDetailType.Special
            };

            var raw = reportRows
                .Where(x => qcTypes.Contains(x.VoucherType))
                .GroupBy(x => x.VoucherType)
                .Select(g => new QCInputQcStatusReportRow
                {
                    StatusName = MapQcStatusName(g.Key),
                    ItemCount = g.Select(x => x.ExternalId).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().Count(),
                    QtyKg = g.Sum(x => x.QtyKg)
                })
                .OrderByDescending(x => x.ItemCount)
                .ToList();

            var total = raw.Sum(x => x.ItemCount);

            foreach (var row in raw)
            {
                row.Percent = total == 0
                    ? 0
                    : Math.Round(row.ItemCount * 100m / total, 1);
            }

            return new List<QCInputQcStatusReportRow>
            {
                MergeQcRow("HÀNG ĐẠT", raw),
                MergeQcRow("HÀNG LỖI", raw),
                MergeQcRow("CHỜ QC", raw),
                MergeQcRow("CHẤP NHẬN ĐẶC BIỆT", raw)
            };
        }

        private static QCInputQcStatusReportRow MergeQcRow(string statusName, List<QCInputQcStatusReportRow> source)
        {
            var row = source.FirstOrDefault(x => x.StatusName == statusName);
            return row ?? new QCInputQcStatusReportRow
            {
                StatusName = statusName,
                ItemCount = 0,
                QtyKg = 0,
                Percent = 0
            };
        }

        private static string MapQcStatusName(VoucherDetailType type) => type switch
        {
            VoucherDetailType.QCPass => "HÀNG ĐẠT",
            VoucherDetailType.QCFail => "HÀNG LỖI",
            VoucherDetailType.Waiter => "CHỜ QC",
            VoucherDetailType.Special => "CHẤP NHẬN ĐẶC BIỆT",
            _ => "KHÁC"
        };

        private static void StyleHeader(IXLRange range)
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.BackgroundColor = XLColor.FromArgb(255, 242, 204);
            range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        private static void StyleTotalRow(IXLRange range)
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.BackgroundColor = XLColor.FromArgb(242, 220, 219);
            range.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        private static void StyleSectionTitle(IXLRange range)
        {
            range.Style.Font.Bold = true;
            range.Style.Font.FontSize = 12;
            range.Style.Fill.BackgroundColor = XLColor.FromArgb(221, 235, 247);
            range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            range.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }
    }
}
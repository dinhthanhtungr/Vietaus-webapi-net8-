using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.QCInputByQCFeatures;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.Helpers.QCInputByQCFeatures
{
    public class ExportQCInputByQCExcel : IExportQCInputByQCExcel
    {
        public byte[] ExportExcel(List<QCInputByQCExportRow> rows)
        {
            rows ??= new List<QCInputByQCExportRow>();

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("QCInput");

            // ===== Title =====
            ws.Cell(1, 1).Value = "DANH SÁCH KIỂM QC NGUYÊN VẬT LIỆU";
            ws.Range(1, 1, 1, 12).Merge();
            ws.Range(1, 1, 1, 12).Style.Font.Bold = true;
            ws.Range(1, 1, 1, 12).Style.Font.FontSize = 14;
            ws.Range(1, 1, 1, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range(1, 1, 1, 12).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            // ===== Header =====
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

            var headerRange = ws.Range(headerRow, 1, headerRow, headers.Length);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(230, 230, 230);
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            // ===== Data =====
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

            // ===== Format =====
            ws.Column(7).Style.DateFormat.Format = "dd/MM/yyyy";
            ws.Column(8).Style.DateFormat.Format = "dd/MM/yyyy";

            ws.Column(10).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(11).Style.NumberFormat.Format = "#,##0.00";

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

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }
    }
}

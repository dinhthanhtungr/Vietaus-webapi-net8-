using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices;

namespace VietausWebAPI.Core.Application.Features.Warehouse.Helpers
{
    public class StockAvailableExcel : IStockAvailableExcel
    {
        public byte[] Render(IReadOnlyList<StockAvailableExportRow> rows)
        {
            rows ??= Array.Empty<StockAvailableExportRow>();

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("TonKho");

            // Header
            ws.Cell(1, 1).Value = "Mã";
            ws.Cell(1, 2).Value = "Tên";
            ws.Cell(1, 3).Value = "Loại";
            ws.Cell(1, 4).Value = "Danh mục";
            ws.Cell(1, 5).Value = "Tổng tồn (Kg)";
            ws.Cell(1, 6).Value = "Kệ";
            ws.Cell(1, 7).Value = "Công ty";
            ws.Cell(1, 8).Value = "Lot No";
            ws.Cell(1, 9).Value = "Tồn tại kệ (Kg)";

            var header = ws.Range(1, 1, 1, 9);
            header.Style.Font.Bold = true;
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            header.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            header.Style.Fill.BackgroundColor = XLColor.Yellow;
            header.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            header.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            // Data
            for (int i = 0; i < rows.Count; i++)
            {
                var r = rows[i];
                var row = i + 2;

                ws.Cell(row, 1).SetValue(r.Code);
                ws.Cell(row, 2).SetValue(r.CodeName);
                ws.Cell(row, 3).SetValue(r.StockType);
                ws.Cell(row, 4).SetValue(r.CategoryName);
                ws.Cell(row, 5).SetValue(r.TotalOnHandKg);
                ws.Cell(row, 6).SetValue(r.ShelfStockCode);
                ws.Cell(row, 7).SetValue(r.CompanyName);
                ws.Cell(row, 8).SetValue(r.LotNo ?? "");
                ws.Cell(row, 9).SetValue(r.OnHandKg);
            }

            var lastRow = Math.Max(1, rows.Count + 1);
            var table = ws.Range(1, 1, lastRow, 9);

            table.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            table.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            table.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            // Align
            ws.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            ws.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Column(8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Column(9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

            // Number format
            ws.Column(5).Style.NumberFormat.Format = "#,##0.000";
            ws.Column(9).Style.NumberFormat.Format = "#,##0.000";

            // Wrap text cho cột dài
            ws.Column(2).Style.Alignment.WrapText = true;
            ws.Column(4).Style.Alignment.WrapText = true;
            ws.Column(7).Style.Alignment.WrapText = true;

            // Width
            ws.Columns(1, 1).Width = 18; // Mã
            ws.Columns(2, 2).Width = 28; // Tên
            ws.Columns(3, 3).Width = 18; // Loại
            ws.Columns(4, 4).Width = 22; // Danh mục
            ws.Columns(5, 5).Width = 16; // Tổng tồn
            ws.Columns(6, 6).Width = 20; // Kệ
            ws.Columns(7, 7).Width = 24; // Công ty
            ws.Columns(8, 8).Width = 18; // Lot
            ws.Columns(9, 9).Width = 16; // Tồn tại kệ

            ws.Rows(1, lastRow).AdjustToContents();
            ws.SheetView.FreezeRows(1);

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }
    }
}

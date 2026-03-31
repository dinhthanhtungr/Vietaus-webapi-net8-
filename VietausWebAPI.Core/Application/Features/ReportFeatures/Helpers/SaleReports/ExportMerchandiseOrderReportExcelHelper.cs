using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.Helpers.SaleReports
{
    public class ExportMerchandiseOrderReportExcelHelper
    {
        public byte[] Export(List<SummaryMOReportDto> rows)
        {
            rows ??= new List<SummaryMOReportDto>();

            rows = rows
                .OrderBy(x => x.OrderDate)
                .ThenBy(x => x.DeliveryRequestDate)
                .ThenBy(x => x.CustomerName ?? string.Empty)
                .ThenBy(x => x.MerchandiseOrderCode ?? string.Empty)
                .ThenBy(x => x.ProductCode ?? string.Empty)
                .ToList();

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("BaoCaoDonHang");

            // ===== TITLE =====
            ws.Cell(1, 1).Value = "BÁO CÁO ĐƠN HÀNG / KẾ HOẠCH GIAO";
            ws.Range(1, 1, 1, 15).Merge();
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 16;
            ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            // ===== HEADER =====
            int headerRow = 3;

            string[] headers =
            {
                "STT",
                "Mã đơn hàng",
                "Mã khách hàng",
                "Tên khách hàng",
                "Mã hàng",
                "Tên hàng",
                "Ngày đặt hàng",
                "Ngày yêu cầu giao",
                "Ngày hứa giao",
                "Ngày giao thực tế",
                "SL yêu cầu",
                "SL đã giao",
                "SL còn lại",
                "Đơn giá",
                "Thành tiền",
                "Trạng thái"
            };

            for (int c = 1; c <= headers.Length; c++)
            {
                ws.Cell(headerRow, c).Value = headers[c - 1];
            }

            var headerRange = ws.Range(headerRow, 1, headerRow, headers.Length);
            headerRange.Style.Fill.BackgroundColor = XLColor.Yellow;
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            headerRange.Style.Alignment.WrapText = true;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            // ===== DATA =====
            int startDataRow = headerRow + 1;

            for (int i = 0; i < rows.Count; i++)
            {
                int r = startDataRow + i;
                var item = rows[i];

                ws.Cell(r, 1).Value = i + 1;
                ws.Cell(r, 2).Value = item.MerchandiseOrderCode;
                ws.Cell(r, 3).Value = item.CustomerCode;
                ws.Cell(r, 4).Value = item.CustomerName;
                ws.Cell(r, 5).Value = item.ProductCode;
                ws.Cell(r, 6).Value = item.ProductName;

                ws.Cell(r, 7).Value = item.OrderDate;
                ws.Cell(r, 7).Style.DateFormat.Format = "dd/MM/yyyy";

                ws.Cell(r, 8).Value = item.DeliveryRequestDate;
                ws.Cell(r, 8).Style.DateFormat.Format = "dd/MM/yyyy";

                if (item.ExpectedDeliveryDate.HasValue)
                {
                    ws.Cell(r, 9).Value = item.ExpectedDeliveryDate.Value;
                    ws.Cell(r, 9).Style.DateFormat.Format = "dd/MM/yyyy";
                }

                if (item.ActualDeliveryDate.HasValue)
                {
                    ws.Cell(r, 10).Value = item.ActualDeliveryDate.Value;
                    ws.Cell(r, 10).Style.DateFormat.Format = "dd/MM/yyyy";
                }

                ws.Cell(r, 11).Value = item.RequestedQuantity;
                ws.Cell(r, 12).Value = item.DeliveredQuantity;
                ws.Cell(r, 13).Value = item.RemainingQuantity;
                ws.Cell(r, 14).Value = item.UnitPrice;
                ws.Cell(r, 15).Value = item.TotalPrice;
                ws.Cell(r, 16).Value = item.Status;

                ws.Cell(r, 11).Style.NumberFormat.Format = "#,##0.##";
                ws.Cell(r, 12).Style.NumberFormat.Format = "#,##0.##";
                ws.Cell(r, 13).Style.NumberFormat.Format = "#,##0.##";
                ws.Cell(r, 14).Style.NumberFormat.Format = "#,##0.##";
                ws.Cell(r, 15).Style.NumberFormat.Format = "#,##0.##";

                var rowRange = ws.Range(r, 1, r, headers.Length);
                rowRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                rowRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                rowRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Cell(r, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                ws.Cell(r, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                ws.Cell(r, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                ws.Cell(r, 14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                ws.Cell(r, 15).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                ws.Cell(r, 4).Style.Alignment.WrapText = true;
                ws.Cell(r, 6).Style.Alignment.WrapText = true;
                ws.Cell(r, 16).Style.Alignment.WrapText = true;

                if (item.RemainingQuantity > 0)
                {
                    ws.Cell(r, 13).Style.Font.FontColor = XLColor.Red;
                    ws.Cell(r, 13).Style.Font.Bold = true;
                }
            }

            // ===== SUMMARY =====
            int summaryRow = startDataRow + rows.Count + 2;

            ws.Cell(summaryRow, 1).Value = "Tổng số dòng";
            ws.Cell(summaryRow, 2).Value = rows.Count;

            ws.Cell(summaryRow + 1, 1).Value = "Tổng SL yêu cầu";
            ws.Cell(summaryRow + 1, 2).Value = rows.Sum(x => x.RequestedQuantity);

            ws.Cell(summaryRow + 2, 1).Value = "Tổng SL đã giao";
            ws.Cell(summaryRow + 2, 2).Value = rows.Sum(x => x.DeliveredQuantity);

            ws.Cell(summaryRow + 3, 1).Value = "Tổng SL còn lại";
            ws.Cell(summaryRow + 3, 2).Value = rows.Sum(x => x.RemainingQuantity);

            ws.Cell(summaryRow + 4, 1).Value = "Tổng thành tiền";
            ws.Cell(summaryRow + 4, 2).Value = rows.Sum(x => x.TotalPrice);

            var summaryRange = ws.Range(summaryRow, 1, summaryRow + 4, 2);
            summaryRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            summaryRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            summaryRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            ws.Range(summaryRow, 1, summaryRow + 4, 1).Style.Font.Bold = true;
            ws.Range(summaryRow, 1, summaryRow + 4, 1).Style.Fill.BackgroundColor = XLColor.BrightGreen;

            ws.Cell(summaryRow + 1, 2).Style.NumberFormat.Format = "#,##0.##";
            ws.Cell(summaryRow + 2, 2).Style.NumberFormat.Format = "#,##0.##";
            ws.Cell(summaryRow + 3, 2).Style.NumberFormat.Format = "#,##0.##";
            ws.Cell(summaryRow + 4, 2).Style.NumberFormat.Format = "#,##0.##";

            // ===== WIDTH =====
            ws.Column(1).Width = 8;
            ws.Column(2).Width = 16;
            ws.Column(3).Width = 16;
            ws.Column(4).Width = 24;
            ws.Column(5).Width = 16;
            ws.Column(6).Width = 28;
            ws.Column(7).Width = 14;
            ws.Column(8).Width = 14;
            ws.Column(9).Width = 14;
            ws.Column(10).Width = 14;
            ws.Column(11).Width = 14;
            ws.Column(12).Width = 14;
            ws.Column(13).Width = 14;
            ws.Column(14).Width = 14;
            ws.Column(15).Width = 16;
            ws.Column(16).Width = 16;

            ws.SheetView.FreezeRows(headerRow);
            ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            ws.PageSetup.FitToPages(1, 0);

            int finalRow = summaryRow + 4;
            if (finalRow >= 1)
            {
                ws.Rows(1, finalRow).AdjustToContents();
            }

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }
    }
}

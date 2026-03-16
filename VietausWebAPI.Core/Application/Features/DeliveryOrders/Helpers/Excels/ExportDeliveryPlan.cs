using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs.ExcelBuilds;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.Helpers.Excels
{
    public class ExportDeliveryPlan : IExportDeliveryPlan
    {
        public byte[] ExportDeliveryPlanExcel(List<DeliveryPlanRow> rows)
        {
            rows ??= new List<DeliveryPlanRow>();

            // ====== SORT để các dòng cùng nhóm đứng sát nhau (merge mới hoạt động) ======
            rows = rows
                .OrderBy(x => x.CustomerName ?? "")
                .ThenBy(x => x.Address ?? "")
                .ThenBy(x => x.Factory ?? "")
                .ThenBy(x => x.PickupTimeText ?? "")
                .ThenBy(x => x.Driver ?? "")
                .ThenBy(x => x.Code ?? "")
                .ThenBy(x => x.LotNo ?? "")
                .ToList();

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("DS giao hàng");

            // ====== NOTE ======
            ws.Cell(1, 1).Value = "Ghi chú: dấu (*) thể hiện rằng sản phẩm này chưa sản xuất xong nên chưa có tổng kết số lượng.";
            ws.Range(1, 1, 1, 10).Merge();
            ws.Cell(1, 1).Style.Font.Bold = true;

            // ====== HEADER ======
            var headerRow = 3;
            string[] headers =
            {
                "STT", "Tên khách hàng", "Code", "Số Lot", "Số Lượng",
                "Nhà Máy", "Thời gian dự kiến lấy hàng", "Tài xế", "Ghi Chú", "Địa chỉ"
            };

            for (int c = 1; c <= headers.Length; c++)
                ws.Cell(headerRow, c).Value = headers[c - 1];

            var headerRange = ws.Range(headerRow, 1, headerRow, 10);
            headerRange.Style.Fill.BackgroundColor = XLColor.Yellow;
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws.Row(headerRow).Height = 22;

            // ====== DATA ======
            var startDataRow = headerRow + 1;

            for (int i = 0; i < rows.Count; i++)
            {
                var r = startDataRow + i;
                var item = rows[i];

                ws.Cell(r, 1).Value = item.Stt;
                ws.Cell(r, 2).Value = item.CustomerName;
                ws.Cell(r, 3).Value = item.Code;
                ws.Cell(r, 4).Value = item.LotNo;
                ws.Cell(r, 5).Value = item.QuantityText;
                ws.Cell(r, 6).Value = item.Factory;
                ws.Cell(r, 7).Value = item.PickupTimeText;
                ws.Cell(r, 8).Value = item.Driver;
                ws.Cell(r, 9).Value = item.Note;
                ws.Cell(r, 10).Value = item.Address;

                // style dòng
                var rowRange = ws.Range(r, 1, r, 10);
                rowRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                rowRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                rowRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // canh giữa
                ws.Cell(r, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Quantity đỏ nếu pending
                if (item.QuantityIsPending)
                {
                    ws.Cell(r, 5).Style.Font.FontColor = XLColor.Red;
                    ws.Cell(r, 5).Style.Font.Bold = true;
                }

                // wrap cho address dài
                ws.Cell(r, 10).Style.Alignment.WrapText = true;
            }

            // ====== MERGE theo nhóm giao hàng (liên tiếp) ======
            // merge cột: Tên KH (2), Nhà máy (6), Thời gian (7), Tài xế (8), Địa chỉ (10)
            MergeByGroup(ws, rows, startDataRow,
                colsToMerge: new[] { 2, 6, 7, 8, 10 },
                keySelector: x => $"{x.CustomerName}");

            // ====== WIDTH ======
            ws.Column(1).Width = 6;
            ws.Column(2).Width = 28;
            ws.Column(3).Width = 12;
            ws.Column(4).Width = 24;
            ws.Column(4).Style.Alignment.WrapText = true;
            ws.Column(5).Width = 12;
            ws.Column(6).Width = 10;
            ws.Column(7).Width = 18;
            ws.Column(8).Width = 14;
            ws.Column(9).Width = 14;
            ws.Column(10).Width = 40;

            ws.SheetView.FreezeRows(headerRow);
            ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            ws.PageSetup.FitToPages(1, 0);

            // nếu address wrap -> tăng chiều cao row cho đẹp
            if (rows.Count > 0)
            {
                var lastRow = startDataRow + rows.Count - 1;
                ws.Rows(startDataRow, lastRow).AdjustToContents();
            }

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        private static void MergeByGroup(
            IXLWorksheet ws,
            List<DeliveryPlanRow> rows,
            int startDataRow,
            int[] colsToMerge,
            Func<DeliveryPlanRow, string> keySelector)
        {
            if (rows.Count <= 1) return;

            string KeyAt(int idx)
                => (keySelector(rows[idx]) ?? "").Trim().ToLowerInvariant();

            var firstRow = startDataRow;
            var lastRow = startDataRow + rows.Count - 1;

            int groupStartRow = firstRow;

            for (int excelRow = firstRow + 1; excelRow <= lastRow + 1; excelRow++)
            {
                bool isEnd = (excelRow == lastRow + 1);

                int prevIdx = (excelRow - 1) - firstRow;
                int currIdx = excelRow - firstRow;

                var prevKey = KeyAt(prevIdx);
                var currKey = isEnd ? "__end__" : KeyAt(currIdx);

                if (isEnd || prevKey != currKey)
                {
                    int groupEndRow = excelRow - 1;

                    if (groupEndRow > groupStartRow)
                    {
                        foreach (var col in colsToMerge)
                        {
                            var rng = ws.Range(groupStartRow, col, groupEndRow, col);
                            rng.Merge();
                            rng.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            // nếu merge address thì wrap vẫn giữ
                            if (col == 10)
                                rng.Style.Alignment.WrapText = true;
                        }
                    }

                    groupStartRow = excelRow;
                }
            }
        }


        public byte[] ExportDeliveryFinish(List<DeliveryFinishRow> rows, DateTime fromDate, DateTime toDate)
        {
            rows ??= new List<DeliveryFinishRow>();

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("DeliveryFinish");

            // ===== Title =====
            ws.Cell(1, 1).Value = "BÁO CÁO GIAO HÀNG HOÀN TẤT";
            ws.Range(1, 1, 1, 14).Merge();
            ws.Range(1, 1, 1, 14).Style.Font.Bold = true;
            ws.Range(1, 1, 1, 14).Style.Font.FontSize = 14;
            ws.Range(1, 1, 1, 14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell(2, 1).Value = $"Từ ngày: {fromDate:dd/MM/yyyy}  -  Đến ngày: {toDate:dd/MM/yyyy}";
            ws.Range(2, 1, 2, 14).Merge();
            ws.Range(2, 1, 2, 14).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // ===== Header =====
            var headerRow = 4;
            string[] headers =
            {
                "Mã số", "Đơn hàng", "Ngày nhận đơn hàng", "Ngày yêu cầu giao hàng",
                "Ngày thực tế giao hàng", "Khách hàng", "Người giao", "Sản phẩm",
                "Kho", "Batch #", "Số lượng (kg)", "Số bao", "Số PO", "Ghi chú"
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

            // ===== Body =====
            var r = headerRow + 1;
            foreach (var x in rows)
            {
                ws.Cell(r, 1).Value = x.DeliveryExternalId;
                ws.Cell(r, 2).Value = x.OrderExternalId;

                ws.Cell(r, 3).Value = x.OrderCreatedDate;
                ws.Cell(r, 4).Value = x.DeliveryRequestDate;
                ws.Cell(r, 5).Value = x.DeliveryActualDate;

                ws.Cell(r, 6).Value = x.CustomerName;
                ws.Cell(r, 7).Value = x.DelivererName;
                ws.Cell(r, 8).Value = x.ProductDisplay;

                ws.Cell(r, 9).Value = x.WarehouseDisplay;
                ws.Cell(r, 10).Value = x.LotNoOrBatch;

                ws.Cell(r, 11).Value = x.QuantityKg;
                ws.Cell(r, 12).Value = x.NumOfBags;

                ws.Cell(r, 13).Value = x.PoNo;
                ws.Cell(r, 14).Value = x.Note;

                r++;
            }

            var lastDataRow = r - 1;

            // ===== Format =====
            ws.Column(3).Style.DateFormat.Format = "dd/MM/yyyy";
            ws.Column(4).Style.DateFormat.Format = "dd/MM/yyyy";
            ws.Column(5).Style.DateFormat.Format = "dd/MM/yyyy";

            ws.Column(11).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(12).Style.NumberFormat.Format = "#,##0";

            // border all data
            if (lastDataRow >= headerRow + 1)
            {
                var dataRange = ws.Range(headerRow + 1, 1, lastDataRow, headers.Length);
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                dataRange.Style.Alignment.WrapText = true;
            }

            // freeze header
            ws.SheetView.FreezeRows(headerRow);

            // nice widths
            ws.Columns().AdjustToContents();

            // margins for readability
            ws.PageSetup.Margins.Top = 0.5;
            ws.PageSetup.Margins.Bottom = 0.5;
            ws.PageSetup.Margins.Left = 0.3;
            ws.PageSetup.Margins.Right = 0.3;

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        public byte[] ExportTransportWorkbook(
            DeliveryTransportWorkbookData data,
            DateTime fromDate,
            DateTime toDate)
        {
            data ??= new DeliveryTransportWorkbookData();

            using var wb = new XLWorkbook();

            BuildSummarySheet(wb, data.SummaryRows, fromDate, toDate);

            foreach (var kv in data.DetailByDeliverer.OrderBy(x => x.Key))
            {
                BuildDelivererSheet(wb, kv.Key, kv.Value, fromDate);
            }

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        private void BuildSummarySheet(
            XLWorkbook wb,
            List<DeliveryTransportSummaryRow> rows,
            DateTime fromDate,
            DateTime toDate)
        {
            var ws = wb.Worksheets.Add("Tổng kết");

            ws.Cell(1, 1).Value = "ĐƠN VỊ: CÔNG TY TNHH CƠ KHÍ NHỰA VIỆT ÚC";
            ws.Range(1, 1, 1, 8).Merge();
            ws.Cell(1, 1).Style.Font.Bold = true;

            ws.Cell(2, 1).Value = $"TỪ NGÀY {fromDate:dd/MM/yyyy} ĐẾN NGÀY {toDate:dd/MM/yyyy}";
            ws.Range(2, 1, 2, 8).Merge();
            ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

            ws.Cell(4, 2).Value = $"BÁO CÁO XÁC NHẬN VẬN CHUYỂN";
            ws.Range(4, 2, 4, 7).Merge();
            ws.Range(4, 2, 4, 7).Style.Font.Bold = true;
            ws.Range(4, 2, 4, 7).Style.Font.FontSize = 14;
            ws.Range(4, 2, 4, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            var headerRow = 6;
            string[] headers =
            {
                "STT",
                "Tên tài xế",
                "Số chuyến",
                "Tổng số lượng",
                "Số tiền",
                "Trừ tiền ứng",
                "Còn nhận",
                "Ghi chú"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cell(headerRow, i + 1).Value = headers[i];
            }

            var headerRange = ws.Range(headerRow, 1, headerRow, headers.Length);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.Yellow;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            var currentRow = headerRow + 1;
            foreach (var item in rows)
            {
                ws.Cell(currentRow, 1).Value = item.Stt;
                ws.Cell(currentRow, 2).Value = item.DelivererName;
                ws.Cell(currentRow, 3).Value = item.TotalTrips;
                ws.Cell(currentRow, 4).Value = item.TotalQuantity;
                ws.Cell(currentRow, 5).Value = item.TotalAmount;
                ws.Cell(currentRow, 6).Value = item.AdvanceAmount;
                ws.Cell(currentRow, 7).Value = item.RemainingAmount;
                ws.Cell(currentRow, 8).Value = item.Note;

                currentRow++;
            }

            if (rows.Any())
            {
                var totalRow = currentRow;
                ws.Cell(totalRow, 1).Value = "Tổng cộng";
                ws.Range(totalRow, 1, totalRow, 2).Merge();

                ws.Cell(totalRow, 3).FormulaA1 = $"SUM(C{headerRow + 1}:C{totalRow - 1})";
                ws.Cell(totalRow, 4).FormulaA1 = $"SUM(D{headerRow + 1}:D{totalRow - 1})";
                ws.Cell(totalRow, 5).FormulaA1 = $"SUM(E{headerRow + 1}:E{totalRow - 1})";
                ws.Cell(totalRow, 6).FormulaA1 = $"SUM(F{headerRow + 1}:F{totalRow - 1})";
                ws.Cell(totalRow, 7).FormulaA1 = $"SUM(G{headerRow + 1}:G{totalRow - 1})";

                var totalRange = ws.Range(totalRow, 1, totalRow, 8);
                totalRange.Style.Font.Bold = true;
                totalRange.Style.Fill.BackgroundColor = XLColor.FromArgb(230, 230, 230);
                totalRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                totalRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            }

            ws.Column(1).Width = 8;
            ws.Column(2).Width = 24;
            ws.Column(3).Width = 12;
            ws.Column(4).Width = 15;
            ws.Column(5).Width = 16;
            ws.Column(6).Width = 16;
            ws.Column(7).Width = 16;
            ws.Column(8).Width = 30;

            ws.Column(4).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(5).Style.NumberFormat.Format = "#,##0";
            ws.Column(6).Style.NumberFormat.Format = "#,##0";
            ws.Column(7).Style.NumberFormat.Format = "#,##0";

            ws.SheetView.FreezeRows(headerRow);
        }

        private void BuildDelivererSheet(
            XLWorkbook wb,
            string delivererName,
            List<DeliveryTransportReportRow> rows,
            DateTime fromDate)
        {
            var sheetName = SafeSheetName(delivererName);
            var ws = wb.Worksheets.Add(sheetName);

            ws.Cell(1, 4).Value = "BẢNG THEO DÕI VẬN CHUYỂN";
            ws.Range(1, 4, 1, 8).Merge();
            ws.Range(1, 4, 1, 8).Style.Font.Bold = true;
            ws.Range(1, 4, 1, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell(2, 4).Value = $"Tháng {fromDate:MM/yyyy}";
            ws.Range(2, 4, 2, 8).Merge();
            ws.Range(2, 4, 2, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Cell(3, 4).Value = $"Người vận chuyển: {delivererName}";
            ws.Range(3, 4, 3, 8).Merge();
            ws.Range(3, 4, 3, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range(3, 4, 3, 8).Style.Font.Bold = true;

            var headerRow = 5;
            string[] headers =
            {
                "STT",
                "Ngày giao",
                "Mã giao hàng",
                "Khách hàng",
                "Địa chỉ",
                "Sản phẩm",
                "Số lượng",
                "Số bao",
                "Số PO",
                "Lot No",
                "Thành tiền",
                "Ghi chú"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cell(headerRow, i + 1).Value = headers[i];
            }

            var headerRange = ws.Range(headerRow, 1, headerRow, headers.Length);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.Yellow;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            var row = headerRow + 1;
            for (int i = 0; i < rows.Count; i++)
            {
                var item = rows[i];

                ws.Cell(row, 1).Value = i + 1;
                ws.Cell(row, 2).Value = item.DeliveryDate;
                ws.Cell(row, 3).Value = item.DeliveryExternalId;
                ws.Cell(row, 4).Value = item.CustomerName;
                ws.Cell(row, 5).Value = item.Address;
                ws.Cell(row, 6).Value = item.ProductDisplay;
                ws.Cell(row, 7).Value = item.TotalQuantity;
                ws.Cell(row, 8).Value = item.TotalBags;
                ws.Cell(row, 9).Value = item.PoNoDisplay;
                ws.Cell(row, 10).Value = item.LotNoDisplay;
                ws.Cell(row, 11).Value = item.DeliveryPrice;
                ws.Cell(row, 12).Value = item.Note;

                var lineRange = ws.Range(row, 1, row, headers.Length);
                lineRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                lineRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                lineRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                lineRange.Style.Alignment.WrapText = true;

                row++;
            }

            if (rows.Any())
            {
                var totalRow = row;
                ws.Cell(totalRow, 1).Value = "Tổng cộng";
                ws.Range(totalRow, 1, totalRow, 6).Merge();

                ws.Cell(totalRow, 7).FormulaA1 = $"SUM(G{headerRow + 1}:G{totalRow - 1})";
                ws.Cell(totalRow, 8).FormulaA1 = $"SUM(H{headerRow + 1}:H{totalRow - 1})";
                ws.Cell(totalRow, 11).FormulaA1 = $"SUM(K{headerRow + 1}:K{totalRow - 1})";

                var totalRange = ws.Range(totalRow, 1, totalRow, headers.Length);
                totalRange.Style.Font.Bold = true;
                totalRange.Style.Fill.BackgroundColor = XLColor.FromArgb(230, 230, 230);
                totalRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                totalRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            }

            ws.Column(1).Width = 8;
            ws.Column(2).Width = 14;
            ws.Column(3).Width = 18;
            ws.Column(4).Width = 28;
            ws.Column(5).Width = 40;
            ws.Column(6).Width = 40;
            ws.Column(7).Width = 14;
            ws.Column(8).Width = 12;
            ws.Column(9).Width = 18;
            ws.Column(10).Width = 18;
            ws.Column(11).Width = 16;
            ws.Column(12).Width = 24;

            ws.Column(2).Style.DateFormat.Format = "dd/MM/yyyy";
            ws.Column(7).Style.NumberFormat.Format = "#,##0.00";
            ws.Column(8).Style.NumberFormat.Format = "#,##0";
            ws.Column(11).Style.NumberFormat.Format = "#,##0";

            ws.SheetView.FreezeRows(headerRow);
        }

        private static string SafeSheetName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "ChuaPhanNguoiGiao";

            var invalidChars = new[] { '\\', '/', '?', '*', '[', ']', ':' };
            var cleaned = new string(input
                .Where(c => !invalidChars.Contains(c))
                .ToArray());

            if (cleaned.Length > 31)
                cleaned = cleaned.Substring(0, 31);

            return string.IsNullOrWhiteSpace(cleaned) ? "Sheet" : cleaned;
        }
    }
}

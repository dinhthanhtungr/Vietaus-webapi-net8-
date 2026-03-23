using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.Pdfs;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.Helpers
{
    public class DeliveryOrderPdfRenderHelper : IDeliveryOrderPdfRenderHelper
    {
        private List<PdfPrinterDeliveryOrderDetail> _deliveryOrders = new List<PdfPrinterDeliveryOrderDetail>();
        private List<GetDeliveryOrderDetail> _DeliveryOrderDetails = new List<GetDeliveryOrderDetail>();
        public byte[] RenderTemplate()
        {
            return Render(new PdfPrinterDeliveryOrder(), true);
        }

        public byte[] Render(PdfPrinterDeliveryOrder d, bool templateOnly = false)
        {
            d ??= new PdfPrinterDeliveryOrder();

            _deliveryOrders = templateOnly
                ? new List<PdfPrinterDeliveryOrderDetail>()
                : (d.Details?.ToList() ?? new List<PdfPrinterDeliveryOrderDetail>());

            _DeliveryOrderDetails = templateOnly
                ? new List<GetDeliveryOrderDetail>()
                : (d.DeliveryOrderDetails?.ToList() ?? new List<GetDeliveryOrderDetail>());

            var doc = Document.Create(c =>
            {
                c.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(t => t.FontFamily("Open Sans").FontSize(9));

                    page.Header().Component(new HeaderComponent());
                    page.Content().Element(x => BuildContent(x, d, templateOnly));
                    page.Footer().Component(new DOFooterComponent());
                });
            });

            return doc.GeneratePdf();
        }

        private void BuildContent(IContainer c, PdfPrinterDeliveryOrder d, bool templateOnly)
        {
            d ??= new PdfPrinterDeliveryOrder();

            var deliveryRows = templateOnly
                ? new List<PdfPrinterDeliveryOrderDetail>()
                : (_deliveryOrders ?? new List<PdfPrinterDeliveryOrderDetail>());

            var deliveryOrderDetails = templateOnly
                ? new List<GetDeliveryOrderDetail>()
                : (_DeliveryOrderDetails ?? new List<GetDeliveryOrderDetail>());

            c.Column(col =>
            {
                col.Item().AlignCenter().PaddingTop(5)
                    .Text("PHIẾU GIAO HÀNG / DELIVERY ORDER")
                    .FontSize(14).Bold();

                col.Item().PaddingTop(5).AlignRight().Row(row =>
                {
                    row.Spacing(25);

                    row.AutoItem().Text($"No: {(templateOnly ? "" : d.ExternalId)}");

                    row.AutoItem().Text($"Ngày tạo/Create Date: {(templateOnly ? "....../....../......" : DateTime.Now.ToString("dd/MM/yyyy"))}");
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
                        table.Cell().Element(CellStyle).Text(label).FontSize(9).SemiBold();
                        table.Cell().Element(CellStyle).Text(string.IsNullOrWhiteSpace(value) ? "" : value).FontSize(9);
                    }

                    static IContainer CellStyle(IContainer container) =>
                        container.BorderBottom(1)
                                 .BorderColor(Colors.Grey.Lighten2)
                                 .PaddingVertical(3);

                    AddRow("Đơn hàng (Product order):", templateOnly ? "" : d.MerchandiseOrderExternalIdList);
                    AddRow("Họ tên người mua (Customer name):", templateOnly ? "" : d.Receiver);
                    AddRow("Đơn vị (Company name):", templateOnly ? "" : d.CustomerName);
                    AddRow("Địa chỉ (Address):", templateOnly ? "" : d.CustomerAddress);
                    AddRow("Hình thức thanh toán (Payment term):", templateOnly ? "" : d.PaymentType);
                    AddRow("Mã số thuế (VAT code):", templateOnly ? "" : d.TaxNumber);
                    AddRow("Ngày giao (Delivery date):", templateOnly ? "....../....../......" : $"{d.CreateDate:dd/MM/yyyy}");
                    AddRow(
                        "Người giao (Deliverer):",
                        templateOnly
                            ? ""
                            : (d.Deliverers != null && d.Deliverers.Any()
                                ? string.Join(", ", d.Deliverers.Select(x => x.Name))
                                : "")
                    );
                });

                col.Item().PaddingTop(10).PaddingBottom(10).Table(table =>
                {
                    table.ColumnsDefinition(cd =>
                    {
                        cd.ConstantColumn(70);
                        cd.RelativeColumn(2.4f);
                        cd.RelativeColumn(1.2f);
                        cd.ConstantColumn(45);
                        cd.RelativeColumn(1.1f);
                        cd.ConstantColumn(65);
                        cd.RelativeColumn(1.1f);
                    });

                    var borderStyle = new Func<IContainer, IContainer>(x => x.Border(1).BorderColor(Colors.Black));

                    table.Header(row =>
                    {
                        void Header(Action<IContainer> build, string vi, string en)
                        {
                            build(row.Cell());
                        }

                        Action<IContainer> cellStyle(string vi, string en) => x =>
                        {
                            borderStyle(x)
                                .Background(Colors.Grey.Lighten1)
                                .AlignCenter()
                                .Text($"{vi}\n{en}")
                                .FontSize(9)
                                .Bold();
                        };

                        cellStyle("Mã SP", "Code")(row.Cell());
                        cellStyle("Tên SP", "Product Name")(row.Cell());
                        cellStyle("Số lô", "Batch No")(row.Cell());
                        cellStyle("Đơn vị", "Unit")(row.Cell());
                        cellStyle("Số lượng", "Quantity")(row.Cell());
                        cellStyle("Số bao", "Bag number")(row.Cell());
                        cellStyle("Số PO", "PO No")(row.Cell());
                    });

                    if (templateOnly)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(4).Text("");
                            borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).Text("");
                            borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).Text("");
                            borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).Text("");
                            borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).Text("");
                            borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).Text("");
                            borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).Text("");
                        }

                        borderStyle(table.Cell().ColumnSpan(7))
                            .PaddingVertical(4).PaddingHorizontal(6).AlignRight()
                            .Text("Tổng cộng: .................. Kg - .................. Bao (Total)")
                            .Bold();
                    }
                    else
                    {
                        var doGroupTotals = deliveryOrderDetails
                            .GroupBy(r => r.ProductExternalIdSnapShot ?? string.Empty)
                            .ToDictionary(
                                g => g.Key,
                                g => new
                                {
                                    Weight = g.Sum(x => (decimal)(x?.Quantity ?? 0m)),
                                    Bags = g.Sum(x => (int)(x?.NumOfBags ?? 0))
                                }
                            );

                        var grandWeight = deliveryOrderDetails.Sum(x => (decimal)(x?.Quantity ?? 0m));
                        var grandBags = deliveryOrderDetails.Sum(x => (int)(x?.NumOfBags ?? 0));

                        var wrGroups = deliveryRows
                            .GroupBy(r => r.ProductCode)
                            .OrderByDescending(g => g.Key)
                            .ToList();

                        foreach (var g in wrGroups)
                        {
                            foreach (var r in g)
                            {
                                borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(4).AlignLeft()
                                    .Text(r?.ProductCode ?? "");

                                borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).AlignLeft()
                                    .Text(r?.ProductName ?? "");

                                borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).AlignLeft()
                                    .Text(r?.LotNumber ?? "");

                                borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).AlignLeft()
                                    .Text("Kg");

                                borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).AlignRight()
                                    .Text($"{(r?.WeightKg ?? 0m):0.00}");

                                borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).AlignRight()
                                    .Text($"{(r?.BagNumber ?? 0)}");

                                borderStyle(table.Cell()).PaddingVertical(4).PaddingHorizontal(6).AlignLeft()
                                    .Text(r?.PONoNumber ?? "");
                            }
                        }

                        borderStyle(table.Cell().ColumnSpan(7))
                            .PaddingVertical(4).PaddingHorizontal(6).AlignRight()
                            .Text(t =>
                            {
                                t.Span($"Tổng cộng: {grandWeight:0.00} Kg - {grandBags} Bao").Bold();
                                t.Span(" (Total)").Bold();
                            });
                    }
                });

                col.Item().PaddingTop(6).Text(txt =>
                {
                    txt.Span("Ghi chú (Note): ").Bold();
                    txt.Span(templateOnly ? "" : d.Note ?? "");
                });

                col.Item().PaddingTop(6).Text(t =>
                {
                    t.Span("Địa chỉ giao hàng (Delivery Address): ").Bold();
                    t.Span(templateOnly ? "" : d.DeliveryAddress ?? "");
                });

                col.Item().Text(t =>
                {
                    t.Span("Liên hệ nhận hàng (Contact person): ").Bold();
                    t.Span(templateOnly ? "" : d.Receiver ?? "");
                    t.Span(templateOnly ? "" : " (").Italic();
                    t.Span(templateOnly ? "" : d.PhoneSnapshot ?? "").Italic();
                    t.Span(templateOnly ? "" : ")").Italic();
                });

                col.Item().PaddingTop(14).Row(r =>
                {
                    void SignCol(RowDescriptor row, string vi, string en)
                    {
                        row.RelativeItem().Column(c2 =>
                        {
                            c2.Item().AlignCenter().Text(t =>
                            {
                                t.Span(vi).SemiBold();
                                t.Span(" (").SemiBold();
                                t.Span(en).SemiBold();
                                t.Span(")").SemiBold();
                            });

                            c2.Item().AlignCenter()
                                .Text("(Tên, chữ ký/Fullname, signature)")
                                .Italic().FontSize(9);

                            c2.Item().MinHeight(60);
                        });
                    }

                    SignCol(r, "Người lập phiếu", "Preparer");
                    SignCol(r, "Thủ kho", "Storekeeper");
                    SignCol(r, "Người nhận hàng", "Receiver");
                });
            });
        }
    }
}
    
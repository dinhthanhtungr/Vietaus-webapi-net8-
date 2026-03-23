
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace VietausWebAPI.Core.Application.Shared.Helper.Pdfs
{
    public class MFGVUFooterComponent : IComponent
    {
        public void Compose(IContainer container)
        {
            container.PaddingTop(20).Column(col =>
            {


                // Website + Slogan 
                col.Item().PaddingTop(10).Row(row =>
                {
                    // Website và Slogan
                    row.RelativeItem().AlignCenter().AlignMiddle().Column(c =>
                    {
                        c.Item().AlignCenter().AlignMiddle().Element(e => e.Text(txt =>
                        {
                            txt.Span("Website: ");
                            txt.Span("https://vietaus.com").FontColor(Colors.Blue.Medium);
                            txt.Span(" - hotline: (84). 8. 73 09 39 69");
                        }));

                        c.Item().AlignCenter().AlignMiddle().Element(e => e.Text("COLOURING YOUR FUTURE WITH SERVICE AT YOUR DOORSTEP")
                            .Bold().FontFamily("Open Sans").FontSize(9));
                    });

                });


                col.Item().PaddingTop(10).Row(row =>
                {
                    //row.RelativeItem()
                    //    .AlignLeft()
                    //    .Element(e => e.Text("ISO 9001 / ISO 14001 / ISO 45001 / GRS")
                    //        .FontFamily("Open Sans")
                    //        .FontSize(9));

                    //row.RelativeItem()
                    //    .AlignCenter()
                    //    .Element(e => e.Text("25-03-2026")
                    //        .FontFamily("Open Sans")
                    //        .FontSize(9));

                    //row.RelativeItem()
                    //    .AlignRight()
                    //    .Element(e => e.Text("VA-PL&PU-F02(04)")
                    //        .FontFamily("Open Sans")
                    //        .FontSize(9));

                    row.RelativeItem()
                        .AlignLeft()
                        .Element(e => e.Text("VA-PL&PU-F02(04)")
                            .FontFamily("Open Sans")
                            .FontSize(9));

                    //row.RelativeItem()
                    //    .AlignCenter()
                    //    .Element(e => e.Text("25-03-2026")
                    //        .FontFamily("Open Sans")
                    //        .FontSize(9));

                    row.RelativeItem()
                        .AlignRight()
                        .Element(e => e.Text("25-03-2026")
                            .FontFamily("Open Sans")
                            .FontSize(9));
                });
            });
        }
    }
}


using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace VietausWebAPI.Core.Application.Shared.Helper.Pdfs
{
    public class FooterComponent : IComponent
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

                // Logo 17ece7c7-9afc-478d-0acd-08ddbe89253f
                col.Item().PaddingTop(10).Row(row =>
                {
                    var imagePath = "wwwroot/images/Iso/bureau-veritas.png";
                    var imageQrPath = "wwwroot/images/Iso/QR.png";
                    var imageGrsPath = "wwwroot/images/Iso/GRS.png";

                    row.RelativeItem().AlignCenter().AlignMiddle().Element(e => e.Row(innerRow =>
                    {
                        if (File.Exists(imagePath))
                        {
                            innerRow.ConstantItem(80).PaddingRight(10).Element(img =>
                            {
                                img.AlignMiddle().Image(imagePath).FitWidth();
                            });
                        }

                        if (File.Exists(imageGrsPath))
                        {
                            innerRow.ConstantItem(80).PaddingRight(10).Element(img =>
                            {
                                img.AlignMiddle().Image(imageGrsPath).FitWidth();
                            });
                        }

                        if (File.Exists(imageQrPath))
                        {
                            innerRow.ConstantItem(45).PaddingRight(10).Element(img =>
                            {
                                img.AlignMiddle().Image(imageQrPath).FitWidth();
                            });
                        }
                        

                    }));
                });


                // Mã form và ngày
                col.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().Element(e => e.Text("VA-QA&QC-F18(03)").FontFamily("Open Sans").FontSize(9));
                    row.ConstantItem(100).AlignRight().Element(e => e.Text("22-08-2023").FontFamily("Open Sans").FontSize(9));
                });
            });
        }
    }
}

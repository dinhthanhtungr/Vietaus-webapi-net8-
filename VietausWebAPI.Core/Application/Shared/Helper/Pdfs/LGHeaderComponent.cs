using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;


namespace VietausWebAPI.Core.Application.Shared.Helper.Pdfs
{
    public class LGHeaderComponent : IComponent
    {

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {

                column.Item().Row(row =>
                {
                    // Logo (1 phần)
                    row.RelativeItem(1).Height(50).AlignMiddle().AlignLeft().Element(e =>
                    {
                        var imagePath = "wwwroot/images/Logos/LongGiang.png";
                        if (File.Exists(imagePath))
                            e.Image(Image.FromFile(imagePath)).FitHeight();
                        else
                            e.Text("Logo").FontSize(10);
                    });

                    // Thông tin công ty (2 phần)
                    row.RelativeItem(2).AlignRight().AlignMiddle().Element(e =>
                    {
                        e.Border(1).PaddingRight(5).PaddingBottom(5).PaddingLeft(5).Column(col =>
                        {
                            //col.Item().Text("VIETAUS POLYMER CO., LTD.")
                            //    .FontFamily("Open Sans").FontSize(9).Bold();

                            col.Item().Text("Address:  No 26/6, Street 12, Tam Binh ward,\nThủ Đức Dist, HO CHI MINH City, Vietnam.")
                                .FontFamily("Open Sans").FontSize(7);

                            col.Item().Text("Tel: (84). 932. 66 36 89     Fax: (84) 28. 37 29 29 64")
                                .FontFamily("Open Sans").FontSize(7);

                            col.Item().Text("Website: www.lgplastic.com")
                                .FontFamily("Open Sans").FontSize(7)
                                .FontColor(Colors.Blue.Medium);
                        });
                    });


                });

                column.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
            });
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

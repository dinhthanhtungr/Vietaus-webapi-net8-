using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace VietausWebAPI.Core.Application.Shared.Helper
{
    public class HeaderComponent : IComponent
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
                        var imagePath = "wwwroot/images/Logos/VietAusLogo.png";
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
                            col.Item().Text("VIETAUS POLYMER CO., LTD.")
                                .FontFamily("Open Sans").FontSize(9).Bold();

                            col.Item().Text("Address:  No.296, Trung Thang, Binh Thang ward,\nDĩ An City, Binh Duong Province, Vietnam.")
                                .FontFamily("Open Sans").FontSize(7);

                            col.Item().Text("Tel: (84). 28. 730 99 369     Fax: (84) 274 3 800 037")
                                .FontFamily("Open Sans").FontSize(7);

                            col.Item().Text("Website: https://vietaus.com")
                                .FontFamily("Open Sans").FontSize(7)
                                .FontColor(Colors.Blue.Medium);
                        });
                    });


                });

                column.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
            });
        }
    }

}

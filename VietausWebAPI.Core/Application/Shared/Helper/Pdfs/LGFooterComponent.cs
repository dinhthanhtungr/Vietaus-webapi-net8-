using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;


namespace VietausWebAPI.Core.Application.Shared.Helper.Pdfs
{
    public class LGFooterComponent : IComponent
    {
        public void Compose(IContainer container)
        {
            container.PaddingTop(20).Column(col =>
            {

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

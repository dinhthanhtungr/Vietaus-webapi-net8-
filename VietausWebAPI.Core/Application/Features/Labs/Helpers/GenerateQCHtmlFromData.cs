using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.Helpers
{
    public static class GenerateQCHtmlFromData
    {
        public static string Generate(List<ProductInspection> data)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "FinalQCReport.html");
            var template = File.ReadAllText(templatePath);

            var first = data.FirstOrDefault();
            if (first == null) return template;

            template = template.Replace("{{ManufacturingDate}}", first.CreateDate.HasValue
                                            ? first.CreateDate.Value.ToString("dd/MM/yyyy")
                                            : ""
                                        );
            for (int i = 0; i < 7; i++)
            {
                var ExternalId = data.Count > i ? data[i].BatchId ?? "" : "";
                template = template.Replace($"{{{{ExternalId{i + 1}}}}}", ExternalId);
                // Gán vào placeholder {{ExternalId1}}, {{ExternalId2}}, ...

                var weight = data.Count > i && data[i].Weight.HasValue ? data[i].Weight.Value.ToString() : "";
                template = template.Replace($"{{{{Weight{i + 1}}}}}", weight);
                // Gán vào placeholder {{Weight1}}, {{Weight2}}, ...

                var rejected = data.Count > i && data[i].Weight.HasValue ? data[i].Weight.Value.ToString() : "";
                template = template.Replace($"{{{{RejectedWeight{i + 1}}}}}", rejected);

                var meshType = data.Count > i ? data[i].MeshType ?? "" : "";
                template = template.Replace($"{{{{MeshType{i + 1}}}}}", meshType);

                // Checkbox tick Y/N cho Gắn lưới
                var isMesh = data.Count > i && data[i].IsMeshAttached.HasValue
                    ? data[i].IsMeshAttached.Value
                        ? @"<label><input type='checkbox' checked />Y</label><label><input type='checkbox' />N</label>"
                        : @"<label><input type='checkbox' />Y</label><label><input type='checkbox' checked />N</label>"
                    : @"<label><input type='checkbox' />Y</label><label><input type='checkbox' />N</label>"; // chưa có dữ liệu

                template = template.Replace($"{{{{MeshAttached{i + 1}}}}}", isMesh);

                // Nếu bạn có field tên là IsCrackFree
                var Defect_Dusty = data.Count > i && data[i].DefectDusty.HasValue
                    ? data[i].DefectDusty.Value
                        ? @"<label><input type='checkbox' checked />Y</label><label><input type='checkbox' />N</label>"
                        : @"<label><input type='checkbox' />Y</label><label><input type='checkbox' checked />N</label>"
                    : @"<label><input type='checkbox' />Y</label><label><input type='checkbox' />N</label>";

                template = template.Replace($"{{{{Defect_Dusty{i + 1}}}}}", Defect_Dusty);


                var isImpurityFree = data.Count > i ? data[i].DefectImpurity : null;
                var Defect_Impurity = isImpurityFree.HasValue
                    ? (isImpurityFree.Value
                        ? @"<label><input type='checkbox' checked />Y</label><label><input type='checkbox' />N</label>"
                        : @"<label><input type='checkbox' />Y</label><label><input type='checkbox' checked />N</label>")
                    : @"<label><input type='checkbox' />Y</label><label><input type='checkbox' />N</label>";

                template = template.Replace($"{{{{Defect_Impurity{i + 1}}}}}", Defect_Impurity);


                var isPass = data.Count > i ? data[i].DefectShortFiber : null;
                var Defect_ShortFiber = isPass.HasValue
                    ? (isPass.Value
                        ? @"<label><input type='checkbox' checked />Y</label><label><input type='checkbox' />N</label>"
                        : @"<label><input type='checkbox' />Y</label><label><input type='checkbox' checked />N</label>")
                    : @"<label><input type='checkbox' />Y</label><label><input type='checkbox' />N</label>";

                template = template.Replace($"{{{{NoShortLong{i + 1}}}}}", Defect_ShortFiber);


                var isPassDefect_BlackDot = data.Count > i ? data[i].DefectBlackDot : null;
                var Defect_BlackDot = isPassDefect_BlackDot.HasValue
                    ? (isPassDefect_BlackDot.Value
                        ? @"<label><input type='checkbox' checked />Y</label><label><input type='checkbox' />N</label>"
                        : @"<label><input type='checkbox' />Y</label><label><input type='checkbox' checked />N</label>")
                    : @"<label><input type='checkbox' />Y</label><label><input type='checkbox' />N</label>";

                template = template.Replace($"{{{{NoBlackDot{i + 1}}}}}", Defect_BlackDot);


                var value = data.Count > i ? data[i].DefectWrongColor : null;

                var html = value.HasValue
                    ? (value.Value
                        ? "<label><input type='checkbox' checked />Y</label><label><input type='checkbox' />N</label>"
                        : "<label><input type='checkbox' />Y</label><label><input type='checkbox' checked />N</label>")
                    : "<label><input type='checkbox' />Y</label><label><input type='checkbox' />N</label>";

                template = template.Replace($"{{{{ColorAppearance{i + 1}}}}}", html);
            }

            var sb = new StringBuilder();

            return template;
        }
    }
}

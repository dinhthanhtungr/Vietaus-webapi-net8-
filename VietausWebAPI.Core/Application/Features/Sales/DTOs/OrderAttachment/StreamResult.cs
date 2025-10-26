using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.OrderAttachment
{

    public sealed record StreamResult(Stream Stream, string ContentType, string FileName); // FileName: tên hiển thị gợi ý cho download
}

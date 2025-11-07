using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Attachments.DTOs
{
    public class StreamResult
    {
        public StreamResult(Stream stream, string? contentType, string fileName)
        {
            Stream = stream;
            ContentType = contentType;
            FileName = fileName;
        }
        public Stream Stream { get; }
        public string? ContentType { get; }
        public string FileName { get; }
    }
}

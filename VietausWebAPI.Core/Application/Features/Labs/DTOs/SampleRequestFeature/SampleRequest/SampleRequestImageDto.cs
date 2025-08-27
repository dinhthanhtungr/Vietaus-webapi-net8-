using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest
{
    public class SampleRequestImageDto
    {
        public Guid SampleRequestImageId { get; set; }
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public long FileSize { get; set; }
        public string FileUrl { get; set; } = null!;
        public bool IsCover { get; set; }
        public int SortOrder { get; set; }
    }
}

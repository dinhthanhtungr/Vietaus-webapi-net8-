using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest
{
    public class ChangeSampleRequestColourCodeSuffixRequest
    {
        public Guid SampleRequestId { get; set; }
        public string Suffix { get; set; } = string.Empty;
    }

}

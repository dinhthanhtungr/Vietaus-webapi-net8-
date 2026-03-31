using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.SampleRequests;

namespace VietausWebAPI.Core.Application.Features.Labs.Helpers.SampleRequests
{
    public static class ResinStandardSpecHelper
    {
        public static ResinStandardSpec GetByResinType(ResinType resinType)
        {
            if (ResinStandardSpecs.All.TryGetValue(resinType, out var spec))
                return spec;

            return ResinStandardSpecs.All[ResinType.Other];
        }
    }
}

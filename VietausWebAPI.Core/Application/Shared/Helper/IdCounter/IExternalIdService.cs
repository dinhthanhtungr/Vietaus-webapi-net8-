using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Shared.Helper.IdCounter
{
    public interface IExternalIdService
    {
        Task<string> NextAsync(string prefix, CancellationToken ct = default);
    }
}

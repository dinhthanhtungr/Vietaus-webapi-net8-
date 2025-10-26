using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.ServiceContracts
{
    public interface IPurchaseOrderPdfService
    {
        Task<byte[]> GenerateAsync(Guid PurchaseOrderId, CancellationToken ct = default);

    }
}

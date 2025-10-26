using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.ServiceContracts
{
    public interface IDeliveryOrderPdfService
    {
        Task<byte[]> GenerateAsync(Guid deliveryOrderId, CancellationToken ct = default);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.PurchaseOrders;

namespace VietausWebAPI.Core.Application.Usecases.PurchaseOrders.ServiceContracts
{
    public interface IPurchaseOrderDetailsService
    {
        Task<IEnumerable<ShowPurchaseOrderDetailsDTO>> ShowPurchaseOrderDetailServiceAsync(Guid POID);
    }
}

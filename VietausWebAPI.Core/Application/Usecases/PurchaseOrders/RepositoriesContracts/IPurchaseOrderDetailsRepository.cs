using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Usecases.PurchaseOrders.RepositoriesContracts
{
    public interface IPurchaseOrderDetailsRepository
    {
        Task GetPurchaseOrderDetail(IEnumerable<PurchaseOrderDetailsMaterialDatum> purchaseOrderDetailsMaterialDatum);
        Task<IEnumerable<PurchaseOrderDetailsMaterialDatum>> PostPurchaseOrderDetail(Guid POID);

        Task<IEnumerable<PurchaseOrderDetailsMaterialDatum>> GetByPOIDAsync(Guid poid);
        Task DeleteByPOIDAsync(Guid guid);
    }
}

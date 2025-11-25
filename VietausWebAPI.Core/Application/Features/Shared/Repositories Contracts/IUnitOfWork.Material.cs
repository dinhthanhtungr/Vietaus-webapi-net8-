using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        IMaterialRepository MaterialRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IMaterialsSupplierRepository MaterialsSupplierRepository { get; }
        IPriceHistorieRepository PriceHistorieRepository { get; }
    }
}

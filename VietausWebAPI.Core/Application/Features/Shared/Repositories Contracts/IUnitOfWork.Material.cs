using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;

namespace VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        IMaterialRepository MaterialRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IUnitRepository UnitRepository { get; }
        IMaterialsSupplierRepository MaterialsSupplierRepository { get; }
        IPriceHistorieRepository PriceHistorieRepository { get; }
    }
}

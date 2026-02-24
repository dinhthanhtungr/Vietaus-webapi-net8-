using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts.SupplierFeatures;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        public IMaterialRepository MaterialRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IUnitRepository UnitRepository { get; }
        public IMaterialsSupplierRepository MaterialsSupplierRepository { get; }
        public IPriceHistorieRepository PriceHistorieRepository { get; }


        // ======================================================================== SupplierFeature ======================================================================== 
        public ISupplierReadRepository SupplierReadRepository { get; }
        public ISupplierWriteRepository SupplierWriteRepository { get; }
    }
}

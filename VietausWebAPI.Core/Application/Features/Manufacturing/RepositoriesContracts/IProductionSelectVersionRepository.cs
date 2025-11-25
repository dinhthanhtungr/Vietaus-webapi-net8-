using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts
{
    public interface IProductionSelectVersionRepository : IRepository<ProductionSelectVersion>
    {
        Task AddRangeAsync(IEnumerable<ProductionSelectVersion> ProductionSelectVersions, CancellationToken ct);
        void UpdateAsync(ProductionSelectVersion ProductStandardFormula, CancellationToken ct);
    }
}

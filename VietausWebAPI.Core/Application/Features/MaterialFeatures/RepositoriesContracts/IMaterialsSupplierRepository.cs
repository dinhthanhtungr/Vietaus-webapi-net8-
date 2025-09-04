using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts
{
    public interface IMaterialsSupplierRepository
    {
        IQueryable<MaterialsSupplier> Query(bool track = true);
        Task AddAsync(MaterialsSupplier material, CancellationToken ct = default);
    }
}

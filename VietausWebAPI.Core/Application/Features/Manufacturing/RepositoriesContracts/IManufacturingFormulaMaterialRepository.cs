using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts
{
    public interface IManufacturingFormulaMaterialRepository
    {
        IQueryable<ManufacturingFormulaMaterial> Query(bool track = true);
        Task AddAsync(ManufacturingFormulaMaterial manufacturingFormulaMaterial, CancellationToken ct = default);
        Task AddRangeAsync(List<ManufacturingFormulaMaterial> manufacturingFormulaMaterials, CancellationToken ct = default);
    }
}

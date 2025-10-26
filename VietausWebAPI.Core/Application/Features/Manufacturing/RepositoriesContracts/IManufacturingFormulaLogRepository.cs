using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts
{
    public interface IManufacturingFormulaLogRepository
    {
        IQueryable<ManufacturingFormulaLog> Query(bool track = true);
        Task AddAsync(ManufacturingFormulaLog manufacturingFormulaMaterial, CancellationToken ct = default);
    }
}

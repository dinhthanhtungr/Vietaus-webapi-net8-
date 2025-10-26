using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures
{
    public interface IFormulaMaterialRepository
    {
        IQueryable<FormulaMaterial> Query(bool track = true);
        Task AddAsync(FormulaMaterial formualMaterial, CancellationToken ct = default);
    }
}

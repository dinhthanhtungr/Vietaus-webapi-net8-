using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

namespace VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures
{
    public interface IFormulaMaterialSnapshotRepository : IRepository<FormulaMaterialSnapshot>
    {
    }
}

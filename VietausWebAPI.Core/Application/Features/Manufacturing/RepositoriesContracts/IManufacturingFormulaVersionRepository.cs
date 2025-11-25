using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts
{
    public interface IManufacturingFormulaVersionRepository : IRepository<ManufacturingFormulaVersion>
    {
    }
}

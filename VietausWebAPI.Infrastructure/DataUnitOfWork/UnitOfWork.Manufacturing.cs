using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts.GetRepositories;
using VietausWebAPI.Core.Application.Features.Planning.RepositoriesContracts;
using VietausWebAPI.Infrastructure.Repositories.Manufacturing.ProductionSelectVersionRepositories;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        public IMfgProductionOrderRepository MfgProductionOrderRepository { get; }
        public IManufacturingFormulaMaterialRepository ManufacturingFormulaMaterialRepository { get; }
        public IManufacturingFormulaRepository ManufacturingFormulaRepository { get; }
        public IProductStandardFormulaRepository ProductStandardFormulaRepository { get; }
        public IProductionSelectVersionRepository ProductionSelectVersionRepository { get; }
        public IProductionSelectVersionReadRepository ProductionSelectVersionReadRepository { get; }

        public IMfgOrderPORepository MfgOrderPORepository { get; }
        public ISchedualMfgRepository SchedualMfgRepository { get; }

        public IScheduealRepository ScheduealRepository { get; }
        //public IMfgProductionOrdersPlanRepository IMfgProductionOrdersPlanRepository { get; }
    }
}

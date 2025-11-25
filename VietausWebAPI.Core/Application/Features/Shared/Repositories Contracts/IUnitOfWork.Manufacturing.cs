using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Planning.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.MerchandiseOrderFeatures;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        IMfgProductionOrderRepository MfgProductionOrderRepository { get; }
        IManufacturingFormulaMaterialRepository ManufacturingFormulaMaterialRepository { get; }
        IManufacturingFormulaRepository ManufacturingFormulaRepository { get; }
        IProductStandardFormulaRepository ProductStandardFormulaRepository { get; }
        IProductionSelectVersionRepository ProductionSelectVersionRepository { get; }
        IMfgOrderPORepository MfgOrderPORepository { get; }
        ISchedualMfgRepository SchedualMfgRepository { get; }

        // Planning
        IScheduealRepository ScheduealRepository { get; }
        //IMfgProductionOrdersPlanRepository IMfgProductionOrdersPlanRepository { get; }
    }
}

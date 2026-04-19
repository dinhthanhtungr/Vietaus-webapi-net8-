using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts.ColorChipManufacturingRecords;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts.GetRepositories;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.ColorChipManufacturingRecords;
using VietausWebAPI.Core.Application.Features.Planning.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.MerchandiseOrderFeatures;

namespace VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        IColorChipManufacturingRecordReadRepository ColorChipManufacturingRecordReadRepository { get; }
        IColorChipManufacturingRecordWriteRepository ColorChipManufacturingRecordWriteRepository { get; }

        IMfgProductionOrderRepository MfgProductionOrderRepository { get; }
        IManufacturingFormulaMaterialRepository ManufacturingFormulaMaterialRepository { get; }
        IManufacturingFormulaRepository ManufacturingFormulaRepository { get; }
        IProductStandardFormulaRepository ProductStandardFormulaRepository { get; }
        IProductionSelectVersionRepository ProductionSelectVersionRepository { get; }
        IProductionSelectVersionReadRepository ProductionSelectVersionReadRepository { get; }
        IMfgOrderPORepository MfgOrderPORepository { get; }
        ISchedualMfgRepository SchedualMfgRepository { get; }

        // Planning
        IScheduealRepository ScheduealRepository { get; }


    }
}

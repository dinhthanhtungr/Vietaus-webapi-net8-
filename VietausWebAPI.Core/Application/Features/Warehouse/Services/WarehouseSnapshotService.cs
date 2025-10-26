using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.Queries;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Enums;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Warehouse.Services
{
    public class WarehouseSnapshotService : IWarehouseSnapshotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWarehouseReadService _warehouseReadService;

        public WarehouseSnapshotService(IUnitOfWork unitOfWork, IWarehouseReadService warehouseReadService)
        {
            _unitOfWork = unitOfWork;
            _warehouseReadService = warehouseReadService;
        }
        //public async Task<Guid> CreateSnapshotAsync(WarehouseSnapshotServiceQuery query, CancellationToken ct)
        //{
        //    using var tx = await _unitOfWork.BeginTransactionAsync();

        //    var set = new WarehouseSnapshotSet
        //    {
        //        SnapshotSetId = Guid.NewGuid(),
        //        CompanyId = query.companyId,
        //        VaCode = query.vaCode,
        //        CreatedBy = query.createdBy,
        //        CreatedDate = DateTime.Now
        //    };
        //    await _unitOfWork.WarehouseSnapshotSetRepository.AddAsync(set);

        //    await _unitOfWork.SaveChangesAsync();

        //    foreach (var code in query.materialCodes.Distinct())
        //    {
        //        var onHand = await _warehouseReadService.GetOnHandAsync(new WarehouseReadServiceQuery
        //        {
        //            companyId = query.companyId,
        //            code = code
        //        }, ct);

        //        await _unitOfWork.WarehouseTempStockRepository.AddAsync(new WarehouseTempStock
        //        {
        //            CompanyId = query.companyId,
        //            SnapshotSetId = set.SnapshotSetId,
        //            TempType = TempType.Snapshot,
        //            VaCode = query.vaCode,
        //            Code = code,
        //            QtyStock = onHand,
        //            CreatedBy = query.createdBy,
        //            CreatedDate = DateTime.Now
        //        });
        //    }

        //    await _unitOfWork.SaveChangesAsync();

        //    await tx.CommitAsync();

        //    return set.SnapshotSetId;
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts
{
    public interface IWarehouseShelfStockRepository
    {
        IQueryable<WarehouseShelfStock> Query(bool track = true);
        Task AddAsync(WarehouseShelfStock warehouseShelfStock, CancellationToken ct = default);

        // Read repositories
        /// <summary>
        /// Lấy danh sách WarehouseShelfStock theo Code, LotNumber và StockType
        /// </summary>
        /// <param name="code"></param>
        /// <param name="lotNumber"></param>
        /// <param name="stockType"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<WarehouseShelfStock>> GetByCodeLotAndStockTypeAsync(string code, string lotNumber, StockType stockType, CancellationToken ct);

        // Write repositories
        void Update(WarehouseShelfStock entity);
        void UpdateRange(IEnumerable<WarehouseShelfStock> entities);
    }
}

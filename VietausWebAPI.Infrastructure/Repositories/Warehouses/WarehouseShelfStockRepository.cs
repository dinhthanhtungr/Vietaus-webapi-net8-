using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;
using VietausWebAPI.Core.Domain.Enums.WareHouses;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Warehouses
{
    public class WarehouseShelfStockRepository : IWarehouseShelfStockRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseShelfStockRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(WarehouseShelfStock warehouseShelfStock, CancellationToken ct = default)
        {
            await _context.WarehouseShelfStocks.AddAsync(warehouseShelfStock, ct);
        }

        public IQueryable<WarehouseShelfStock> Query(bool track = true)
        {
            var db = _context.WarehouseShelfStocks.AsQueryable();
            return track ? db : db.AsNoTracking();
        }

        // Read repositories
        /// <summary>
        /// Lấy danh sách WarehouseShelfStock theo Code, LotNumber và StockType
        /// </summary>
        /// <param name="code"></param>
        /// <param name="lotNumber"></param>
        /// <param name="stockType"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<WarehouseShelfStock>> GetByCodeLotAndStockTypeAsync(string code, string lotNumber, StockType stockType, CancellationToken ct)
        {
            return await _context.WarehouseShelfStocks
                .Where(x => x.Code == code
                         && x.LotNo == lotNumber
                         && x.StockType == stockType)
                .ToListAsync(ct);
        }

        // Write repositories
        public void Update(WarehouseShelfStock entity)
        {
            _context.WarehouseShelfStocks.Update(entity);
        }

        public void UpdateRange(IEnumerable<WarehouseShelfStock> entities)
        {
            _context.WarehouseShelfStocks.UpdateRange(entities);
        }
    }
}

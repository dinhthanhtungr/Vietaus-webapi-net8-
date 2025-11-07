//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
//using VietausWebAPI.Core.Domain.Entities;
//using VietausWebAPI.WebAPI.DatabaseContext;

//namespace VietausWebAPI.Infrastructure.Repositories.Manufacturing
//{
//    public class ManufacturingFormulaLogRepository : IManufacturingFormulaLogRepository
//    {
//        private readonly ApplicationDbContext _context;

//        public ManufacturingFormulaLogRepository(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task AddAsync(ManufacturingFormulaLog manufacturingFormulaLog, CancellationToken ct = default)
//        {
//            await _context.ManufacturingFormulaLogs.AddAsync(manufacturingFormulaLog, ct);
//        }

//        public IQueryable<ManufacturingFormulaLog> Query(bool track = true)
//        {
//            var db = _context.ManufacturingFormulaLogs.AsQueryable();
//            return track ? db : db.AsNoTracking();
//        }
//    }
//}

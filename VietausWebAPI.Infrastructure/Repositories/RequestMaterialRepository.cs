using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories
{
    public class RequestMaterialRepository : IRequestMaterialRepository
    {
        private readonly ApplicationDbContext  _context;
        private IDbContextTransaction _transaction;

        public RequestMaterialRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRequestDetailMaterialAsync(List<RequestDetailMaterialDatum> requestDetail)
        {
            await _context.RequestDetailMaterialData.AddRangeAsync(requestDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }



        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }
            await transaction.CommitAsync();
        }

        public async Task<SupplyRequestsMaterialDatum> CreateRequestAsync(SupplyRequestsMaterialDatum request)
        {
            await _context.SupplyRequestsMaterialData.AddRangeAsync(request);
            //await _context.SaveChangesAsync();
            return request;
        }

        public async Task<SupplyRequestsMaterialDatum> GetLastRequestIdRepository()
        {
            //var lastRequestId = await _context.SupplyRequestsMaterialData.OrderByDescending(x => x.RequestId).FirstOrDefaultAsync();

            return await _context.SupplyRequestsMaterialData.OrderByDescending(x => x.RequestId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SupplyRequestsMaterialDatum>> GetRequestRepository(RequestMaterialQuery query)
        {
            var queryable = _context.SupplyRequestsMaterialData
                .AsNoTracking()
                .Include(r => r.RequestDetailMaterialData)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.RequestId))
            {
                queryable = queryable.Where(x => x.RequestId == query.RequestId);
            }

            if (query.RequestDate != null)
            {
                queryable = queryable.Where(x => x.RequestDate == query.RequestDate);
            }

            if (!string.IsNullOrEmpty(query.EmployeeId))
            {
                queryable = queryable.Where(x => x.EmployeeId == query.EmployeeId);
            }

            if (!string.IsNullOrEmpty(query.RequestStatus))
            {
                queryable = queryable.Where(x => x.RequestStatus == query.RequestStatus);
            }

            return await queryable.ToListAsync();
        }


        public async Task RollbackAsync(IDbContextTransaction transaction)
        {
            await _transaction.RollbackAsync();
        }
    }
}

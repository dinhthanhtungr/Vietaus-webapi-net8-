using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;
using VietausWebAPI.WebAPI.DatabaseContext;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Infrastructure.Repositories.Materials
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddNewSuplier(Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Supplier> Query()
        {
            throw new NotImplementedException();
        }
    }
}

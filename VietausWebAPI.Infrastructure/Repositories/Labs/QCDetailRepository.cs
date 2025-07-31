using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Labs
{
    public class QCDetailRepository : IQCDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public QCDetailRepository(ApplicationDbContext context )
        {
            _context = context;
        }

        public async Task AddQCDetail(QCDetail qCDetail)
        {
            await _context.QCDetails.AddAsync(qCDetail);
        }
    }
}

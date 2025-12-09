//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature;
//using VietausWebAPI.Core.Domain.Entities;
//using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

//namespace VietausWebAPI.Infrastructure.Repositories.Labs.QAQCFeature
//{
//    public class QCDetailRepository : IQCDetailRepository
//    {
//        private readonly ApplicationDbContext _context;

//        public QCDetailRepository(ApplicationDbContext context )
//        {
//            _context = context;
//        }

//        public async Task AddQCDetail(Qcdetail qCDetail)
//        {
//            await _context.Qcdetails.AddAsync(qCDetail);
//        }
//    }
//}

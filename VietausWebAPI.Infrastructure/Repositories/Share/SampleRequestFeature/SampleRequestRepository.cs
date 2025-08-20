using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Share.SampleRequestFeature
{
    public class SampleRequestRepository : ISampleRequestRepository
    {
        private readonly ApplicationDbContext _context; 
        public SampleRequestRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        /// <summary>
        /// Thêm một yêu cầu mẫu mới vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="sampleRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task AddAsync(SampleRequest sampleRequest, CancellationToken ct = default)
        {
            await _context.SampleRequests.AddAsync(sampleRequest, ct);
        }


        /// <summary>
        /// Tạo lệnh query để truy vấn yêu cầu mẫu từ cơ sở dữ liệu.
        /// </summary>
        /// <returns></returns>
        public IQueryable<SampleRequest> Query()
        {
            return _context.SampleRequests.AsNoTracking();
        }
    }
}

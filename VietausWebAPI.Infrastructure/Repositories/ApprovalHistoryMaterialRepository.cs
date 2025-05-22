using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories
{
    public class ApprovalHistoryMaterialRepository : IApprovalHistoryMaterialRepository
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public ApprovalHistoryMaterialRepository (ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Thêm lịch sử phê duyệt vật tư
        /// </summary>
        /// <param name="approvalLevelsCommonData"></param>
        /// <returns></returns>
        public async Task AddApprovalHistoryMaterialRepositoryAsync(ApprovalHistoryMaterialDatum approvalLevelsCommonData)
        {
            await _context.ApprovalHistoryMaterialData.AddRangeAsync(approvalLevelsCommonData);
        }
        /// <summary>
        /// Lấy lịch sử phê duyệt vật tư
        /// </summary>
        /// <param name="requestID"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ApprovalHistoryMaterialDatum>> GetApprovalHistoryMaterialRepositoryAsync(string requestID)
        {
            return await _context.ApprovalHistoryMaterialData
                .AsNoTracking()
                .Include(r => r.Employee)
                .Include(r => r.Request)
                .Where(x => x.RequestId == requestID)
                .ToListAsync();
        }

    }
}

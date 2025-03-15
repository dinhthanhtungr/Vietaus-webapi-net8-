using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories
{
    public class ApprovalHistoryMaterialRepository : IApprovalHistoryMaterialRepository
    {
        private readonly ApplicationDbContext _context;

        public ApprovalHistoryMaterialRepository (ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddApprovalHistoryMaterialRepositoryAsync(IEnumerable<ApprovalHistoryMaterialDatum> approvalLevelsCommonData)
        {
            await _context.ApprovalHistoryMaterialData.AddRangeAsync(approvalLevelsCommonData);
            await _context.SaveChangesAsync(); 
        }

        public async Task<IEnumerable<ApprovalHistoryMaterialDatum>> GetApprovalHistoryMaterialRepositoryAsync()
        {
            return await _context.ApprovalHistoryMaterialData.ToListAsync();
        }
    }
}

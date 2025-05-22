using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Internal;
using VietausWebAPI.Core.Application.DTOs.Approval;
using VietausWebAPI.Core.Application.Usecases.Approvals.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.WebAPI.DatabaseContext;
using VietausWebAPI.Infrastructure.Utilities;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace VietausWebAPI.Infrastructure.Repositories.Approval
{
    public class ApprovalRepository : IApprovalRepository
    {
        private readonly ApplicationDbContext _context;
        public ApprovalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveApprovalHistoryHandler(ApprovalHistoryMaterialDatum approvalHistoryMaterialDatum)
        {
            await _context.ApprovalHistoryMaterialData.AddAsync(approvalHistoryMaterialDatum);
        }
    }

}

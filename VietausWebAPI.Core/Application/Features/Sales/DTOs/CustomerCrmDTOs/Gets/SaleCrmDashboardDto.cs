using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets
{
    public class SaleCrmDashboardDto
    {
        public int TodayTasks { get; set; }
        public int OverdueTasks { get; set; }
        public int PendingTasks { get; set; }
        public int CompletedTasksThisMonth { get; set; }
        public int InteractionsThisMonth { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
    }
}

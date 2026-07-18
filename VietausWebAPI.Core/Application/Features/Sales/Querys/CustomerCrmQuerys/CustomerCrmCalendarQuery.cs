using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.Querys.CustomerCrmQuerys
{
    public class CustomerCrmCalendarQuery
    {
        public string? View { get; set; } = "Month";
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? AssignedSaleEmployeeId { get; set; }
        public bool OnlyMine { get; set; }
        public List<string> ActivityTypes { get; set; } = new List<string> { nameof(EventTypeColorKey.FollowUpTask) };
        public bool ShowWeekends { get; set; } = true;
        public bool ShowDeclinedEvents { get; set; } = true;
        public bool ShowCompletedTasks { get; set; } = true;
    }
}

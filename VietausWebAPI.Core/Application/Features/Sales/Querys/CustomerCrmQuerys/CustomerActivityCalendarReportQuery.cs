using System;

namespace VietausWebAPI.Core.Application.Features.Sales.Querys.CustomerCrmQuerys
{
    public class CustomerActivityCalendarReportQuery
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? AssignedSaleEmployeeId { get; set; }
        public bool OnlyMine { get; set; }
        public bool IncludeCustomersWithoutActivity { get; set; }
        public bool IncludeCompletedTasksAsActivity { get; set; } = true;
        public string? Keyword { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets
{
    public class CustomerActivityCalendarReportDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int DayCount { get; set; }
        public int CustomerCount { get; set; }
        public int TotalActivityCount { get; set; }
        public int TotalContactCount { get; set; }
        public decimal TotalRevenueAmount { get; set; }
        public IReadOnlyList<CustomerActivityCalendarSaleSummaryDto> SaleSummaries { get; set; } = new List<CustomerActivityCalendarSaleSummaryDto>();
        public IReadOnlyList<CustomerActivityCalendarWeekDto> Weeks { get; set; } = new List<CustomerActivityCalendarWeekDto>();
        public IReadOnlyList<CustomerActivityCalendarGroupDto> Groups { get; set; } = new List<CustomerActivityCalendarGroupDto>();
    }

    public class CustomerActivityCalendarWeekDto
    {
        public int WeekNumber { get; set; }
        public int FromDay { get; set; }
        public int ToDay { get; set; }
        public string Label { get; set; } = string.Empty;
    }

    public class CustomerActivityCalendarGroupDto
    {
        public string GroupKey { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public decimal? MinRevenueAmount { get; set; }
        public decimal? MaxRevenueAmount { get; set; }
        public int CustomerCount { get; set; }
        public decimal TotalRevenueAmount { get; set; }
        public int TotalActivityCount { get; set; }
        public int TotalContactCount { get; set; }
        public IReadOnlyList<CustomerActivityCalendarCustomerRowDto> Rows { get; set; } = new List<CustomerActivityCalendarCustomerRowDto>();
    }

    public class CustomerActivityCalendarCustomerRowDto
    {
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public Guid? AssignedSaleEmployeeId { get; set; }
        public string? AssignedSaleEmployeeCode { get; set; }
        public string? AssignedSaleEmployeeName { get; set; }
        public decimal RevenueAmount { get; set; }
        public int TotalActivityCount { get; set; }
        public int TotalContactCount { get; set; }
        public int InteractionCount { get; set; }
        public int CompletedTaskCount { get; set; }
        public int OpenTaskCount { get; set; }
        public IReadOnlyList<CustomerActivityCalendarDayCellDto> Days { get; set; } = new List<CustomerActivityCalendarDayCellDto>();
    }

    public class CustomerActivityCalendarDayCellDto
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public bool HasActivity { get; set; }
        public string Mark { get; set; } = string.Empty;
        public int ActivityCount { get; set; }
        public int ContactCount { get; set; }
        public int InteractionCount { get; set; }
        public int CompletedTaskCount { get; set; }
        public int OpenTaskCount { get; set; }
    }

    public class CustomerActivityCalendarSaleSummaryDto
    {
        public Guid SaleEmployeeId { get; set; }
        public string SaleEmployeeCode { get; set; } = string.Empty;
        public string SaleEmployeeName { get; set; } = string.Empty;
        public int TotalInteractionCount { get; set; }
        public int OldCustomerCallCount { get; set; }
        public int NewCustomerCallCount { get; set; }
        public int MeetingCount { get; set; }
    }
}

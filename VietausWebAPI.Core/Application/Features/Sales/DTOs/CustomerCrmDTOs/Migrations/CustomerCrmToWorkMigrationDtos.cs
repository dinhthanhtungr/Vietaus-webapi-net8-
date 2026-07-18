using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Migrations
{
    public sealed class CustomerCrmToWorkMigrationOptions
    {
        public Guid? CompanyId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool IncludeInactive { get; set; }
        public bool DryRun { get; set; } = true;
        public bool SkipExisting { get; set; } = true;
        public bool CreatePrimaryAssigneeFromAssignedSale { get; set; } = true;
    }

    public sealed class CustomerCrmToWorkMigrationResultDto
    {
        public bool DryRun { get; set; }
        public int SourceTaskCount { get; set; }
        public int SourceWorkPlanCount { get; set; }
        public int CreatedWorkTaskCount { get; set; }
        public int CreatedWorkTaskAssigneeCount { get; set; }
        public int CreatedWorkTaskReferenceCount { get; set; }
        public int CreatedWorkPlanCount { get; set; }
        public int CreatedWorkPlanAssigneeCount { get; set; }
        public int CreatedWorkPlanReferenceCount { get; set; }
        public int SkippedTaskCount { get; set; }
        public int SkippedWorkPlanCount { get; set; }
        public List<CustomerCrmToWorkMigrationIssueDto> Issues { get; set; } = new();
    }

    public sealed class CustomerCrmToWorkMigrationIssueDto
    {
        public string Source { get; set; } = string.Empty;
        public Guid SourceId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

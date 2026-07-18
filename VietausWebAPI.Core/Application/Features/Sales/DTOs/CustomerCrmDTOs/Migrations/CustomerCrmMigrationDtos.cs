using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Migrations
{
    public sealed class CustomerCrmLegacyExportQuery
    {
        public Guid? CompanyId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool IncludeInactive { get; set; }
        public bool CustomerOnly { get; set; } = true;
    }

    public sealed class CustomerCrmMigrationImportOptions
    {
        public bool DryRun { get; set; } = true;
        public bool SkipExisting { get; set; } = true;
        public bool SyncCustomerCrmSnapshot { get; set; } = true;
    }

    public sealed class CustomerCrmMigrationImportResultDto
    {
        public bool DryRun { get; set; }
        public int InteractionRows { get; set; }
        public int TaskRows { get; set; }
        public int TaskAssigneeRows { get; set; }
        public int WorkPlanRows { get; set; }
        public int CreatedInteractions { get; set; }
        public int CreatedTasks { get; set; }
        public int CreatedTaskAssignees { get; set; }
        public int CreatedWorkPlans { get; set; }
        public int SkippedRows { get; set; }
        public List<CustomerCrmMigrationRowIssueDto> Issues { get; set; } = new();
    }

    public sealed class CustomerCrmMigrationRowIssueDto
    {
        public string Sheet { get; set; } = string.Empty;
        public int RowNumber { get; set; }
        public string LegacyId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}

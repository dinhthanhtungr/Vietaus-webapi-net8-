namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.DTOs;

public sealed class WorkAssigneeDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

using VietausWebAPI.Core.Domain.Enums.WorkTaskEnums;

namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.DTOs;

public sealed class WorkReferenceRequest
{
    public WorkReferenceType ReferenceType { get; set; }
    public Guid ReferenceId { get; set; }
    public string? ReferenceCodeSnapshot { get; set; }
    public string? ReferenceNameSnapshot { get; set; }
    public bool IsPrimary { get; set; }
}

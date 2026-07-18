using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.Shared.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Notifications.ServiceContracts;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;

namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.Services;

public sealed partial class WorkManagementService : IWorkManagementService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly IVisibilityHelper _visibilityHelper;
    private readonly IWorkTaskRepository _taskRepository;
    private readonly IWorkTaskAssigneeRepository _taskAssigneeRepository;
    private readonly IWorkTaskReferenceRepository _taskReferenceRepository;
    private readonly IWorkPlanRepository _planRepository;
    private readonly IWorkPlanAssigneeRepository _planAssigneeRepository;
    private readonly IWorkPlanReferenceRepository _planReferenceRepository;
    private readonly INotificationService _notificationService;

    public WorkManagementService(
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IVisibilityHelper visibilityHelper,
        IWorkTaskRepository taskRepository,
        IWorkTaskAssigneeRepository taskAssigneeRepository,
        IWorkTaskReferenceRepository taskReferenceRepository,
        IWorkPlanRepository planRepository,
        IWorkPlanAssigneeRepository planAssigneeRepository,
        IWorkPlanReferenceRepository planReferenceRepository,
        INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _visibilityHelper = visibilityHelper;
        _taskRepository = taskRepository;
        _taskAssigneeRepository = taskAssigneeRepository;
        _taskReferenceRepository = taskReferenceRepository;
        _planRepository = planRepository;
        _planAssigneeRepository = planAssigneeRepository;
        _planReferenceRepository = planReferenceRepository;
        _notificationService = notificationService;
    }
}

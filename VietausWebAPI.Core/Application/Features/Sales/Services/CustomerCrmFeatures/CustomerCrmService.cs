using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Patchs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Posts;
using VietausWebAPI.Core.Application.Features.Sales.Helpers.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Sales.Querys.CustomerCrmQuerys;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.Shared.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerCrmFeatures
{
    /// <summary>
    /// Service tổng của CRM khách hàng, gom các nghiệp vụ chăm sóc khách hàng thủ công cho sale.
    /// Các feature chi tiết được tách thành partial file để dễ bảo trì.
    /// </summary>
    public partial class CustomerCrmService : ICustomerCrmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly ICustomerInteractionRepository _interactionRepository;
        private readonly ICustomerFollowUpTaskRepository _followUpTaskRepository;
        private readonly ICustomerFollowUpTaskAssigneeRepository _followUpTaskAssigneeRepository;
        private readonly ICustomerWorkPlanRepository _workPlanRepository;
        private readonly ICustomerInteractionAiSummaryRepository _aiSummaryRepository;
        private readonly IVisibilityHelper _visibilityHelper;
        private readonly IGeminiCustomerSummaryClient _geminiCustomerSummaryClient;
        private readonly IGeminiRateLimitService _geminiRateLimitService;

        public CustomerCrmService(
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            ICustomerInteractionRepository interactionRepository,
            ICustomerFollowUpTaskRepository followUpTaskRepository,
            ICustomerFollowUpTaskAssigneeRepository followUpTaskAssigneeRepository,
            ICustomerWorkPlanRepository workPlanRepository,
            ICustomerInteractionAiSummaryRepository aiSummaryRepository,
            IVisibilityHelper visibilityHelper,
            IGeminiCustomerSummaryClient geminiCustomerSummaryClient,
            IGeminiRateLimitService geminiRateLimitService)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _interactionRepository = interactionRepository;
            _followUpTaskRepository = followUpTaskRepository;
            _followUpTaskAssigneeRepository = followUpTaskAssigneeRepository;
            _workPlanRepository = workPlanRepository;
            _aiSummaryRepository = aiSummaryRepository;
            _visibilityHelper = visibilityHelper;
            _geminiCustomerSummaryClient = geminiCustomerSummaryClient;
            _geminiRateLimitService = geminiRateLimitService;
        }
    }
}

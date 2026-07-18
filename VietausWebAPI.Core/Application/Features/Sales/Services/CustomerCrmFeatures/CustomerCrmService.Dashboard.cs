using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Patchs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Posts;
using VietausWebAPI.Core.Application.Features.Sales.Querys.CustomerCrmQuerys;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerCrmFeatures
{
    /// <summary>
    /// Feature tổng hợp số liệu dashboard CRM cho sale, leader và director.
    /// </summary>
    public partial class CustomerCrmService
    {
        /// <summary>
        /// Lấy dashboard CRM theo sale hiện tại, gồm task hôm nay, task quá hạn, task đang mở và tương tác trong tháng.
        /// </summary>
        public async Task<OperationResult<SaleCrmDashboardDto>> GetSaleDashboardAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var today = now.Date;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var companyId = _currentUser.CompanyId;
            var employeeId = _currentUser.EmployeeId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var taskQ = _followUpTaskRepository.Query()
                .Where(x => x.CompanyId == companyId
                            && x.IsActive
                            && x.AssignedSaleEmployeeId == employeeId
                            && visibleCustomerIds.Contains(x.CustomerId));

            var interactionQ = _interactionRepository.Query()
                .Where(x => x.CompanyId == companyId
                            && x.IsActive
                            && x.AssignedSaleEmployeeId == employeeId
                            && visibleCustomerIds.Contains(x.CustomerId));

            var dto = new SaleCrmDashboardDto
            {
                TodayTasks = await taskQ.CountAsync(x => x.DueDate.HasValue && x.DueDate.Value.Date == today && x.Status != CustomerFollowUpTaskStatus.Done && x.Status != CustomerFollowUpTaskStatus.Canceled, ct),
                OverdueTasks = await taskQ.CountAsync(x => x.DueDate.HasValue && x.DueDate.Value.Date < today && x.Status != CustomerFollowUpTaskStatus.Done && x.Status != CustomerFollowUpTaskStatus.Canceled, ct),
                PendingTasks = await taskQ.CountAsync(x => x.Status == CustomerFollowUpTaskStatus.Pending || x.Status == CustomerFollowUpTaskStatus.InProgress, ct),
                CompletedTasksThisMonth = await taskQ.CountAsync(x => x.CompletedDate >= monthStart && x.Status == CustomerFollowUpTaskStatus.Done, ct),
                InteractionsThisMonth = await interactionQ.CountAsync(x => x.InteractionAt >= monthStart, ct),
                NextFollowUpDate = await taskQ.Where(x => x.DueDate >= today && x.Status != CustomerFollowUpTaskStatus.Done && x.Status != CustomerFollowUpTaskStatus.Canceled).MinAsync(x => (DateTime?)x.DueDate, ct)
            };

            return OperationResult<SaleCrmDashboardDto>.Ok(dto);
        }

        /// <summary>
        /// Lấy dashboard CRM cấp leader để theo dõi tình hình chăm sóc khách hàng của nhóm.
        /// </summary>
        public async Task<OperationResult<LeaderCrmDashboardDto>> GetLeaderDashboardAsync(CancellationToken ct = default)
        {
            var dto = await BuildLeaderDashboardAsync(ct);
            return OperationResult<LeaderCrmDashboardDto>.Ok(dto);
        }

        /// <summary>
        /// Lấy dashboard CRM cấp director để xem tổng quan khách hàng, task, tương tác và work plan toàn công ty.
        /// </summary>
        public async Task<OperationResult<DirectorCrmDashboardDto>> GetDirectorDashboardAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var today = now.Date;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var companyId = _currentUser.CompanyId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var taskQ = _followUpTaskRepository.Query()
                .Where(x => x.CompanyId == companyId
                            && x.IsActive
                            && visibleCustomerIds.Contains(x.CustomerId));

            var interactionQ = _interactionRepository.Query()
                .Where(x => x.CompanyId == companyId
                            && x.IsActive
                            && visibleCustomerIds.Contains(x.CustomerId));

            var workPlanQ = _workPlanRepository.Query()
                .Where(x => x.CompanyId == companyId
                            && x.IsActive
                            && visibleCustomerIds.Contains(x.CustomerId));

            var dto = new DirectorCrmDashboardDto
            {
                ActiveCustomers = await visibleCustomerIds.CountAsync(ct),
                OpenTasks = await taskQ.CountAsync(x => x.Status != CustomerFollowUpTaskStatus.Done && x.Status != CustomerFollowUpTaskStatus.Canceled, ct),
                OverdueTasks = await taskQ.CountAsync(x => x.DueDate.HasValue && x.DueDate.Value.Date < today && x.Status != CustomerFollowUpTaskStatus.Done && x.Status != CustomerFollowUpTaskStatus.Canceled, ct),
                CompletedTasksThisMonth = await taskQ.CountAsync(x => x.CompletedDate >= monthStart && x.Status == CustomerFollowUpTaskStatus.Done, ct),
                InteractionsThisMonth = await interactionQ.CountAsync(x => x.InteractionAt >= monthStart, ct),
                ActiveWorkPlans = await workPlanQ.CountAsync(x => x.Status == CustomerWorkPlanStatus.Active, ct)
            };

            return OperationResult<DirectorCrmDashboardDto>.Ok(dto);
        }

        /// <summary>
        /// Build dữ liệu dashboard dùng chung cho cấp leader.
        /// </summary>
        private async Task<LeaderCrmDashboardDto> BuildLeaderDashboardAsync(CancellationToken ct)
        {
            var now = DateTime.Now;
            var today = now.Date;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var companyId = _currentUser.CompanyId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var taskQ = _followUpTaskRepository.Query()
                .Where(x => x.CompanyId == companyId
                            && x.IsActive
                            && visibleCustomerIds.Contains(x.CustomerId));

            var interactionQ = _interactionRepository.Query()
                .Where(x => x.CompanyId == companyId
                            && x.IsActive
                            && visibleCustomerIds.Contains(x.CustomerId));

            var workPlanQ = _workPlanRepository.Query()
                .Where(x => x.CompanyId == companyId
                            && x.IsActive
                            && visibleCustomerIds.Contains(x.CustomerId));

            return new LeaderCrmDashboardDto
            {
                ActiveCustomers = await visibleCustomerIds.CountAsync(ct),
                OpenTasks = await taskQ.CountAsync(x => x.Status != CustomerFollowUpTaskStatus.Done && x.Status != CustomerFollowUpTaskStatus.Canceled, ct),
                OverdueTasks = await taskQ.CountAsync(x => x.DueDate.HasValue && x.DueDate.Value.Date < today && x.Status != CustomerFollowUpTaskStatus.Done && x.Status != CustomerFollowUpTaskStatus.Canceled, ct),
                InteractionsThisMonth = await interactionQ.CountAsync(x => x.InteractionAt >= monthStart, ct),
                ActiveWorkPlans = await workPlanQ.CountAsync(x => x.Status == CustomerWorkPlanStatus.Active, ct)
            };
        }
    }
}

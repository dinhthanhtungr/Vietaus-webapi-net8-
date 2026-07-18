using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets;
using VietausWebAPI.Core.Application.Features.Sales.Querys.CustomerCrmQuerys;
using VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;
using VietausWebAPI.Core.Domain.Enums.Merchadises;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerCrmFeatures
{
    public partial class CustomerCrmService
    {
        private const decimal HighRevenueThreshold = 100_000_000m;
        private const decimal LowRevenueThreshold = 50_000_000m;

        /// <summary>
        /// Tạo báo cáo lịch hoạt động khách hàng trong một khoảng thời gian.
        /// Method này gom dữ liệu khách hàng theo quyền nhìn thấy của user hiện tại,
        /// tính doanh số, số lần tương tác, task hoàn tất, task còn mở và chia nhóm khách hàng theo doanh số.
        /// </summary>
        /// <param name="query">Điều kiện lọc báo cáo như tháng/năm, khoảng ngày, sale phụ trách, khách hàng, keyword và tùy chọn hiển thị.</param>
        /// <param name="ct">Token dùng để hủy thao tác bất đồng bộ khi request bị hủy.</param>
        /// <returns>Báo cáo lịch hoạt động khách hàng đã được gom nhóm và tổng hợp.</returns>
        public async Task<OperationResult<CustomerActivityCalendarReportDto>> GetCustomerActivityCalendarReportAsync(
            CustomerActivityCalendarReportQuery query,
            CancellationToken ct = default)
        {
            query ??= new CustomerActivityCalendarReportQuery();

            var (from, toExclusive) = ResolveActivityCalendarRange(query);
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);
            var scopedEmployeeId = ResolveScopedEmployeeId(viewer, query.AssignedSaleEmployeeId, query.OnlyMine);
            var companyId = _currentUser.CompanyId;

            var customerQ = _unitOfWork.CustomerRepository.Query()
                .Where(x =>
                    x.CompanyId == companyId
                    && x.IsActive == true
                    && visibleCustomerIds.Contains(x.CustomerId));

            if (query.CustomerId.HasValue)
                customerQ = customerQ.Where(x => x.CustomerId == query.CustomerId.Value);

            if (scopedEmployeeId.HasValue)
            {
                var employeeId = scopedEmployeeId.Value;
                var interactionCustomerIds = _interactionRepository.Query()
                    .Where(i =>
                        i.CompanyId == companyId
                        && i.IsActive
                        && i.InteractionAt >= from
                        && i.InteractionAt < toExclusive
                        && (i.AssignedSaleEmployeeId == employeeId || i.CreatedBy == employeeId))
                    .Select(i => i.CustomerId);

                customerQ = customerQ.Where(x =>
                    x.CurrentSaleId == employeeId
                    || x.CustomerAssignments.Any(ca =>
                        ca.IsActive
                        && ca.CompanyId == companyId
                        && ca.EmployeeId == employeeId)
                    || interactionCustomerIds.Contains(x.CustomerId));
            }

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var keyword = query.Keyword.Trim();
                customerQ = customerQ.Where(x =>
                    x.ExternalId.Contains(keyword)
                    || x.CustomerName.Contains(keyword));
            }

            var customerRows = await customerQ
                .Select(x => new CustomerActivityCalendarCustomerSource
                {
                    CustomerId = x.CustomerId,
                    CustomerCode = x.ExternalId,
                    CustomerName = x.CustomerName,
                    CurrentSaleId = x.CurrentSaleId
                })
                .ToListAsync(ct);

            var customerIds = customerRows
                .Select(x => x.CustomerId)
                .Distinct()
                .ToList();

            var saleLookup = await BuildCustomerSaleLookupAsync(customerIds, ct);
            var revenueLookup = await BuildCustomerRevenueLookupAsync(customerIds, from, toExclusive, ct);
            var interactionLookup = await BuildInteractionActivityLookupAsync(customerIds, from, toExclusive, scopedEmployeeId, ct);
            var completedTaskLookup = query.IncludeCompletedTasksAsActivity
                ? await BuildCompletedTaskActivityLookupAsync(customerIds, from, toExclusive, scopedEmployeeId, ct)
                : new Dictionary<(Guid CustomerId, DateTime Date), int>();
            var openTaskLookup = await BuildOpenTaskLookupAsync(customerIds, from, toExclusive, scopedEmployeeId, ct);
            var saleSummaries = await BuildSaleInteractionSummaryAsync(customerIds, from, toExclusive, scopedEmployeeId, ct);

            var days = Enumerable.Range(0, (toExclusive.Date - from.Date).Days)
                .Select(offset => from.Date.AddDays(offset))
                .ToList();

            var rows = customerRows
                .Select(customer =>
                {
                    var sale = saleLookup.TryGetValue(customer.CustomerId, out var assignedSale)
                        ? assignedSale
                        : null;

                    var dayCells = days
                        .Select(date =>
                        {
                            var key = (customer.CustomerId, date);
                            var interactionCounts = interactionLookup.TryGetValue(key, out var interactions)
                                ? interactions
                                : CustomerActivityCalendarInteractionCounts.Empty;
                            var completedTaskCount = completedTaskLookup.TryGetValue(key, out var completedTasks) ? completedTasks : 0;
                            var openTaskCount = openTaskLookup.TryGetValue(key, out var openTasks) ? openTasks : 0;
                            var activityCount = interactionCounts.DirectMeetingCount;
                            var contactCount = interactionCounts.OtherInteractionCount;
                            var interactionCount = interactionCounts.TotalInteractionCount;

                            return new CustomerActivityCalendarDayCellDto
                            {
                                Day = date.Day,
                                Date = date,
                                HasActivity = activityCount > 0 || contactCount > 0,
                                Mark = activityCount > 0 || contactCount > 0 ? "X" : string.Empty,
                                ActivityCount = activityCount,
                                ContactCount = contactCount,
                                InteractionCount = interactionCount,
                                CompletedTaskCount = completedTaskCount,
                                OpenTaskCount = openTaskCount
                            };
                        })
                        .ToList();

                    return new CustomerActivityCalendarCustomerRowDto
                    {
                        CustomerId = customer.CustomerId,
                        CustomerCode = customer.CustomerCode,
                        CustomerName = customer.CustomerName,
                        AssignedSaleEmployeeId = sale?.EmployeeId ?? customer.CurrentSaleId,
                        AssignedSaleEmployeeCode = sale?.EmployeeCode,
                        AssignedSaleEmployeeName = sale?.EmployeeName,
                        RevenueAmount = revenueLookup.TryGetValue(customer.CustomerId, out var revenue) ? revenue : 0m,
                        TotalActivityCount = dayCells.Sum(x => x.ActivityCount),
                        TotalContactCount = dayCells.Sum(x => x.ContactCount),
                        InteractionCount = dayCells.Sum(x => x.InteractionCount),
                        CompletedTaskCount = dayCells.Sum(x => x.CompletedTaskCount),
                        OpenTaskCount = dayCells.Sum(x => x.OpenTaskCount),
                        Days = dayCells
                    };
                })
                .Where(x =>
                    query.IncludeCustomersWithoutActivity
                    || x.InteractionCount > 0
                    || x.OpenTaskCount > 0
                    || x.RevenueAmount > 0m)
                .OrderByDescending(x => x.RevenueAmount)
                .ThenBy(x => x.CustomerName)
                .ToList();

            var groups = rows
                .GroupBy(BuildRevenueGroup)
                .OrderBy(x => GetRevenueGroupSort(x.Key.GroupKey))
                .Select(g => new CustomerActivityCalendarGroupDto
                {
                    GroupKey = g.Key.GroupKey,
                    GroupName = g.Key.GroupName,
                    MinRevenueAmount = g.Key.MinRevenueAmount,
                    MaxRevenueAmount = g.Key.MaxRevenueAmount,
                    CustomerCount = g.Count(),
                    TotalRevenueAmount = g.Sum(x => x.RevenueAmount),
                    TotalActivityCount = g.Sum(x => x.TotalActivityCount),
                    TotalContactCount = g.Sum(x => x.TotalContactCount),
                    Rows = g.ToList()
                })
                .ToList();

            var report = new CustomerActivityCalendarReportDto
            {
                From = from,
                To = toExclusive.AddTicks(-1),
                Year = from.Year,
                Month = from.Month,
                DayCount = days.Count,
                CustomerCount = rows.Count,
                TotalActivityCount = rows.Sum(x => x.TotalActivityCount),
                TotalContactCount = rows.Sum(x => x.TotalContactCount),
                TotalRevenueAmount = rows.Sum(x => x.RevenueAmount),
                SaleSummaries = saleSummaries,
                Weeks = BuildActivityCalendarWeeks(from, toExclusive),
                Groups = groups
            };

            return OperationResult<CustomerActivityCalendarReportDto>.Ok(report);
        }

        /// <summary>
        /// Xuất báo cáo lịch hoạt động khách hàng ra file Excel.
        /// Method này gọi lại logic tạo báo cáo, sau đó build workbook bằng ClosedXML.
        /// </summary>
        /// <param name="query">Điều kiện lọc báo cáo cần xuất Excel.</param>
        /// <param name="ct">Token dùng để hủy thao tác bất đồng bộ khi request bị hủy.</param>
        /// <returns>Mảng byte của file Excel; trả về mảng rỗng nếu không tạo được báo cáo.</returns>
        public async Task<byte[]> ExportCustomerActivityCalendarReportExcelAsync(
            CustomerActivityCalendarReportQuery query,
            CancellationToken ct = default)
        {
            var result = await GetCustomerActivityCalendarReportAsync(query, ct);
            if (!result.Success || result.Data == null)
                return Array.Empty<byte>();

            return BuildCustomerActivityCalendarWorkbook(result.Data);
        }

        /// <summary>
        /// Tạo lookup thông tin sale đang được assign cho từng khách hàng.
        /// Nếu một khách hàng có nhiều assignment, method lấy assignment mới nhất theo UpdatedDate/CreatedDate.
        /// </summary>
        /// <param name="customerIds">Danh sách khách hàng cần lấy sale phụ trách.</param>
        /// <param name="ct">Token dùng để hủy thao tác bất đồng bộ.</param>
        /// <returns>Dictionary có key là CustomerId và value là thông tin sale phụ trách.</returns>
        private async Task<Dictionary<Guid, CustomerActivityCalendarSaleSource>> BuildCustomerSaleLookupAsync(
            IReadOnlyCollection<Guid> customerIds,
            CancellationToken ct)
        {
            if (customerIds.Count == 0)
                return new Dictionary<Guid, CustomerActivityCalendarSaleSource>();

            var companyId = _currentUser.CompanyId;

            var rows = await _unitOfWork.CustomerAssignmentRepository.Query()
                .Where(x =>
                    x.IsActive
                    && x.CompanyId == companyId
                    && customerIds.Contains(x.CustomerId))
                .OrderByDescending(x => x.UpdatedDate)
                .ThenByDescending(x => x.CreatedDate)
                .Select(x => new CustomerActivityCalendarSaleSource
                {
                    CustomerId = x.CustomerId,
                    EmployeeId = x.EmployeeId,
                    EmployeeCode = x.Employee.ExternalId,
                    EmployeeName = x.Employee.FullName
                })
                .ToListAsync(ct);

            return rows
                .GroupBy(x => x.CustomerId)
                .ToDictionary(g => g.Key, g => g.First());
        }

        /// <summary>
        /// Tạo lookup thông tin sale đang được assign cho từng khách hàng.
        /// Nếu một khách hàng có nhiều assignment, method lấy assignment mới nhất theo UpdatedDate/CreatedDate.
        /// </summary>
        /// <param name="customerIds">Danh sách khách hàng cần lấy sale phụ trách.</param>
        /// <param name="ct">Token dùng để hủy thao tác bất đồng bộ.</param>
        /// <returns>Dictionary có key là CustomerId và value là thông tin sale phụ trách.</returns>
        private async Task<Dictionary<Guid, decimal>> BuildCustomerRevenueLookupAsync(
            IReadOnlyCollection<Guid> customerIds,
            DateTime from,
            DateTime toExclusive,
            CancellationToken ct)
        {
            if (customerIds.Count == 0)
                return new Dictionary<Guid, decimal>();

            var companyId = _currentUser.CompanyId;

            var orderQ = _unitOfWork.MerchandiseOrderRepository.Query()
                .Where(x =>
                    x.IsActive
                    && x.CompanyId == companyId
                    && x.OrderType == OrderType.Merchandise
                    && x.CreateDate >= from
                    && x.CreateDate < toExclusive
                    && customerIds.Contains(x.CustomerId));

            var revenueRows = await (
                    from mo in orderQ
                    join mod in _unitOfWork.MerchandiseOrderRepository.QueryDetail()
                            .Where(x => x.IsActive)
                        on mo.MerchandiseOrderId equals mod.MerchandiseOrderId
                    group mod by mo.CustomerId into g
                    select new
                    {
                        CustomerId = g.Key,
                        RevenueAmount = g.Sum(x => x.TotalPriceAgreed)
                    })
                .ToListAsync(ct);

            return revenueRows.ToDictionary(x => x.CustomerId, x => x.RevenueAmount);
        }

        /// <summary>
        /// Tạo lookup số lượng lịch sử tương tác của từng khách hàng theo từng ngày.
        /// Kết quả dùng để đánh dấu ô ACT trong lịch hoạt động khách hàng.
        /// </summary>
        /// <param name="customerIds">Danh sách khách hàng cần thống kê tương tác.</param>
        /// <param name="from">Ngày bắt đầu thống kê, tính cả ngày này.</param>
        /// <param name="toExclusive">Ngày kết thúc thống kê dạng exclusive, không tính ngày này.</param>
        /// <param name="scopedEmployeeId">Sale được lọc theo phạm vi xem dữ liệu; null nghĩa là không lọc theo sale.</param>
        /// <param name="ct">Token dùng để hủy thao tác bất đồng bộ.</param>
        /// <returns>Dictionary có key là cặp CustomerId/Date và value là số lần tương tác trực tiếp/liên hệ khác trong ngày đó.</returns>
        private async Task<Dictionary<(Guid CustomerId, DateTime Date), CustomerActivityCalendarInteractionCounts>> BuildInteractionActivityLookupAsync(
            IReadOnlyCollection<Guid> customerIds,
            DateTime from,
            DateTime toExclusive,
            Guid? scopedEmployeeId,
            CancellationToken ct)
        {
            if (customerIds.Count == 0)
                return new Dictionary<(Guid CustomerId, DateTime Date), CustomerActivityCalendarInteractionCounts>();

            var companyId = _currentUser.CompanyId;

            var q = _interactionRepository.Query()
                .Where(x =>
                    x.CompanyId == companyId
                    && x.IsActive
                    && x.InteractionAt >= from
                    && x.InteractionAt < toExclusive
                    && customerIds.Contains(x.CustomerId));

            if (scopedEmployeeId.HasValue)
                q = q.Where(x => x.AssignedSaleEmployeeId == scopedEmployeeId.Value || x.CreatedBy == scopedEmployeeId.Value);

            var rows = await q
                .GroupBy(x => new { x.CustomerId, Date = x.InteractionAt.Date })
                .Select(g => new
                {
                    g.Key.CustomerId,
                    g.Key.Date,
                    DirectMeetingCount = g.Count(x => x.InteractionType == CustomerInteractionType.Meeting || x.InteractionType == CustomerInteractionType.Visit),
                    OtherInteractionCount = g.Count(x => x.InteractionType != CustomerInteractionType.Meeting && x.InteractionType != CustomerInteractionType.Visit),
                    TotalInteractionCount = g.Count()
                })
                .ToListAsync(ct);

            return rows.ToDictionary(
                x => (x.CustomerId, x.Date),
                x => new CustomerActivityCalendarInteractionCounts(
                    x.DirectMeetingCount,
                    x.OtherInteractionCount,
                    x.TotalInteractionCount));
        }

        /// <summary>
        /// Tạo bảng tổng hợp hoạt động của từng sale trong kỳ báo cáo.
        /// Bao gồm tổng số tương tác, cuộc gọi khách hàng cũ, cuộc gọi khách hàng mới và số buổi hẹn gặp/visit.
        /// </summary>
        /// <param name="customerIds">Danh sách khách hàng nằm trong phạm vi báo cáo.</param>
        /// <param name="from">Ngày bắt đầu thống kê, tính cả ngày này.</param>
        /// <param name="toExclusive">Ngày kết thúc thống kê dạng exclusive, không tính ngày này.</param>
        /// <param name="scopedEmployeeId">Sale được lọc theo phạm vi xem dữ liệu; null nghĩa là tổng hợp tất cả sale nhìn thấy được.</param>
        /// <param name="ct">Token dùng để hủy thao tác bất đồng bộ.</param>
        /// <returns>Danh sách tổng hợp hoạt động theo từng sale, sắp xếp theo số lượng tương tác giảm dần.</returns>
        private async Task<List<CustomerActivityCalendarSaleSummaryDto>> BuildSaleInteractionSummaryAsync(
            IReadOnlyCollection<Guid> customerIds,
            DateTime from,
            DateTime toExclusive,
            Guid? scopedEmployeeId,
            CancellationToken ct)
        {
            if (customerIds.Count == 0)
                return new List<CustomerActivityCalendarSaleSummaryDto>();

            var companyId = _currentUser.CompanyId;

            var q = _interactionRepository.Query()
                .Where(x =>
                    x.CompanyId == companyId
                    && x.IsActive
                    && x.InteractionAt >= from
                    && x.InteractionAt < toExclusive
                    && customerIds.Contains(x.CustomerId));

            if (scopedEmployeeId.HasValue)
                q = q.Where(x => x.AssignedSaleEmployeeId == scopedEmployeeId.Value || x.CreatedBy == scopedEmployeeId.Value);

            var rows = await q
                .Select(x => new
                {
                    SaleEmployeeId = x.AssignedSaleEmployeeId ?? x.CreatedBy,
                    SaleEmployeeCode = x.AssignedSaleEmployeeId.HasValue && x.AssignedSaleEmployee != null
                        ? x.AssignedSaleEmployee.ExternalId
                        : x.CreatedByNavigation.ExternalId,
                    SaleEmployeeName = x.AssignedSaleEmployeeId.HasValue && x.AssignedSaleEmployee != null
                        ? x.AssignedSaleEmployee.FullName
                        : x.CreatedByNavigation.FullName,
                    x.InteractionType,
                    IsNewCustomer = x.Customer.IsLead
                })
                .ToListAsync(ct);

            return rows
                .GroupBy(x => new
                {
                    x.SaleEmployeeId,
                    x.SaleEmployeeCode,
                    x.SaleEmployeeName
                })
                .Select(g => new CustomerActivityCalendarSaleSummaryDto
                {
                    SaleEmployeeId = g.Key.SaleEmployeeId,
                    SaleEmployeeCode = g.Key.SaleEmployeeCode,
                    SaleEmployeeName = g.Key.SaleEmployeeName,
                    TotalInteractionCount = g.Count(),
                    OldCustomerCallCount = g.Count(x => x.InteractionType == CustomerInteractionType.Call && !x.IsNewCustomer),
                    NewCustomerCallCount = g.Count(x => x.InteractionType == CustomerInteractionType.Call && x.IsNewCustomer),
                    MeetingCount = g.Count(x => x.InteractionType == CustomerInteractionType.Meeting || x.InteractionType == CustomerInteractionType.Visit)
                })
                .OrderByDescending(x => x.TotalInteractionCount)
                .ThenBy(x => x.SaleEmployeeName)
                .ToList();
        }

        /// <summary>
        /// Tạo lookup số lượng follow-up task đã hoàn tất của từng khách hàng theo từng ngày.
        /// Lookup này chỉ được dùng khi báo cáo cần tính task hoàn tất như một loại hoạt động.
        /// </summary>
        /// <param name="customerIds">Danh sách khách hàng cần thống kê task hoàn tất.</param>
        /// <param name="from">Ngày bắt đầu thống kê, tính cả ngày này.</param>
        /// <param name="toExclusive">Ngày kết thúc thống kê dạng exclusive, không tính ngày này.</param>
        /// <param name="scopedEmployeeId">Sale được lọc theo phạm vi xem dữ liệu; null nghĩa là không lọc theo sale.</param>
        /// <param name="ct">Token dùng để hủy thao tác bất đồng bộ.</param>
        /// <returns>Dictionary có key là cặp CustomerId/Date và value là số task hoàn tất trong ngày đó.</returns>
        private async Task<Dictionary<(Guid CustomerId, DateTime Date), int>> BuildCompletedTaskActivityLookupAsync(
            IReadOnlyCollection<Guid> customerIds,
            DateTime from,
            DateTime toExclusive,
            Guid? scopedEmployeeId,
            CancellationToken ct)
        {
            if (customerIds.Count == 0)
                return new Dictionary<(Guid CustomerId, DateTime Date), int>();

            var companyId = _currentUser.CompanyId;

            var q = _followUpTaskRepository.Query()
                .Where(x =>
                    x.CompanyId == companyId
                    && x.IsActive
                    && x.Status == CustomerFollowUpTaskStatus.Done
                    && x.CompletedDate.HasValue
                    && x.CompletedDate.Value >= from
                    && x.CompletedDate.Value < toExclusive
                    && customerIds.Contains(x.CustomerId));

            if (scopedEmployeeId.HasValue)
                q = q.Where(x => x.AssignedSaleEmployeeId == scopedEmployeeId.Value || x.CreatedBy == scopedEmployeeId.Value || x.CompletedBy == scopedEmployeeId.Value);

            var rows = await q
                .GroupBy(x => new { x.CustomerId, Date = x.CompletedDate!.Value.Date })
                .Select(g => new
                {
                    g.Key.CustomerId,
                    g.Key.Date,
                    Count = g.Count()
                })
                .ToListAsync(ct);

            return rows.ToDictionary(x => (x.CustomerId, x.Date), x => x.Count);
        }

        /// <summary>
        /// Tạo lookup số lượng follow-up task còn mở của từng khách hàng theo ngày đến hạn.
        /// Task đã Done hoặc Canceled sẽ không được tính là task còn mở.
        /// </summary>
        /// <param name="customerIds">Danh sách khách hàng cần thống kê task còn mở.</param>
        /// <param name="from">Ngày bắt đầu thống kê, tính cả ngày này.</param>
        /// <param name="toExclusive">Ngày kết thúc thống kê dạng exclusive, không tính ngày này.</param>
        /// <param name="scopedEmployeeId">Sale được lọc theo phạm vi xem dữ liệu; null nghĩa là không lọc theo sale.</param>
        /// <param name="ct">Token dùng để hủy thao tác bất đồng bộ.</param>
        /// <returns>Dictionary có key là cặp CustomerId/Date và value là số task còn mở đến hạn trong ngày đó.</returns>
        private async Task<Dictionary<(Guid CustomerId, DateTime Date), int>> BuildOpenTaskLookupAsync(
            IReadOnlyCollection<Guid> customerIds,
            DateTime from,
            DateTime toExclusive,
            Guid? scopedEmployeeId,
            CancellationToken ct)
        {
            if (customerIds.Count == 0)
                return new Dictionary<(Guid CustomerId, DateTime Date), int>();

            var companyId = _currentUser.CompanyId;

            var q = _followUpTaskRepository.Query()
                .Where(x =>
                    x.CompanyId == companyId
                    && x.IsActive
                    && x.DueDate.HasValue
                    && x.DueDate.Value >= from
                    && x.DueDate.Value < toExclusive
                    && x.Status != CustomerFollowUpTaskStatus.Done
                    && x.Status != CustomerFollowUpTaskStatus.Canceled
                    && customerIds.Contains(x.CustomerId));

            if (scopedEmployeeId.HasValue)
                q = q.Where(x => x.AssignedSaleEmployeeId == scopedEmployeeId.Value || x.CreatedBy == scopedEmployeeId.Value);

            var rows = await q
                .GroupBy(x => new { x.CustomerId, Date = x.DueDate!.Value.Date })
                .Select(g => new
                {
                    g.Key.CustomerId,
                    g.Key.Date,
                    Count = g.Count()
                })
                .ToListAsync(ct);

            return rows.ToDictionary(x => (x.CustomerId, x.Date), x => x.Count);
        }

        /// <summary>
        /// Xác định khoảng thời gian báo cáo.
        /// Ưu tiên From/To nếu hợp lệ; nếu không có thì dùng Year/Month hoặc tháng hiện tại.
        /// </summary>
        /// <param name="query">Query chứa From/To hoặc Year/Month.</param>
        /// <returns>Cặp ngày bắt đầu và ngày kết thúc dạng exclusive.</returns>
        private static (DateTime From, DateTime ToExclusive) ResolveActivityCalendarRange(CustomerActivityCalendarReportQuery query)
        {
            if (query.From.HasValue && query.To.HasValue && query.To.Value.Date >= query.From.Value.Date)
                return (query.From.Value.Date, query.To.Value.Date.AddDays(1));

            var now = DateTime.Now;
            var year = query.Year.GetValueOrDefault(now.Year);
            var month = query.Month.GetValueOrDefault(now.Month);

            if (month < 1 || month > 12)
                month = now.Month;

            var start = new DateTime(year, month, 1);
            return (start, start.AddMonths(1));
        }

        /// <summary>
        /// Chia khoảng ngày báo cáo thành các nhóm tuần để render header WEEK trong file Excel.
        /// Mỗi tuần tối đa 7 ngày và tuần cuối có thể ít hơn 7 ngày.
        /// </summary>
        /// <param name="from">Ngày bắt đầu báo cáo.</param>
        /// <param name="toExclusive">Ngày kết thúc báo cáo dạng exclusive.</param>
        /// <returns>Danh sách thông tin tuần dùng cho header lịch hoạt động.</returns>
        private static List<CustomerActivityCalendarWeekDto> BuildActivityCalendarWeeks(DateTime from, DateTime toExclusive)
        {
            var dayCount = (toExclusive.Date - from.Date).Days;
            var weeks = new List<CustomerActivityCalendarWeekDto>();

            for (var startDay = 1; startDay <= dayCount; startDay += 7)
            {
                var endDay = Math.Min(startDay + 6, dayCount);
                var weekNumber = weeks.Count + 1;

                weeks.Add(new CustomerActivityCalendarWeekDto
                {
                    WeekNumber = weekNumber,
                    FromDay = startDay,
                    ToDay = endDay,
                    Label = $"WEEK {weekNumber}"
                });
            }

            return weeks;
        }

        /// <summary>
        /// Xác định nhóm doanh số của một khách hàng dựa trên RevenueAmount.
        /// Nhóm này dùng để chia block khách hàng trong báo cáo và file Excel.
        /// </summary>
        /// <param name="row">Dòng dữ liệu khách hàng đã có doanh số trong kỳ.</param>
        /// <returns>Thông tin key, tên nhóm và ngưỡng doanh số tương ứng.</returns>
        private static CustomerActivityCalendarRevenueGroupKey BuildRevenueGroup(CustomerActivityCalendarCustomerRowDto row)
        {
            if (row.RevenueAmount >= HighRevenueThreshold)
            {
                return new CustomerActivityCalendarRevenueGroupKey(
                    "GT100",
                    "Nhom KH co doanh so >= 100 trieu",
                    HighRevenueThreshold,
                    null);
            }

            if (row.RevenueAmount >= LowRevenueThreshold)
            {
                return new CustomerActivityCalendarRevenueGroupKey(
                    "GT50_LT100",
                    "Nhom KH co doanh so tu 50 den duoi 100 trieu",
                    LowRevenueThreshold,
                    HighRevenueThreshold);
            }

            if (row.RevenueAmount > 0m)
            {
                return new CustomerActivityCalendarRevenueGroupKey(
                    "LT50",
                    "Nhom KH co doanh so < 50 trieu",
                    0m,
                    LowRevenueThreshold);
            }

            return new CustomerActivityCalendarRevenueGroupKey(
                "NO_REVENUE",
                "Nhom KH chua co doanh so trong ky",
                null,
                0m);
        }

        /// <summary>
        /// Trả về thứ tự sắp xếp của nhóm doanh số khi hiển thị báo cáo.
        /// Nhóm doanh số cao được ưu tiên đứng trước.
        /// </summary>
        /// <param name="groupKey">Mã nhóm doanh số.</param>
        /// <returns>Số thứ tự dùng để sắp xếp nhóm.</returns>
        private static int GetRevenueGroupSort(string groupKey)
        {
            return groupKey switch
            {
                "GT100" => 1,
                "GT50_LT100" => 2,
                "LT50" => 3,
                _ => 4
            };
        }

        /// <summary>
        /// Build workbook Excel cho báo cáo lịch hoạt động khách hàng.
        /// Sheet chính gồm thông tin khách hàng, cột ngày trong tháng, dấu ACT và block tổng hợp sale.
        /// </summary>
        /// <param name="report">Dữ liệu báo cáo đã được tổng hợp.</param>
        /// <returns>Mảng byte của file Excel sau khi render bằng ClosedXML.</returns>
        private static byte[] BuildCustomerActivityCalendarWorkbook(CustomerActivityCalendarReportDto report)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Customer ACT");

            var monthText = report.From.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture);
            var dayCount = report.DayCount;
            var lastColumn = 2 + dayCount;

            ws.Cell(1, 1).Value = "Ma KH";
            ws.Cell(1, 2).Value = "Customer Name" + Environment.NewLine + "Ten KH";
            ws.Range(1, 1, 3, 1).Merge();
            ws.Range(1, 2, 3, 2).Merge();

            foreach (var week in report.Weeks)
            {
                var fromColumn = 2 + week.FromDay;
                var toColumn = 2 + week.ToDay;
                ws.Range(1, fromColumn, 1, toColumn).Merge();
                ws.Cell(1, fromColumn).Value = week.Label;
            }

            for (var day = 1; day <= dayCount; day++)
            {
                var column = 2 + day;
                ws.Cell(2, column).Value = $"{day}-{monthText}";
                ws.Cell(3, column).Value = "ACT";
            }

            var headerRange = ws.Range(1, 1, 3, lastColumn);
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#10A9D8");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Font.FontColor = XLColor.Black;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            BuildSaleSummaryBlock(ws, report, lastColumn + 2);

            var rowIndex = 4;
            foreach (var group in report.Groups)
            {
                ws.Range(rowIndex, 1, rowIndex, lastColumn).Merge();
                ws.Cell(rowIndex, 1).Value = group.GroupName;
                ws.Cell(rowIndex, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#10A9D8");
                ws.Cell(rowIndex, 1).Style.Font.Bold = true;
                ws.Cell(rowIndex, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                rowIndex++;

                foreach (var customer in group.Rows)
                {
                    ws.Cell(rowIndex, 1).Value = customer.CustomerCode;
                    ws.Cell(rowIndex, 2).Value = customer.CustomerName;

                    foreach (var day in customer.Days)
                    {
                        var column = 2 + day.Day;
                        if (day.HasActivity)
                        {
                            ws.Cell(rowIndex, column).Value = day.Mark;
                            ws.Cell(rowIndex, column).Style.Font.FontColor = XLColor.Red;
                            ws.Cell(rowIndex, column).Style.Font.Bold = true;
                        }
                    }

                    var dataRange = ws.Range(rowIndex, 1, rowIndex, lastColumn);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(rowIndex, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    rowIndex++;
                }
            }

            if (rowIndex == 4)
            {
                ws.Range(rowIndex, 1, rowIndex, lastColumn).Merge();
                ws.Cell(rowIndex, 1).Value = "Khong co du lieu";
            }

            ws.Columns(1, lastColumn).AdjustToContents();
            ws.Column(1).Width = Math.Max(ws.Column(1).Width, 14);
            ws.Column(2).Width = Math.Max(ws.Column(2).Width, 34);

            for (var column = 3; column <= lastColumn; column++)
                ws.Column(column).Width = 8;

            ws.SheetView.FreezeRows(3);
            ws.SheetView.FreezeColumns(2);

            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            return stream.ToArray();
        }

        /// <summary>
        /// Render block tổng hợp hoạt động theo sale ở bên phải bảng lịch hoạt động trong Excel.
        /// Block này hiển thị tổng tương tác, cuộc gọi khách hàng cũ, cuộc gọi khách hàng mới và hẹn gặp.
        /// </summary>
        /// <param name="ws">Worksheet Excel đang được build.</param>
        /// <param name="report">Dữ liệu báo cáo chứa danh sách tổng hợp theo sale.</param>
        /// <param name="startColumn">Cột bắt đầu render block tổng hợp sale.</param>
        private static void BuildSaleSummaryBlock(IXLWorksheet ws, CustomerActivityCalendarReportDto report, int startColumn)
        {
            if (report.SaleSummaries.Count == 0)
                return;

            ws.Cell(1, startColumn).Value = string.Empty;
            ws.Cell(2, startColumn).Value = string.Empty;

            for (var index = 0; index < report.SaleSummaries.Count; index++)
            {
                var summary = report.SaleSummaries[index];
                var column = startColumn + index + 1;
                ws.Cell(1, column).Value = summary.SaleEmployeeName;
                ws.Cell(2, column).Value = summary.SaleEmployeeCode;
            }

            var labels = new[]
            {
                "Tổng",
                "Cuộc gọi khách hàng củ",
                "Cuộc gọi KH mới",
                "Hẹn gặp"
            };

            for (var rowOffset = 0; rowOffset < labels.Length; rowOffset++)
            {
                ws.Cell(3 + rowOffset, startColumn).Value = labels[rowOffset];
            }

            for (var index = 0; index < report.SaleSummaries.Count; index++)
            {
                var summary = report.SaleSummaries[index];
                var column = startColumn + index + 1;

                ws.Cell(3, column).Value = summary.TotalInteractionCount;
                ws.Cell(4, column).Value = summary.OldCustomerCallCount;
                ws.Cell(5, column).Value = summary.NewCustomerCallCount;
                ws.Cell(6, column).Value = summary.MeetingCount;
            }

            var lastSummaryColumn = startColumn + report.SaleSummaries.Count;
            var summaryRange = ws.Range(1, startColumn, 6, lastSummaryColumn);
            summaryRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            summaryRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            summaryRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            summaryRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            summaryRange.Style.Font.Bold = true;

            ws.Range(1, startColumn, 2, startColumn).Style.Fill.BackgroundColor = XLColor.FromHtml("#10A9D8");
            ws.Range(3, startColumn, 6, startColumn).Style.Fill.BackgroundColor = XLColor.FromHtml("#10A9D8");
            ws.Range(1, startColumn + 1, 6, lastSummaryColumn).Style.Fill.BackgroundColor = XLColor.FromHtml("#B6FF2D");

            ws.Column(startColumn).Width = 22;
            for (var column = startColumn + 1; column <= lastSummaryColumn; column++)
                ws.Column(column).Width = 13;
        }

        private sealed class CustomerActivityCalendarCustomerSource
        {
            public Guid CustomerId { get; set; }
            public string CustomerCode { get; set; } = string.Empty;
            public string CustomerName { get; set; } = string.Empty;
            public Guid? CurrentSaleId { get; set; }
        }

        private sealed class CustomerActivityCalendarSaleSource
        {
            public Guid CustomerId { get; set; }
            public Guid EmployeeId { get; set; }
            public string EmployeeCode { get; set; } = string.Empty;
            public string EmployeeName { get; set; } = string.Empty;
        }

        private sealed record CustomerActivityCalendarInteractionCounts(
            int DirectMeetingCount,
            int OtherInteractionCount,
            int TotalInteractionCount)
        {
            public static CustomerActivityCalendarInteractionCounts Empty { get; } = new(0, 0, 0);
        }

        private sealed record CustomerActivityCalendarRevenueGroupKey(
            string GroupKey,
            string GroupName,
            decimal? MinRevenueAmount,
            decimal? MaxRevenueAmount);
    }
}

using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.SaleReports;
using VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;
using VietausWebAPI.Core.Domain.Enums.Merchadises;
using VietausWebAPI.Core.Domain.Enums.Visibilitys;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

public partial class MerchandiseOrderReportRepository
{
    /// <summary>
    /// Áp dụng quyền xem dữ liệu theo người dùng hiện tại.
    /// Admin/LabFull được xem toàn bộ trong công ty.
    /// Sale thường chỉ xem khách hàng được assign hoặc claim.
    /// Leader xem dữ liệu theo group của mình.
    /// </summary>
    private IQueryable<MerchandiseOrder> ApplyReportVisibility(
        IQueryable<MerchandiseOrder> query,
        ViewerScope viewerScope)
    {
        query = query.Where(x => x.CompanyId == viewerScope.CompanyId);

        if (viewerScope.ScopeType is ViewerScopeType.AdminFull or ViewerScopeType.LabFull)
        {
            return query;
        }

        var now = viewerScope.Now;

        return query.Where(mo =>
            _context.Customers.Any(c =>
                c.IsActive == true
                && c.CustomerId == mo.CustomerId
                && (
                    (c.IsLead && (
                        !_context.CustomerClaims.Any(cl =>
                            cl.IsActive
                            && cl.Type == ClaimType.Work
                            && cl.CustomerId == c.CustomerId
                            && cl.ExpiresAt > now)
                        || _context.CustomerClaims.Any(cl =>
                            cl.IsActive
                            && cl.Type == ClaimType.Work
                            && cl.CustomerId == c.CustomerId
                            && cl.ExpiresAt > now
                            && cl.EmployeeId == viewerScope.EmployeeId)
                        || (viewerScope.IsLeader && viewerScope.GroupId != null && _context.CustomerClaims.Any(cl =>
                            cl.IsActive
                            && cl.Type == ClaimType.Work
                            && cl.CustomerId == c.CustomerId
                            && cl.ExpiresAt > now
                            && cl.GroupId == viewerScope.GroupId))))
                    || (!c.IsLead && (
                        (!viewerScope.IsLeader && (
                            _context.CustomerAssignments.Any(a =>
                                a.IsActive
                                && a.CustomerId == c.CustomerId
                                && a.EmployeeId == viewerScope.EmployeeId)
                            || _context.CustomerClaims.Any(cl =>
                                cl.IsActive
                                && cl.Type == ClaimType.Work
                                && cl.CustomerId == c.CustomerId
                                && cl.ExpiresAt > now
                                && cl.EmployeeId == viewerScope.EmployeeId)))
                        || (viewerScope.IsLeader && viewerScope.GroupId != null && (
                            _context.CustomerAssignments.Any(a =>
                                a.IsActive
                                && a.CustomerId == c.CustomerId
                                && a.GroupId == viewerScope.GroupId)
                            || _context.CustomerClaims.Any(cl =>
                                cl.IsActive
                                && cl.Type == ClaimType.Work
                                && cl.CustomerId == c.CustomerId
                                && cl.ExpiresAt > now
                                && cl.GroupId == viewerScope.GroupId))))))));
    }

    /// <summary>
    /// Tinh vat cho đơn hàng nếu có, hiện tại đang để mặc định không bao gồm vat.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="vatPercent"></param>
    /// <param name="includeVat"></param>
    /// <returns></returns>
    private static decimal ApplyVat(decimal amount, decimal? vatPercent, bool includeVat)
    {
        if (!includeVat) return amount;

        var vat = vatPercent ?? 0m;
        return amount * (1 + vat / 100m);
    }
}

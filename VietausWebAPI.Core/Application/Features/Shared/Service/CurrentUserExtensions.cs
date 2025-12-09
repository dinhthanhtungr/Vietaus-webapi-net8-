using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;

namespace VietausWebAPI.Core.Application.Features.Shared.Service
{

    public static class CurrentUserExtensions
    {
        public static bool IsAdminOrPresident(this ICurrentUser u)
            => u.IsInRole(AppRoles.Admin) || u.IsInRole(AppRoles.President);

        public static bool IsDeveloper(this ICurrentUser u)
            => u.IsInRole(AppRoles.Developer);

        public static bool IsSales(this ICurrentUser u)
            => u.IsInRole(AppRoles.SaleUser);

        public static bool IsLab(this ICurrentUser u)
            => u.IsInRole(AppRoles.LabUser);

        public static bool IsLeaderRole(this ICurrentUser u)
            => u.IsInRole(AppRoles.Leader);

        // Bypass filter KH (Admin/President/Developer/CustomerViewAll)
        public static bool CanViewAllCustomers(this ICurrentUser u)
            => u.IsInRole(AppRoles.CustomerViewAll) || u.IsAdminOrPresident() || u.IsDeveloper();
    }
}

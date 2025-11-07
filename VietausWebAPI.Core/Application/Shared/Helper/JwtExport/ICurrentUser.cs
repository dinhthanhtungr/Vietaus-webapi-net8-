namespace VietausWebAPI.Core.Application.Shared.Helper.JwtExport
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        Guid EmployeeId { get; }         // GUID muốn dùng cho CreatedBy
        string EmployeeExternalId { get; }       // mã nhân viên: EMPxxxx
        Guid CompanyId { get; }

        Guid UserAccountId { get; }       // nếu cần phân biệt account (sub/uid)
        string personName { get; }
        string UserName { get; }

        IEnumerable<string> Roles { get; }
        bool IsInRole(string role) => Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
    }

}

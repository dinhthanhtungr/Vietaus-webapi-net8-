namespace VietausWebAPI.WebAPI.Helpers
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        Guid? UserId { get; }
        Guid? CompanyId { get; }     // nếu bạn dùng multi-tenant
        string? UserName { get; }
        string? Email { get; }
        IEnumerable<string> Roles { get; }
    }
}

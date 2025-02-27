using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? personName {  get; set; }
        public string? RefreshToken {  get; set; }
        public DateTime RefreshTokenExpirationDateTime {  get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}

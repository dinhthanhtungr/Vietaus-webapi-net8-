using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Identity;

namespace VietausWebAPI.Core.Application.Features.Shared.DTO
{
    public class JwtModels
    {
        public ApplicationUser User { get; set; } = new ApplicationUser();

        public Guid PartId { get; set; }
        public string PartExternalId { get; set; } = string.Empty;
        public string PartName { get; set; } = string.Empty;

        public Guid EmployeeId { get; set; }
        public string EmployeeExternalId { get; set; } = string.Empty;

        public Guid CompanyId { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();

    }
}

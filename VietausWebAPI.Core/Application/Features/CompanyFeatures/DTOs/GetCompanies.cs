using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.CompanyFeatures.DTOs
{
    public class GetCompanies
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
    }
}

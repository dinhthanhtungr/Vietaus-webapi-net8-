using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.HR.DTOs.Groups
{
    public class PostGroupDTOs
    {
        public string? GroupType { get; set; }

        public string? ExternalId { get; set; }
        public Guid? PartId { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public Guid? CompanyId { get; set; }
    }
}

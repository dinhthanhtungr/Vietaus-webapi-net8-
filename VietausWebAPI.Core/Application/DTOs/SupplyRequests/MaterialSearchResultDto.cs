using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.SupplyRequests
{
    public class MaterialSearchResultDto
    {
        public Guid MaterialId { get; set; }
        public string? MaterialName { get; set; }
        public string? Unit { get; set; }
        public string? MaterialGroupId { get; set; }
    }
}

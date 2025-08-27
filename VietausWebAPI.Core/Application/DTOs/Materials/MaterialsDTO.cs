using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.Materials
{
    public class MaterialsDTO
    {
        public string? Name { get; set; }

        public string? Unit { get; set; }

        public DateTime? CreateDate { get; set; } = DateTime.Now;

        public string? EmployeeId { get; set; }
        public Guid? MaterialGroupId { get; set; }
    }
}

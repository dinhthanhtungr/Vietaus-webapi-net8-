using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.HR.DTOs.Employees
{
    public class EmployeesCommonDatumDTO
    {
        public string EmployeeId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string LevelId { get; set; } = null!;
        public string PartId { get; set; } = null!;
        public string PartName { get; set; } = null!;   
    }
}

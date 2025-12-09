using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs
{
    public class AssignLeadRequestNoice
    {
        public string SaleName { get; set; } = string.Empty;
        public DateTime ExprievDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Enums.Hrms
{
    public enum SalaryComponentValueType
    {
        FixedAmount = 1,
        Percentage = 2,
        Formula = 3,
        ManualInput = 4
    }
}

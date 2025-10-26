using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Enums
{
    public enum FormulaSource
    {
        Production,   // ManufacturingFormulaRepository
        Research,     // FormulaRepository
        Both
    }
}

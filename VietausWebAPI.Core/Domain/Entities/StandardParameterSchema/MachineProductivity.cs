using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities.StandardParameterSchema
{
    public class MachineProductivity
    {
        public int Id { get; set; }                             // PK (identity)
        public string MachineId { get; set; } = string.Empty;   // text/citext
        public string ProductionCode { get; set; } = string.Empty; // text/citext
        public int Quantity { get; set; }
    }
}

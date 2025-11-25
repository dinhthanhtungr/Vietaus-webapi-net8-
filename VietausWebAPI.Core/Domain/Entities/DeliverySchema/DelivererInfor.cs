using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities.DeliverySchema
{
    public class DelivererInfor
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? DelivererType { get; set; }
        public string? Phone { get; set; }
        public string? Note { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

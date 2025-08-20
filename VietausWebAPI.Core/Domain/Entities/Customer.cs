using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities
{
    public class Customer
    {
        public string? ExternalId { get; set; }

        public string? CustomerName { get; set; }
    }
}

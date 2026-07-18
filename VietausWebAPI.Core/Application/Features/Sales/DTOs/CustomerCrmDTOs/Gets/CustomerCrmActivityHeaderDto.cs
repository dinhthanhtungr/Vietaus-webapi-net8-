using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets
{
    public class CustomerCrmActivityHeaderDto
    {
        public List<string> ActivityTypes { get; set; } = new List<string>();
        public Guid CustomerId { get; set; }
        public string? CustomerExternalId { get; set; }
        public string? CustomerName { get; set; }

        public int TotalCount { get; set; }
        public int OpenTaskCount { get; set; }
        public int CompletedTaskCount { get; set; }
        public int CanceledTaskCount { get; set; }
        public int OverdueTaskCount { get; set; }
        public int InteractionCount { get; set; }

        public DateTime? LastActivityDate { get; set; }
    }
}

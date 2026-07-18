using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Patchs
{
    public class CompleteCustomerFollowUpTaskRequest
    {
        public string? CompletionNote { get; set; }
        public CustomerFollowUpTaskStatus status { get; set; } = CustomerFollowUpTaskStatus.Done;
    }

}

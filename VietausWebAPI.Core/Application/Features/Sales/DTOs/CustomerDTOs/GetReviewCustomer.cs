using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs
{
    public class GetReviewCustomer
    {
        public Guid CustomerId { get; set; }
        public Guid? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? ExternalId { get; set; }
        public string? Name { get; set; }
        public string? RegNo { get; set; }
        public string? Phone { get; set; }
        public string? Group { get; set; }
        public string? Address { get; set; }
        public string? DeliveryName { get; set; }



        public string? CustomerSpectialRequirement { get; set; }
        public string? paymentType { get; set; }
        public string? delivieryType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductFeatures
{
    public sealed class ChangeCustomerForProductRequest
    {
        public Guid ProductId { get; set; }
        public Guid NewCustomerId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductFeatures
{
    public class ProductDTO
    {
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public string? ColourCode { get; set; }
    }
}

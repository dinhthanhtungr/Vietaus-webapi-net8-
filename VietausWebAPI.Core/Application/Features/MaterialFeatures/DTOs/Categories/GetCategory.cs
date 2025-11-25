using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Categories
{
    public class GetCategory
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}

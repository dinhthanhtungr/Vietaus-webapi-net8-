using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Categories
{
    public class GetUnit
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

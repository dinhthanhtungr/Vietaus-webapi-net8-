using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders
{
    public record ProductRow(Guid ProductId, string ColourCode, string Name, Guid? CategoryId, string ColourName);
}

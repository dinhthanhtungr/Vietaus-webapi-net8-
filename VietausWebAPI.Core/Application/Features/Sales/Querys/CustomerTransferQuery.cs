using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.Querys
{
    public class CustomerTransferQuery : PaginationQuery
    {
        public Guid? CompanyId { get; set; }
        public string? Keyword { get; set; }
        public TransferType? TransferType { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}

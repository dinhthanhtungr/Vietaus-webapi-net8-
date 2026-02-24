using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.Queries.QCInputByQCFeatures
{
    public class QCInputQuery : PaginationQuery
    {
        public Guid? QCInputByQCId { get; set; }
        public long VoucherDetailId { get; set; }
        public string? Keyword { get; set; }
        public bool? HasQC { get; set; }
        public VoucherDetailType? VoucherDetailType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}

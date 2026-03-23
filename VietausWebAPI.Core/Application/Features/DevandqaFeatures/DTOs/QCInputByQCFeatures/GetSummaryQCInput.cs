using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.QCInputByQCFeatures
{
    public class GetSummaryQCInput
    {
        public long VoucherDetailId { get; set; }

        public string ExternalId { get; set; } = string.Empty; // ProductCode
        public string Name { get; set; } = string.Empty;       // ProductName

        public string LotNumber { get; set; } = string.Empty;
        public decimal QtyKg { get; set; }
        public string Unit { get; set; } = "kg";

        public string CategoryName { get; set; } = string.Empty; // dùng để hiển thị, lấy từ WarehouseVoucherDetail.ProductCode -> map sang category name

        public VoucherDetailType VoucherType { get; set; }
        public string RequestCode { get; set; } = string.Empty;

        // QC summary
        public bool HasQC { get; set; }
        public Guid? QCInputByQCId { get; set; }      // nullable cho đúng nghĩa "chưa QC"
        public string QCEmployeeName { get; set; } = string.Empty;
        public DateTime? QCCreatedDate { get; set; } // nullable
    }

}

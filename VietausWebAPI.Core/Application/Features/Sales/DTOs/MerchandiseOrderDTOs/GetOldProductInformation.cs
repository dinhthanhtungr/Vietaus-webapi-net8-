using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs
{
    public class GetOldProductInformation
    {
        public string? BagType { get; set; }
        public string? PackageWeight { get; set; }
        public int ExpectedQuantity { get; set; }
        public string? FormulaExternalIdSnapshot { get; set; }
        public string? Comment { get; set; }
        public decimal UnitPriceAgreed { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}

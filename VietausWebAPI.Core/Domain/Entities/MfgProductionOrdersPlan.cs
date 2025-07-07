using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities
{
    public class MfgProductionOrdersPlan
    {
        public Guid Id { get; set; }

        public string Requirement { get; set; } = default!;

        public DateTime? ExpiryDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? ExternalId { get; set; }

        public string? Product_ExternalId { get; set; }

        public string? Product_Name { get; set; }

        public string? Product_ExpiryType { get; set; }

        public string? Product_Package { get; set; }

        public bool? Product_RohsStandard { get; set; }

        public float? Product_RecycleRate { get; set; }

        public float? Product_Weight { get; set; }

        public string? Product_CustomerExternalId { get; set; }

        public float? Product_MaxTemp { get; set; }

        public float? Product_AddRate { get; set; }

        public Guid Product_Id { get; set; }
    }
}

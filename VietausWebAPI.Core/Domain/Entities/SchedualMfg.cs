using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class SchedualMfg
{
    public Guid Id { get; set; }

    public Guid? ProductId { get; set; }        
    public Guid? MfgProductionOrderId { get; set; }        

    public string? ExternalId { get; set; }

    public string? MachineId { get; set; }

    public int? Number { get; set; }

    public string? ColorName { get; set; }

    public string? ColorCode { get; set; }

    public int? Quantity { get; set; }

    public string? CustomerExternalId { get; set; }

    public DateTime? CustomerRequiredDate { get; set; }

    public DateTime? ExpectedDeliveryDate { get; set; }
    public string? ProductName { get; set; }
    public string? requirement { get; set; }
    public string? ProductExpiryType { get; set; }

    public bool? ProductRohsStandard { get; set; }

    public double? ProductRecycleRate { get; set; }

    public double? ProductWeight { get; set; }

    public string? ProductCustomerExternalId { get; set; }

    public double? ProductMaxTemp { get; set; }

    public double? ProductAddRate { get; set; }


    public string? VerifyBatches { get; set; }

    public string? Note { get; set; }

    public string? BagType { get; set; }

    public DateTime? ExpectedCompletionDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Status { get; set; }

    public DateTime? PlanDate { get; set; }

    public string? Qcstatus { get; set; }
}

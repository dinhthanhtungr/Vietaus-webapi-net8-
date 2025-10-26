using System;
using System.Collections.Generic;

 namespace VietausWebAPI.Core.Domain.Entities;

public partial class Formula
{
    public Guid FormulaId { get; set; }

    public string? ExternalId { get; set; }

    public string? Name { get; set; }


    public Guid ProductId { get; set; }

    // Trạng thái hiện tại của công thức
    public string Status { get; set; } = "Draft";


    public Guid? CheckBy { get; set; }          // UNIQUEIDENTIFIER
    public DateTime? CheckDate { get; set; }       // DATETIME
    public string? CheckNameSnapshot { get; set; }  // NVARCHAR


    public Guid? SentBy { get; set; }          // UNIQUEIDENTIFIER
    public DateTime? SentDate { get; set; }       // DATETIME
    public string? SentByNameSnapshot { get; set; }  // NVARCHAR



    public decimal? TotalPrice { get; set; }
    public bool? IsSelect { get; set; }
    public bool? IsActive { get; set; }
    //public bool? IsCustomerSelect { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public Guid? CompanyId { get; set; }


    public virtual Company? Company { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual ICollection<FormulaMaterial> FormulaMaterials { get; set; } = new List<FormulaMaterial>();
    public virtual ICollection<ManufacturingFormula> ManufacturingFormulas { get; set; } = new List<ManufacturingFormula>();
    public virtual ICollection<ManufacturingFormula> ManufacturingFormulaSources { get; set; } = new List<ManufacturingFormula>();

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<SampleRequest> SampleRequests { get; set; } = new List<SampleRequest>();
    public virtual ICollection<FormulaStatusLog> StatusLogs { get; set; } = new List<FormulaStatusLog>();
    public virtual ICollection<MerchandiseOrderDetail> MerchandiseOrderDetails { get; set; } = new List<MerchandiseOrderDetail>();
    public virtual Employee? UpdatedByNavigation { get; set; }
    public virtual Employee? CheckByNavigation { get; set; }
    public virtual Employee? SentByNavigation { get; set; }

}

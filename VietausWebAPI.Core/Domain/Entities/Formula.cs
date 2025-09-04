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


    public decimal? TotalPrice { get; set; }


    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public Guid? CompanyId { get; set; }


    public virtual Company? Company { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual ICollection<FormulaMaterial> FormulaMaterials { get; set; } = new List<FormulaMaterial>();

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<SampleRequest> SampleRequests { get; set; } = new List<SampleRequest>();
    public virtual ICollection<FormulaStatusLog> StatusLogs { get; set; } = new List<FormulaStatusLog>();

    public virtual Employee? UpdatedByNavigation { get; set; }

}

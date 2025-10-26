using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;

 namespace VietausWebAPI.Core.Domain.Entities;

public partial class FormulaMaterial
{
    public Guid FormulaMaterialId { get; set; }

    public Guid FormulaId { get; set; }
    public Guid MaterialId { get; set; }
    public Guid CategoryId { get; set; }

    public decimal Quantity { get; set; }            // DECIMAL(18,6)
    public decimal UnitPrice { get; set; }           // DECIMAL(16,2)
    public decimal TotalPrice { get; set; }          // DECIMAL(16,2)


    public string? MaterialNameSnapshot { get; set; }         // NVARCHAR
    public string? MaterialExternalIdSnapshot { get; set; }   // VARCHAR
    public string? Unit { get; set; }                         // VARCHAR
    public bool? IsActive { get; set; }


    public virtual Formula Formula { get; set; } = null!;

    public virtual Material Material { get; set; } = null!;

    public virtual Category? Category { get; set; }
}

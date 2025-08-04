using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class MaterialsMaterialDatum
{
    public Guid MaterialId { get; set; }

    public string? ExternalId { get; set; }

    public string? Name { get; set; }

    public string? Unit { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? EmployeeId { get; set; }

    public Guid? MaterialGroupId { get; set; }

    public Guid? SupplierId { get; set; }

    public virtual EmployeesCommonDatum? Employee { get; set; }

    public virtual ICollection<InventoryReceiptsMaterialDatum> InventoryReceiptsMaterialData { get; set; } = new List<InventoryReceiptsMaterialDatum>();

    public virtual MaterialGroupsMaterialDatum? MaterialGroup { get; set; }

    public virtual ICollection<MaterialsSuppliersMaterialDatum> MaterialsSuppliersMaterialData { get; set; } = new List<MaterialsSuppliersMaterialDatum>();

    public virtual ICollection<PriceHistoryMaterialDatum> PriceHistoryMaterialData { get; set; } = new List<PriceHistoryMaterialDatum>();

    public virtual ICollection<PurchaseOrderDetailsMaterialDatum> PurchaseOrderDetailsMaterialData { get; set; } = new List<PurchaseOrderDetailsMaterialDatum>();

    public virtual ICollection<RequestDetailMaterialDatum> RequestDetailMaterialData { get; set; } = new List<RequestDetailMaterialDatum>();
}

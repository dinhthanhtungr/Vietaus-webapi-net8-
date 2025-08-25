using System;
using System.Collections.Generic;

namespace VietausWebAPI.WebAPI;

public partial class Category
{
    public Guid CategoryId { get; set; }

    public string? ExternalId { get; set; }

    public string? Types { get; set; }

    public string? Name { get; set; }

    public Guid CompanyId { get; set; }

    public bool? IsActive { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

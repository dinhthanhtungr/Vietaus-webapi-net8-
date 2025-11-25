using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;

namespace VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext
{
    public partial class ApplicationDbContext
    {
        public virtual DbSet<MerchandiseOrder> MerchandiseOrders { get; set; } = default!;
        public virtual DbSet<MerchandiseOrderDetail> MerchandiseOrderDetails { get; set; } = default!;
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; } = default!;
        public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = default!;
        public virtual DbSet<PurchaseOrderSnapshot> PurchaseOrderSnapshots { get; set; } = default!;
        public virtual DbSet<PurchaseOrderLink> PurchaseOrderLinks { get; set; } = default!;
    }
}

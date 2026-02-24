using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs
{
    public partial class ApplicationDbContext
    {
        public virtual DbSet<DeliveryOrder> DeliveryOrders { get; set; }
        public virtual DbSet<DeliveryOrderPO> DeliveryOrderPOs { get; set; }
        public virtual DbSet<DeliveryOrderDetail> DeliveryOrderDetails { get; set; }
        public virtual DbSet<DelivererInfor> DelivererInfors { get; set; }
        public virtual DbSet<Deliverer> Deliverers { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.DeliverySchema
{
    public sealed class DeliveryTripOrderConfiguration : IEntityTypeConfiguration<DeliveryTripOrder>
    {
        public void Configure(EntityTypeBuilder<DeliveryTripOrder> entity)
        {
            entity.ToTable("DeliveryTripOrders", "DeliveryOrder");

            entity.HasKey(e => new { e.DeliveryTripId, e.DeliveryOrderId })
                  .HasName("PK_DeliveryTripOrders");

            entity.Property(e => e.IsActive)
                  .HasDefaultValue(true);

            entity.HasIndex(e => e.DeliveryOrderId, "IX_DeliveryTripOrders_DeliveryOrderId");
            entity.HasIndex(e => new { e.DeliveryTripId, e.StopSequence }, "IX_DeliveryTripOrders_DeliveryTripId_StopSequence");

            entity.HasOne(e => e.DeliveryTrip)
                  .WithMany(t => t.Orders)
                  .HasForeignKey(e => e.DeliveryTripId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_DeliveryTripOrders_DeliveryTrip");

            entity.HasOne(e => e.DeliveryOrder)
                  .WithMany()
                  .HasForeignKey(e => e.DeliveryOrderId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_DeliveryTripOrders_DeliveryOrder");
        }
    }
}

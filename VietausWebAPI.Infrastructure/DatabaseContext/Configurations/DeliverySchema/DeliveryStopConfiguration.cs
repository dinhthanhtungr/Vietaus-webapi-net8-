using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.DeliverySchema
{
    public sealed class DeliveryStopConfiguration : IEntityTypeConfiguration<DeliveryStop>
    {
        public void Configure(EntityTypeBuilder<DeliveryStop> entity)
        {
            entity.ToTable("DeliveryStops", "DeliveryOrder");

            entity.HasKey(e => e.Id).HasName("PK_DeliveryStops");

            entity.Property(e => e.Id)
                  .HasDefaultValueSql("gen_random_uuid()")
                  .HasColumnName("ID");

            entity.Property(e => e.StopType)
                  .IsRequired()
                  .HasMaxLength(64);

            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(e => e.Address)
                  .IsRequired()
                  .HasColumnType("text");

            entity.Property(e => e.ContactName)
                  .HasMaxLength(255);

            entity.Property(e => e.ContactPhone)
                  .HasMaxLength(50);

            entity.Property(e => e.Status)
                  .IsRequired()
                  .HasMaxLength(64);

            entity.Property(e => e.FailureReason)
                  .HasColumnType("text");

            entity.Property(e => e.IsActive)
                  .HasDefaultValue(true);

            entity.HasIndex(e => e.DeliveryTripId, "IX_DeliveryStops_DeliveryTripId");
            entity.HasIndex(e => e.DeliveryOrderId, "IX_DeliveryStops_DeliveryOrderId");
            entity.HasIndex(e => new { e.DeliveryTripId, e.SequenceNo }, "IX_DeliveryStops_DeliveryTripId_SequenceNo");

            entity.HasOne(e => e.DeliveryTrip)
                  .WithMany(t => t.Stops)
                  .HasForeignKey(e => e.DeliveryTripId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_DeliveryStops_DeliveryTrip");

            entity.HasOne(e => e.DeliveryOrder)
                  .WithMany()
                  .HasForeignKey(e => e.DeliveryOrderId)
                  .OnDelete(DeleteBehavior.SetNull)
                  .HasConstraintName("FK_DeliveryStops_DeliveryOrder");
        }
    }
}

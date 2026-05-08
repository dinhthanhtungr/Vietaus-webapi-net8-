using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.DeliverySchema
{
    public sealed class DeliveryProofOfDeliveryConfiguration : IEntityTypeConfiguration<DeliveryProofOfDelivery>
    {
        public void Configure(EntityTypeBuilder<DeliveryProofOfDelivery> entity)
        {
            entity.ToTable("DeliveryProofOfDeliveries", "DeliveryOrder");

            entity.HasKey(e => e.Id).HasName("PK_DeliveryProofOfDeliveries");

            entity.Property(e => e.Id)
                  .HasDefaultValueSql("gen_random_uuid()")
                  .HasColumnName("ID");

            entity.Property(e => e.ReceiverName)
                  .HasMaxLength(255);

            entity.Property(e => e.ReceiverPhone)
                  .HasMaxLength(50);

            entity.Property(e => e.SignatureUrl)
                  .HasMaxLength(1000);

            entity.Property(e => e.PhotoUrl)
                  .HasMaxLength(1000);

            entity.Property(e => e.Note)
                  .HasColumnType("text");

            entity.Property(e => e.Latitude)
                  .HasPrecision(18, 8);

            entity.Property(e => e.Longitude)
                  .HasPrecision(18, 8);

            entity.Property(e => e.IsActive)
                  .HasDefaultValue(true);

            entity.HasIndex(e => e.DeliveryOrderId, "IX_DeliveryProofOfDeliveries_DeliveryOrderId");
            entity.HasIndex(e => e.ConfirmedBy, "IX_DeliveryProofOfDeliveries_ConfirmedBy");

            entity.HasOne(e => e.DeliveryOrder)
                  .WithMany(o => o.ProofOfDeliveries)
                  .HasForeignKey(e => e.DeliveryOrderId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_DeliveryProofOfDeliveries_DeliveryOrder");
        }
    }
}

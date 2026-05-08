using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.DeliverySchema
{
    public sealed class DeliveryVehicleConfiguration : IEntityTypeConfiguration<DeliveryVehicle>
    {
        public void Configure(EntityTypeBuilder<DeliveryVehicle> entity)
        {
            entity.ToTable("DeliveryVehicles", "DeliveryOrder");

            entity.HasKey(e => e.Id).HasName("PK_DeliveryVehicles");

            entity.Property(e => e.Id)
                  .HasDefaultValueSql("gen_random_uuid()")
                  .HasColumnName("ID");

            entity.Property(e => e.PlateNumber)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.VehicleType)
                  .HasMaxLength(100);

            entity.Property(e => e.MaxLoadKg)
                  .HasPrecision(18, 3);

            entity.Property(e => e.MaxVolumeM3)
                  .HasPrecision(18, 3);

            entity.Property(e => e.OwnerName)
                  .HasMaxLength(255);

            entity.Property(e => e.Phone)
                  .HasMaxLength(50);

            entity.Property(e => e.Note)
                  .HasColumnType("text");

            entity.Property(e => e.IsInternal)
                  .HasDefaultValue(true);

            entity.Property(e => e.IsActive)
                  .HasDefaultValue(true);

            entity.HasIndex(e => e.CompanyId, "IX_DeliveryVehicles_CompanyId");
            entity.HasIndex(e => e.PlateNumber, "IX_DeliveryVehicles_PlateNumber");
            entity.HasIndex(e => new { e.CompanyId, e.IsActive }, "IX_DeliveryVehicles_CompanyId_IsActive");
        }
    }
}

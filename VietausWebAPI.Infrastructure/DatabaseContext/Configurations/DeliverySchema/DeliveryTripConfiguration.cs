using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.DeliverySchema
{
    public sealed class DeliveryTripConfiguration : IEntityTypeConfiguration<DeliveryTrip>
    {
        public void Configure(EntityTypeBuilder<DeliveryTrip> entity)
        {
            entity.ToTable("DeliveryTrips", "DeliveryOrder");

            entity.HasKey(e => e.Id).HasName("PK_DeliveryTrips");

            entity.Property(e => e.Id)
                  .HasDefaultValueSql("gen_random_uuid()")
                  .HasColumnName("ID");

            entity.Property(e => e.ExternalId)
                  .IsRequired()
                  .HasColumnType("citext");

            entity.Property(e => e.Status)
                  .IsRequired()
                  .HasMaxLength(64);

            entity.Property(e => e.RouteCode)
                  .HasMaxLength(100);

            entity.Property(e => e.Note)
                  .HasColumnType("text");

            entity.Property(e => e.IsActive)
                  .HasDefaultValue(true);

            entity.HasIndex(e => e.CompanyId, "IX_DeliveryTrips_CompanyId");
            entity.HasIndex(e => e.TripDate, "IX_DeliveryTrips_TripDate");
            entity.HasIndex(e => e.VehicleId, "IX_DeliveryTrips_VehicleId");
            entity.HasIndex(e => e.DriverId, "IX_DeliveryTrips_DriverId");
            entity.HasIndex(e => e.DispatcherId, "IX_DeliveryTrips_DispatcherId");
            entity.HasIndex(e => new { e.CompanyId, e.Status, e.TripDate }, "IX_DeliveryTrips_CompanyId_Status_TripDate");
            entity.HasIndex(e => e.ExternalId, "UX_DeliveryTrips_ExternalId")
                  .IsUnique();

            entity.HasOne(e => e.Vehicle)
                  .WithMany(v => v.Trips)
                  .HasForeignKey(e => e.VehicleId)
                  .OnDelete(DeleteBehavior.SetNull)
                  .HasConstraintName("FK_DeliveryTrips_Vehicle");

            entity.HasOne(e => e.Driver)
                  .WithMany()
                  .HasForeignKey(e => e.DriverId)
                  .OnDelete(DeleteBehavior.SetNull)
                  .HasConstraintName("FK_DeliveryTrips_Driver");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs.Configurations.DeliverySchema
{
    public class DeliveryOrderConfiguration : IEntityTypeConfiguration<DeliveryOrder>
    {
        public void Configure(EntityTypeBuilder<DeliveryOrder> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("DeliveryOrders", "DeliveryOrder");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("ID");

            entity.Property(e => e.Status)
                .HasMaxLength(64)
                .IsRequired();

            entity.Property(e => e.ExternalId)
                .HasColumnType("citext");

            entity.Property(e => e.CustomerExternalIdSnapShot)
                .HasColumnType("citext");

            entity.Property(e => e.Receiver)
                .HasMaxLength(255);

            entity.Property(e => e.DeliveryAddress)
                .HasColumnType("text");

            entity.Property(e => e.PaymentType)
                .HasMaxLength(100);

            entity.Property(e => e.PaymentDeadline)
                .HasMaxLength(100);

            entity.Property(e => e.TaxNumber)
                .HasMaxLength(100);

            entity.Property(e => e.PhoneSnapshot)
                .HasMaxLength(50);

            entity.Property(e => e.Note)
                .HasColumnType("text");

            entity.Property(e => e.DeliveryPrice)
                .HasPrecision(18, 2);

            entity.Property(e => e.DeliveryWindowFrom)
                .HasMaxLength(50);

            entity.Property(e => e.DeliveryWindowTo)
                .HasMaxLength(50);

            entity.Property(e => e.FailureReason)
                .HasColumnType("text");

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.HasPrinted)
                .HasDefaultValue(false);

            entity.Property(e => e.IsDeliveredSuccessfully)
                .HasDefaultValue(false);

            entity.HasIndex(e => e.CompanyId);
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.CreatedBy);
            entity.HasIndex(e => e.UpdatedBy);
            entity.HasIndex(e => e.DeliveryTripId);
            entity.HasIndex(e => new { e.CompanyId, e.CreatedDate });
            entity.HasIndex(e => new { e.CompanyId, e.Status, e.CreatedDate });

            entity.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_DeliveryOrders_Customer");

            entity.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_DeliveryOrders_Company");

            entity.HasOne(e => e.CreatedByNavigation)
                .WithMany(e => e.DeliveryOrderCreatedByNavigations)
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_DeliveryOrders_CreatedBy");

            entity.HasOne(e => e.UpdatedByNavigation)
                .WithMany(e => e.DeliveryOrderUpdatedByNavigations)
                .HasForeignKey(e => e.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_DeliveryOrders_UpdatedBy");

            entity.HasOne(e => e.DeliveryTrip)
                .WithMany()
                .HasForeignKey(e => e.DeliveryTripId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_DeliveryOrders_DeliveryTrip");
        }
    }
}
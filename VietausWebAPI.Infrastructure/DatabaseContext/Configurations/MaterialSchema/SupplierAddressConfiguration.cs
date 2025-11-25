using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;

namespace VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext.Configurations.MaterialSchema
{
    public class SupplierAddressConfiguration : IEntityTypeConfiguration<SupplierAddress>
    {
        public void Configure(EntityTypeBuilder<SupplierAddress> entity)
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Supplier__091C2AFB0B8862E7");
            entity.ToTable("SupplierAddresses", "Material");

            entity.HasIndex(e => e.SupplierId, "IX_SupplierAddresses_SupplierId");

            entity.Property(e => e.AddressId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.AddressLine).HasMaxLength(250);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.District).HasMaxLength(100);
            entity.Property(e => e.IsPrimary).HasDefaultValue(false);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.Province).HasMaxLength(100);

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierAddresses)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierAddress_Supplier");
        }
    }
}

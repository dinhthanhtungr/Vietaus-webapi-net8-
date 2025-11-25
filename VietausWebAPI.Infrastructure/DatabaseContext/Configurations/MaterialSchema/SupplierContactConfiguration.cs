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
    public class SupplierContactConfiguration : IEntityTypeConfiguration<SupplierContact>
    {
        public void Configure(EntityTypeBuilder<SupplierContact> entity)
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Supplier__5C66259B7A3571DD");
            entity.ToTable("SupplierContacts", "Material");

            entity.HasIndex(e => e.SupplierId, "IX_SupplierContacts_SupplierId");

            entity.Property(e => e.ContactId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierContacts)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupplierContact_Supplier");
        }
    }
}

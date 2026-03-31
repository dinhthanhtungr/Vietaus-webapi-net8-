using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

namespace VietausWebAPI.Infrastructure.Persistence.Configurations.SampleRequestSchema
{
    public class ColorChipRecordConfiguration : IEntityTypeConfiguration<ColorChipRecord>
    {
        public void Configure(EntityTypeBuilder<ColorChipRecord> b)
        {
            b.ToTable("color_chip_records", "SampleRequests");

            b.HasKey(x => x.ColorChipRecordId);

            b.Property(x => x.ColorChipRecordId)
             .HasColumnName("color_chip_record_id")
             .HasDefaultValueSql("gen_random_uuid()");

            b.Property(x => x.RecordType)
             .HasColumnName("record_type")
             .HasConversion<int>()
             .IsRequired();

            b.Property(x => x.ChipPurpose)
             .HasColumnName("chip_purpose")
             .HasConversion<int>()
             .IsRequired();

            b.Property(x => x.ProductId)
             .HasColumnName("product_id");

            b.Property(x => x.ProductCodeSnapshot)
             .HasColumnName("product_code_snapshot")
             .HasMaxLength(100);

            b.Property(x => x.ProductNameSnapshot)
             .HasColumnName("product_name_snapshot")
             .HasMaxLength(500);

            b.Property(x => x.ColorNameSnapshot)
             .HasColumnName("color_name_snapshot")
             .HasMaxLength(500);

            b.Property(x => x.ManufacturingFormulaId)
             .HasColumnName("manufacturing_formula_id");

            b.Property(x => x.ManufacturingFormulaExternalIdSnapshot)
             .HasColumnName("manufacturing_formula_external_id_snapshot")
             .HasMaxLength(100);

            b.Property(x => x.DevelopmentFormulaId)
             .HasColumnName("development_formula_id");

            b.Property(x => x.DevelopmentFormulaExternalIdSnapshot)
             .HasColumnName("development_formula_external_id_snapshot")
             .HasMaxLength(100);

            b.Property(x => x.AttachmentCollectionId)
             .HasColumnName("attachment_collection_id")
             .IsRequired();

            b.Property(x => x.RecordDate)
             .HasColumnName("record_date");

            b.Property(x => x.CustomerId)
             .HasColumnName("customer_id");

            b.Property(x => x.CustomerExternalIdSnapshot)
             .HasColumnName("customer_external_id_snapshot")
             .HasMaxLength(100);

            b.Property(x => x.CustomerNameSnapshot)
             .HasColumnName("customer_name_snapshot")
             .HasMaxLength(500);

            b.Property(x => x.AddRate)
             .HasColumnName("add_rate")
             .HasPrecision(18, 6);

            b.Property(x => x.Resin)
             .HasColumnName("resin")
             .HasMaxLength(200);

            b.Property(x => x.TemperatureMin)
             .HasColumnName("temperature_min")
             .HasPrecision(18, 6);

            b.Property(x => x.TemperatureMax)
             .HasColumnName("temperature_max")
             .HasPrecision(18, 6);

            b.Property(x => x.SizeText)
             .HasColumnName("size_text")
             .HasMaxLength(100);

            b.Property(x => x.PelletWeightGram)
             .HasColumnName("pellet_weight_gram")
             .HasPrecision(18, 6);

            b.Property(x => x.AntiStaticInfo)
             .HasColumnName("anti_static_info")
             .HasMaxLength(200);

            b.Property(x => x.Note)
             .HasColumnName("note")
             .HasMaxLength(2000);

            b.Property(x => x.PrintNote)
             .HasColumnName("print_note")
             .HasMaxLength(2000);

            b.Property(x => x.CreatedDate)
             .HasColumnName("created_date")
             .IsRequired();

            b.Property(x => x.CreatedBy)
             .HasColumnName("created_by")
             .IsRequired();

            b.Property(x => x.UpdatedDate)
             .HasColumnName("updated_date");

            b.Property(x => x.UpdatedBy)
             .HasColumnName("updated_by");

            b.Property(x => x.CompanyId)
             .HasColumnName("company_id")
             .IsRequired();

            b.Property(x => x.IsActive)
             .HasColumnName("is_active")
             .HasDefaultValue(true)
             .IsRequired();

            // Relationship: ColorChipRecord -> AttachmentCollection
            b.HasOne(x => x.AttachmentCollection)
             .WithMany()
             .HasForeignKey(x => x.AttachmentCollectionId)
             .OnDelete(DeleteBehavior.Restrict);

            // Relationship: ColorChipRecord -> Customer
            b.HasOne(x => x.Customer)
             .WithMany()
             .HasForeignKey(x => x.CustomerId)
             .OnDelete(DeleteBehavior.SetNull);

            // Relationship: ColorChipRecord -> DevelopmentFormula
            b.HasOne(x => x.DevelopmentFormula)
             .WithMany(x => x.ColorChipRecords)
             .HasForeignKey(x => x.DevelopmentFormulaId)
             .OnDelete(DeleteBehavior.SetNull);

            // Relationship: ColorChipRecord -> ManufacturingFormula
            b.HasOne(x => x.ManufacturingFormula)
             .WithMany(x => x.ColorChipRecords)
             .HasForeignKey(x => x.ManufacturingFormulaId)
             .OnDelete(DeleteBehavior.SetNull);

            // Relationship: ColorChipRecord -> Product
            b.HasOne(x => x.Product)
             .WithMany(x => x.ColorChipRecords)
             .HasForeignKey(x => x.ProductId)
             .OnDelete(DeleteBehavior.SetNull);

            b.HasIndex(x => new { x.ManufacturingFormulaId, x.IsActive })
             .HasDatabaseName("ix_color_chip_records_manufacturing_formula_id_is_active");

            b.HasIndex(x => new { x.DevelopmentFormulaId, x.IsActive })
             .HasDatabaseName("ix_color_chip_records_development_formula_id_is_active");

            b.HasIndex(x => new { x.ProductId, x.IsActive })
             .HasDatabaseName("ix_color_chip_records_product_id_is_active");

            b.HasIndex(x => new { x.CustomerId, x.IsActive })
             .HasDatabaseName("ix_color_chip_records_customer_id_is_active");

            b.HasIndex(x => new { x.RecordType, x.ProductCodeSnapshot, x.IsActive })
             .HasDatabaseName("ix_color_chip_records_record_type_product_code_snapshot_is_active");

            b.HasIndex(x => new { x.ChipPurpose, x.IsActive })
             .HasDatabaseName("ix_color_chip_records_chip_purpose_is_active");

            b.HasIndex(x => new { x.CustomerExternalIdSnapshot, x.IsActive })
             .HasDatabaseName("ix_color_chip_records_customer_external_id_snapshot_is_active");

            b.HasIndex(x => new { x.ManufacturingFormulaExternalIdSnapshot, x.IsActive })
             .HasDatabaseName("ix_color_chip_records_mfg_formula_external_id_snapshot_is_active");

            b.HasIndex(x => new { x.DevelopmentFormulaExternalIdSnapshot, x.IsActive })
             .HasDatabaseName("ix_color_chip_records_dev_formula_external_id_snapshot_is_active");
        }
    }
}
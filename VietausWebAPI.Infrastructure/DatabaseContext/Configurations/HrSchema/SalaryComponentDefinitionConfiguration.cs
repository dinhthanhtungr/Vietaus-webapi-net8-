using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.HrSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.HrSchema
{
    public class SalaryComponentDefinitionConfiguration : IEntityTypeConfiguration<SalaryComponentDefinition>
    {
        public void Configure(EntityTypeBuilder<SalaryComponentDefinition> entity)
        {
            entity.ToTable("salary_component_definitions", "hr");

            entity.HasKey(e => e.SalaryComponentDefinitionId).HasName("pk_salary_component_definitions");

            entity.Property(e => e.SalaryComponentDefinitionId)
                .HasColumnName("salary_component_definition_id")
                .ValueGeneratedNever();

            entity.Property(e => e.Code)
                .HasColumnName("code")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.ValueType).HasColumnName("value_type");

            entity.Property(e => e.AffectsGross).HasColumnName("affects_gross").HasDefaultValue(false);
            entity.Property(e => e.AffectsNet).HasColumnName("affects_net").HasDefaultValue(false);
            entity.Property(e => e.AffectsInsuranceBase).HasColumnName("affects_insurance_base").HasDefaultValue(false);
            entity.Property(e => e.AffectsTaxableIncome).HasColumnName("affects_taxable_income").HasDefaultValue(false);

            entity.Property(e => e.SortOrder).HasColumnName("sort_order").HasDefaultValue(0);
            entity.Property(e => e.IsSystem).HasColumnName("is_system").HasDefaultValue(false);
            entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);

            entity.Property(e => e.FormulaTemplate).HasColumnName("formula_template").HasColumnType("text");
            entity.Property(e => e.Note).HasColumnName("note").HasColumnType("text");

            entity.HasIndex(e => e.Code).IsUnique().HasDatabaseName("ux_salary_component_definitions_code");
            entity.HasIndex(e => new { e.Category, e.IsActive }).HasDatabaseName("ix_salary_component_definitions_category_active");
        }
    }
}

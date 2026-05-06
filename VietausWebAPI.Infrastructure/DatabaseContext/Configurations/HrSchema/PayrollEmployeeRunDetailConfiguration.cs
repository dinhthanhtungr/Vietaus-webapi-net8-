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
    public class PayrollEmployeeRunDetailConfiguration : IEntityTypeConfiguration<PayrollEmployeeRunDetail>
    {
        public void Configure(EntityTypeBuilder<PayrollEmployeeRunDetail> entity)
        {
            entity.ToTable("payroll_employee_run_details", "hr");

            entity.HasKey(e => e.PayrollEmployeeRunDetailId).HasName("pk_payroll_employee_run_details");

            entity.Property(e => e.PayrollEmployeeRunDetailId)
                .HasColumnName("payroll_employee_run_detail_id")
                .ValueGeneratedNever();

            entity.Property(e => e.PayrollEmployeeRunId).HasColumnName("payroll_employee_run_id");
            entity.Property(e => e.SalaryComponentDefinitionId).HasColumnName("salary_component_definition_id");

            entity.Property(e => e.ComponentCodeSnapshot).HasColumnName("component_code_snapshot").HasMaxLength(50);
            entity.Property(e => e.ComponentNameSnapshot).HasColumnName("component_name_snapshot").HasMaxLength(255);
            entity.Property(e => e.CategorySnapshot).HasColumnName("category_snapshot");

            entity.Property(e => e.Quantity).HasColumnName("quantity").HasPrecision(18, 4);
            entity.Property(e => e.Rate).HasColumnName("rate").HasPrecision(18, 6);
            entity.Property(e => e.Amount).HasColumnName("amount").HasPrecision(18, 2);

            entity.Property(e => e.SourceType).HasColumnName("source_type");
            entity.Property(e => e.SourceReference).HasColumnName("source_reference").HasMaxLength(255);
            entity.Property(e => e.FormulaTextSnapshot).HasColumnName("formula_text_snapshot").HasColumnType("text");
            entity.Property(e => e.SortOrder).HasColumnName("sort_order").HasDefaultValue(0);
            entity.Property(e => e.Note).HasColumnName("note").HasColumnType("text");

            entity.HasIndex(e => e.PayrollEmployeeRunId).HasDatabaseName("ix_payroll_employee_run_details_run");
            entity.HasIndex(e => e.SalaryComponentDefinitionId).HasDatabaseName("ix_payroll_employee_run_details_component");

            entity.HasOne(e => e.PayrollEmployeeRun)
                .WithMany(e => e.PayrollEmployeeRunDetails)
                .HasForeignKey(e => e.PayrollEmployeeRunId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_payroll_employee_run_details_run");

            entity.HasOne(e => e.SalaryComponentDefinition)
                .WithMany(e => e.PayrollEmployeeRunDetails)
                .HasForeignKey(e => e.SalaryComponentDefinitionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_payroll_employee_run_details_component");
        }
    }
}

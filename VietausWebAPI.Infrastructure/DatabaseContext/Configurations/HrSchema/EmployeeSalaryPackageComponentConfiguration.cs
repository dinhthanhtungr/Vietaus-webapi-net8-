using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.HrSchema.Salary_models;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.HrSchema
{
    public class EmployeeSalaryPackageComponentConfiguration : IEntityTypeConfiguration<EmployeeSalaryPackageComponent>
    {
        public void Configure(EntityTypeBuilder<EmployeeSalaryPackageComponent> entity)
        {
            entity.ToTable("employee_salary_package_components", "hr");

            entity.HasKey(e => e.EmployeeSalaryPackageComponentId).HasName("pk_employee_salary_package_components");

            entity.Property(e => e.EmployeeSalaryPackageComponentId)
                .HasColumnName("employee_salary_package_component_id")
                .ValueGeneratedNever();

            entity.Property(e => e.EmployeeSalaryPackageId).HasColumnName("employee_salary_package_id");
            entity.Property(e => e.SalaryComponentDefinitionId).HasColumnName("salary_component_definition_id");
            entity.Property(e => e.Amount).HasColumnName("amount").HasPrecision(18, 2);
            entity.Property(e => e.Rate).HasColumnName("rate").HasPrecision(18, 6);
            entity.Property(e => e.FormulaText).HasColumnName("formula_text").HasColumnType("text");
            entity.Property(e => e.IsRecurring).HasColumnName("is_recurring").HasDefaultValue(true);
            entity.Property(e => e.EffectiveFrom).HasColumnName("effective_from");
            entity.Property(e => e.EffectiveTo).HasColumnName("effective_to");
            entity.Property(e => e.Note).HasColumnName("note").HasColumnType("text");

            entity.HasIndex(e => e.EmployeeSalaryPackageId).HasDatabaseName("ix_employee_salary_package_components_package");

            entity.HasOne(e => e.EmployeeSalaryPackage)
                .WithMany(e => e.EmployeeSalaryPackageComponents)
                .HasForeignKey(e => e.EmployeeSalaryPackageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_employee_salary_package_components_package");

            entity.HasOne(e => e.SalaryComponentDefinition)
                .WithMany()
                .HasForeignKey(e => e.SalaryComponentDefinitionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_employee_salary_package_components_definition");
        }
    }
}

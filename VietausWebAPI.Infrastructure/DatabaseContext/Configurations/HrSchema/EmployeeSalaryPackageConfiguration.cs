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
    public class EmployeeSalaryPackageConfiguration : IEntityTypeConfiguration<EmployeeSalaryPackage>
    {
        public void Configure(EntityTypeBuilder<EmployeeSalaryPackage> entity)
        {
            entity.ToTable("employee_salary_packages", "hr");

            entity.HasKey(e => e.EmployeeSalaryPackageId).HasName("pk_employee_salary_packages");

            entity.Property(e => e.EmployeeSalaryPackageId)
                .HasColumnName("employee_salary_package_id")
                .ValueGeneratedNever();

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.BasicSalary).HasColumnName("basic_salary").HasPrecision(18, 2);
            entity.Property(e => e.InsuranceSalary).HasColumnName("insurance_salary").HasPrecision(18, 2);
            entity.Property(e => e.StandardWorkingDays).HasColumnName("standard_working_days").HasPrecision(10, 2);
            entity.Property(e => e.StandardWorkingHours).HasColumnName("standard_working_hours").HasPrecision(10, 2);

            entity.Property(e => e.EffectiveFrom).HasColumnName("effective_from");
            entity.Property(e => e.EffectiveTo).HasColumnName("effective_to");
            entity.Property(e => e.IsCurrent).HasColumnName("is_current").HasDefaultValue(true);

            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method").HasMaxLength(50);
            entity.Property(e => e.BankAccountId).HasColumnName("bank_account_id");
            entity.Property(e => e.Note).HasColumnName("note").HasColumnType("text");

            entity.HasIndex(e => e.EmployeeId).HasDatabaseName("ix_employee_salary_packages_employee");
            entity.HasIndex(e => new { e.EmployeeId, e.IsCurrent })
                .IsUnique()
                .HasFilter("\"is_current\" = true")
                .HasDatabaseName("ux_employee_salary_packages_one_current");

            entity.HasOne(e => e.Employee)
                .WithMany()
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_employee_salary_packages_employee");

            entity.HasOne(e => e.BankAccount)
                .WithMany()
                .HasForeignKey(e => e.BankAccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_employee_salary_packages_bank_account");
        }
    }
}

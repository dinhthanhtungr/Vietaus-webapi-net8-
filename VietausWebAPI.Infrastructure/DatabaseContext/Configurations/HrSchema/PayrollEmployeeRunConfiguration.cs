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
    public class PayrollEmployeeRunConfiguration : IEntityTypeConfiguration<PayrollEmployeeRun>
    {
        public void Configure(EntityTypeBuilder<PayrollEmployeeRun> entity)
        {
            entity.ToTable("payroll_employee_runs", "hr");

            entity.HasKey(e => e.PayrollEmployeeRunId).HasName("pk_payroll_employee_runs");

            entity.Property(e => e.PayrollEmployeeRunId)
                .HasColumnName("payroll_employee_run_id")
                .ValueGeneratedNever();

            entity.Property(e => e.PayrollPeriodId).HasColumnName("payroll_period_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

            entity.Property(e => e.EmployeeCodeSnapshot).HasColumnName("employee_code_snapshot").HasMaxLength(100);
            entity.Property(e => e.EmployeeNameSnapshot).HasColumnName("employee_name_snapshot").HasMaxLength(255);
            entity.Property(e => e.DepartmentSnapshot).HasColumnName("department_snapshot").HasMaxLength(255);
            entity.Property(e => e.JobTitleSnapshot).HasColumnName("job_title_snapshot").HasMaxLength(255);
            entity.Property(e => e.BankAccountSnapshot).HasColumnName("bank_account_snapshot").HasMaxLength(100);

            entity.Property(e => e.BaseSalarySnapshot).HasColumnName("base_salary_snapshot").HasPrecision(18, 2);
            entity.Property(e => e.InsuranceSalarySnapshot).HasColumnName("insurance_salary_snapshot").HasPrecision(18, 2);

            entity.Property(e => e.GrossIncome).HasColumnName("gross_income").HasPrecision(18, 2);
            entity.Property(e => e.TotalEarnings).HasColumnName("total_earnings").HasPrecision(18, 2);
            entity.Property(e => e.TotalDeductions).HasColumnName("total_deductions").HasPrecision(18, 2);
            entity.Property(e => e.EmployerContributionTotal).HasColumnName("employer_contribution_total").HasPrecision(18, 2);
            entity.Property(e => e.NetIncome).HasColumnName("net_income").HasPrecision(18, 2);

            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Note).HasColumnName("note").HasColumnType("text");
            entity.Property(e => e.LockedBy).HasColumnName("locked_by");
            entity.Property(e => e.LockedAt).HasColumnName("locked_at");

            entity.HasIndex(e => new { e.PayrollPeriodId, e.EmployeeId })
                .IsUnique()
                .HasDatabaseName("ux_payroll_employee_runs_period_employee");

            entity.HasOne(e => e.PayrollPeriod)
                .WithMany(e => e.PayrollEmployeeRuns)
                .HasForeignKey(e => e.PayrollPeriodId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_payroll_employee_runs_period");

            entity.HasOne(e => e.Employee)
                .WithMany()
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_payroll_employee_runs_employee");

            entity.HasOne(e => e.LockedByNavigation)
                .WithMany()
                .HasForeignKey(e => e.LockedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_payroll_employee_runs_locked_by");
        }
    }
}

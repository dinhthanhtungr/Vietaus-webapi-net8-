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
    public class EmployeeInsuranceContributionConfiguration : IEntityTypeConfiguration<EmployeeInsuranceContribution>
    {
        public void Configure(EntityTypeBuilder<EmployeeInsuranceContribution> entity)
        {
            entity.ToTable("employee_insurance_contributions", "hr");

            entity.HasKey(e => e.EmployeeInsuranceContributionId).HasName("pk_employee_insurance_contributions");

            entity.Property(e => e.EmployeeInsuranceContributionId)
                .HasColumnName("employee_insurance_contribution_id")
                .ValueGeneratedNever();

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.PayrollPeriodId).HasColumnName("payroll_period_id");
            entity.Property(e => e.ContributionMonth).HasColumnName("contribution_month");

            entity.Property(e => e.EmployeeNameSnapshot).HasColumnName("employee_name_snapshot").HasMaxLength(255);
            entity.Property(e => e.DepartmentSnapshot).HasColumnName("department_snapshot").HasMaxLength(255);
            entity.Property(e => e.JobTitleSnapshot).HasColumnName("job_title_snapshot").HasMaxLength(255);
            entity.Property(e => e.SocialInsuranceNumberSnapshot).HasColumnName("social_insurance_number_snapshot").HasMaxLength(100);

            entity.Property(e => e.SocialInsuranceSalary).HasColumnName("social_insurance_salary").HasPrecision(18, 2);
            entity.Property(e => e.UnemploymentInsuranceSalary).HasColumnName("unemployment_insurance_salary").HasPrecision(18, 2);

            entity.Property(e => e.CompanyRetirementRate).HasColumnName("company_retirement_rate").HasPrecision(9, 6);
            entity.Property(e => e.CompanySicknessMaternityRate).HasColumnName("company_sickness_maternity_rate").HasPrecision(9, 6);
            entity.Property(e => e.CompanyOccupationalAccidentRate).HasColumnName("company_occupational_accident_rate").HasPrecision(9, 6);
            entity.Property(e => e.CompanyHealthInsuranceRate).HasColumnName("company_health_insurance_rate").HasPrecision(9, 6);
            entity.Property(e => e.CompanyUnemploymentRate).HasColumnName("company_unemployment_rate").HasPrecision(9, 6);
            entity.Property(e => e.CompanyTotalRate).HasColumnName("company_total_rate").HasPrecision(9, 6);

            entity.Property(e => e.EmployeeSocialInsuranceRate).HasColumnName("employee_social_insurance_rate").HasPrecision(9, 6);
            entity.Property(e => e.EmployeeHealthInsuranceRate).HasColumnName("employee_health_insurance_rate").HasPrecision(9, 6);
            entity.Property(e => e.EmployeeUnemploymentRate).HasColumnName("employee_unemployment_rate").HasPrecision(9, 6);
            entity.Property(e => e.EmployeeTotalRate).HasColumnName("employee_total_rate").HasPrecision(9, 6);
            entity.Property(e => e.MonthlyTotalPayableRate).HasColumnName("monthly_total_payable_rate").HasPrecision(9, 6);

            entity.Property(e => e.CompanyRetirementAmount).HasColumnName("company_retirement_amount").HasPrecision(18, 2);
            entity.Property(e => e.CompanySicknessMaternityAmount).HasColumnName("company_sickness_maternity_amount").HasPrecision(18, 2);
            entity.Property(e => e.CompanyOccupationalAccidentAmount).HasColumnName("company_occupational_accident_amount").HasPrecision(18, 2);
            entity.Property(e => e.CompanyHealthInsuranceAmount).HasColumnName("company_health_insurance_amount").HasPrecision(18, 2);
            entity.Property(e => e.CompanyUnemploymentAmount).HasColumnName("company_unemployment_amount").HasPrecision(18, 2);
            entity.Property(e => e.CompanyTotalAmount).HasColumnName("company_total_amount").HasPrecision(18, 2);

            entity.Property(e => e.EmployeeSocialInsuranceAmount).HasColumnName("employee_social_insurance_amount").HasPrecision(18, 2);
            entity.Property(e => e.EmployeeHealthInsuranceAmount).HasColumnName("employee_health_insurance_amount").HasPrecision(18, 2);
            entity.Property(e => e.EmployeeUnemploymentAmount).HasColumnName("employee_unemployment_amount").HasPrecision(18, 2);
            entity.Property(e => e.EmployeeTotalAmount).HasColumnName("employee_total_amount").HasPrecision(18, 2);
            entity.Property(e => e.MonthlyTotalPayableAmount).HasColumnName("monthly_total_payable_amount").HasPrecision(18, 2);

            entity.Property(e => e.Note).HasColumnName("note").HasColumnType("text");

            entity.HasIndex(e => new { e.EmployeeId, e.ContributionMonth }).HasDatabaseName("ix_employee_insurance_contributions_employee_month");

            entity.HasOne(e => e.Employee)
                .WithMany()
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_employee_insurance_contributions_employee");

            entity.HasOne(e => e.PayrollPeriod)
                .WithMany(e => e.EmployeeInsuranceContributions)
                .HasForeignKey(e => e.PayrollPeriodId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_employee_insurance_contributions_period");
        }
    }
}

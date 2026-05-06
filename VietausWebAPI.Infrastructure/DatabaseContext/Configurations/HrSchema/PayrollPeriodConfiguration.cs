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
    public class PayrollPeriodConfiguration : IEntityTypeConfiguration<PayrollPeriod>
    {
        public void Configure(EntityTypeBuilder<PayrollPeriod> entity)
        {
            entity.ToTable("payroll_periods", "hr");

            entity.HasKey(e => e.PayrollPeriodId).HasName("pk_payroll_periods");

            entity.Property(e => e.PayrollPeriodId)
                .HasColumnName("payroll_period_id")
                .ValueGeneratedNever();

            entity.Property(e => e.Code)
                .HasColumnName("code")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.FromDate).HasColumnName("from_date");
            entity.Property(e => e.ToDate).HasColumnName("to_date");
            entity.Property(e => e.PayrollType).HasColumnName("payroll_type");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Note).HasColumnName("note").HasColumnType("text");

            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.ApprovedDate).HasColumnName("approved_date");

            entity.HasIndex(e => e.Code).IsUnique().HasDatabaseName("ux_payroll_periods_code");
            entity.HasIndex(e => new { e.Year, e.Month, e.PayrollType }).HasDatabaseName("ix_payroll_periods_year_month_type");

            entity.HasOne(e => e.CreatedByNavigation)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_payroll_periods_created_by");

            entity.HasOne(e => e.ApprovedByNavigation)
                .WithMany()
                .HasForeignKey(e => e.ApprovedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_payroll_periods_approved_by");
        }
    }
}

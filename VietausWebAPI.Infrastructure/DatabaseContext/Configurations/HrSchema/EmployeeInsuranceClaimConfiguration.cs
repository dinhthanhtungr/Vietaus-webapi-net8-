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
    public class EmployeeInsuranceClaimConfiguration : IEntityTypeConfiguration<EmployeeInsuranceClaim>
    {
        public void Configure(EntityTypeBuilder<EmployeeInsuranceClaim> entity)
        {
            entity.ToTable("employee_insurance_claims", "hr");

            entity.HasKey(e => e.EmployeeInsuranceClaimId).HasName("pk_employee_insurance_claims");

            entity.Property(e => e.EmployeeInsuranceClaimId)
                .HasColumnName("employee_insurance_claim_id")
                .ValueGeneratedNever();

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.ClaimMonth).HasColumnName("claim_month");
            entity.Property(e => e.ClaimMonthLabel).HasColumnName("claim_month_label").HasMaxLength(50);
            entity.Property(e => e.SequenceNo).HasColumnName("sequence_no");

            entity.Property(e => e.EmployeeNameSnapshot).HasColumnName("employee_name_snapshot").HasMaxLength(255);
            entity.Property(e => e.SocialInsuranceNumberSnapshot).HasColumnName("social_insurance_number_snapshot").HasMaxLength(100);

            entity.Property(e => e.LeaveReason).HasColumnName("leave_reason").HasColumnType("text");
            entity.Property(e => e.LeaveFromDate).HasColumnName("leave_from_date");
            entity.Property(e => e.LeaveToDate).HasColumnName("leave_to_date");
            entity.Property(e => e.LeaveDays).HasColumnName("leave_days").HasPrecision(10, 2);

            entity.Property(e => e.ClaimType).HasColumnName("claim_type").HasMaxLength(255);
            entity.Property(e => e.ClaimNumber).HasColumnName("claim_number").HasMaxLength(100);
            entity.Property(e => e.ProcessingStatus).HasColumnName("processing_status").HasMaxLength(100);

            entity.Property(e => e.AppointmentDate).HasColumnName("appointment_date");
            entity.Property(e => e.ReturnedToEmployeeDate).HasColumnName("returned_to_employee_date");
            entity.Property(e => e.Note).HasColumnName("note").HasColumnType("text");

            entity.HasIndex(e => e.EmployeeId).HasDatabaseName("ix_employee_insurance_claims_employee");
            entity.HasIndex(e => e.ClaimMonth).HasDatabaseName("ix_employee_insurance_claims_month");
            entity.HasIndex(e => e.ClaimNumber).HasDatabaseName("ix_employee_insurance_claims_claim_number");

            entity.HasOne(e => e.Employee)
                .WithMany()
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_employee_insurance_claims_employee");
        }
    }
}

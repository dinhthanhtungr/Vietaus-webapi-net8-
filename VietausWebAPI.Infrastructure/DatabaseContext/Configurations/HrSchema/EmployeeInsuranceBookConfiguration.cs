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
    public class EmployeeInsuranceBookConfiguration : IEntityTypeConfiguration<EmployeeInsuranceBook>
    {
        public void Configure(EntityTypeBuilder<EmployeeInsuranceBook> entity)
        {
            entity.ToTable("employee_insurance_books", "hr");

            entity.HasKey(e => e.EmployeeInsuranceBookId).HasName("pk_employee_insurance_books");

            entity.Property(e => e.EmployeeInsuranceBookId)
                .HasColumnName("employee_insurance_book_id")
                .ValueGeneratedNever();

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EmployeeCodeSnapshot).HasColumnName("employee_code_snapshot").HasMaxLength(100);
            entity.Property(e => e.EmployeeNameSnapshot).HasColumnName("employee_name_snapshot").HasMaxLength(255);
            entity.Property(e => e.JobTitleSnapshot).HasColumnName("job_title_snapshot").HasMaxLength(255);
            entity.Property(e => e.SocialInsuranceNumberSnapshot).HasColumnName("social_insurance_number_snapshot").HasMaxLength(100);

            entity.Property(e => e.HasBookCover).HasColumnName("has_book_cover").HasDefaultValue(false);
            entity.Property(e => e.HasDetachedLeaf).HasColumnName("has_detached_leaf").HasDefaultValue(false);
            entity.Property(e => e.DetachedLeafCount).HasColumnName("detached_leaf_count");
            entity.Property(e => e.DetachedLeafFromDate).HasColumnName("detached_leaf_from_date");
            entity.Property(e => e.DetachedLeafToDate).HasColumnName("detached_leaf_to_date");
            entity.Property(e => e.Note).HasColumnName("note").HasColumnType("text");

            entity.HasIndex(e => e.EmployeeId).HasDatabaseName("ix_employee_insurance_books_employee");
            entity.HasIndex(e => e.SocialInsuranceNumberSnapshot).HasDatabaseName("ix_employee_insurance_books_social_number");

            entity.HasOne(e => e.Employee)
                .WithMany()
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_employee_insurance_books_employee");
        }
    }
}

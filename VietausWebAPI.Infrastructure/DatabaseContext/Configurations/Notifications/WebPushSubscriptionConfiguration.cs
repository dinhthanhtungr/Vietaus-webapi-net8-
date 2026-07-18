using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.Notifications;

namespace VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs.Configurations.Notifications
{
    public class WebPushSubscriptionConfiguration : IEntityTypeConfiguration<WebPushSubscription>
    {
        private const string Schema = "notification";

        public void Configure(EntityTypeBuilder<WebPushSubscription> entity)
        {
            entity.ToTable("web_push_subscriptions", Schema);

            entity.HasKey(x => x.WebPushSubscriptionId);

            entity.Property(x => x.WebPushSubscriptionId).HasColumnName("web_push_subscription_id");
            entity.Property(x => x.CompanyId).HasColumnName("company_id");
            entity.Property(x => x.EmployeeId).HasColumnName("employee_id");

            entity.Property(x => x.Endpoint)
                  .IsRequired()
                  .HasColumnName("endpoint");

            entity.Property(x => x.P256dh)
                  .IsRequired()
                  .HasMaxLength(256)
                  .HasColumnName("p256dh");

            entity.Property(x => x.Auth)
                  .IsRequired()
                  .HasMaxLength(128)
                  .HasColumnName("auth");

            entity.Property(x => x.DeviceName)
                  .HasMaxLength(256)
                  .HasColumnName("device_name");

            entity.Property(x => x.UserAgent)
                  .HasColumnName("user_agent");

            entity.Property(x => x.IsActive).HasColumnName("is_active");

            entity.Property(x => x.CreatedAt)
                  .HasPrecision(6)
                  .HasColumnName("created_at");

            entity.Property(x => x.LastSuccessAt)
                  .HasPrecision(6)
                  .HasColumnName("last_success_at");

            entity.Property(x => x.LastFailureAt)
                  .HasPrecision(6)
                  .HasColumnName("last_failure_at");

            entity.Property(x => x.FailureCount).HasColumnName("failure_count");

            entity.HasOne(x => x.Company)
                  .WithMany(x => x.WebPushSubscriptions)
                  .HasForeignKey(x => x.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("fk_web_push_subscriptions_company");

            entity.HasOne(x => x.Employee)
                  .WithMany(x => x.WebPushSubscriptions)
                  .HasForeignKey(x => x.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("fk_web_push_subscriptions_employee");

            entity.HasIndex(x => x.Endpoint)
                  .IsUnique()
                  .HasDatabaseName("ux_web_push_subscriptions_endpoint");

            entity.HasIndex(x => new { x.CompanyId, x.EmployeeId, x.IsActive })
                  .HasDatabaseName("ix_web_push_subscriptions_company_employee_active");
        }
    }
}

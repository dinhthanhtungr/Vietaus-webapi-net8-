using VietausWebAPI.Core.Models;
using VietausWebAPI.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VietausWebAPI.WebAPI.DatabaseContext
{
    // Trong IdentityDbCOntext  khi kế thừa sẽ tạo ra các bản 
    // AspNetUsers lưu user
    // AspNetRole lưu role
    // AspNetUserRole Quan hệ nhiều-nhiều giữa User & Role
    // AspNetUserClaims lưu claims của từng user
    // AspNetRoleClaims Lưu claims của từng role
    // AspNetUserLogins Hỗ trợ đăng nhập OAuth như Google, Facebook
    // AspNetUserTokens Lưu token đăng nhập
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
    IdentityUserClaim<Guid>, ApplicationUserRole, IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>, IdentityUserToken<Guid>> // Cái này khá hay là khiu mình kế thừa
    // IdentityDbContext thì nó sẽ tự tạo ra các bản database về User cũng như về Role
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option) { } 
        public ApplicationDbContext() { }
        //public virtual DbSet<Cities> Cities {  get; set; } // Virtual giúp cho database này có thể sử dụng lazy loading nhưng không cần dùng trong api thuần
        public DbSet<Cities> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cities>().HasData(new Cities { Id = Guid.Parse("E9A77912-953B-4E0F-A650-6D8FA72DAE54"), Name = "Rem" });

            // Nếu chỉ cần User và Role, không muốn các bản như AspNetUserClaims, AspNetUserLogins có thể làm như sau
            //modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("IgnoreTable");
            //modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("IgnoreTable");
            //modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("IgnoreTable");
            // Đảm bảo cấu hình đúng cho ApplicationUserRole

            modelBuilder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId);

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);

                userRole.Property(ur => ur.IsActive)
                    .IsRequired()
                    .HasDefaultValue(true);
            });
        }
    }
}

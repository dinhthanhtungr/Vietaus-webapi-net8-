using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.ShiftReportSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.ShiftReportSchema
{
    public class EndOfShiftReportDetailForAllConfiguration : IEntityTypeConfiguration<EndOfShiftReportDetailForAll>
    {
        public void Configure(EntityTypeBuilder<EndOfShiftReportDetailForAll> entity)
        {
            entity.ToTable("endofshiftreportdetailforall", "shiftreports");
            entity.HasNoKey();

            entity.Property(x => x.ShiftReportDetailForAllId)
                  .UseIdentityAlwaysColumn()
                  .HasColumnName("shiftreportdetailforall_id");

            entity.Property(x => x.ExternalId)
                  .HasColumnName("externalid")
                  .HasColumnType("citext");

            entity.Property(x => x.ProductType)
                  .HasColumnName("producttype");

            entity.Property(x => x.NetWeight)
                  .HasColumnName("netweight")
                  .HasPrecision(16, 3);

            entity.Property(x => x.WeightStockedKg)
                  .HasColumnName("weightstockedkg")
                  .HasPrecision(16, 3);

            // tìm nhanh theo externalid (liên kết mềm)
            entity.HasIndex(x => x.ExternalId).HasDatabaseName("ix_eosrdf_externalid");
        }
    }
}

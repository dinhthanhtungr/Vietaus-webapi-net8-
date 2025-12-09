using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Helpers.IdCounter
{
    public class ExternalIdServicePostgres : IExternalIdService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public ExternalIdServicePostgres(ApplicationDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<string> NextAsync(string prefix, CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var companyId = _currentUser.CompanyId;
            var period = now.ToString("yyMM");

            var conn = _context.Database.GetDbConnection();

            var needOpen = conn.State != System.Data.ConnectionState.Open;
            if (needOpen) await conn.OpenAsync(ct);

            try
            {
                await using var cmd = conn.CreateCommand();
                if (_context.Database.CurrentTransaction is IDbContextTransaction tx)
                {
                    cmd.Transaction = _context.Database.CurrentTransaction.GetDbTransaction();
                }


                cmd.CommandText = @"
                    INSERT INTO public.""IdCounters"" (""CompanyId"", ""Prefix"", ""Period"", ""LastNo"")
                    VALUES (@CompanyId, @Prefix, @Period, 1)
                    ON CONFLICT (""CompanyId"", ""Prefix"", ""Period"")
                    DO UPDATE SET ""LastNo"" = ""IdCounters"".""LastNo"" + 1
                    RETURNING ""LastNo"";";

                //cmd.CommandText = @"
                //    INSERT INTO public.""IdCounters"" (""CompanyId"", ""Prefix"", ""Period"", ""LastNo"")
                //    VALUES (@CompanyId, @Prefix, @Period, 1)
                //    ON CONFLICT (""CompanyId"", ""Prefix"")
                //    DO UPDATE SET
                //      ""LastNo"" = CASE
                //                     WHEN public.""IdCounters"".""Period"" = EXCLUDED.""Period""
                //                       THEN public.""IdCounters"".""LastNo"" + 1
                //                     ELSE 1
                //                   END,
                //      ""Period"" = EXCLUDED.""Period""
                //    RETURNING ""LastNo"";";

                cmd.Parameters.Add(new NpgsqlParameter("CompanyId", companyId));
                cmd.Parameters.Add(new NpgsqlParameter("Prefix", prefix));
                cmd.Parameters.Add(new NpgsqlParameter("Period", period));


                var nextNo = Convert.ToInt32(await cmd.ExecuteScalarAsync(ct));
                return $"{prefix}{now:yyMM}{nextNo:00000}";
            }
            finally { if (needOpen) await conn.CloseAsync(); }
        }
    }
}

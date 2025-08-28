using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts
{
    public interface IMaterialRepository
    {
        // Base query: NoTracking để chỉ đọc (Service sẽ .Where/.Select/.ToListAsync)
        IQueryable<Material> Query();

        // Thêm 1 log (header)
        Task AddAsync(Material material, CancellationToken ct = default);

        /// <summary>
        /// lấy sô cuối cùng của code
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        Task<string?> GetLatestExternalIdStartsWithAsync(string prefix);
    }
}

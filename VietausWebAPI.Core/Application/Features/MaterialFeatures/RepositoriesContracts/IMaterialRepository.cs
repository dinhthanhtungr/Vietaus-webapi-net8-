using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts
{
    public interface IMaterialRepository
    {
        // Base query: NoTracking để chỉ đọc (Service sẽ .Where/.Select/.ToListAsync)
        IQueryable<Material> Query(bool track = false);

        // Thêm 1 log (header)
        Task AddAsync(Material material, CancellationToken ct = default);

        /// <summary>
        /// lấy sô cuối cùng của code
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        Task<string?> GetLatestExternalIdStartsWithAsync(string prefix);

        /// <summary>
        /// Xóa mềm vật tư
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> DeleteMaterialAsync(Guid Id, CancellationToken ct = default);

    }
}

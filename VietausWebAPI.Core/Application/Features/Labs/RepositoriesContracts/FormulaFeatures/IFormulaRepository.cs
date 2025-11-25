using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

namespace VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures
{
    public interface IFormulaRepository
    {
        /// <summary>
        /// Tạo lệnh query để truy vấn công thức từ cơ sở dữ liệu.
        /// </summary>
        /// <returns></returns>
        IQueryable<Formula> Query(bool track = false);

        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AddAsync(Formula formula, CancellationToken ct = default);

        /// <summary>
        /// Lấy số cuối cùng của code
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>        
        Task<string?> GetLatestExternalIdStartsWithAsync(string prefix, Guid? id = null);
    }
}

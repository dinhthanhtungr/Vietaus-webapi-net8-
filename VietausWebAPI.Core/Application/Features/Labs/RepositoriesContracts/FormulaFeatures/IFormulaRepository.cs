using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures
{
    public interface IFormulaRepository
    {
        /// <summary>
        /// Tạo lệnh query để truy vấn công thức từ cơ sở dữ liệu.
        /// </summary>
        /// <returns></returns>
        IQueryable<Formula> Query();

        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AddAsync(Formula formula, CancellationToken ct = default);

    }
}

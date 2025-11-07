using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Shared.Helper.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query(bool track = false);

        /// <summary>
        /// Tạo moit ban ghi moi vao co so du lieu.
        /// </summary>
        /// <param name="merchandiseOrder"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AddAsync(T entity, CancellationToken ct = default);

        void Remove(T entity); // sync
    }
}

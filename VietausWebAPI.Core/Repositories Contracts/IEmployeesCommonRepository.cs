using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IEmployeesCommonRepository
    {
        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public Task<IEnumerable<EmployeesCommonDatum>> GetEmployeesWithIdRepositoryAsync(string EmployeeId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.Entities;

namespace VietausWebAPI.Core.ServiceContracts
{
    public interface IEmployeesCommonService
    {
        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public Task<IEnumerable<EmployeesCommonDatumDTO>> GetEmployeesWithIdServiceAsync(string EmployeeId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.Core.Service
{
    public class EmployeesCommonService : IEmployeesCommonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public EmployeesCommonService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeesCommonDatumDTO>> GetEmployeesWithIdServiceAsync(string EmployeeId)
        {
            var employees = await _unitOfWork.EmployeesCommonRepository.GetEmployeesWithIdRepositoryAsync(EmployeeId);
            return _mapper.Map<IEnumerable<EmployeesCommonDatumDTO>>(employees);
        }
        // Implement methods from IEmployeesCommonService here
    }
}

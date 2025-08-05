using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.DTOs;
using VietausWebAPI.Core.Application.Features.HR.Querys;
using VietausWebAPI.Core.Application.Features.HR.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.HR.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public EmployeesService(IUnitOfWork unitOfWork, IMapper mapper)
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

        public async Task<PagedResult<EmployeeSummary>> GetPagedAsync(EmployeeQuery? query)
        {
            var pagedResult = await _unitOfWork.EmployeesRepository.GetPagedAsync(query);
            try
            {
                var pagedResultMapped = _mapper.Map<PagedResult<EmployeeSummary>>(pagedResult);
                return pagedResultMapped;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhân viên: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OperationResult> PostEmployees(EmployeesPostDTOs employee)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if(string.IsNullOrWhiteSpace(employee.ExternalId))
                {
                    employee.ExternalId = await ExternalIdGenerator.GenerateExternalId(
                        "EMP",
                        prefix => _unitOfWork.EmployeesRepository.GetLatestExternalIdStartsWithAsync(prefix)
                    );
                }

                var employeeEntity = _mapper.Map<Employee>(employee);
                await _unitOfWork.EmployeesRepository.PostEmployees(employeeEntity);


                var affected = await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return affected > 0
                    ? OperationResult.Ok("Tạo thành công")
                    : OperationResult.Fail("Thất bại.");
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi tạo nhân viên: {ex.Message}");
            }
        }
        // Implement methods from IEmployeesCommonService here
    }
}

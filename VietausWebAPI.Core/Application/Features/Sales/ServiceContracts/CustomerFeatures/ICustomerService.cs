using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures
{
    public interface ICustomerService
    {
        /// <summary>
        /// Lấy danh sách phân trang
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedResult<GetReviewCustomer>> GetAllAsync(CustomerQuery? query);
        /// <summary>
        /// Thêm khách hàng mới
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task<OperationResult> AddNewCustomer(PostCustomer customer);

        /// <summary>
        /// Lấy thông tin khách hàng theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GetCustomer?> GetCustomerByIdAsync(Guid id);

        /// <summary>
        /// Xóa mềm khách hàng theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OperationResult> DeleteCustomerByIdAsync(Guid id);

        /// <summary>
        /// Lấy danh sách khách hàng theo phân công khách hàng của nhân viên 
        /// </summary>
        /// <param name="isAdmin"></param>
        /// <param name="employeeId"></param>
        /// <param name="keyword"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PagedResult<GetReviewCustomer>> GetCustomerByEmployeeAssignment(
                    bool isAdmin,
                    Guid employeeId,
                    Guid? customerId = null,
                    string? keyword = null,
                    int pageNumber = 1,
                    int pageSize = 15,
                    CancellationToken ct = default);

        Task<OperationResult> UpdateCustomerAsync(PatchCustomer customer);
    }
}

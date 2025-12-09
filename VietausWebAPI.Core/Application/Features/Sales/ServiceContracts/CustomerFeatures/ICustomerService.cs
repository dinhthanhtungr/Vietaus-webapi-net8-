using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs.ResultDtos;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures
{
    public interface ICustomerService
    {

        // ======================================================================== Get ========================================================================
        /// <summary>
        /// Lấy danh sách phân trang
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<OperationResult<PagedResult<GetReviewCustomer>>> GetAllAsync(CustomerQuery? query);

        /// <summary>
        /// Lấy danh sách khách hàng được phân công cho nhân viên cụ thể và yêu cầu đặt biệt của khách hàng ở đơn hàng gần nhất
        /// </summary>
        /// <param name="isAdmin"></param>
        /// <param name="employeeId"></param>
        /// <param name="customerId"></param>
        /// <param name="keyword"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception> 
        Task<OperationResult<PagedResult<GetReviewCustomer>>> GetCustomerByEmployeeAssignment(CustomerQuery query, CancellationToken ct = default);

        /// <summary>
        /// Lấy thông tin khách hàng theo ID cho các phòng ban khác nhau
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GetCustomer> GetCustomerByIdAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Lấy thông tin khách hàng theo ID cho phòng Sales
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OperationResult<GetCustomer>> GetCustomerByIdForSalesAsync(Guid id, CancellationToken ct = default);

        Task<OperationResult<IReadOnlyList<GetCustomerLeadOwner>>> GetCustomerLeadOwner(Guid CustomerId, CancellationToken ct = default);
        // ======================================================================== Post ========================================================================
        /// <summary>
        /// Thêm khách hàng mới
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task<OperationResult<AddCustomerResultDto>> AddNewCustomer(PostCustomer customer);

        // ======================================================================== Patch ========================================================================
        /// <summary>
        /// Xóa mềm khách hàng theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OperationResult> DeleteCustomerByIdAsync(Guid id);

        /// <summary>
        /// Thay đổi thông tin khách hàng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task<OperationResult> UpdateCustomerAsync(PatchCustomer req, CancellationToken ct = default);
    }
}

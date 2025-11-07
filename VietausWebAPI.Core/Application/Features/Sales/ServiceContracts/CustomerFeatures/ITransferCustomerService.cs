using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures
{
    public interface ITransferCustomerService
    {
        /// <summary>
        /// Tạo mới một lần chuyển khách hàng từ nhân viên này sang nhân viên khác
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        Task<PagedResult<TransferCustomerDTO>> GetTransfersAsync(CustomerTransferQuery query, CancellationToken ct = default);

        /// <summary>
        /// Lấy thông tin một lần chuyển khách hàng theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<TransferCustomerDTO?> GetTransferByIdAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách các lần chuyển khách hàng theo bộ lọc trong query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<TransferCustomerDTO> CreateTransferAsync(TransferCustomersRequest req, CancellationToken ct);
    }
}

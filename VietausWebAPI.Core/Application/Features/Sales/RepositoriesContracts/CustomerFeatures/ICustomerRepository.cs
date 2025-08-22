using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Lấy danh sách phân trang khách hàng
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedResult<Customer>> GetAllAsync(CustomerQuery? query);
        /// <summary>
        /// Thêm khách hàng mới
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        Task AddNewCustomer(Customer customer);

        /// <summary>
        /// Lấy thông tin khách hàng theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GetCustomer?> GetCustomerByIdAsync(Guid id);

        /// <summary>
        /// Xoa khách hàng theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteCustomerByIdAsync(Guid id);

        Task<bool> UpdateCustomerAsync(PatchCustomer customer);

        IQueryable<Customer> Query();

        /// <summary>
        /// lấy sô cuối cùng của code
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        Task<string?> GetLatestExternalIdStartsWithAsync(string prefix);
    }
}

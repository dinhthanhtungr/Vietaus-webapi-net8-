using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.DTOs.ExtruderOperationHistoryDTOs;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.Querys.ExtruderOperationHistoryQuery;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.ShiftReportSchema;

namespace VietausWebAPI.Core.Application.Features.ShiftReportFeatures.ServiceContracts.ExtruderOperationHistoryFeatures.GetServices
{
    public interface IExtruderOperationHistoryService
    {
        /// <summary>
        /// Lấy danh sách lịch sử hoạt động của máy đùn theo mã sản phẩm và/hoặc externalId, có phân trang
        /// </summary>
        /// <param name="query">Thông tin truy vấn bao gồm mã sản phẩm, externalId và thông tin phân trang</param>
        /// <param name="cancellationToken">Token hủy bỏ</param>
        /// <returns>Kết quả phân trang chứa danh sách lịch sử hoạt động</returns>
        Task<PagedResult<OperationHistoryMd>> GetPagedAsync(
            ExtruderOperationHistoryQuery query,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Lấy tất cả dữ liệu lịch sử hoạt động của máy đùn theo externalId, không phân trang
        /// </summary>
        /// <param name="externalId">Số LotVa</param>
        /// <param name="cancellationToken">Token hủy bỏ</param>
        /// <returns>Danh sách tất cả lịch sử hoạt động của máy đùn theo externalId</returns>   
        Task<List<OperationHistoryMd>> GetByExternalIdAsync(
            string externalId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy bản ghi lịch sử hoạt động mới nhất của máy đùn theo externalId
        /// </summary>
        /// <param name="externalId">Mã định danh bên ngoài của máy đùn</param>
        /// <param name="cancellationToken">Token hủy bỏ</param>
        /// <returns>Bản ghi lịch sử hoạt động mới nhất của máy đùn theo externalId</returns>
        Task<OperationHistoryMd?> GetLatestByExternalIdAsync(
            string externalId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Kiểm tra xem có tồn tại dữ liệu lịch sử hoạt động của máy đùn theo mã sản phẩm hoặc externalId hay không
        /// </summary>
        /// <param name="query">Thông tin truy vấn bao gồm mã sản phẩm và externalId</param>
        /// <param name="cancellationToken">Token hủy bỏ</param>
        /// <returns>Dữ liệu lịch sử hoạt động nếu tồn tại, null nếu không tồn tại</returns>
        Task<ExtruderOperationHistoryHasData> GetIsValidAsync(
            ExtruderOperationHistoryQuery query,
            CancellationToken cancellationToken = default);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Core.Domain.Entities.CompanySchema;


namespace VietausWebAPI.Core.Application.Features.Shared.ServiceContracts
{
    public interface IVisibilityHelper
    {
        /// <summary>
        /// Tạo phạm vi người xem dựa trên ngữ cảnh hiện tại
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<ViewerScope> BuildViewerScopeAsync(CancellationToken ct = default);

        /// <summary>
        /// Thêm bộ lọc phạm vi người xem vào truy vấn khách hàng
        /// Quy tắc:
        /// - Admin/President/Developer/CustomerViewAll/Lab: thấy tất cả
        /// - Sales thường: KH mình quản + claim Work (còn hạn)
        /// - Leader: KH do group quản + claim Work của group (còn hạn)
        /// </summary>
        /// <param name="q">Truy vấn khách hàng</param>
        /// <param name="v">Phạm vi người xem</param>
        /// <returns></returns>
        IQueryable<Customer> ApplyCustomer(IQueryable<Customer> q, ViewerScope v);

        /// <summary>
        /// Thêm bộ lọc phạm vi người xem vào truy vấn yêu cầu mẫu
        /// </summary>
        /// <param name="q">Truy vấn yêu cầu mẫu</param>
        /// <param name="v">Phạm vi người xem</param>
        /// <returns></returns>
        IQueryable<SampleRequest> ApplySampleRequest(IQueryable<SampleRequest> q, ViewerScope v);

        /// <summary>
        /// Thêm bộ lọc phạm vi người xem vào truy vấn đơn hàng hàng hóa
        /// </summary>
        /// <param name="q">Truy vấn đơn hàng hàng hóa</param>
        /// <param name="v">Phạm vi người xem</param>
        /// <returns></returns>
        IQueryable<MerchandiseOrder> ApplyMerchandiseOrder(IQueryable<MerchandiseOrder> q, ViewerScope v);

        /// <summary>
        /// Phân quyền xem nhóm: Admin/President/Developer/CustomerViewAll/Lab thấy tất cả, Sales thường thấy nhóm có thành viên là mình, Leader thấy nhóm do mình quản lý
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IQueryable<Group>> ApplyGroupAsync(IQueryable<Group> q, CancellationToken ct = default);
    }
}

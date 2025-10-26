using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.Querys.Groups;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature

{
    public interface ISampleRequestRepository
    {
        /// <summary>
        /// Tạo lệnh query để truy vấn yêu cầu mẫu từ cơ sở dữ liệu.
        /// </summary>
        /// <returns></returns>
        IQueryable<SampleRequest> Query(bool track = false);
        /// <summary>
        /// Tạo mới một yêu cầu mẫu
        /// </summary>
        /// <param name="sampleRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AddAsync(SampleRequest sampleRequest, CancellationToken ct = default);
        /// <summary>
        /// lấy sô cuối cùng của code
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        Task<string?> GetLatestExternalIdStartsWithAsync(string prefix);

        /// <summary>
        /// Xóa mẫu theo điều kiện
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<int> DeleteSampleRequestAsync(Guid id);

        /// <summary>
        /// Thay đổi thông tin của một yêu cầu mẫu đã tồn tại.
        /// </summary>
        /// <param name="sampleRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<int> UpdateSampleRequestAsync(UpdateSampleRequest sampleRequest, CancellationToken ct = default);
    }
}

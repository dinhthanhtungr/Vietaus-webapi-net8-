using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature

{
    public interface ISampleRequestRepository
    {
        /// <summary>
        /// Tạo lệnh query để truy vấn yêu cầu mẫu từ cơ sở dữ liệu.
        /// </summary>
        /// <returns></returns>
        IQueryable<SampleRequest> Query();
        /// <summary>
        /// Tạo mới một yêu cầu mẫu
        /// </summary>
        /// <param name="sampleRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AddAsync(SampleRequest sampleRequest, CancellationToken ct = default);
    }
}

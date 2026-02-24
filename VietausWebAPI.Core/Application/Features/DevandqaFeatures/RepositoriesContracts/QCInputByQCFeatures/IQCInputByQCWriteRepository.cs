using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.QCInputByQCFeatures;
using VietausWebAPI.Core.Domain.Entities.DevandqaSchema;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;
using VietausWebAPI.Core.Domain.Enums.Attachment;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts.QCInputByQCFeatures
{
    public interface IQCInputByQCWriteRepository
    {
        // ======================================================================== Post ======================================================================== 
        /// <summary>
        /// Thêm một thông tin về QCInputByQC
        /// </summary>
        /// <param name="qCInputByQC"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<QCInputByQC> AddAsync(QCInputByQC entity, CancellationToken ct);

        Task<QCInputByQC> PatchByVoucherDetailIdAsync(
            long voucherDetailId,
            PatchQCInputByQC patch,
            Guid userId,
            CancellationToken ct);

        Task<QCInputByQC> UpdateAttachmentStatusByVoucherDetailIdAsync(
            long voucherDetailId,
            AttachmentUploadStatus status,
            string? lastError,
            CancellationToken ct);

    }
}

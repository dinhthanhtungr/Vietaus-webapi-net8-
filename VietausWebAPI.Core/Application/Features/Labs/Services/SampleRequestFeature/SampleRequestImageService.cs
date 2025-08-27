using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
using VietausWebAPI.Core.Application.Features.Labs.Helpers.ImageStorage;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature
{
    public class SampleRequestImageService : ISampleRequestImageService
    {

        private static readonly HashSet<string> AllowedImageTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/bmp",
            "image/webp"
        };

        private readonly IFileStorage _storage;                    // nơi lưu file vật lý (ổ đĩa/S3)
        private readonly IUnitOfWork _unitOfWork;
        public SampleRequestImageService(IUnitOfWork unitOfWork, IFileStorage fileStorage)
        {
            _unitOfWork = unitOfWork;
            _storage = fileStorage;
        }
        public async Task<List<SampleRequestImageDto>> ListAsync(Guid sampleRequestId, CancellationToken ct)
        {
            return await _unitOfWork.SampleRequestImageRepository.ListAsync(sampleRequestId, ct)
                .ContinueWith(t => t.Result.Select(img => new SampleRequestImageDto
                {
                    SampleRequestImageId = img.SampleRequestImageId,
                    FileName = img.FileName,
                    FileType = img.FileType,
                    FileSize = img.FileSize,
                    FileUrl = img.FileUrl,
                    SortOrder = img.SortOrder,
                    IsCover = img.IsCover
                }).ToList(), ct);
        }

        public async Task<UploadImageResultDto> UploadAsync(Guid sampleRequestId, string originalFileName, string contentType, long contentLength, Stream content, CancellationToken ct)
        {
            // 2) Lưu file (chỉ service biết folder quy ước)
            var ext = Path.GetExtension(originalFileName);
            var imageId = Guid.NewGuid();
            var fileNameOnDisk = $"{imageId:N}{(string.IsNullOrEmpty(ext) ? ".jpg" : ext)}";
            var folder = $"uploads/sample-requests/{sampleRequestId:N}";
            var (publicUrl, storageKey) = await _storage.SaveAsync(content, contentType, fileNameOnDisk, folder);

            // 3) Tính sort & auto-cover
            //var lastOrder = await _imgRepo.GetMaxSortOrderAsync(sampleRequestId, ct) ?? -1;
            //var hasCover = await _imgRepo.HasCoverAsync(sampleRequestId, ct); 

            var entity = new SampleRequestImage
            {
                SampleRequestId = sampleRequestId,
                SampleRequestImageId = imageId,
                FileName = originalFileName,
                FileType = contentType,
                FileSize = contentLength,
                FileUrl = publicUrl
            };

            await _unitOfWork.SampleRequestImageRepository.AddAsync(entity, ct);

            await _unitOfWork.SaveChangesAsync();

            return new UploadImageResultDto
            {
                SampleRequestImageId = entity.SampleRequestImageId,
                FileName = entity.FileName,
                FileType = entity.FileType,
                FileSize = entity.FileSize,
                FileUrl = entity.FileUrl
            };
        }
    }
}

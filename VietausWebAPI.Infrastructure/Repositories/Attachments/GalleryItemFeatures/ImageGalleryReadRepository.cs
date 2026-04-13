using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Attachments.DTOs.GalleryItemFeatures.GetDtos;
using VietausWebAPI.Core.Application.Features.Attachments.Queries.GalleryItemFeatures;
using VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts.GalleryItemFeatures;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Enums.Attachment;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Attachments.GalleryItemFeatures
{
    public class ImageGalleryReadRepository : IImageGalleryReadRepository
    {
        private readonly ApplicationDbContext _context;

        public ImageGalleryReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<GetSampleRequestSummaryPlusDTO>> GetPagedSampleRequestSummariesAsync(
            ImageGalleryQuery query,
            CancellationToken ct)
        {
            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 20;

            var sampleRequests = _context.SampleRequests
                .AsNoTracking()
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var keyword = query.Keyword.Trim();

                sampleRequests = sampleRequests.Where(x =>
                    x.ExternalId.Contains(keyword) ||
                    (x.Product != null && (
                        (x.Product.Name != null && x.Product.Name.Contains(keyword)) ||
                        (x.Product.ColourCode != null && x.Product.ColourCode.Contains(keyword)) ||
                        (x.Product.Code != null && x.Product.Code.Contains(keyword))
                    )) ||
                    (x.Customer != null && (
                        (x.Customer.CustomerName != null && x.Customer.CustomerName.Contains(keyword)) ||
                        (x.Customer.ExternalId != null && x.Customer.ExternalId.Contains(keyword))
                    ))
                );
            }

            if (!string.IsNullOrWhiteSpace(query.ColorSpace))
            {
                var colorSpace = query.ColorSpace.Trim();
                sampleRequests = sampleRequests.Where(x =>
                    x.Product != null &&
                    x.Product.ColourName != null &&
                    x.Product.ColourName.Contains(colorSpace)
                );
            }

            if (!string.IsNullOrWhiteSpace(query.Additive))
            {
                var additive = query.Additive.Trim();
                sampleRequests = sampleRequests.Where(x =>
                    x.Product != null &&
                    x.Product.Additive != null &&
                    x.Product.Additive.Contains(additive)
                );
            }

            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                var status = query.Status.Trim();
                sampleRequests = sampleRequests.Where(x =>
                    x.Status != null &&
                    x.Status.Contains(status)
                );
            }

            if (query.CategoryId.HasValue && query.CategoryId.Value != Guid.Empty)
            {
                var categoryId = query.CategoryId.Value;
                sampleRequests = sampleRequests.Where(x =>
                    x.Product != null &&
                    x.Product.CategoryId == categoryId
                );
            }

            var projected = sampleRequests.Select(sr => new
            {
                Sample = sr,
                LatestColor = sr.Product != null
                    ? sr.Product.ColorChipRecords
                        .Where(c => c.IsActive)
                        .OrderByDescending(c => c.RecordDate ?? c.CreatedDate)
                        .Select(c => new
                        {
                            L = (decimal?)c.Lightness,
                            A = (decimal?)c.AValue,
                            B = (decimal?)c.BValue
                        })
                        .FirstOrDefault()
                    : null
            })
            .Select(x => new GetSampleRequestSummaryPlusDTO
            {
                SampleRequestId = x.Sample.SampleRequestId,
                ExternalId = x.Sample.ExternalId,

                ProductId = x.Sample.ProductId,
                ProductName = x.Sample.Product != null ? x.Sample.Product.Name : null,
                ColourCode = x.Sample.Product != null ? x.Sample.Product.ColourCode : null,

                Status = x.Sample.Status,

                CustomerName = x.Sample.Customer != null ? x.Sample.Customer.CustomerName : null,
                CustomerExternalId = x.Sample.Customer != null ? x.Sample.Customer.ExternalId : null,

                CreatedBy = x.Sample.CreatedByNavigation != null ? x.Sample.CreatedByNavigation.FullName : null,
                
                CategoryName = x.Sample.Product != null && x.Sample.Product.Category != null ? x.Sample.Product.Category.Name : null,
                ColourName = x.Sample.Product != null ? x.Sample.Product.ColourName : null,
                Additive = x.Sample.Product != null ? x.Sample.Product.Additive : null,

                CreatedDate = x.Sample.CreatedDate,
                RequestDeliveryDate = x.Sample.RequestDeliveryDate,

                // lấy 1 ảnh đầu tiên của sample request
                AttachmentId = x.Sample.AttachmentCollection.Attachments
                    .OrderBy(a => a.Slot)
                    .Select(a => (Guid?)a.AttachmentId)
                    .FirstOrDefault(),

                AttachmentCollectionId = x.Sample.AttachmentCollectionId,

                Slot = x.Sample.AttachmentCollection.Attachments
                    .OrderBy(a => a.Slot)
                    .Select(a => (int?)a.Slot)
                    .FirstOrDefault(),

                FileName = x.Sample.AttachmentCollection.Attachments
                    .OrderBy(a => a.Slot)
                    .Select(a => a.FileName)
                    .FirstOrDefault(),

                StoragePath = x.Sample.AttachmentCollection.Attachments
                    .OrderBy(a => a.Slot)
                    .Select(a => a.StoragePath)
                    .FirstOrDefault(),

                SizeBytes = x.Sample.AttachmentCollection.Attachments
                    .OrderBy(a => a.Slot)
                    .Select(a => (long?)a.SizeBytes)
                    .FirstOrDefault(),

                LabName = x.Sample.Product != null && x.Sample.Product.CreatedByNavigation != null
                    ? x.Sample.Product.CreatedByNavigation.FullName
                    : null,

                // lấy ColorChipRecord mới nhất theo product
                L = x.LatestColor != null ? x.LatestColor.L : null,
                A = x.LatestColor != null ? x.LatestColor.A : null,
                B = x.LatestColor != null ? x.LatestColor.B : null,
            });

            var hasLabSearch = query.L.HasValue && query.A.HasValue && query.B.HasValue;

            if (!hasLabSearch)
            {
                var totalCount = await projected.CountAsync(ct);

                var items = await projected
                    .OrderByDescending(x => x.CreatedDate)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToListAsync(ct);

                return new PagedResult<GetSampleRequestSummaryPlusDTO>(
                    items,
                    totalCount,
                    query.PageNumber,
                    query.PageSize
                );
            }

            var inputL = query.L!.Value;
            var inputA = query.A!.Value;
            var inputB = query.B!.Value;

            // Lọc sơ bộ ở DB để giảm tải
            const decimal roughTolerance = 20m;

            projected = projected.Where(x =>
                x.L.HasValue && x.A.HasValue && x.B.HasValue &&
                Math.Abs(x.L.Value - inputL) <= roughTolerance &&
                Math.Abs(x.A.Value - inputA) <= roughTolerance &&
                Math.Abs(x.B.Value - inputB) <= roughTolerance
            );

            var rawItems = await projected.ToListAsync(ct);

            var ranked = rawItems
                .Select(x =>
                {
                    var deltaE = Math.Sqrt(
                        Math.Pow((double)(x.L!.Value - inputL), 2) +
                        Math.Pow((double)(x.A!.Value - inputA), 2) +
                        Math.Pow((double)(x.B!.Value - inputB), 2)
                    );

                    x.DeltaE = deltaE; // DTO cần có thêm field này
                    return x;
                })
                .OrderBy(x => x.DeltaE)
                .ThenByDescending(x => x.CreatedDate);

            var total = ranked.Count();

            var itemsWithPaging = ranked
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return new PagedResult<GetSampleRequestSummaryPlusDTO>(
                itemsWithPaging,
                total,
                query.PageNumber,
                query.PageSize
            );
        }

        public async Task<PagedResult<AttachmentModel>> GetPagedImagesAsync(
            ImageGalleryQuery query,
            CancellationToken ct)
        {
            var imageSlots = new[]
            {
                AttachmentSlot.Photo,
                AttachmentSlot.SampleRequest,
                AttachmentSlot.ColouredChip
            };

            var dbQuery = _context.AttachmentModels
                .Where(x => x.IsActive && imageSlots.Contains(x.Slot));

            if (query.CollectionId.HasValue && query.CollectionId.Value != Guid.Empty)
            {
                dbQuery = dbQuery.Where(x => x.AttachmentCollectionId == query.CollectionId.Value);
            }

            if (query.Slot.HasValue)
            {
                dbQuery = dbQuery.Where(x => x.Slot == query.Slot.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var keyword = query.Keyword.Trim();

                dbQuery = dbQuery.Where(x =>
                    EF.Functions.ILike(x.FileName, $"%{keyword}%") ||
                    EF.Functions.ILike(x.StoragePath, $"%{keyword}%"));
            }

            var totalCount = await dbQuery.CountAsync(ct);

            var items = await dbQuery
                .OrderByDescending(x => x.CreateDate)
                .ThenByDescending(x => x.AttachmentId)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(ct);

            return new PagedResult<AttachmentModel>
            (
                items,
                totalCount,
                query.PageNumber,
                query.PageSize
            );
        }

        public async Task<AttachmentModel?> GetImageByIdAsync(
            Guid attachmentId,
            CancellationToken ct)
        {
            var imageSlots = new[]
            {
                AttachmentSlot.Photo,
                AttachmentSlot.SampleRequest,
                AttachmentSlot.ColouredChip
            };

            return await _context.AttachmentModels
                .FirstOrDefaultAsync(x =>
                    x.AttachmentId == attachmentId &&
                    x.IsActive &&
                    imageSlots.Contains(x.Slot),
                    ct);
        }

        public async Task<GetSampleRequestGalleryDetailDTO?> GetDetailBySampleRequestIdAsync(
             Guid sampleRequestId,
             CancellationToken ct)
        {
            var item = await _context.SampleRequests
                .AsNoTracking()
                .Where(x => x.IsActive && x.SampleRequestId == sampleRequestId)
                .Select(sr => new GetSampleRequestGalleryDetailDTO
                {
                    SampleRequestId = sr.SampleRequestId,
                    ProductId = sr.ProductId,   
                    EndUserName = sr.Product != null ? sr.Product.EndUser : null,

                    ExpectedDeliveryDate = sr.ExpectedDeliveryDate,
                    RealDeliveryDate = sr.RealDeliveryDate,
                    RealPriceQuoteDate = sr.RealPriceQuoteDate,
                    ExpectedPriceQuoteDate = sr.ExpectedPriceQuoteDate,
                })
                .FirstOrDefaultAsync(ct);

            if (item == null)
                return null;

            var sampleImages = await GetSampleRequestImagesAsync(sampleRequestId, ct);

            var colorChipImages = new List<GetGalleryImageItemDTO>();
            if (item.ProductId != Guid.Empty)
            {
                colorChipImages = await GetColorChipImagesByProductIdAsync(item.ProductId, ct);
            }

            item.Images.AddRange(sampleImages);
            item.Images.AddRange(colorChipImages);

            item.Images = item.Images
                .GroupBy(x => x.AttachmentId)
                .Select(g => g.First())
                .OrderBy(x => x.SourceType == "SampleRequest" ? 0 : 1)
                .ThenBy(x => x.Slot)
                .ToList();

            item.Thumbnail = item.Images.FirstOrDefault();

            return item;
        }


        // =========================================== PRIVATE HELPERS ===========================================
        private async Task<List<GetGalleryImageItemDTO>> GetSampleRequestImagesAsync(
            Guid sampleRequestId,
            CancellationToken ct)
        {
            return await _context.SampleRequests
                .AsNoTracking()
                .Where(x => x.SampleRequestId == sampleRequestId)
                .Where(x => x.AttachmentCollectionId != null)
                .SelectMany(x => x.AttachmentCollection!.Attachments.Select(a => new GetGalleryImageItemDTO
                {
                    AttachmentId = a.AttachmentId,
                    AttachmentCollectionId = x.AttachmentCollectionId,
                    Slot = a.Slot,
                    FileName = a.FileName,
                    StoragePath = a.StoragePath,
                    SizeBytes = a.SizeBytes,
                    SourceType = "SampleRequest",
                    SourceId = x.SampleRequestId
                }))
                .ToListAsync(ct);
        }

        private async Task<List<GetGalleryImageItemDTO>> GetColorChipImagesByProductIdAsync(
            Guid productId,
            CancellationToken ct)
        {
            return await _context.ColorChipRecords
                .AsNoTracking()
                .Where(c => c.IsActive)
                .Where(c => c.ProductId == productId)
                .Where(c => c.AttachmentCollectionId != null)
                .SelectMany(c => c.AttachmentCollection!.Attachments.Select(a => new GetGalleryImageItemDTO
                {
                    AttachmentId = a.AttachmentId,
                    AttachmentCollectionId = c.AttachmentCollectionId,
                    Slot = a.Slot,
                    FileName = a.FileName,
                    StoragePath = a.StoragePath,
                    SizeBytes = a.SizeBytes,
                    SourceType = "ColorChipRecord",
                    SourceId = c.ColorChipRecordId
                }))
                .ToListAsync(ct);
        }

    }
}

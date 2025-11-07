//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.IO; // nhớ thêm
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using VietausWebAPI.Core.Application.Features.Sales.DTOs.OrderAttachment;
//using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.MerchandiseOrderFeatures;
//using VietausWebAPI.Core.Application.Shared.Helper.FileStorage;
//using VietausWebAPI.Core.Domain.Entities;
//using VietausWebAPI.Core.Domain.Enums.Attachment;
//using VietausWebAPI.Core.Repositories_Contracts;

//public class AttachmentService : IAttachmentService
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IFileShareStorage _fileStorage;
//    private readonly IMapper _mapper;

//    // Cho phép PDF/PNG/JPEG/WEBP
//    private static readonly string[] AllowedTypes = { "application/pdf", "image/png", "image/jpeg", "image/webp" };
//    private const long MaxSize = 50_000_000; // 50 MB

//    public AttachmentService(IUnitOfWork unitOfWork, IFileShareStorage fileStorage, IMapper mapper)
//    {
//        _unitOfWork = unitOfWork;
//        _fileStorage = fileStorage;
//        _mapper = mapper;
//    }

//    /// <summary>
//    /// Câp nhật tệp đính kèm cho đơn hàng
//    /// </summary>
//    /// <param name="orderId"></param>
//    /// <param name="files"></param>
//    /// <param name="slot"></param>
//    /// <param name="createdBy"></param>
//    /// <param name="ct"></param>
//    /// <returns></returns>
//    public async Task<List<OrderAttachment>> UploadAsync(
//        Guid orderId,
//        List<IFormFile> files,
//        AttachmentSlot slot,
//        Guid? createdBy,
//        CancellationToken ct = default)
//    {
//        if (files == null || files.Count == 0) return new();

//        var savedKeys = new List<string>(); // để rollback file khi DB fail

//        await using var tx = await _unitOfWork.BeginTransactionAsync();

//        try
//        {
//            // 1) Kiểm tra đơn hàng
//            var exists = await _unitOfWork.MerchandiseOrderRepository.Query()
//                .AsNoTracking()
//                .AnyAsync(o => o.MerchandiseOrderId == orderId, ct);
//            if (!exists)
//                throw new InvalidOperationException($"Order {orderId} not found.");

//            // 2) Thư mục tương đối trong kho hậu trường
//            var folder = Path.Combine("uploads", "orders", orderId.ToString(), slot.ToString());

//            var attachments = new List<OrderAttachment>(files.Count);

//            foreach (var f in files.Where(x => x?.Length > 0))
//            {
//                // 3) Chặn file quá lớn
//                if (f.Length > MaxSize)
//                    throw new InvalidOperationException($"File '{f.FileName}' > {MaxSize / 1024 / 1024} MB.");

//                // 4) Chặn loại không hợp lệ (không phân biệt hoa thường)
//                var ctType = string.IsNullOrWhiteSpace(f.ContentType) ? "application/octet-stream" : f.ContentType;
//                if (!AllowedTypes.Contains(ctType, StringComparer.OrdinalIgnoreCase))
//                    throw new InvalidOperationException($"File type not allowed: {ctType}");

//                // Replace this line:
//                // await using var s = f.OpenReadStream(MaxSize); // thêm limit để an toàn

//                // With this line:
//                await using var s = f.OpenReadStream(); // IFormFile.OpenReadStream() does not take any arguments
//                var storageKey = await _fileStorage.SaveAsync(s, ctType, f.FileName, folder, ct);
//                savedKeys.Add(storageKey);

//                // 6) Lưu metadata
//                attachments.Add(new OrderAttachment
//                {
//                    AttachmentId = Guid.NewGuid(),
//                    MerchandiseOrderId = orderId,
//                    Slot = slot,
//                    FileName = Path.GetFileName(f.FileName),
//                    SizeBytes = f.Length,
//                    StoragePath = storageKey,   // ví dụ: "uploads/orders/{orderId}/{slot}/<uniqueName>.pdf"
//                    CreateDate = DateTime.Now,
//                    CreateBy = createdBy
//                });
//            }

//            if (attachments.Count > 0)
//            {
//                await _unitOfWork.AttachmentRepository.AddRangeAsync(attachments, ct);
//                await _unitOfWork.SaveChangesAsync(); 
//            }

//            await tx.CommitAsync(ct);
//            return attachments;
//        }
//        catch
//        {
//            // Dọn rác file đã lưu nếu DB/commit thất bại
//            foreach (var key in savedKeys)
//            {
//                try { await _fileStorage.DeleteAsync(key, ct); } catch { /* bỏ qua lỗi cleanup */ }
//            }

//            await tx.RollbackAsync(ct);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Hàm truy vấn danh sách file đính kèm của một đơn hàng (Merchandise Order), trả về dưới dạng DTO để dùng ở UI/API.
//    /// </summary>
//    /// <param name="orderId"></param>
//    /// <param name="slot"></param>
//    /// <param name="ct"></param>
//    /// <returns></returns>
//    public async Task<List<OrderAttachmentDTO>> ListAsync(
//        Guid orderId, AttachmentSlot? slot, CancellationToken ct = default)
//    {
//        var q = _unitOfWork.AttachmentRepository.Query()
//            .AsNoTracking()
//            .Where(a => a.MerchandiseOrderId == orderId);

//        if (slot.HasValue)
//            q = q.Where(a => a.Slot == slot.Value);

//        return await q
//            .OrderByDescending(a => a.CreateDate)
//            .ProjectTo<OrderAttachmentDTO>(_mapper.ConfigurationProvider)
//            .ToListAsync(ct);
//    }

//    /// <summary>
//    /// Mở và trả nội dung (stream) của một file đính kèm theo attachmentId — phục vụ xem/preview inline hoặc tải xuống từ API.
//    /// </summary>
//    /// <param name="attachmentId"></param>
//    /// <param name="ct"></param>
//    /// <returns></returns>
//    /// <exception cref="KeyNotFoundException"></exception>
//    public async Task<StreamResult> GetContentAsync(Guid attachmentId, CancellationToken ct = default)
//    {
//        var att = await _unitOfWork.AttachmentRepository.Query()
//                     .FirstOrDefaultAsync(a => a.AttachmentId == attachmentId, ct)
//                  ?? throw new KeyNotFoundException("Attachment not found");
            
//        var (stream, contentType, storedName) = await _fileStorage.OpenReadAsync(att.StoragePath, ct);
//        // View inline: dùng tên gốc nếu có
//        var name = string.IsNullOrWhiteSpace(att.FileName) ? storedName : att.FileName;
//        return new StreamResult(stream, contentType, name);
//    }

//    public async Task<StreamResult> GetDownloadAsync(Guid attachmentId, CancellationToken ct = default)
//        => await GetContentAsync(attachmentId, ct); // có thể khác logic tên nếu bạn muốn



//    public async Task<List<ImageItemDTO>> ListImagesAsync(
//    Guid orderId, AttachmentSlot? slot, CancellationToken ct = default)
//    {
//        var q = _unitOfWork.AttachmentRepository.Query()
//            .AsNoTracking()
//            .Where(a => a.MerchandiseOrderId == orderId);

//        if (slot.HasValue) q = q.Where(a => a.Slot == slot.Value);

//        // chỉ lấy ảnh và pdf (ưu tiên FileType nếu có, fallback theo đuôi)
//        q = q.Where(a => a.FileName != null && (
//            EF.Functions.ILike(a.FileName, "%.png") ||
//            EF.Functions.ILike(a.FileName, "%.jpg") ||
//            EF.Functions.ILike(a.FileName, "%.jpeg") ||
//            EF.Functions.ILike(a.FileName, "%.webp") ||
//            EF.Functions.ILike(a.FileName, "%.pdf")  // thêm PDF
//        ));
//        var rows = await q
//            .OrderByDescending(a => a.CreateDate)
//            .Select(a => new {
//                a.AttachmentId,
//                a.FileName,
//                a.Slot,
//                a.SizeBytes,
//                a.CreateDate
//            })
//            .ToListAsync(ct);

//        static string GuessContentType(string name, string? ft) =>
//            !string.IsNullOrWhiteSpace(ft) ? ft :
//            name.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ? "image/png" :
//            name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ? "image/jpeg" :
//            name.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ? "image/jpeg" :
//            name.EndsWith(".webp", StringComparison.OrdinalIgnoreCase) ? "image/webp" :
//            name.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) ? "application/pdf" :

//            "application/octet-stream";

//        return rows.Select(a => new ImageItemDTO(
//            a.AttachmentId,
//            a.FileName,
//            a.Slot,
//            a.SizeBytes,
//            a.CreateDate,
//            $"/api/orders/{orderId}/attachments/api/attachments/{a.AttachmentId}/content",
//            $"/api/orders/{orderId}/attachments/api/attachments/{a.AttachmentId}/download"
//        )).ToList();
//    }

//}

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Attachments.DTOs;
using VietausWebAPI.Core.Application.Features.Attachments.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper.FileStorage;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Enums.Attachment;
using VietausWebAPI.Core.Domain.Security.Rules.Attachment;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Attachments.Services
{
    public class AttachmentSchemaService : IAttachmentSchemaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileShareStorage _fileShareStorage; 
        public AttachmentSchemaService(IUnitOfWork unitOfWork, IFileShareStorage fileShareStorage)
        {
            _unitOfWork = unitOfWork;
            _fileShareStorage = fileShareStorage;
        }

        /// <summary>
        /// Xóa mềm một tệp đính kèm
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task DeleteAsync(Guid attachmentId, CancellationToken ct)
        {
            // Soft-delete: cần tracking để update
            var att = await _unitOfWork.AttachmentModelRepository.Query(track: true)
                      .FirstOrDefaultAsync(a => a.AttachmentId == attachmentId, ct)
                      ?? throw new KeyNotFoundException("Attachment not found.");

            if (!att.IsActive) return; // đã inactive rồi thì thôi
            att.IsActive = false;

            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Lấy nội dung tệp đính kèm
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<StreamResult> GetContentAsync(Guid attachmentId, CancellationToken ct)
        {
            var att = await _unitOfWork.AttachmentModelRepository.Query()
                      .FirstOrDefaultAsync(a => a.AttachmentId == attachmentId && a.IsActive, ct)
                      ?? throw new KeyNotFoundException("Attachment not found.");

            var (stream, contentType, storedName) = await _fileShareStorage.OpenReadAsync(att.StoragePath, ct);
            var displayName = string.IsNullOrWhiteSpace(att.FileName) ? storedName : att.FileName;

            return new StreamResult(stream, contentType, displayName);
        }

        /// <summary>
        /// Danh sách tệp đính kèm trong một collection
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="slot"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<AttachmentDTO>> ListAsync(Guid collectionId, AttachmentSlot? slot, CancellationToken ct)
        {
            var query = _unitOfWork.AttachmentModelRepository
                .Query(track: false)
                .Where(a => a.AttachmentCollectionId == collectionId && a.IsActive);

            if (slot.HasValue) 
            {
                query = query.Where(a => a.Slot == slot.Value);
            }

            var data = await query.Select(a => new AttachmentDTO
            {
                AttachmentId = a.AttachmentId,
                AttachmentCollectionId = a.AttachmentCollectionId,
                Slot = a.Slot,
                FileName = a.FileName,
                SizeBytes = a.SizeBytes,
                StoragePath = a.StoragePath,
                IsActive = a.IsActive
            }).ToListAsync(ct);

            return data;
        }

        /// <summary>
        /// Tải lên tệp đính kèm vào một collection
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="slot"></param>
        /// <param name="file"></param>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<AttachmentDTO> UploadAsync(Guid collectionId, AttachmentSlot slot, IFormFile file, Guid userId, CancellationToken ct)
        {
            // 0) đảm bảo collection tồn tại
            var colExists = await _unitOfWork.AttachmentCollectionRepository.Query()
                              .AnyAsync(c => c.AttachmentCollectionId == collectionId, ct);
            if (!colExists) throw new KeyNotFoundException("Attachment collection not found.");

            // 1) Validate rule
            if (!AttachmentRules.Map.TryGetValue(slot, out var rule))
                throw new InvalidOperationException($"No rule configured for slot '{slot}'.");

            if (file.Length <= 0) throw new InvalidOperationException("Tệp rỗng.");
            if (file.Length > rule.MaxBytes) throw new InvalidOperationException("Tệp vượt quá kích thước cho phép.");

            var contentType = file.ContentType ?? "application/octet-stream";
            var mimeOk = rule.AllowedMimePrefixes.Any(p => contentType.StartsWith(p, StringComparison.OrdinalIgnoreCase));
            if (!mimeOk) throw new InvalidOperationException($"Định dạng '{contentType}' không được phép cho slot {slot}.");

            // 2) Slot single → hạ IsActive các bản hiện hành TRƯỚC (để tránh unique-index conflict)
            if (!rule.AllowMultiple)
            {
                var currents = await _unitOfWork.AttachmentModelRepository.Query(track: true)
                    .Where(a => a.AttachmentCollectionId == collectionId && a.Slot == slot && a.IsActive)
                    .ToListAsync(ct);

                if (currents.Count > 0)
                {
                    foreach (var a in currents) a.IsActive = false;
                    await _unitOfWork.SaveChangesAsync(); // commit hạ cũ trước
                }
            }

            // 3) Lưu file vào storage
            var relativeFolder = $"collections/{collectionId}/{slot}";
            await using var s = file.OpenReadStream();
            var safeName = Sanitize(file.FileName);
            var relativePath = await _fileShareStorage.SaveAsync(s, contentType, safeName, relativeFolder, ct);

            // 4) Ghi DB (đúng entity: AttachmentModel)
            var entity = new AttachmentModel
            {
                AttachmentId = Guid.CreateVersion7(),
                AttachmentCollectionId = collectionId,
                Slot = slot,
                FileName = file.FileName,
                SizeBytes = file.Length,
                StoragePath = relativePath,
                IsActive = true,
                CreateDate = DateTime.Now, // bổ sung audit thời gian
                CreateBy = userId,            // nếu muốn theo dõi người upload
                                              // ContentHash = await ComputeSHA256Async(file, ct), // nếu cần chống trùng
            };

            await _unitOfWork.AttachmentModelRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync();

            return new AttachmentDTO
            {
                AttachmentId = entity.AttachmentId,
                AttachmentCollectionId = entity.AttachmentCollectionId,
                Slot = entity.Slot,
                FileName = entity.FileName,
                SizeBytes = entity.SizeBytes,
                StoragePath = entity.StoragePath,
                IsActive = entity.IsActive
            };
        }

        /// <summary>
        /// Xóa cứng một tệp đính kèm
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task HardDeleteAsync(Guid attachmentId, CancellationToken ct)
        {
            // 1) Lấy entity có tracking để xóa
            var att = await _unitOfWork.AttachmentModelRepository.Query(track: true)
                      .FirstOrDefaultAsync(a => a.AttachmentId == attachmentId, ct)
                      ?? throw new KeyNotFoundException("Attachment not found.");

            // 2) Giữ lại đường dẫn file để xóa sau khi DB commit
            var path = att.StoragePath;

            // 3) Xóa record khỏi DB
            _unitOfWork.AttachmentModelRepository.Remove(att);
            await _unitOfWork.SaveChangesAsync(); // commit DB trước (tránh “mồ côi” record)

            // 4) Xóa file vật lý (nếu lỗi thì log lại để dọn rác sau)
            try
            {
                await _fileShareStorage.DeleteAsync(path, ct);
            }
            catch (Exception)
            {
                // TODO: inject ILogger và log warning, hoặc enqueue job dọn rác.
                // Không throw lại để API hard-delete vẫn coi như thành công ở DB.
            }
        }

        public async Task<List<AttachmentDTO>> UploadListAsync(
            Guid collectionId,
            AttachmentSlot slot,
            List<IFormFile> files,
            Guid userId,
            CancellationToken ct)
        {
            // 0) Collection tồn tại?
            var colExists = await _unitOfWork.AttachmentCollectionRepository.Query()
                .AnyAsync(c => c.AttachmentCollectionId == collectionId, ct);
            if (!colExists) throw new KeyNotFoundException("Attachment collection not found.");

            // 1) Rule & validate
            if (!AttachmentRules.Map.TryGetValue(slot, out var rule))
                throw new InvalidOperationException($"No rule configured for slot '{slot}'.");

            if (files == null || files.Count == 0)
                throw new InvalidOperationException("Không có tệp nào được upload.");

            if (!rule.AllowMultiple && files.Count > 1)
                throw new InvalidOperationException($"Slot '{slot}' chỉ cho phép 1 tệp.");

            foreach (var f in files)
            {
                if (f.Length <= 0) throw new InvalidOperationException($"Tệp '{f.FileName}' rỗng.");
                if (f.Length > rule.MaxBytes) throw new InvalidOperationException(
                    $"Tệp '{f.FileName}' vượt quá kích thước cho phép.");

                var contentType = f.ContentType ?? "application/octet-stream";
                var ok = rule.AllowedMimePrefixes.Any(p =>
                    contentType.StartsWith(p, StringComparison.OrdinalIgnoreCase));
                if (!ok) throw new InvalidOperationException(
                    $"Định dạng '{contentType}' của tệp '{f.FileName}' không được phép cho slot {slot}.");
            }

            // 2) Lưu files vào storage trước, gom entity
            var relativeFolder = $"collections/{collectionId}/{slot}";
            var savedPaths = new List<string>(files.Count);
            var entities = new List<AttachmentModel>(files.Count);

            try
            {
                foreach (var f in files)
                {
                    await using var s = f.OpenReadStream();
                    var safeName = Sanitize(f.FileName);
                    var contentType = f.ContentType ?? "application/octet-stream";

                    var relativePath = await _fileShareStorage
                        .SaveAsync(s, contentType, safeName, relativeFolder, ct);

                    savedPaths.Add(relativePath);

                    entities.Add(new AttachmentModel
                    {
                        AttachmentId = Guid.CreateVersion7(),
                        AttachmentCollectionId = collectionId,
                        Slot = slot,
                        FileName = f.FileName,
                        SizeBytes = f.Length,
                        StoragePath = relativePath,
                        IsActive = true,
                        CreateDate = DateTime.Now,
                        CreateBy = userId,
                        // ContentHash = await ComputeSHA256Async(f, ct),
                    });
                }

                // 3) Transaction: hạ cũ (nếu cần) + thêm mới + commit
                // Nếu UnitOfWork có API transaction riêng thì dùng nó; nếu không, dùng DbContext.Database
                await using var tx = await _unitOfWork.BeginTransactionAsync(); // hoặc DbContext.Database.BeginTransactionAsync

                if (!rule.AllowMultiple)
                {
                    var currents = await _unitOfWork.AttachmentModelRepository.Query(track: true)
                        .Where(a => a.AttachmentCollectionId == collectionId && a.Slot == slot && a.IsActive)
                        .ToListAsync(ct);

                    if (currents.Count > 0)
                    {
                        foreach (var a in currents) a.IsActive = false;
                        await _unitOfWork.SaveChangesAsync(); // hạ cũ trước để tránh unique-index conflict
                    }
                }

                await _unitOfWork.AttachmentModelRepository.AddRangeAsync(entities, ct);
                await _unitOfWork.SaveChangesAsync();

                await tx.CommitAsync(ct);
            }
            catch
            {
                // Dọn rác storage nếu DB fail
                foreach (var path in savedPaths)
                {
                    try { await _fileShareStorage.DeleteAsync(path, ct); } catch { /* ignore */ }
                }
                throw;
            }

            // 4) Map DTO
            return entities.Select(e => new AttachmentDTO
            {
                AttachmentId = e.AttachmentId,
                AttachmentCollectionId = e.AttachmentCollectionId,
                Slot = e.Slot,
                FileName = e.FileName,
                SizeBytes = e.SizeBytes,
                StoragePath = e.StoragePath,
                IsActive = e.IsActive
            }).ToList();
        }

        /// <summary>
        /// Chuẩn hóa tên tệp để tránh ký tự không hợp lệ
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string Sanitize(string fileName)
        {
            var name = Path.GetFileName(fileName);
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }

    }
}

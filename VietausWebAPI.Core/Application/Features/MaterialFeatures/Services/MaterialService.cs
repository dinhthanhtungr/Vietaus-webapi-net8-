using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Material;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaterialService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Tạo mới vật tư theo nghiệp vụ:
        /// 1) Nếu thiếu ExternalId -> sinh theo NVL_<CategoryExternalId>_<NNN>.
        /// 2) Map trường scalar từ DTO -> entity (nav sẽ xử lý thủ công).
        /// 3) Gắn quan hệ Materials_Suppliers (đảm bảo duy nhất 1 IsPreferred).
        /// 4) (Tuỳ chọn) Seed 1 bản PriceHistory active đầu tiên + đồng bộ CurrentPrice ở bảng nối.
        /// Tất cả chạy trong transaction. Trả OperationResult.
        /// </summary>
        /// <param name="material"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddNewMaterialAsync(PostMaterial material, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {

                // 1) Generate ExternalId nếu thiếu (NVL_<catExt>_xyz)
                if (string.IsNullOrWhiteSpace(material.ExternalId))
                {
                    var categoryExternalID = await _unitOfWork.CategoryRepository.Query()
                        .Where(m => m.CategoryId == material.CategoryId)
                        .Select(m => m.ExternalId)
                        .FirstOrDefaultAsync();


                    material.ExternalId = await ExternalIdGenerator.GenerateCode(
                        "NVL",
                        categoryExternalID,
                        prefix => _unitOfWork.MaterialRepository.GetLatestExternalIdStartsWithAsync(prefix)
                    );
                }

                // 2) Map scalar -> Material
                var materialEntity = _mapper.Map<Material>(material);


                // 3) Gắn Suppliers (bảng nối)
                if (material.Suppliers?.Any() == true)
                {
                    // đảm bảo chỉ 1 preferred
                    var preferredCount = material.Suppliers.Count(s => s.IsPreferred.GetValueOrDefault());
                    if (preferredCount > 1) return OperationResult.Fail("Chỉ được chọn 1 nhà cung cấp ưu tiên.");

                    var now = DateTime.Now;
                    // Build bảng nối (distinct theo SupplierId để tránh trùng)
                    materialEntity.MaterialsSuppliers = material.Suppliers
                        .DistinctBy(s => s.SupplierId)
                        .Select(svm => {
                            var ms = _mapper.Map<MaterialsSupplier>(svm);
                            ms.Currency = string.IsNullOrWhiteSpace(ms.Currency) ? "VND" : ms.Currency;
                            ms.CreatedBy = material.CreatedBy;
                            ms.CreateDate = now;
                            ms.UpdatedBy = material.UpdatedBy;
                            ms.UpdatedDate = now;
                            return ms;
                        })
                        .ToList();

                    // 4) Seed PriceHistory CHO TỪNG NCC (1 active / NCC)
                    foreach (var ms in materialEntity.MaterialsSuppliers)
                    {
                        materialEntity.PriceHistories.Add(new PriceHistory
                        {
                            PriceHistoryId = Guid.NewGuid(),
                            SupplierId = ms.SupplierId,
                            // MaterialId sẽ được EF set khi SaveChanges vì thêm qua navigation
                            OldPrice = null,          // lần đầu chưa có giá cũ
                            NewPrice = ms.CurrentPrice!.Value,
                            Currency = ms.Currency ?? "VND",
                            CreateDate = now,
                            CreatedBy = material.CreatedBy,
                            EndDate = null,
                            IsActive = true
                        });
                    }
                }

                await _unitOfWork.MaterialRepository.AddAsync(materialEntity, ct);

                var affected = await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
                return affected > 0
                    ? OperationResult.Ok("Thêm vật tư mới thành công")
                    : OperationResult.Fail("Thất bại");
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi thêm vật tư mới: {ex.Message}");
            }
        }



        public async Task<PagedResult<GetMaterialSummary>> GetAllAsync(MaterialQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                // Base Query
                IQueryable<Material> result = _unitOfWork.MaterialRepository.Query();

                // Filter
                if (query.MaterialId.HasValue)
                    result = result.Where(x => x.MaterialId == query.MaterialId.Value);

                if (query.CategoryId.HasValue)
                    result = result.Where(x => x.Category.CategoryId == query.CategoryId.Value);

                if (query.From.HasValue)
                    result = result.Where(x => x.CreatedDate >= query.From.Value);

                if (query.To.HasValue)
                    result = result.Where(x => x.CreatedDate <= query.To.Value);

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    // Tìm theo tên/mã NV hoặc tên/mã khách trong batch
                    result = result.Where(x =>
                            x.ExternalId.Contains(query.Keyword)

                    );
                }

                if (!string.IsNullOrWhiteSpace(query.Category))
                {
                    // Tìm loại vật tư
                    result = result.Where(x =>
                            x.Category.ExternalId.Contains(query.Category)

                    );
                }

                result = result.OrderByDescending(x => x.CreatedDate);


                int total = await result.CountAsync(ct);

                var items = await result
                    .Where(c => c.IsActive == true)
                    .ProjectTo<GetMaterialSummary>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return new PagedResult<GetMaterialSummary>(items, total, query.PageNumber, query.PageSize);
            }

            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhà cung cấp: {ex.Message}");
            }
        }

        public async Task<GetMaterial> GetMaterialByIdAsync(Guid Id, CancellationToken ct = default)
        {
            try
            {
                IQueryable<Material> result = _unitOfWork.MaterialRepository.Query();

                
                    result = result.Where(x => x.MaterialId == Id);

                var material = await result
                    .Where(c => c.IsActive == true)
                    .ProjectTo<GetMaterial>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();



                if (material == null)
                    return null;

                material.materialSuppliers = _unitOfWork.MaterialsSupplierRepository.Query()
                    .Where(x => x.MaterialId == Id && x.IsActive == true)
                    .ProjectTo<GetMaterialSupplier>(_mapper.ConfigurationProvider)
                    .ToList();

                return material;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhà cung cấp: {ex.Message}");
            }

        }

        public async Task<PagedResult<GetMaterialSupplier>> GetMaterialSupplierAsync(MaterialQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                // Base Query
                IQueryable<MaterialsSupplier> result = _unitOfWork.MaterialsSupplierRepository.Query();

                result = result.OrderByDescending(x => x.CreateDate);

                // Filter
                if (query.MaterialId.HasValue)
                    result = result.Where(x => x.MaterialId == query.MaterialId);

                int total = await result.CountAsync(ct);

                var items = await result
                    .Where(c => c.IsActive == true)
                    .ProjectTo<GetMaterialSupplier>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return new PagedResult<GetMaterialSupplier>(items, total, query.PageNumber, query.PageSize);
            }

            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhà cung cấp: {ex.Message}");
            }
        }

        public async Task<OperationResult> UpsertMaterialAsync(GetMaterial req, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var now = DateTime.Now;

                var mat = await _unitOfWork.MaterialRepository.Query(track: true)
                    .Include(m => m.MaterialsSuppliers)
                    .FirstOrDefaultAsync(m => m.MaterialId == req.MaterialId, ct);
                if (mat == null) return OperationResult.Fail("Không tìm thấy vật tư");
                // Replace this line:
                // if (req.materialSuppliers.Where(x => x.CurrentPrice == null))

                // Kiểm tra nếu có nhà cung cấp nào mà CurrentPrice là null
                if (req.materialSuppliers.Any(x => x.CurrentPrice == null)) return OperationResult.Fail("Không được để chống giá");


                // 1) Cập nhật các field của Material - chỉ khi có gửi và khác giá trị cũ
                void SetIf<T>(T? incoming, Func<T> current, Action<T> apply) where T : struct
                { if (incoming.HasValue && !EqualityComparer<T>.Default.Equals(incoming.Value, current())) apply(incoming.Value); }

                void SetIfRef(string? incoming, Func<string?> current, Action<string?> apply)
                { if (incoming != null && incoming != current()) apply(incoming); }

                SetIfRef(req.ExternalId, () => mat.ExternalId, v => mat.ExternalId = v);
                SetIfRef(req.CustomCode, () => mat.CustomCode, v => mat.CustomCode = v);
                SetIfRef(req.Name, () => mat.Name, v => mat.Name = v);
                SetIf(req.CategoryId, () => mat.CategoryId, v => mat.CategoryId = v);
                SetIf(req.Weight.GetValueOrDefault(), () => mat.Weight.GetValueOrDefault(), v => mat.Weight = v);
                SetIfRef(req.Unit, () => mat.Unit, v => mat.Unit = v);
                SetIfRef(req.Package, () => mat.Package, v => mat.Package = v);
                SetIfRef(req.Comment, () => mat.Comment, v => mat.Comment = v);
                SetIf(req.MinQuantity.GetValueOrDefault(), () => mat.MinQuantity.GetValueOrDefault(), v => mat.MinQuantity = v);
                SetIf(req.IsActive.GetValueOrDefault(), () => mat.IsActive.GetValueOrDefault(), v => mat.IsActive = v);
                mat.UpdatedDate = now; mat.UpdatedBy = req.UpdatedBy;

                // 2) Upsert Suppliers
                var existing = mat.MaterialsSuppliers.ToDictionary(x => x.SupplierId, x => x);

                foreach (var s in req.materialSuppliers)
                {
                    if (!existing.TryGetValue(s.SupplierId, out var link))
                    {
                        // NEW link
                        link = new MaterialsSupplier
                        {
                            MaterialsSuppliersId = Guid.NewGuid(),
                            MaterialId = mat.MaterialId,
                            SupplierId = s.SupplierId,
                            MinDeliveryDays = s.MinDeliveryDays ?? 0,
                            CurrentPrice = s.CurrentPrice ?? 0m,
                            Currency = string.IsNullOrWhiteSpace(s.Currency) ? "VND" : s.Currency!,
                            IsPreferred = s.IsPreferred ?? false,
                            IsActive = s.isActive ?? true,
                            CreateDate = now,
                            CreatedBy = req.UpdatedBy
                        };
                        await _unitOfWork.MaterialsSupplierRepository.AddAsync(link, ct);



                        // Price history mở mới nếu có giá
                        if (s.CurrentPrice.HasValue)
                        {
                            await _unitOfWork.PriceHistorieRepository.AddAsync(new PriceHistory
                            {
                                PriceHistoryId = Guid.NewGuid(),
                                MaterialId = mat.MaterialId,
                                SupplierId = s.SupplierId,
                                OldPrice = null,
                                NewPrice = s.CurrentPrice.Value,
                                Currency = link.Currency,
                                CreateDate = now,
                                CreatedBy = req.UpdatedBy,
                                IsActive = true
                            }, ct);


                            // Cập nhật giá trong Materials_Supplier
                            link.CurrentPrice = s.CurrentPrice.Value;
                        }
                    }
                    else
                    {
                        // UPDATE link theo field có gửi
                        if (s.MinDeliveryDays.HasValue && s.MinDeliveryDays.Value != link.MinDeliveryDays)
                            link.MinDeliveryDays = s.MinDeliveryDays.Value;

                        if (!string.IsNullOrWhiteSpace(s.Currency) && s.Currency != link.Currency)
                            link.Currency = s.Currency!;

                        if (s.isActive.HasValue && s.isActive.Value != link.IsActive)
                            link.IsActive = s.isActive.Value;

                        // Preferred: nếu set true -> các active khác phải false
                        if (s.IsPreferred.HasValue)
                            link.IsPreferred = s.IsPreferred.Value;

                        // PRICE: nếu có gửi và khác giá hiện tại -> đóng bản cũ + tạo bản mới
                        if (s.CurrentPrice.HasValue && s.CurrentPrice.Value != link.CurrentPrice)
                        {

                            Console.WriteLine($"Updating Price: {link.CurrentPrice} -> {s.CurrentPrice.Value}");
                            // Tắt các bản ghi PriceHistory cũ trước khi tạo bản ghi mới
                            // Tắt các bản ghi PriceHistory cũ trước khi tạo bản ghi mới
                            var actives = await _unitOfWork.PriceHistorieRepository.Query(track: true)
                                .Where(p => p.MaterialId == mat.MaterialId && p.SupplierId == s.SupplierId && p.IsActive == true)
                                .ToListAsync(ct);

                            // Đóng các bản ghi cũ (set IsActive = false)
                            foreach (var h in actives)
                            {
                                h.IsActive = false;
                                // Nếu có cột EndDate, bạn có thể set EndDate = DateTime.UtcNow.
                                h.EndDate = now; // Nếu có cột EndDate
                            }

                            await _unitOfWork.PriceHistorieRepository.AddAsync(new PriceHistory
                            {
                                PriceHistoryId = Guid.NewGuid(),
                                MaterialId = mat.MaterialId,
                                SupplierId = s.SupplierId,
                                OldPrice = link.CurrentPrice,
                                NewPrice = s.CurrentPrice.Value,
                                Currency = string.IsNullOrWhiteSpace(s.Currency) ? link.Currency : s.Currency!,
                                CreateDate = now,
                                CreatedBy = req.UpdatedBy,
                                IsActive = true
                            }, ct);

                            link.CurrentPrice = s.CurrentPrice.Value;
                        }

                        link.UpdatedDate = now; link.UpdatedBy = req.UpdatedBy;
                    }
                }

                // 3) Nếu cần thay thế toàn bộ danh sách nhà cung cấp
                //if (req.ReplaceEntireSupplierList)
                //{
                //    var incomingIds = req.Suppliers.Select(x => x.SupplierId).ToHashSet();
                //    foreach (var old in existing.Values.Where(x => !incomingIds.Contains(x.SupplierId)))
                //    {
                //        if (old.IsActive) { old.IsActive = false; old.UpdatedDate = now; old.UpdatedBy = userId; }
                //    }
                //}

                // 4) Chốt “chỉ 1 Preferred đang Active”
                var winner = req.materialSuppliers.FirstOrDefault(x => x.IsPreferred == true && (x.isActive ?? true))?.SupplierId;
                if (winner.HasValue)
                {
                    var all = await _unitOfWork.MaterialsSupplierRepository.Query(track: true)
                        .Where(x => x.MaterialId == mat.MaterialId && x.IsActive == true)
                        .ToListAsync(ct);
                    foreach (var x in all) x.IsPreferred = (x.SupplierId == winner.Value);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail(ex.Message);
            }
        }

        public async Task<OperationResult> DeleteMaterialAsync(Guid Id, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _unitOfWork.MaterialRepository.DeleteMaterialAsync(Id, ct);  
                var affected = await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
                return affected > 0
                    ? OperationResult.Ok("Xoá vật tư thành công")
                    : OperationResult.Fail("Thất bại");
            }

            catch(Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi xoá vật tư: {ex.Message}");   
            }
        }

    }
}

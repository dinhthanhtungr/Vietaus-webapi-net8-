using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material;
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
                    var preferredCount = material.Suppliers.Count(s => s.IsPreferred);
                    if (preferredCount > 1) return OperationResult.Fail("Chỉ được chọn 1 nhà cung cấp ưu tiên.");

                    materialEntity.MaterialsSuppliers = material.Suppliers
                        .DistinctBy(s => s.SupplierId)
                        .Select(svm => {
                            var ms = _mapper.Map<MaterialsSupplier>(svm);
                            // LastPrice = CurrentPrice lúc tạo, nếu có
                            ms.LastPrice = svm.CurrentPrice;
                            return ms;
                        })
                        .ToList();
                }

                // 4) Seed PriceHistory (tuỳ chọn)
                if (material.InitialPrice != null)
                {
                    var ph = _mapper.Map<PriceHistory>(material.InitialPrice);
                    materialEntity.PriceHistories.Add(ph);

                    // Nếu có MaterialsSupplier trùng supplier → đồng bộ CurrentPrice
                    var ms = materialEntity.MaterialsSuppliers.FirstOrDefault(x => x.SupplierId == material.InitialPrice.SupplierId);
                    if (ms != null)
                    {
                        ms.CurrentPrice = material.InitialPrice.NewPrice;
                        ms.Currency = material.InitialPrice.Currency;
                        ms.UpdatedDate = DateTime.Now;
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

        public Task<PagedResult<GetMaterialSummary>> GetAllAsync(MaterialQuery query, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }



        //public async Task SetActivePriceAsync(Guid materialId, Guid supplierId, decimal newPrice, string currency, CancellationToken ct)
        //{
        //    await _uow.BeginTransactionAsync(ct);
        //    try
        //    {
        //        // 1) Tắt bản ghi đang active (nếu có)
        //        var now = DateTime.UtcNow;

        //        await _db.PriceHistories
        //            .Where(p => p.MaterialId == materialId && p.SupplierId == supplierId && p.IsActive)
        //            .ExecuteUpdateAsync(s => s
        //                .SetProperty(p => p.IsActive, false)
        //                .SetProperty(p => p.EndDate, now)
        //                .SetProperty(p => p.UpdatedDate, now), ct);

        //        // 2) Lấy giá cũ (nếu cần ghi vào OldPrice)
        //        var last = await _db.PriceHistories
        //            .Where(p => p.MaterialId == materialId && p.SupplierId == supplierId)
        //            .OrderByDescending(p => p.CreateDate)
        //            .FirstOrDefaultAsync(ct);

        //        // 3) Thêm bản ghi mới (active)
        //        _db.PriceHistories.Add(new PriceHistory
        //        {
        //            PriceHistoryId = Guid.NewGuid(),
        //            MaterialId = materialId,
        //            SupplierId = supplierId,
        //            OldPrice = last?.NewPrice,
        //            NewPrice = newPrice,
        //            Currency = currency,
        //            CreateDate = now,
        //            EndDate = null,
        //            IsActive = true
        //        });

        //        // (tuỳ chọn) đồng bộ bảng nối
        //        var ms = await _db.MaterialsSuppliers
        //            .FirstOrDefaultAsync(x => x.MaterialId == materialId && x.SupplierId == supplierId, ct);
        //        if (ms != null)
        //        {
        //            ms.LastPrice = last?.NewPrice;
        //            ms.CurrentPrice = newPrice;
        //            ms.Currency = currency;
        //            ms.UpdatedDate = now;
        //        }

        //        await _db.SaveChangesAsync(ct);
        //        await _uow.CommitTransactionAsync(ct);
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        await _uow.RollbackTransactionAsync(ct);
        //        // nếu hiếm khi race-condition, có thể retry 1 lần:
        //        // if (ex.InnerException is PostgresException pg && pg.SqlState == "23505") { ... retry ... }
        //        throw;
        //    }
        //}
    }
}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Enums;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Services
{
    public class MfgProductionOrderService : IMfgProductionOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MfgProductionOrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OperationResult> CreateAsync(PostMfgProductionOrder req, CancellationToken ct = default)
        {

            throw new NotImplementedException();
        }
        public async Task<PagedResult<GetSummaryMfgProductionOrder>> GetAllAsync(MfgProductionOrderQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var result = _unitOfWork.MfgProductionOrderRepository.Query();

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim().ToLower();
                    result = result.Where(po =>
                        po.ExternalId.ToLower().Contains(keyword)
                    );

                }

                if (query.CompanyId.HasValue && query.CompanyId.Value != Guid.Empty)
                {
                    result = result.Where(p => p.CompanyId == query.CompanyId.Value);
                }

                if (query.MfgProductionOrderId.HasValue && query.MfgProductionOrderId.Value != Guid.Empty)
                {
                    result = result.Where(p => p.MfgProductionOrderId == query.MfgProductionOrderId.Value);
                }

                int totalCount = await result.CountAsync(ct);

                var items = await result
                    .Where(f => f.IsActive == true)
                    .OrderByDescending(c => c.CreateDate) // "F1" -> "F0000000001"
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ProjectTo<GetSummaryMfgProductionOrder>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return new PagedResult<GetSummaryMfgProductionOrder>(items, totalCount, query.PageNumber, query.PageSize);
            }

            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách: {ex.Message}", ex);
            }
        }

        public async Task<PagedResult<GetSampleMfgFormula>> GetAllMfgFormulaAsync(MfgProductionOrderQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var q = _unitOfWork.ManufacturingFormulaRepository.Query();
                var mpoQ = _unitOfWork.MfgProductionOrderRepository.Query();

                if (query.MfgProductionOrderId.HasValue && query.MfgProductionOrderId.Value != Guid.Empty)
                {
                    q = q.Where(f => f.mfgProductionOrderId == query.MfgProductionOrderId.Value);
                }

                if (query.MfgFormulaId.HasValue && query.MfgFormulaId.Value != Guid.Empty)
                {
                    q = q.Where(f => f.ManufacturingFormulaId == query.MfgFormulaId.Value);
                }

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim();
                    // Tìm theo tên/mã NV hoặc tên/mã khách trong batch
                    q = q.Where(x =>
                        (x.MfgProductionOrder.Product.ColourCode ?? "").Contains(keyword) ||
                        (x.MfgProductionOrder.Product.Name ?? "").Contains(keyword)
                    );
                }
                var totalCount = await q.CountAsync(ct);
                var items = q
                    // Đừng sort trước; để sau khi tính được cờ isStandard của *bản ghi hiện tại*
                    .Select(f => new
                    {
                        f,
                        std = q
                            .Join(mpoQ,
                                  fs => fs.mfgProductionOrderId,
                                  o => o.MfgProductionOrderId,
                                  (fs, o) => new { fs, o })
                            .Where(x => x.fs.IsActive == true
                                        && x.fs.IsStandard == true
                                        && x.fs.VUFormulaId == f.VUFormulaId
                                        && x.o.ProductId == f.MfgProductionOrder.ProductId)
                            // IsStandard đã lọc = true, nên 2 dòng OrderBy dưới là thừa; giữ createdDate nếu muốn
                            .OrderByDescending(x => x.fs.createdDate)
                            .Select(x => new { x.fs.ManufacturingFormulaId, x.fs.ExternalId })
                            .FirstOrDefault()
                    })
                    // TÍNH cờ isStandard cho *bản ghi hiện tại*
                    .Select(x => new
                    {
                        x.f,
                        x.std,
                        isStandard = x.std != null && x.std.ManufacturingFormulaId == x.f.ManufacturingFormulaId
                    })
                    // >>> SORT: chuẩn trước, rồi bản được chọn, rồi theo ngày
                    .OrderByDescending(x => x.isStandard)
                    .ThenByDescending(x => x.f.IsSelect == true)
                    .ThenBy(x => x.f.createdDate)
                    .Select(x => new GetSampleMfgFormula
                    {
                        ManufacturingFormulaId = x.f.ManufacturingFormulaId,
                        MfgProductionOrderExternalId = x.f.MfgProductionOrderExternalId,
                        ExternalId = x.f.ExternalId,
                        Name = x.f.Name,
                        VUFormulaId = x.f.VUFormulaId,
                        FormulaExternalIdSnapshot = x.f.FormulaExternalIdSnapshot,

                        MfgFormulaId = x.std == null ? (Guid?)null : x.std.ManufacturingFormulaId,
                        MfgFormulaExternalIdSnapshot = x.std == null ? null : x.std.ExternalId,

                        Status = x.f.Status,
                        TotalPrice = x.f.TotalPrice,
                        isStandard = x.isStandard,                // <— dùng cờ đúng nghĩa
                        IsSelect = x.f.IsSelect,
                        CreatedDate = x.f.createdDate ?? DateTime.MinValue
                    });


                return new PagedResult<GetSampleMfgFormula>(items, totalCount, query.PageNumber, query.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách: {ex.Message}", ex);
            }
        }

        public async Task<GetMfgProductionOrder?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _unitOfWork.MfgProductionOrderRepository.Query()
                .Where(p => p.MfgProductionOrderId == id && p.IsActive == true)
                .ProjectTo<GetMfgProductionOrder>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<PagedResult<GetFormulaAndMfgFormula>> GetFormulaAndMfgFormulaAsync(FormulaAndMfgFormulaQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var q = _unitOfWork.FormulaRepository.Query();

                if (query.ProductId.HasValue && query.ProductId.Value != Guid.Empty)
                {
                    q = q.Where(f => f.ProductId == query.ProductId.Value);
                }

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim();
                    // Tìm theo tên/mã NV hoặc tên/mã khách trong batch
                    q = q.Where(x =>
                        (x.Name ?? "").Contains(keyword) ||
                        (x.Product.ColourCode ?? "").Contains(keyword) ||
                        (x.Product.Name ?? "").Contains(keyword)
                    );
                }

                var totalCount = await q.CountAsync(ct);

                var items = await q
                    .OrderByDescending(f => f.CreatedDate)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ProjectTo<GetFormulaAndMfgFormula>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return new PagedResult<GetFormulaAndMfgFormula>(items, totalCount, query.PageNumber, query.PageSize);

            }

            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách: {ex.Message}", ex);
            }
        }

        public async Task<OperationResult> UpdateInformationAsync(PatchMfgProductionOrderInformation req, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;

                var existing = await _unitOfWork.MfgProductionOrderRepository.Query(track: true)
                    .Where(p => p.MfgProductionOrderId == req.MfgProductionOrderId && p.IsActive == true)
                    .FirstOrDefaultAsync(ct);

                if (existing == null)
                    return OperationResult.Fail($"Không tìm thấy lệnh sản xuất với ID {req.MfgProductionOrderId}");

                // Chỉ được phép sửa những field sau:
                existing.UpdatedDate = now;

                existing.UpdatedBy = req.UpdatedBy;

                // Lưu thay đổi
                PatchHelper.SetIfRef(req.PlpuNote, () => existing.PlpuNote, v => existing.PlpuNote = v);
                PatchHelper.SetIfRef(req.LabNote, () => existing.LabNote, v => existing.LabNote = v);
                PatchHelper.SetIfRef(req.Requirement, () => existing.Requirement, v => existing.Requirement = v);

                PatchHelper.SetIfRef(req.Status, () => existing.Status, v => existing.Status = v);
                PatchHelper.SetIfRef(req.QcCheck, () => existing.QcCheck, v => existing.QcCheck = v);

                PatchHelper.SetIf(req.TotalQuantity, () => existing.TotalQuantity.GetValueOrDefault(), v => existing.TotalQuantity = v);
                PatchHelper.SetIf(req.NumOfBatches, () => existing.NumOfBatches.GetValueOrDefault(), v => existing.NumOfBatches = v);
                
                PatchHelper.SetIf(req.ExpectedDate, () => existing.ExpectedDate.GetValueOrDefault(), v => existing.ExpectedDate = v);
                PatchHelper.SetIf(req.requiredDate, () => existing.requiredDate.GetValueOrDefault(), v => existing.requiredDate = v);

                PatchHelper.SetIf(req.QualifiedQuantity, () => existing.QualifiedQuantity.GetValueOrDefault(), v => existing.QualifiedQuantity = v);
                PatchHelper.SetIf(req.RejectedQuantity, () => existing.RejectedQuantity.GetValueOrDefault(), v => existing.RejectedQuantity = v);
                PatchHelper.SetIf(req.WasteQuantity, () => existing.WasteQuantity.GetValueOrDefault(), v => existing.WasteQuantity = v);

                // 2) Materials của công thức
                var formulaExist = await _unitOfWork.ManufacturingFormulaRepository.Query(track: true)
                    .Include(f => f.ManufacturingFormulaMaterials) // << bắt buộc Include
                    .FirstOrDefaultAsync(f => f.ManufacturingFormulaId == req.ManufacturingFormulaId && f.IsActive == true, ct);

                if (formulaExist == null)
                    return OperationResult.Fail($"Không tìm thấy công thức sản xuất với ID {req.ManufacturingFormulaId}");

                // Đừng lọc IsActive ở đây để còn re-activate hoặc patch dòng cũ
                var existingMaterials = formulaExist.ManufacturingFormulaMaterials
                    .ToDictionary(fm => fm.MaterialId, fm => fm);

                foreach (var m in req.ManufacturingFormulaMaterials ?? Enumerable.Empty<PatchMfgFormulaMaterial>())
                {
                    if (!existingMaterials.TryGetValue(m.MaterialId, out var link) || link == null)
                    {
                        // Nếu chỉ muốn patch các dòng đã có, bỏ qua dòng không tồn tại:
                        //  -> continue;

                        // Nếu muốn tạo mới khi chưa có, dùng khối dưới:
                        link = new ManufacturingFormulaMaterial
                        {
                            ManufacturingFormulaMaterialId = Guid.NewGuid(),
                            ManufacturingFormulaId = formulaExist.ManufacturingFormulaId,
                            MaterialId = m.MaterialId,
                            CategoryId = m.CategoryId,
                            Quantity = m.Quantity.GetValueOrDefault(),
                            UnitPrice = m.UnitPrice.GetValueOrDefault(),
                            TotalPrice = decimal.Round((m.Quantity ?? 0m) * (m.UnitPrice ?? 0m), 2, MidpointRounding.AwayFromZero),
                            Unit = m.Unit,
                            MaterialNameSnapshot = m.MaterialNameSnapshot,
                            MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot,
                            IsActive = true
                        };
                        await _unitOfWork.ManufacturingFormulaMaterialRepository.AddAsync(link, ct);
                        existingMaterials[m.MaterialId] = link; // nhớ add vào dictionary
                    }

                    // Tới đây link chắc chắn != null
                    PatchHelper.SetIfNullable(m.StockId, () => link.StockId, v => link.StockId = v);
                    PatchHelper.SetIfRef(m.LotNo, () => link.LotNo, v => link.LotNo = v);

                    // Nếu dòng này từng bị tắt, bật lại
                    if (link.IsActive == false) link.IsActive = true;
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return OperationResult.Ok("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail(ex.Message);
            }
        }
    }
}

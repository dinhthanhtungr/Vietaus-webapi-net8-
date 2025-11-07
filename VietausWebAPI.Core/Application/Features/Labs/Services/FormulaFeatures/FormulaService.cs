using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Queries.FormulaFeature;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.FormulaFeatures;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Enums.Products;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Labs.Services.FormulaFeatures
{
    public class FormulaService : IFormulaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;

        public FormulaService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Tạo công thức mới cho sản phẩm (theo VU)
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<OperationResult> CreateAsync(PostFormula req, CancellationToken ct = default)
        {
            if (req.ProductId == Guid.Empty) throw new ArgumentException("ProductId cannot be empty", nameof(req.ProductId));
            if (req.CreatedBy == Guid.Empty) throw new ArgumentException("CreatedBy  cannot be empty", nameof(req.CreatedBy));
            if (req.materialFormulas is null || !req.materialFormulas.Any())
                throw new ArgumentException("Công thức phải có ít nhất 1 vật tư.", nameof(req.materialFormulas));

            int affected = 0;//
            await using var tx = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                Guid guid = req.ProductId;


                // Case1: Kiểm tra sản phẩm có tồn tại không
                var exists = await _unitOfWork.ProductRepository.Query()
                    .AnyAsync(p => p.ProductId == req.ProductId, ct);

                if (!exists) throw new ArgumentException("Product does not exist", nameof(req.ProductId));

                // Tạo mới công thức
                var formula = _mapper.Map<Formula>(req);

                // Làm sạch để tránh lỗi null
                formula.FormulaMaterials = (formula.FormulaMaterials ?? new List<FormulaMaterial>())
                    .Where(x => x != null
                             && x.MaterialId != Guid.Empty
                             && x.Quantity > 0
                             && x.UnitPrice >= 0)
                    .ToList();

                //    - Tính TotalPrice từng dòng (round 2 số thập phân, có thể đổi theo rule công ty)
                foreach (var i in formula.FormulaMaterials)
                {
                    i.TotalPrice = Math.Round(i.Quantity * i.UnitPrice, 2, MidpointRounding.AwayFromZero);
                }

                //    - Tổng cộng công thức
                formula.TotalPrice = formula.FormulaMaterials.Sum(i => i.TotalPrice);


                formula.ExternalId = await ExternalIdGenerator.GenerateExternalId(
                    "VU",
                    prefix => _unitOfWork.FormulaRepository.GetLatestExternalIdStartsWithAsync(prefix)
                );

                formula.Name = await ExternalIdGenerator.GenerateFormulaCode(
                    "F",
                    prefix => _unitOfWork.FormulaRepository.GetLatestExternalIdStartsWithAsync(prefix, guid)
                );

                formula.ProductId = req.ProductId;
                await _unitOfWork.FormulaRepository.AddAsync(formula, ct);
                affected = await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync(ct);
                return affected > 0
                    ? OperationResult.Ok("Tạo thành công")
                    : OperationResult.Fail("Tạo thất bại");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                return OperationResult.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách công thức
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PagedResult<GetFormula>> GetAllAsync(FormulaQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 10;

                var q = _unitOfWork.FormulaRepository.Query().AsNoTracking();

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim();
                    q = q.Where(x =>
                        (x.Name ?? "").Contains(keyword) ||
                        (x.Product.ColourCode ?? "").Contains(keyword) ||
                        (x.Product.Name ?? "").Contains(keyword)
                    );
                }

                if (query.CompanyId is Guid cid && cid != Guid.Empty)
                    q = q.Where(p => p.CompanyId == cid);

                if (query.FormulaId is Guid fid && fid != Guid.Empty)
                    q = q.Where(p => p.FormulaId == fid);

                if (query.ProductId is Guid pid && pid != Guid.Empty)
                    q = q.Where(p => p.ProductId == pid);

                // Đếm trước
                var totalCount = await q.CountAsync(ct);

                // Sort + Paging trên ENTITY
                q = q.Where(f => f.FormulaMaterials.Any(a => a.IsActive == true))
                     .OrderByDescending(c => c.Name.Substring(1).PadLeft(10, '0'))
                     .Skip((query.PageNumber - 1) * query.PageSize)
                     .Take(query.PageSize);

                // 👉 Projection tay để tính TotalPrice bằng giá gần nhất
                var ms = _unitOfWork.MaterialsSupplierRepository.Query();

                var items = await q
                    .Where(f => f.FormulaMaterials.Any(a => a.IsActive == true)) // vẫn giữ rule active
                    .OrderByDescending(c => c.Name.Substring(1).PadLeft(10, '0'))
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(f => new GetFormula
                    {
                        FormulaId = f.FormulaId,
                        ExternalId = f.ExternalId,
                        Name = f.Name,
                        ProductCode = f.Product.ColourCode,
                        Status = f.Status,
                        CreatedDate = f.CreatedDate,
                        IsSelect = f.IsSelect,

                        CheckDate = f.CheckDate,
                        CheckNameSnapshot = f.CheckByNavigation != null ? f.CheckByNavigation.FullName : null,
                        SentDate = f.SentDate,
                        SentByNameSnapshot = f.SentByNavigation != null ? f.SentByNavigation.FullName : null,

                        CreatedByName = f.CreatedByNavigation != null ? (f.CreatedByNavigation.FullName).Trim() : null,
                        
                        // ==== TÍNH TOTAL PRICE THEO RULE VA/VU ====
                        //TotalPrice =
                        //    // 1) Thử lấy theo VA: chọn VA IsStandard gần nhất, rồi SUM vật tư của VA
                        //    (
                        //        f.ManufacturingFormulaSources
                        //         .Where(mf => mf.IsActive == true && mf.IsStandard == true)
                        //         .OrderByDescending(mf => (DateTime?)(mf.UpdatedDate ))
                        //         .Select(mf => (decimal?)              // Ép về decimal? để dùng ??
                        //             mf.ManufacturingFormulaMaterials
                        //               .Where(mm => mm.IsActive == true)
                        //               .Select(mm =>
                        //                   (mm.Quantity) *
                        //                   (
                        //                       ms.Where(s => s.MaterialId == mm.MaterialId && (s.IsActive ?? true))
                        //                         .OrderByDescending(s => (DateTime?)(s.UpdatedDate ?? s.CreateDate))
                        //                         .ThenByDescending(s => (s.IsPreferred ?? false))
                        //                         .Select(s => (decimal?)s.CurrentPrice)
                        //                         .FirstOrDefault() ?? 0m
                        //                   )
                        //               ).Sum()
                        //         )
                        //         .FirstOrDefault()
                        //    )
                        //    // 2) Nếu không có VA chuẩn → fallback về VU (FormulaMaterials)
                        //    ??
                        //    (
                        //        (decimal?)
                        //        f.FormulaMaterials
                        //         .Where(m => m.IsActive == true)
                        //         .Select(m =>
                        //             (m.Quantity) *
                        //             (
                        //                 ms.Where(s => s.MaterialId == m.MaterialId && (s.IsActive ?? true))
                        //                   .OrderByDescending(s => (DateTime?)(s.UpdatedDate ?? s.CreateDate))
                        //                   .ThenByDescending(s => (s.IsPreferred ?? false))
                        //                   .Select(s => (decimal?)s.CurrentPrice)
                        //                   .FirstOrDefault() ?? 0m
                        //             )
                        //         ).Sum()
                        //    ),

                            materialFormulas = f.FormulaMaterials
                                .Where(m => m.IsActive == true)
                                .Select(m => new GetMaterialFormula
                                {
                                    MaterialId = m.MaterialId,
                                    CategoryId = m.CategoryId,
                                    Quantity = m.Quantity,
                                    UnitPrice = m.UnitPrice,
                                    TotalPrice = m.TotalPrice,
                                    Unit = m.Unit,
                                    MaterialNameSnapshot = m.MaterialNameSnapshot,
                                    MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot
                                }).ToList()
                    })
                    .ToListAsync(ct);


                return new PagedResult<GetFormula>(items, totalCount, query.PageNumber, query.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật dữ liệu công thức
        /// </summary>
        /// <param name="patch"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateInformationAsync(PatchFormulaInformation patch, CancellationToken? cancellationToken = null)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var now = DateTime.Now;
                var userId = _currentUser.EmployeeId;

                var formulaExist = await _unitOfWork.FormulaRepository.Query(track: true)
                    .FirstOrDefaultAsync(f => f.FormulaId == patch.FormulaId, cancellationToken ?? default);

                var sampleRequestExist = await _unitOfWork.SampleRequestRepository.Query(track: true)
                    .FirstOrDefaultAsync(s => s.SampleRequestId == patch.SampleRequestId, cancellationToken ?? default);

                if (formulaExist == null) return OperationResult.Fail("Công thức không tồn tại");

                PatchHelper.SetIfRef(patch.Status, () => formulaExist.Status, v => formulaExist.Status = v);

                if (patch.CheckBy.HasValue)
                {
                    formulaExist.CheckDate = now;
                    formulaExist.CheckBy = userId;
                }

                if (patch.SentBy.HasValue)
                {
                    formulaExist.SentDate = now;
                    formulaExist.SentBy = userId;

                    if (sampleRequestExist != null)
                    {
                        sampleRequestExist.RealDeliveryDate = now;
                        sampleRequestExist.RealPriceQuoteDate = now;
                    }
                }

                // Fix: Only access sampleRequestExist.SendBy if sampleRequestExist is not null
                if (sampleRequestExist != null && formulaExist != null)
                {
                    //var isChangeSendBy = PatchHelper.SetIf(patch.SentBy, () => sampleRequestExist.SendBy.GetValueOrDefault(), v => sampleRequestExist.SendBy = v);

                    if (sampleRequestExist.Status == SampleRequestStatus.InProgress.ToString())
                    {
                        sampleRequestExist.ResponseDeliveryDate = now;
                        sampleRequestExist.Status = SampleRequestStatus.SampleSent.ToString();
                        sampleRequestExist.SendBy = userId;
                    }
                }

                // Fix: Ensure formulaExist is not null before dereferencing
                if (formulaExist != null)
                {
                    formulaExist.UpdatedDate = now;
                    formulaExist.UpdatedBy = userId;
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

        /// <summary>
        /// Cập nhật dữ liệu về nghuyên vật liệu có ttrong công thức
        /// </summary>
        /// <param name="req"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpsertFormulaAsync(PatchFormula req, CancellationToken? cancellationToken = null)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var now = DateTime.Now;

                var formulaExist = await _unitOfWork.FormulaRepository.Query(track: true)
                    .Include(f => f.FormulaMaterials)
                    .FirstOrDefaultAsync(f => f.FormulaId == req.FormulaId, cancellationToken ?? default);

                if(formulaExist == null) return OperationResult.Fail("Công thức không tồn tại");

                // 1) Cập nhật thông tin công thức - chỉ khi có gửi giá trị cũ
                void SetIf<T>(T? incoming, Func<T> current, Action<T> apply) where T : struct
                {
                    if (incoming.HasValue && !EqualityComparer<T>.Default.Equals(incoming.Value, current()))
                    {
                        apply(incoming.Value);
                    }
                }

                void SetIfRef(string? incoming, Func<string?> current, Action<string?> apply)
                {
                    if (incoming != null && incoming != current())
                    {
                        apply(incoming);
                    }
                }

                // Patch các field đơn
                SetIfRef(req.Note, () => formulaExist.Note, v => formulaExist.Note = v);
                SetIfRef(req.Name, () => formulaExist.Name, v => formulaExist.Name = v);
                SetIfRef(req.Status, () => formulaExist.Status, v => formulaExist.Status = v ?? "Daft");
                //SetIf(req.TotalPrice, () => formulaExist.TotalPrice.GetValueOrDefault(), v => formulaExist.TotalPrice = v);
                SetIf(req.IsSelect, () => formulaExist.IsSelect, v => formulaExist.IsSelect = v);


                // 2) Cập nhật công thức vật tư - xóa thêm sửa

                var existingMaterials = formulaExist.FormulaMaterials.ToDictionary(fm => fm.MaterialId, fm => fm);

                foreach(var m in req.materialFormulas)
                {
                    if(!existingMaterials.TryGetValue(m.MaterialId, out var link))
                    {
                        // New link
                        link = new FormulaMaterial
                        {
                            FormulaMaterialId = Guid.NewGuid(),
                            FormulaId = formulaExist.FormulaId,
                            MaterialId = m.MaterialId,
                            CategoryId = m.CategoryId,
                            Quantity = m.Quantity ,
                            UnitPrice = m.UnitPrice,
                            TotalPrice = decimal.Round(m.Quantity * m.UnitPrice, 2, MidpointRounding.AwayFromZero),
                            Unit = m.Unit,
                            MaterialNameSnapshot = m.MaterialNameSnapshot,
                            MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot,
                            IsActive = true
                        };

                        await _unitOfWork.FormulaMaterialRepository.AddAsync(link, cancellationToken ?? default);
                    }

                    else
                    {
                        // UPDATE: so sánh và gán khi khác
                        if (m.CategoryId != link.CategoryId) link.CategoryId = m.CategoryId;

                        if (m.Quantity != link.Quantity) link.Quantity = m.Quantity;
                        if (m.UnitPrice != link.UnitPrice) link.UnitPrice = m.UnitPrice;

                        // luôn đồng bộ TotalPrice theo BE để nhất quán
                        var newTotal = decimal.Round(link.Quantity * link.UnitPrice, 2, MidpointRounding.AwayFromZero);
                        if (newTotal != link.TotalPrice) link.TotalPrice = newTotal;

                        SetIfRef(m.Unit, () => link.Unit, v => link.Unit = v);
                        SetIfRef(m.MaterialNameSnapshot, () => link.MaterialNameSnapshot, v => link.MaterialNameSnapshot = v);
                        SetIfRef(m.MaterialExternalIdSnapshot, () => link.MaterialExternalIdSnapshot, v => link.MaterialExternalIdSnapshot = v);

                        if (link.IsActive == false) link.IsActive = true; // RE-activate nếu trước đó bị xóa mềm
                    }
                }

                // SOFT-DELETE: những dòng đang active nhưng không còn trong payload
                var incomingIds = req.materialFormulas.Select(x => x.MaterialId).ToHashSet();
                foreach (var old in formulaExist.FormulaMaterials.Where(x => x.IsActive && !incomingIds.Contains(x.MaterialId)))
                {
                    old.IsActive = false;
                }

                // Cách an toàn nhất: bỏ qua req.TotalPrice, luôn tính theo dòng còn hiệu lực
                formulaExist.TotalPrice = formulaExist.FormulaMaterials
                    .Where(x => x.IsActive)              // nhớ lọc IsActive
                    .Sum(x => x.TotalPrice);


                formulaExist.UpdatedDate = now;
                formulaExist.UpdatedBy = req.UpdatedBy;

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

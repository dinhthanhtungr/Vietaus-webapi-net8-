using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Helpers.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Queries.FormulaFeature;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Notifications.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Core.Domain.Enums.Formulas;
using VietausWebAPI.Core.Domain.Enums.Products;

namespace VietausWebAPI.Core.Application.Features.Labs.Services.FormulaFeatures
{
    public class FormulaService : IFormulaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFormulaPDF _IFormulaPDF;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly INotificationService _notificationService;

        private readonly IExternalIdService _externalId;

        public FormulaService(IUnitOfWork unitOfWork
                            , IMapper mapper
                            , ICurrentUser currentUser
                            , INotificationService notificationService
                            , IFormulaPDF IFormulaPDF
            , IExternalIdService externalIdService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
            _notificationService = notificationService;
            _IFormulaPDF = IFormulaPDF;

            _externalId = externalIdService;
        }

        // ======================================================================== Get ======================================================================== 
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

                if(!string.IsNullOrWhiteSpace(query.status))
                {
                    q = q.Where(p => p.Status == query.status);
                }

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
                        Note = f.Note,
                        TotalPrice = (decimal?)f.FormulaMaterials.Where(m => m.IsActive == true)
                                    .Select(m =>
                                        (m.Quantity) *
                                        (
                                            ms.Where(s => s.MaterialId == m.MaterialId && (s.IsActive ?? true))
                                              .OrderByDescending(s => (DateTime?)(s.UpdatedDate ?? s.CreateDate))
                                              .ThenByDescending(s => (s.IsPreferred ?? false))
                                              .Select(s => (decimal?)s.CurrentPrice)
                                              .FirstOrDefault() ?? 0m
                                        )
                                    ).Sum(),

                        materialFormulas = f.FormulaMaterials
                                .Where(m => m.IsActive == true)
                                .Select(m => new GetMaterialFormula
                                {
                                    ItemId = m.MaterialId ?? Guid.Empty, // Fix CS0266 and CS8629: assign Guid.Empty if null
                                    itemType = m.itemType,

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

        // ======================================================================== Post =======================================================================

        /// <summary>
        /// Tạo công thức mới cho sản phẩm (theo VU)
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<OperationResult> CreateAsync(PostFormula req, CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;

            if (req.Items is null || !req.Items.Any())
                return OperationResult.Fail("Công thức phải có ít nhất 1 vật tư.");


            // 1) Kiểm tra product tồn tại
            var productExists = await _unitOfWork.ProductRepository.Query()
                .AnyAsync(p => p.ProductId == req.ProductId, ct);

            if (!productExists)
                return OperationResult.Fail("Sản phẩm không tồn tại.");

            // 2) Làm sạch dữ liệu đầu vào
            var cleaned = req.Items.Where(x => x != null
                     && x.ItemId != Guid.Empty
                     && x.Quantity > 0
                     && x.UnitPrice >= 0)
            .ToList();

            if (cleaned.Count == 0)
                return OperationResult.Fail("Không có dòng hợp lệ (ItemId/Quantity/UnitPrice).");


            int affected = 0;
            await using var tx = await _unitOfWork.BeginTransactionAsync(); 

            try
            {
                // 3) Kiểm tra tồn tại theo từng ItemType
                var materialIds = cleaned
                    .Where(x => x.itemType == ItemType.Material)
                    .Select(x => x.ItemId)
                    .Distinct()
                    .ToList();

                var productItemIds = cleaned
                    .Where(x => x.itemType == ItemType.Product)
                    .Select(x => x.ItemId)
                    .Distinct()
                    .ToList();

                if(materialIds.Count > 0)
                {
                    var existingMaterialIds = await _unitOfWork.MaterialRepository.Query()
                        .Where(m => materialIds.Contains(m.MaterialId))
                        .Select(m => m.MaterialId)
                        .ToListAsync(ct);

                    var missingMaterialIds = materialIds.Except(existingMaterialIds).ToList();
                    if (missingMaterialIds.Any())
                        return OperationResult.Fail($"Materials do not exist: {string.Join(", ", missingMaterialIds)}");
                }

                if(productItemIds.Count > 0)
                {
                    var existingProductIds = await _unitOfWork.ProductRepository.Query()
                        .Where(p => productItemIds.Contains(p.ProductId))
                        .Select(p => p.ProductId)
                        .ToListAsync(ct);
                    var missingProductIds = productItemIds.Except(existingProductIds).ToList();
                    if (missingProductIds.Any())
                        return OperationResult.Fail($"Products do not exist: {string.Join(", ", missingProductIds)}");
                }

                // 4) Tạo mới công thức
                var formula = new Formula
                {
                    FormulaId = Guid.CreateVersion7(),
                    ProductId = req.ProductId,
                    CompanyId = companyId,
                    CreatedBy = userId,
                    CreatedDate = now,
                    IsActive = true,
                    Note = req.Note,

                    FormulaMaterials = new List<FormulaMaterial>()
                };

                // 5) Tạo công thức vật tư
                Guid guid = req.ProductId;

                formula.Name = await ExternalIdGenerator.GenerateFormulaCode(
                    "F",
                    prefix => _unitOfWork.FormulaRepository.GetLatestExternalIdStartsWithAsync(prefix, guid)
                );

                formula.ExternalId = await _externalId.NextAsync(DocumentPrefix.VU.ToString(), ct: ct);

                //6) Map dtos -> entity
                foreach (var item in cleaned)
                {
                    var lineTotal = Math.Round(item.Quantity * item.UnitPrice, 2, MidpointRounding.AwayFromZero);

                    var formulaMaterial = new FormulaMaterial
                    {
                        FormulaMaterialId =  Guid.CreateVersion7(),
                        FormulaId = formula.FormulaId,

                        MaterialId = item.itemType == ItemType.Material
                            ? item.ItemId
                            : (Guid?)null,

                        ProductId = item.itemType == ItemType.Product
                            ? item.ItemId
                            : (Guid?)null,

                        itemType = item.itemType,

                        CategoryId = item.CategoryId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = lineTotal,
                        Unit = item.Unit,
                        MaterialNameSnapshot = item.MaterialNameSnapshot,
                        MaterialExternalIdSnapshot = item.MaterialExternalIdSnapshot,
                        IsActive = true
                    };
                    formula.FormulaMaterials.Add(formulaMaterial);
                }

                // Cách an toàn nhất: bỏ qua req.TotalPrice, luôn tính theo dòng còn hiệu lực
                formula.TotalPrice = formula.FormulaMaterials.Sum(x => x.TotalPrice);

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

        // ====================================================================== Patch ======================================================================

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

                static (ItemType type, Guid id) KeyOf(FormulaMaterial fm)
                {
                    if (fm.MaterialId.HasValue) return (ItemType.Material, fm.MaterialId.Value);
                    if (fm.ProductId.HasValue) return (ItemType.Product, fm.ProductId.Value);
                    return (ItemType.Material, Guid.Empty); // tránh crash; ideally never happens
                }


                // Patch các field đơn
                SetIfRef(req.Note, () => formulaExist.Note, v => formulaExist.Note = v);
                SetIfRef(req.Name, () => formulaExist.Name, v => formulaExist.Name = v);
                SetIfRef(req.Status, () => formulaExist.Status, v => formulaExist.Status = v ?? "Daft");
                //SetIf(req.TotalPrice, () => formulaExist.TotalPrice.GetValueOrDefault(), v => formulaExist.TotalPrice = v);
                SetIf(req.IsSelect, () => formulaExist.IsSelect, v => formulaExist.IsSelect = v);


                // 2) Cập nhật công thức vật tư - xóa thêm sửa

                var existing = formulaExist.FormulaMaterials
                    .Where(x => x.IsActive) // hoặc lấy tất cả rồi xử lý IsActive sau
                    .ToDictionary(KeyOf, fm => fm);

                foreach (var m in req.materialFormulas)
                {
                    var key = (m.itemType, m.ItemId);
                    if (m.ItemId == Guid.Empty) continue; // bỏ row rác

                    if (!existing.TryGetValue(key, out var link))
                    {
                        link = new FormulaMaterial
                        {
                            FormulaMaterialId = Guid.CreateVersion7(),
                            FormulaId = formulaExist.FormulaId,
                            CategoryId = m.CategoryId,
                            Quantity = m.Quantity,
                            UnitPrice = m.UnitPrice,
                            TotalPrice = decimal.Round(m.Quantity * m.UnitPrice, 2, MidpointRounding.AwayFromZero),
                            Unit = m.Unit,
                            MaterialNameSnapshot = m.MaterialNameSnapshot,
                            MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot,
                            IsActive = true,

                            // ✅ map theo itemType
                            itemType = m.itemType,
                            MaterialId = m.itemType == ItemType.Material ? m.ItemId : (Guid?)null,
                            ProductId = m.itemType == ItemType.Product ? m.ItemId : (Guid?)null,
                        };

                        await _unitOfWork.FormulaMaterialRepository.AddAsync(link, cancellationToken ?? default);
                    }
                    else
                    {
                        // UPDATE
                        if (m.CategoryId != link.CategoryId) link.CategoryId = m.CategoryId;
                        if (m.Quantity != link.Quantity) link.Quantity = m.Quantity;
                        if (m.UnitPrice != link.UnitPrice) link.UnitPrice = m.UnitPrice;

                        var newTotal = decimal.Round(link.Quantity * link.UnitPrice, 2, MidpointRounding.AwayFromZero);
                        if (newTotal != link.TotalPrice) link.TotalPrice = newTotal;

                        SetIfRef(m.Unit, () => link.Unit, v => link.Unit = v);
                        SetIfRef(m.MaterialNameSnapshot, () => link.MaterialNameSnapshot, v => link.MaterialNameSnapshot = v);
                        SetIfRef(m.MaterialExternalIdSnapshot, () => link.MaterialExternalIdSnapshot, v => link.MaterialExternalIdSnapshot = v);

                        // ✅ đảm bảo đúng type/id (tránh case đổi Material<->Product)
                        if (link.itemType != m.itemType) link.itemType = m.itemType;
                        link.MaterialId = m.itemType == ItemType.Material ? m.ItemId : (Guid?)null;
                        link.ProductId = m.itemType == ItemType.Product ? m.ItemId : (Guid?)null;

                        if (link.IsActive == false) link.IsActive = true;
                    }
                }


                // SOFT-DELETE: những dòng đang active nhưng không còn trong payload
                var incomingKeys = req.materialFormulas
                    .Where(x => x.ItemId != Guid.Empty)
                    .Select(x => (x.itemType, x.ItemId))
                    .ToHashSet();

                foreach (var old in formulaExist.FormulaMaterials.Where(x => x.IsActive))
                {
                    var oldKey = KeyOf(old);
                    if (oldKey.id != Guid.Empty && !incomingKeys.Contains(oldKey))
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


        // ===================================================== Export PDF ===================================================================
        public async Task<byte[]> ExportToPdfAsync(Guid data, CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;

            // NOTE: READ-ONLY => transaction không cần thiết.
            // Nhưng bạn muốn giữ "y chang" thì mình vẫn để đây.
            using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 1) Validate input tối thiểu
                if (data == null)
                    throw new ArgumentNullException(nameof(data));


                // 2) Lấy dữ liệu thật từ DB theo FormulaExternalId + CompanyId
                var dto = await _unitOfWork.FormulaRepository.Query()
                    .Where(f =>
                        f.IsActive &&
                        f.CompanyId.HasValue && f.CompanyId.Value == companyId &&
                        f.FormulaId == data)
                    .Select(f => new FormulaPDFDTOs
                    {
                        FormulaExternalId = f.ExternalId,
                        Note = f.Note,

                        colourCode = f.Product.ColourCode ?? "",
                        productName = f.Product.Name ?? "",

                        // AddRate: hiện schema bạn đưa không có AddRate => map tạm từ UsageRate (double?) -> decimal
                        AddRate = (decimal)(f.Product.UsageRate ?? 0),

                        materials = f.FormulaMaterials
                            .Where(m => m.IsActive)
                            .OrderBy(m => m.MaterialExternalIdSnapshot ?? m.Material.ExternalId)
                            .Select(m => new FormulaPDFMaterialDTOs
                            {
                                ExternalId = (m.MaterialExternalIdSnapshot ?? m.Material.ExternalId) ?? "",
                                MaterialName = (m.MaterialNameSnapshot ?? m.Material.Name) ?? "",
                                Quantity = m.Quantity,
                                CategoryId = m.CategoryId
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync(ct);

                if (dto == null)
                    throw new KeyNotFoundException($"Không tìm thấy Formula: {data} (CompanyId={companyId}).");

                // 3) Render PDF
                //var pdfBytes = _IFormulaPDF.Render(dto);

                // 4) Commit (nếu vẫn giữ transaction)
                await tx.CommitAsync(ct);

                throw new KeyNotFoundException($"Không tìm thấy Formula: {data} (CompanyId={companyId}).");
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }
    }
}

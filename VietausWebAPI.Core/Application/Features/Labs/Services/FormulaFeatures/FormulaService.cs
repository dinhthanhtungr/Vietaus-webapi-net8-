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
using VietausWebAPI.Core.Application.Features.Shared.Service.StaticCurrentPriceHelpers;
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
        private readonly IFormulaXML _formulaXml;
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
                            , IFormulaXML formulaXml
            , IExternalIdService externalIdService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
            _notificationService = notificationService;
            _IFormulaPDF = IFormulaPDF;
            _formulaXml = formulaXml;
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

                var baseQ = _unitOfWork.FormulaRepository.Query()
                    .AsNoTracking()
                    .Where(f => f.FormulaMaterials.Any(m => m.IsActive == true));

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim();
                    baseQ = baseQ.Where(x =>
                        (x.ExternalId ?? "").Contains(keyword) ||
                        (x.Name ?? "").Contains(keyword) ||
                        (x.Product.ColourCode ?? "").Contains(keyword) ||
                        (x.Product.Name ?? "").Contains(keyword));
                }

                if (query.CompanyId is Guid cid && cid != Guid.Empty)
                    baseQ = baseQ.Where(p => p.CompanyId == cid);

                if (query.FormulaId is Guid fid && fid != Guid.Empty)
                    baseQ = baseQ.Where(p => p.FormulaId == fid);

                if (query.ProductId is Guid pid && pid != Guid.Empty)
                    baseQ = baseQ.Where(p => p.ProductId == pid);

                if (!string.IsNullOrWhiteSpace(query.status))
                    baseQ = baseQ.Where(p => p.Status == query.status);

                var totalCount = await baseQ.CountAsync(ct);

                // 1) Lấy page formula trước
                var formulaPageTemp = baseQ
                    .OrderByDescending(f => f.CreatedDate)

                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(f => new
                    {
                        f.FormulaId,
                        f.ExternalId,
                        f.Name,
                        ProductCode = f.Product.ColourCode,
                        f.Status,
                        f.CreatedDate,
                        f.IsSelect,
                        f.CheckDate,

                        f.EffectiveDate,
                        f.ProductionPrice,
                        f.PresidentPrice,
                        f.ProfitMarginPrice,

                        CheckNameSnapshot = f.CheckByNavigation != null ? f.CheckByNavigation.FullName : null,
                        f.SentDate,
                        SentByNameSnapshot = f.SentByNavigation != null ? f.SentByNavigation.FullName : null,
                        CreatedByName = f.CreatedByNavigation != null ? f.CreatedByNavigation.FullName.Trim() : null,
                        f.Note
                    });

                if(query.IsMerchadiseOrder)
                {
                    formulaPageTemp = formulaPageTemp.Where(f => f.Status != "Draft" && f.Status != "Inprocess");
                }

                var formulaPage = await formulaPageTemp
                    .ToListAsync(ct);

                var formulaIds = formulaPage.Select(x => x.FormulaId).ToList();

                if (formulaIds.Count == 0)
                    return new PagedResult<GetFormula>(new List<GetFormula>(), totalCount, query.PageNumber, query.PageSize);

                // 2) Lấy toàn bộ material của page hiện tại
                var materialRows = await _unitOfWork.FormulaMaterialRepository.Query()
                    .AsNoTracking()
                    .Where(m => m.IsActive == true && formulaIds.Contains(m.FormulaId))
                    .Select(m => new
                    {
                        m.FormulaId,
                        m.FormulaMaterialId,
                        m.LineNo,
                        m.MaterialId,
                        m.ProductId,
                        m.itemType,
                        m.CategoryId,
                        m.Quantity,
                        m.Unit,
                        m.MaterialNameSnapshot,
                        m.MaterialExternalIdSnapshot
                    })
                    .ToListAsync(ct);

                var materialIds = materialRows
                    .Where(x => x.MaterialId.HasValue && x.MaterialId.Value != Guid.Empty)
                    .Select(x => x.MaterialId!.Value)
                    .Distinct()
                    .ToList();

                // 3) Lấy giá mới nhất cho toàn bộ material cần dùng
                //var priceInfoDict = await MaterialPriceQueryHelper.LoadLatestMaterialPriceInfoDictAsync(
                //    _unitOfWork.PurchaseOrderDetailRepository.Query(),
                //    _unitOfWork.MaterialsSupplierRepository.Query(),
                //    materialIds.Cast<Guid?>(),
                //    ct);

                var priceInfoDict  = await MaterialPriceQueryHelper.LoadLatestItemPriceInfoDictAsync(
                    _unitOfWork.PurchaseOrderDetailRepository.Query(),
                    _unitOfWork.MaterialsSupplierRepository.Query(),
                    _unitOfWork.MerchandiseOrderRepository.QueryDetail(),
                    materialRows.Select(x => (x.itemType, x.MaterialId, x.ProductId)),
                    ct);

                // 4) Group materials theo FormulaId rồi map trong memory
                var materialLookup = materialRows
                    .GroupBy(x => x.FormulaId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.OrderBy(x => x.LineNo).Select(x =>
                        {
                            var unitPrice = 0m;
                            DateTime? expiryDate = null;

                            //if (x.MaterialId.HasValue && x.MaterialId.Value != Guid.Empty)
                            //{
                            //    //unitPrice = MaterialPriceQueryHelper.ResolveLatestPrice(
                            //    //    priceInfoDict,
                            //    //    x.MaterialId,
                            //    //    0m);

                            //    //expiryDate = MaterialPriceQueryHelper.ResolveLatestPriceDate(
                            //    //    priceInfoDict,
                            //    //    x.MaterialId);

                            //    Guid? itemId = x.itemType == ItemType.Material ? x.MaterialId : x.ProductId;

                            //    unitPrice = MaterialPriceQueryHelper.ResolveLatestItemPrice(
                            //        priceInfoDict,
                            //        x.itemType,
                            //        itemId,
                            //        0m);

                            //    expiryDate = MaterialPriceQueryHelper.ResolveLatestItemPriceDate(
                            //        priceInfoDict,
                            //        x.itemType,
                            //        itemId);
                            //}

                            var itemId = x.itemType == ItemType.Material ? x.MaterialId : x.ProductId;

                            if (itemId.HasValue && itemId.Value != Guid.Empty)
                            {
                                unitPrice = MaterialPriceQueryHelper.ResolveLatestItemPrice(
                                    priceInfoDict,
                                    x.itemType,
                                    itemId,
                                    0m);

                                expiryDate = MaterialPriceQueryHelper.ResolveLatestItemPriceDate(
                                    priceInfoDict,
                                    x.itemType,
                                    itemId);
                            }

                            return new GetMaterialFormula
                            {
                                FormulaMaterialId = x.FormulaMaterialId,
                                LineNo = x.LineNo,
                                ItemId = x.MaterialId ?? x.ProductId ?? Guid.Empty,
                                itemType = x.itemType,
                                CategoryId = x.CategoryId,
                                Quantity = x.Quantity,
                                UnitPrice = unitPrice,
                                TotalPrice = x.Quantity * unitPrice,
                                ExpiryDate = expiryDate,
                                Unit = x.Unit,
                                MaterialNameSnapshot = x.MaterialNameSnapshot,
                                MaterialExternalIdSnapshot = x.MaterialExternalIdSnapshot
                            };
                        }).ToList()
                    );

                var items = formulaPage.Select(f =>
                {
                    materialLookup.TryGetValue(f.FormulaId, out var mats);
                    mats ??= new List<GetMaterialFormula>();

                    return new GetFormula
                    {
                        FormulaId = f.FormulaId,
                        ExternalId = f.ExternalId,
                        Name = f.Name,
                        ProductCode = f.ProductCode,
                        Status = f.Status,
                        CreatedDate = f.CreatedDate,
                        IsSelect = f.IsSelect,
                        CheckDate = f.CheckDate,
                        CheckNameSnapshot = f.CheckNameSnapshot,
                        SentDate = f.SentDate,
                        SentByNameSnapshot = f.SentByNameSnapshot,

                        EffectiveDate = f.EffectiveDate,
                        ProductionPrice = f.ProductionPrice,
                        PresidentPrice = f.PresidentPrice,
                        ProfitMarginPrice = f.ProfitMarginPrice,

                        CreatedByName = f.CreatedByName,
                        Note = f.Note,
                        materialFormulas = mats,
                        TotalPrice = mats.Sum(x => x.TotalPrice)
                    };
                }).ToList();

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

            var activeMaterials = req.Items
                .Select((x, i) => new { Item = x, Row = i + 1 })
                .ToList();

            var invalidRows = activeMaterials
                .Where(x => x.Item.ItemId == Guid.Empty)
                .Select(x => x.Row)
                .ToList();

            if (invalidRows.Any())
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail(
                    $"Có dòng vật tư chưa chọn ItemId: {string.Join(", ", invalidRows)}");
            }

            var totalQuantity = activeMaterials.Sum(x => x.Item.Quantity);

            if (totalQuantity < 1 || totalQuantity > 1)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail("Tổng Quantity của các dòng đang hoạt động phải = 1.");
            }

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
                var lineNo = 1;
                foreach (var item in cleaned)
                {
                    var lineTotal = Math.Round(item.Quantity * item.UnitPrice, 2, MidpointRounding.AwayFromZero);

                    var formulaMaterial = new FormulaMaterial
                    {
                        FormulaMaterialId = Guid.CreateVersion7(),
                        FormulaId = formula.FormulaId,

                        LineNo = lineNo++,

                        MaterialId = item.itemType == ItemType.Material ? item.ItemId : (Guid?)null,
                        ProductId = item.itemType == ItemType.Product ? item.ItemId : (Guid?)null,

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

                if (req.materialFormulas == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return OperationResult.Fail("Công thức phải có ít nhất 1 dòng vật tư đang hoạt động.");
                }

                var activeMaterials = req.materialFormulas
                    .Select((x, i) => new { Item = x, Row = i + 1 })
                    .ToList();

                var invalidRows = activeMaterials
                    .Where(x => x.Item.ItemId == Guid.Empty)
                    .Select(x => x.Row)
                    .ToList();

                if (invalidRows.Any())
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return OperationResult.Fail(
                        $"Có dòng vật tư chưa chọn ItemId: {string.Join(", ", invalidRows)}");
                }

                var totalQuantity = activeMaterials.Sum(x => x.Item.Quantity);

                if (totalQuantity < 1 || totalQuantity > 1)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return OperationResult.Fail("Tổng Quantity của các dòng đang hoạt động phải = 1.");
                }

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
                SetIfRef(req.Status, () => formulaExist.Status, v => formulaExist.Status = v ?? "Draft");
                SetIf(req.IsSelect, () => formulaExist.IsSelect, v => formulaExist.IsSelect = v);


                var productionPriceChanged =
                    req.ProductionPrice.HasValue &&
                    req.ProductionPrice.Value != formulaExist.ProductionPrice.GetValueOrDefault();

                var presidentPriceChanged =
                    req.PresidentPrice.HasValue &&
                    req.PresidentPrice.Value != formulaExist.PresidentPrice.GetValueOrDefault();

                var profitMarginPriceChanged =
                    req.ProfitMarginPrice.HasValue &&
                    req.ProfitMarginPrice.Value != formulaExist.ProfitMarginPrice.GetValueOrDefault();
                        
                SetIf(req.ProductionPrice, () => formulaExist.ProductionPrice.GetValueOrDefault(), v => formulaExist.ProductionPrice = v);
                SetIf(req.PresidentPrice, () => formulaExist.PresidentPrice.GetValueOrDefault(), v => formulaExist.PresidentPrice = v);
                SetIf(req.ProfitMarginPrice, () => formulaExist.ProfitMarginPrice.GetValueOrDefault(), v => formulaExist.ProfitMarginPrice = v);

                if (productionPriceChanged || presidentPriceChanged || profitMarginPriceChanged)
                {
                    formulaExist.EffectiveDate = DateTime.Now;
                }
                // 2) Cập nhật công thức vật tư - xóa thêm sửa

                var existingById = formulaExist.FormulaMaterials
                    .Where(x => x.IsActive)
                    .ToDictionary(x => x.FormulaMaterialId, x => x);

                var incomingRows = req.materialFormulas
                    .Where(x => x.ItemId != Guid.Empty)
                    .ToList();

                for (int i = 0; i < incomingRows.Count; i++)
                {
                    incomingRows[i].LineNo = i + 1;
                }

                foreach (var m in incomingRows)
                {
                    FormulaMaterial? link = null;

                    if (m.FormulaMaterialId.HasValue &&
                        existingById.TryGetValue(m.FormulaMaterialId.Value, out var existingRow))
                    {
                        link = existingRow;
                    }

                    if (link == null)
                    {
                        link = new FormulaMaterial
                        {
                            FormulaMaterialId = Guid.CreateVersion7(),
                            FormulaId = formulaExist.FormulaId,
                            CategoryId = m.CategoryId,
                            Quantity = m.Quantity,
                            UnitPrice = m.UnitPrice,
                            LineNo = m.LineNo,
                            TotalPrice = decimal.Round(m.Quantity * m.UnitPrice, 2, MidpointRounding.AwayFromZero),
                            Unit = m.Unit,
                            MaterialNameSnapshot = m.MaterialNameSnapshot,
                            MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot,
                            IsActive = true,

                            itemType = m.itemType,
                            MaterialId = m.itemType == ItemType.Material ? m.ItemId : (Guid?)null,
                            ProductId = m.itemType == ItemType.Product ? m.ItemId : (Guid?)null,
                        };

                        await _unitOfWork.FormulaMaterialRepository.AddAsync(link, cancellationToken ?? default);
                    }
                    else
                    {
                        link.LineNo = m.LineNo;
                        link.CategoryId = m.CategoryId;
                        link.Quantity = m.Quantity;
                        link.UnitPrice = m.UnitPrice;
                        link.TotalPrice = decimal.Round(m.Quantity * m.UnitPrice, 2, MidpointRounding.AwayFromZero);

                        SetIfRef(m.Unit, () => link.Unit, v => link.Unit = v);
                        SetIfRef(m.MaterialNameSnapshot, () => link.MaterialNameSnapshot, v => link.MaterialNameSnapshot = v);
                        SetIfRef(m.MaterialExternalIdSnapshot, () => link.MaterialExternalIdSnapshot, v => link.MaterialExternalIdSnapshot = v);

                        link.itemType = m.itemType;
                        link.MaterialId = m.itemType == ItemType.Material ? m.ItemId : (Guid?)null;
                        link.ProductId = m.itemType == ItemType.Product ? m.ItemId : (Guid?)null;
                        link.IsActive = true;
                    }
                }


                // SOFT-DELETE: những dòng đang active nhưng không còn trong payload
                var incomingIds = incomingRows
                    .Where(x => x.FormulaMaterialId.HasValue)
                    .Select(x => x.FormulaMaterialId!.Value)
                    .ToHashSet();

                foreach (var old in formulaExist.FormulaMaterials.Where(x => x.IsActive))
                {
                    // dòng cũ nào có id mà không còn xuất hiện trong payload thì tắt
                    if (!incomingIds.Contains(old.FormulaMaterialId))
                    {
                        // chỉ soft delete các dòng cũ
                        // dòng mới vừa add chưa có trong existingById nên không vào đây
                        if (existingById.ContainsKey(old.FormulaMaterialId))
                            old.IsActive = false;
                    }
                }

                // Cách an toàn nhất: bỏ qua req.TotalPrice, luôn tính theo dòng còn hiệu lực
                formulaExist.TotalPrice = incomingRows
                    .Sum(x => decimal.Round(x.Quantity * x.UnitPrice, 2, MidpointRounding.AwayFromZero));


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
                if (data == Guid.Empty)
                    throw new ArgumentException("FormulaId không hợp lệ.", nameof(data));

                // 2) Lấy dữ liệu thật từ DB theo FormulaExternalId + CompanyId
                var dto = await _unitOfWork.FormulaRepository.Query()
                    .Include(f => f.Product)
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
                        AddRate = (double)(f.Product.UsageRate ?? 0),

                        RequestDate = f.Product.SampleRequests
                            .Where(sr => sr.IsActive)
                            .OrderBy(sr => sr.CreatedDate)
                            .Select(sr => (DateTime?)sr.RequestDeliveryDate)
                            .FirstOrDefault() ?? DateTime.MinValue,

                        materials = f.FormulaMaterials
                            .Where(m => m.IsActive)
                            .OrderBy(m => m.LineNo)
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

        public async Task<byte[]> ExportToXmlAsync(Guid productId, CancellationToken ct = default)
        {
            var companyId = _currentUser.CompanyId;

            using var tx = await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (productId == Guid.Empty)
                    throw new ArgumentException("ProductId không hợp lệ.", nameof(productId));

                // 1) Lấy thông tin product + danh sách formula + materials

                var data = await _unitOfWork.SampleRequestRepository.Query()
                    .AsNoTracking()
                    .Where(sr => sr.IsActive
                        && sr.ProductId == productId)   // ✅ input rõ ràng
                    .Select(sr => new
                    {
                        Tp = sr.ExternalId,                 // ✅ TP_15679
                        Code = sr.Product.ColourCode,       // ✅ WP21042 (tuỳ bạn map)
                        Name = sr.Product.Name ?? "",       // ✅ HẠT MÀU
                        ProductCode = sr.Product.Code,      // nếu cần

                        Formulas = sr.Product.Formulas
                            .Where(f => f.IsActive)
                            .OrderBy(f => f.ExternalId)
                            .Select(f => new GetFormula
                            {
                                FormulaId = f.FormulaId,
                                ExternalId = f.ExternalId,      // ✅ Số mẻ (VUxxxx)
                                Name = f.Name,
                                ProductCode = sr.Product.Code,

                                materialFormulas = f.FormulaMaterials
                                    .Where(m => m.IsActive == true)
                                    .Select(m => new GetMaterialFormula
                                    {
                                        LineNo = m.LineNo,
                                        ItemId = m.MaterialId ?? m.ProductId ?? Guid.Empty,
                                        itemType = m.itemType,
                                        CategoryId = m.CategoryId,
                                        Quantity = m.Quantity,
                                        UnitPrice = m.UnitPrice,
                                        TotalPrice = m.TotalPrice,
                                        Unit = m.Unit,
                                        MaterialNameSnapshot = m.MaterialNameSnapshot,
                                        MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot
                                    })
                                    .OrderBy(m => m.LineNo)
                                    .ToList()
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync(ct);

                if (data == null)
                    throw new KeyNotFoundException($"Không tìm thấy Product: {productId} (CompanyId={companyId}).");

                // 2) Flatten ra rows y như excel: TP / Số mẻ / Code / Tên / NVL / Detail / Định mức / Khách hàng
                var rows = data.Formulas
                    .SelectMany(f => f.materialFormulas.Select(m => new FormulaExportRow
                    {
                        TP = data.Tp,
                        SoMe = f.ExternalId ?? "",
                        Code = data.Code,
                        Ten = data.Name,
                        NVL = m.MaterialExternalIdSnapshot ?? "",
                        Detail = m.MaterialNameSnapshot ?? "",
                        DinhMuc = m.Quantity,      // giữ decimal (0.0062500000)
                        KhachHang = ""             // nếu có nguồn thì map sau
                    }))
                    .ToList();

                var bytes = _formulaXml.Render(rows); // Render nhận List<Row> thay vì 1 GetFormula
                await tx.CommitAsync(ct);
                return bytes;
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }
    }

}

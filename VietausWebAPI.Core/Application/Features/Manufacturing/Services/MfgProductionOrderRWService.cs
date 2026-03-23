using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;

namespace VietausWebAPI.Core.Application.Features.ManufacturingFeature.Services
{
    /// <summary>
    /// Service đọc dữ liệu chi tiết màn hình RW của lệnh sản xuất (MFG Production Order).
    /// 
    /// Luồng ưu tiên chọn công thức:
    /// 1. Standard hiện hành của Product
    /// 2. Select hiện hành của chính MFG Production Order
    /// 3. Fallback về công thức VU gốc của MFG Production Order
    /// 
    /// Ngoài ra service còn hỗ trợ:
    /// - So sánh công thức VU hiện tại với công thức VU của MPO trước đó cùng Product
    /// - Nếu khác nhau thì chèn thêm 1 dòng "Improvement" vào đầu danh sách summary
    /// </summary>
    public class MfgProductionOrderRWService : IMfgProductionOrderRWService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Mốc phân chia công thức sản xuất cũ / mới.
        /// 
        /// Formula có CreatedDate trước ngày này => ProductionOld
        /// Formula có CreatedDate từ ngày này trở đi => Production
        /// </summary>
        private static readonly DateTime ProductionOldCutoff = new(2026, 3, 1);

        /// <summary>
        /// Khởi tạo service với UnitOfWork để truy vấn dữ liệu từ repository.
        /// </summary>
        public MfgProductionOrderRWService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Lấy đầy đủ dữ liệu hiển thị cho màn hình RW của một MFG Production Order.
        /// 
        /// Các bước chính:
        /// 1. Lấy dữ liệu nền của MPO hiện tại
        /// 2. Tìm công thức VU của MPO trước đó cùng Product để xem có phát sinh Improvement không
        /// 3. Nếu có Standard hiện hành => trả danh sách Standard
        /// 4. Nếu không có Standard mà có Select hiện hành => trả danh sách Select/Production
        /// 5. Nếu không có cả hai => fallback về công thức VU gốc
        /// </summary>
        /// <param name="mfgProductionOrderId">Id của lệnh sản xuất cần xem.</param>
        /// <returns>
        /// DTO RW hoàn chỉnh nếu tìm thấy dữ liệu; ngược lại trả về null.
        /// </returns>
        public async Task<GetMfgProductionOrderRWs?> GetByIdAsync(Guid mfgProductionOrderId)
        {
            var baseData = await _unitOfWork.MfgProductionOrderRepository
                .Query(false)
                .Where(x => x.MfgProductionOrderId == mfgProductionOrderId && x.IsActive)
                .Select(x => new
                {
                    x.MfgProductionOrderId,
                    x.ExternalId,

                    x.ProductId,
                    x.ProductExternalIdSnapshot,
                    x.ProductNameSnapshot,

                    x.CustomerId,
                    x.CustomerNameSnapshot,
                    x.CustomerExternalIdSnapshot,

                    x.FormulaId,
                    x.FormulaExternalIdSnapshot,

                    x.ManufacturingDate,
                    x.ExpectedDate,
                    x.RequiredDate,

                    x.TotalQuantityRequest,
                    x.TotalQuantity,
                    x.NumOfBatches,
                    x.UnitPriceAgreed,

                    x.Status,
                    x.LabNote,
                    x.Requirement,
                    x.PlpuNote,
                    x.BagType,
                    x.QcCheck,
                    x.StepOfProduct,
                    x.CreatedDate,

                    OrderLink = _unitOfWork.MfgOrderPORepository.Query(false)
                        .Where(link => link.MfgProductionOrderId == x.MfgProductionOrderId && link.IsActive)
                        .Select(link => new
                        {
                            MerchandiseOrderDetailId = link.MerchandiseOrderDetailId,
                            MerchandiseOrderId = link.Detail.MerchandiseOrderId,
                            MerchandiseOrderExternalId = link.Detail.MerchandiseOrder.ExternalId
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (baseData == null)
                return null;

            // Lấy công thức VU của MPO trước đó cùng Product để so với công thức VU hiện tại.
            var previousVu = await GetPreviousVuFormulaAsync(
                baseData.ProductId,
                baseData.MfgProductionOrderId,
                baseData.CreatedDate);

            // Nếu công thức VU hiện tại khác công thức VU trước đó thì xem là có Improvement.
            var shouldAddImprovement =
                baseData.FormulaId.HasValue &&
                baseData.FormulaId != Guid.Empty &&
                previousVu.HasValue &&
                previousVu.Value.FormulaId.HasValue &&
                previousVu.Value.FormulaId != Guid.Empty &&
                baseData.FormulaId.Value != previousVu.Value.FormulaId.Value;

            var result = new GetMfgProductionOrderRWs
            {
                MfgProductionOrderId = baseData.MfgProductionOrderId,
                ExternalId = baseData.ExternalId,

                MerchandiseOrderId = baseData.OrderLink?.MerchandiseOrderId ?? Guid.Empty,
                MerchandiseOrderDetailId = baseData.OrderLink?.MerchandiseOrderDetailId ?? Guid.Empty,
                MerchandiseOrderExternalId = baseData.OrderLink?.MerchandiseOrderExternalId,

                CustomerNameSnapshot = baseData.CustomerNameSnapshot,
                CustomerExternalIdSnapshot = baseData.CustomerExternalIdSnapshot,

                ProductId = baseData.ProductId,
                ProductExternalIdSnapshot = baseData.ProductExternalIdSnapshot,
                ProductNameSnapshot = baseData.ProductNameSnapshot,

                FormulaCustomerSelect = baseData.FormulaId ?? Guid.Empty,
                FormulaCustomerExternalIdSelect = baseData.FormulaExternalIdSnapshot ?? string.Empty,

                ManufacturingDate = baseData.ManufacturingDate,
                ExpectedDate = baseData.ExpectedDate,
                RequiredDate = baseData.RequiredDate,

                TotalQuantityRequest = baseData.TotalQuantityRequest,
                TotalQuantity = baseData.TotalQuantity,
                NumOfBatches = baseData.NumOfBatches,
                UnitPriceAgreed = baseData.UnitPriceAgreed,

                Status = baseData.Status,
                LabNote = baseData.LabNote,
                Requirement = baseData.Requirement,
                PlpuNote = baseData.PlpuNote,
                BagType = baseData.BagType,
                QcCheck = baseData.QcCheck,
                StepOfProduct = baseData.StepOfProduct
            };

            // Ưu tiên 1: lấy Standard hiện hành của Product.
            var currentStandard = await GetCurrentStandardAsync(baseData.ProductId);

            if (currentStandard is { } standard)
            {
                result.ManufacturingFormulaIdIsSelect = standard.ManufacturingFormulaId;
                result.ManufacturingFormulaExternalIdIsSelect = standard.ExternalId;

                var summaries = await GetStandardSummariesAsync(baseData.ProductId);

                // Nếu VU hiện tại khác VU của MPO trước đó thì chèn thêm dòng Improvement lên đầu danh sách.
                if (shouldAddImprovement)
                {
                    var improvement = await BuildImprovementSummaryAsync(
                        baseData.MfgProductionOrderId,
                        baseData.FormulaId,
                        baseData.FormulaExternalIdSnapshot,
                        baseData.CreatedDate);

                    if (improvement != null && !summaries.Any(x => x.Id == improvement.Id))
                        summaries.Insert(0, improvement);
                }

                result.MfgProductionOrderRWSummaries = summaries;
                return result;
            }

            // Ưu tiên 2: nếu không có Standard thì lấy Select hiện hành của chính MPO.
            var currentSelect = await GetCurrentSelectByProductAsync(baseData.ProductId);

            if (currentSelect is { } select)
            {
                result.ManufacturingFormulaIdIsSelect = select.ManufacturingFormulaId;
                result.ManufacturingFormulaExternalIdIsSelect = select.ExternalId;

                var summaries = await GetSelectSummariesByProductAsync(baseData.ProductId);

                // Nếu VU hiện tại khác VU của MPO trước đó thì chèn thêm dòng Improvement lên đầu danh sách.
                if (shouldAddImprovement)
                {
                    var improvement = await BuildImprovementSummaryAsync(
                        baseData.MfgProductionOrderId,
                        baseData.FormulaId,
                        baseData.FormulaExternalIdSnapshot,
                        baseData.CreatedDate);

                    if (improvement != null && !summaries.Any(x => x.Id == improvement.Id))
                        summaries.Insert(0, improvement);
                }

                result.MfgProductionOrderRWSummaries = summaries;
                return result;
            }

            // Ưu tiên 3: nếu không có cả Standard lẫn Select thì fallback về công thức VU gốc.
            result.ManufacturingFormulaIdIsSelect = null;
            result.ManufacturingFormulaExternalIdIsSelect = null;
            result.MfgProductionOrderRWSummaries = await GetVuFallbackSummaryAsync(
                baseData.MfgProductionOrderId,
                baseData.FormulaId,
                baseData.FormulaExternalIdSnapshot,
                baseData.CreatedDate);

            return result;
        }

        /// <summary>
        /// Lấy Standard hiện hành của một Product.
        /// 
        /// Standard hiện hành được hiểu là:
        /// - cùng ProductId
        /// - có ManufacturingFormulaId
        /// - ValidTo == null
        /// 
        /// Nếu có thể truy ngược từ ManufacturingFormulaId sang ProductionSelectVersion
        /// thì lấy thêm MfgProductionOrderId tương ứng.
        /// Nếu không truy ra được thì MfgProductionOrderId để null, không phát sinh lỗi.
        /// </summary>
        /// <param name="productId">Id của Product cần lấy Standard hiện hành.</param>
        /// <returns>
        /// Tuple gồm ManufacturingFormulaId, MfgProductionOrderId (nullable), ExternalId;
        /// null nếu không có Standard hiện hành.
        /// </returns>
        private async Task<(Guid ManufacturingFormulaId, Guid? MfgProductionOrderId, string? ExternalId)?> GetCurrentStandardAsync(Guid productId)
        {
            var current = await _unitOfWork.ProductStandardFormulaRepository
                .Query(false)
                .Where(x =>
                    x.ProductId == productId &&
                    x.ManufacturingFormulaId != null &&
                    x.ValidTo == null)
                .OrderByDescending(x => x.ValidFrom)
                .Select(x => new
                {
                    ManufacturingFormulaId = x.ManufacturingFormulaId!.Value,
                    ExternalId = x.ManufacturingFormula != null ? x.ManufacturingFormula.ExternalId : null,

                    // Nếu formula này từng được chọn ở ProductionSelectVersion thì lấy MPO gần nhất.
                    // Nếu không có dữ liệu select tương ứng thì kết quả sẽ là null.
                    MfgProductionOrderId = _unitOfWork.ProductionSelectVersionRepository
                        .Query(false)
                        .Where(sv => sv.ManufacturingFormulaId == x.ManufacturingFormulaId)
                        .OrderByDescending(sv => sv.ValidFrom)
                        .Select(sv => (Guid?)sv.MfgProductionOrderId)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (current == null) return null;

            return (current.ManufacturingFormulaId, current.MfgProductionOrderId, current.ExternalId);
        }

        /// <summary>
        /// Lấy công thức Select hiện hành của chính MFG Production Order.
        /// 
        /// Rule:
        /// - cùng MfgProductionOrderId
        /// - có ManufacturingFormulaId
        /// - lấy bản ghi mới nhất theo ValidFrom
        /// </summary>
        /// <param name="mfgProductionOrderId">Id của MFG Production Order.</param>
        /// <returns>
        /// Tuple gồm ManufacturingFormulaId và ExternalId; null nếu không có Select.
        /// </returns>
        private async Task<(Guid ManufacturingFormulaId, Guid? MfgProductionOrderId, string? ExternalId)?> GetCurrentSelectByProductAsync(Guid productId)
        {
            var current = await _unitOfWork.ProductionSelectVersionRepository
                .Query(false)
                .Where(x =>
                    x.MfgProductionOrder != null &&
                    x.MfgProductionOrder.ProductId == productId &&
                    x.ManufacturingFormulaId != null)
                .OrderByDescending(x => x.ValidFrom)
                .Select(x => new
                {
                    ManufacturingFormulaId = x.ManufacturingFormulaId!.Value,
                    MfgProductionOrderId = (Guid?)x.MfgProductionOrderId,
                    ExternalId = x.ManufacturingFormula != null ? x.ManufacturingFormula.ExternalId : null
                })
                .FirstOrDefaultAsync();

            if (current == null) return null;

            return (current.ManufacturingFormulaId, current.MfgProductionOrderId, current.ExternalId);
        }
        /// <summary>
        /// Lấy toàn bộ lịch sử Standard của Product, sắp giảm dần theo ValidFrom.
        /// 
        /// Mỗi dòng summary là một công thức chuẩn.
        /// Trong phiên bản hiện tại, MfgProductionOrderId của Standard đang để null
        /// để tránh áp đặt quan hệ bắt buộc giữa Standard và Production Select.
        /// </summary>
        /// <param name="productId">Id của Product.</param>
        /// <returns>Danh sách summary công thức chuẩn.</returns>
        private async Task<List<GetMfgProductionOrderRWSummary>> GetStandardSummariesAsync(Guid productId)
        {
            return await _unitOfWork.ProductStandardFormulaRepository
                .Query(false)
                .Where(x => x.ProductId == productId && x.ManufacturingFormulaId != null)
                .OrderByDescending(x => x.ValidFrom)
                .Select(x => new GetMfgProductionOrderRWSummary
                {
                    MfgProductionOrderId = _unitOfWork.ProductionSelectVersionRepository
                        .Query(false)
                        .Where(sv =>
                            sv.ManufacturingFormulaId == x.ManufacturingFormulaId &&
                            sv.MfgProductionOrder != null &&
                            sv.MfgProductionOrder.ProductId == x.ProductId)
                        .OrderByDescending(sv => sv.ValidFrom)
                        .Select(sv => (Guid?)sv.MfgProductionOrderId)
                        .FirstOrDefault(),

                    FormulaType = FormulaType.Standard,
                    ExternalId = x.ManufacturingFormula != null ? x.ManufacturingFormula.ExternalId : null,
                    CreatedDate = x.ManufacturingFormula != null
                        ? x.ManufacturingFormula.CreatedDate
                        : x.ValidFrom,
                    Note = !string.IsNullOrWhiteSpace(x.Note)
                        ? x.Note
                        : (x.ManufacturingFormula != null ? x.ManufacturingFormula.Note : null),
                    Id = x.ManufacturingFormulaId!.Value
                })
                .ToListAsync();
        }
        /// <summary>
        /// Lấy danh sách các công thức đã từng được chọn trong ProductionSelectVersion
        /// của các MPO cùng Product.
        /// 
        /// Dữ liệu được sắp xếp giảm dần theo ValidFrom và giới hạn số lượng bằng tham số take.
        /// Công thức được phân loại:
        /// - ProductionOld nếu ngày tạo formula < ProductionOldCutoff
        /// - Production nếu ngày tạo formula >= ProductionOldCutoff
        /// </summary>
        /// <param name="productId">Id của Product.</param>
        /// <param name="take">Số lượng bản ghi tối đa cần lấy.</param>
        /// <returns>Danh sách summary công thức production/select.</returns>
        private async Task<List<GetMfgProductionOrderRWSummary>> GetSelectSummariesByProductAsync(Guid productId, int take = 10)
        {
            return await _unitOfWork.ProductionSelectVersionRepository
                .Query(false)
                .Where(x =>
                    x.MfgProductionOrder != null &&
                    x.MfgProductionOrder.ProductId == productId &&
                    x.ManufacturingFormulaId != null)
                .OrderByDescending(x => x.ValidFrom)
                .Select(x => new
                {
                    x.MfgProductionOrderId,
                    x.ManufacturingFormulaId,
                    x.ValidFrom,
                    FormulaExternalId = x.ManufacturingFormula != null ? x.ManufacturingFormula.ExternalId : null,
                    FormulaCreatedDate = x.ManufacturingFormula != null
                        ? x.ManufacturingFormula.CreatedDate
                        : x.ValidFrom,
                    FormulaNote = x.ManufacturingFormula != null ? x.ManufacturingFormula.Note : null
                })
                .Take(take)
                .Select(x => new GetMfgProductionOrderRWSummary
                {
                    MfgProductionOrderId = x.MfgProductionOrderId,
                    FormulaType = x.FormulaCreatedDate < ProductionOldCutoff
                        ? FormulaType.ProductionOld
                        : FormulaType.Production,
                    ExternalId = x.FormulaExternalId,
                    CreatedDate = x.FormulaCreatedDate.GetValueOrDefault(),
                    Note = x.FormulaNote,
                    Id = x.ManufacturingFormulaId!.Value
                })
                .ToListAsync();
        }

        /// <summary>
        /// Tạo danh sách fallback khi không có Standard hiện hành và cũng không có Select hiện hành.
        /// 
        /// Trường hợp này hệ thống sẽ dùng công thức VU gốc đang gắn trên MPO hiện tại.
        /// Nếu MPO không có FormulaId thì trả về danh sách rỗng.
        /// </summary>
        /// <param name="mfgProductionOrderId">Id của MPO hiện tại.</param>
        /// <param name="formulaId">Id công thức VU gốc.</param>
        /// <param name="formulaExternalIdSnapshot">ExternalId snapshot của công thức VU.</param>
        /// <param name="mpoCreatedDate">Ngày tạo MPO để fallback nếu Formula không còn tồn tại.</param>
        /// <returns>Danh sách summary chỉ gồm 1 dòng FromVu hoặc rỗng.</returns>
        private async Task<List<GetMfgProductionOrderRWSummary>> GetVuFallbackSummaryAsync(Guid mfgProductionOrderId, Guid? formulaId, string? formulaExternalIdSnapshot, DateTime mpoCreatedDate)
        {
            if (formulaId == null || formulaId == Guid.Empty)
                return new List<GetMfgProductionOrderRWSummary>();

            var formulaInfo = await _unitOfWork.FormulaRepository
                .Query(false)
                .Where(x => x.FormulaId == formulaId.Value)
                .Select(x => new
                {
                    x.FormulaId,
                    x.ExternalId,
                    x.Note,
                    x.CreatedDate
                })
                .FirstOrDefaultAsync();

            return new List<GetMfgProductionOrderRWSummary>
            {
                new GetMfgProductionOrderRWSummary
                {
                    MfgProductionOrderId = mfgProductionOrderId,
                    FormulaType = FormulaType.FromVu,
                    ExternalId = formulaInfo?.ExternalId ?? formulaExternalIdSnapshot,
                    CreatedDate = formulaInfo?.CreatedDate ?? mpoCreatedDate,
                    Note = formulaInfo?.Note,
                    Id = formulaId.Value
                }
            };
        }

        /// <summary>
        /// Kiểm tra công thức VU hiện tại có phải là một Improvement so với công thức VU trước đó hay không.
        /// 
        /// Điều kiện:
        /// - cả currentFormulaId và previousFormulaId đều phải có giá trị hợp lệ
        /// - và 2 giá trị phải khác nhau
        /// </summary>
        /// <param name="currentFormulaId">FormulaId hiện tại.</param>
        /// <param name="previousFormulaId">FormulaId của bản ghi trước đó.</param>
        /// <returns>True nếu là improvement; ngược lại false.</returns>
        private static bool IsVuImproved(Guid? currentFormulaId, Guid? previousFormulaId)
        {
            if (!currentFormulaId.HasValue || currentFormulaId == Guid.Empty)
                return false;

            if (!previousFormulaId.HasValue || previousFormulaId == Guid.Empty)
                return false;

            return currentFormulaId.Value != previousFormulaId.Value;
        }

        /// <summary>
        /// Lấy công thức VU của MPO trước đó cùng Product để phục vụ so sánh Improvement.
        /// 
        /// MPO trước đó được xác định theo:
        /// - cùng ProductId
        /// - khác MfgProductionOrderId hiện tại
        /// - IsActive = true
        /// - CreatedDate nhỏ hơn MPO hiện tại
        /// - lấy bản ghi gần nhất theo CreatedDate giảm dần
        /// </summary>
        /// <param name="productId">Id của Product.</param>
        /// <param name="currentMfgProductionOrderId">Id MPO hiện tại.</param>
        /// <param name="currentCreatedDate">Ngày tạo MPO hiện tại.</param>
        /// <returns>
        /// Tuple gồm FormulaId và FormulaExternalIdSnapshot của MPO trước đó; null nếu không có.
        /// </returns>
        private async Task<(Guid? FormulaId, string? FormulaExternalIdSnapshot)?> GetPreviousVuFormulaAsync(
            Guid productId,
            Guid currentMfgProductionOrderId,
            DateTime currentCreatedDate)
        {
            var previous = await _unitOfWork.MfgProductionOrderRepository
                .Query(false)
                .Where(x =>
                    x.ProductId == productId &&
                    x.MfgProductionOrderId != currentMfgProductionOrderId &&
                    x.IsActive &&
                    x.CreatedDate < currentCreatedDate)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new
                {
                    x.FormulaId,
                    x.FormulaExternalIdSnapshot
                })
                .FirstOrDefaultAsync();

            if (previous == null) return null;
            return (previous.FormulaId, previous.FormulaExternalIdSnapshot);
        }

        /// <summary>
        /// Tạo một dòng summary loại Improvement từ công thức VU hiện tại của MPO.
        /// 
        /// Dòng này được chèn lên đầu danh sách khi phát hiện công thức VU hiện tại
        /// khác công thức VU của MPO trước đó cùng Product.
        /// </summary>
        /// <param name="mfgProductionOrderId">Id của MPO hiện tại.</param>
        /// <param name="formulaId">Id công thức VU hiện tại.</param>
        /// <param name="formulaExternalIdSnapshot">ExternalId snapshot của công thức VU hiện tại.</param>
        /// <param name="mpoCreatedDate">Ngày tạo MPO để fallback nếu Formula không còn tồn tại.</param>
        /// <returns>Summary loại Improvement; null nếu formulaId không hợp lệ.</returns>
        private async Task<GetMfgProductionOrderRWSummary?> BuildImprovementSummaryAsync(
            Guid mfgProductionOrderId,
            Guid? formulaId,
            string? formulaExternalIdSnapshot,
            DateTime mpoCreatedDate)
        {
            if (formulaId == null || formulaId == Guid.Empty)
                return null;

            var formulaInfo = await _unitOfWork.FormulaRepository
                .Query(false)
                .Where(x => x.FormulaId == formulaId.Value)
                .Select(x => new
                {
                    x.FormulaId,
                    x.ExternalId,
                    x.Note,
                    x.CreatedDate
                })
                .FirstOrDefaultAsync();

            return new GetMfgProductionOrderRWSummary
            {
                MfgProductionOrderId = mfgProductionOrderId,
                FormulaType = FormulaType.Improvement,
                ExternalId = formulaInfo?.ExternalId ?? formulaExternalIdSnapshot,
                CreatedDate = formulaInfo?.CreatedDate ?? mpoCreatedDate,
                Note = formulaInfo?.Note,
                Id = formulaId.Value
            };
        }
    }
}
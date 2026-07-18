using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using System.Text.Json;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulaAdjustments;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgFormulaAdjustments;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
using VietausWebAPI.Core.Application.Features.Notifications.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Enums.Notifications;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Services
{
    public partial class MfgAdjustmenService : IMfgAdjustmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IManufacturingFormulaAdjustmentRepository _repository;
        private readonly IMfgProductionOrderRepository _mfgProductionOrderRepository;
        private readonly IManufacturingFormulaRepository _manufacturingFormulaRepository;
        private readonly ICurrentUser _currentUser;
        private readonly INotificationService _notificationService;
        private readonly Guid _groupId = Guid.Parse("115f897a-535b-42e9-9a62-d00ac6fcdeae");
        private readonly Guid _PLPUPlan = Guid.Parse("81b58984-c3da-4fab-8faa-b28bc20738aa");

        public MfgAdjustmenService(
            IUnitOfWork unitOfWork,
            IManufacturingFormulaAdjustmentRepository repository,
            IMfgProductionOrderRepository mfgProductionOrderRepository,
            IManufacturingFormulaRepository manufacturingFormulaRepository,
            ICurrentUser currentUser,
            INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mfgProductionOrderRepository = mfgProductionOrderRepository;
            _manufacturingFormulaRepository = manufacturingFormulaRepository;
            _currentUser = currentUser;
            _notificationService = notificationService;
        }

        public async Task<OperationResult<GetManufacturingFormulaAdjustment>> CreateAsync(
            PostManufacturingFormulaAdjustment request,
            CancellationToken ct = default)
        {
            var validation = await ValidateCreateAsync(request, ct);
            if (!validation.Success)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail(validation.Message ?? "Du lieu khong hop le.");

            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId;
            var adjustmentId = Guid.CreateVersion7();

            var entity = MapAdjustment(request, adjustmentId, now, userId);

            await _repository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            await EnsureNotificationAsync(entity, _groupId, ct);

            var data = await GetByIdAsync(entity.ManufacturingFormulaAdjustmentId, ct);
            return data == null
                ? OperationResult<GetManufacturingFormulaAdjustment>.Fail("Tao thanh cong nhung khong lay lai duoc du lieu.")
                : OperationResult<GetManufacturingFormulaAdjustment>.Ok(data, "Da tao phieu chinh mau.");
        }

        public async Task<OperationResult<GetManufacturingFormulaAdjustment>> PatchAsync(
            Guid id,
            PatchManufacturingFormulaAdjustment request,
            CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Id khong hop le.");

            if (request == null)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Request khong duoc de trong.");

            var entity = await _repository.GetTrackedWithBatchesOnlyAsync(id, _currentUser.CompanyId, ct);
            if (entity == null)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Khong tim thay phieu chinh mau.");

            var validation = await ValidatePatchAsync(request, ct);
            if (!validation.Success)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail(validation.Message ?? "Du lieu khong hop le.");

            ApplyHeaderPatch(entity, request);

            if (request.Batches != null)
                ReplaceBatches(entity, request.Batches);

            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedBy = _currentUser.EmployeeId;

            await _unitOfWork.SaveChangesAsync(ct);
            await EnsureNotificationAsync(entity, _groupId, ct);
            var data = await GetByIdAsync(id, ct);
            return data == null
                ? OperationResult<GetManufacturingFormulaAdjustment>.Fail("Cap nhat thanh cong nhung khong lay lai duoc du lieu.")
                : OperationResult<GetManufacturingFormulaAdjustment>.Ok(data, "Da cap nhat phieu chinh mau.");
        }

        public async Task<GetManufacturingFormulaAdjustment?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return null;

            return await ProjectToDto(_repository.Query())
                .FirstOrDefaultAsync(x => x.ManufacturingFormulaAdjustmentId == id, ct);
        }

        public async Task<OperationResult<GetManufacturingFormulaAdjustment>> AddBatchAsync(
            Guid id,
            PostManufacturingFormulaAdjustmentBatch request,
            CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Id khong hop le.");

            if (request == null)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Request khong duoc de trong.");

            var entity = await _repository.GetTrackedWithBatchesOnlyAsync(id, _currentUser.CompanyId, ct);
            if (entity == null)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Khong tim thay phieu chinh mau.");

            var validation = await ValidateReferencesAndBatchesAsync(entity.ManufacturingFormulaId, new() { request }, ct);
            if (!validation.Success)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail(validation.Message ?? "Du lieu khong hop le.");

            var existingBatch = entity.Batches.FirstOrDefault(x =>
                x.IsActive &&
                x.BatchNo == request.BatchNo &&
                x.TrialNo == request.TrialNo);

            if (existingBatch != null)
            {
                await ApplyBatchPatchAsync(existingBatch, request, ct);
                await _unitOfWork.SaveChangesAsync(ct);

                var updatedData = await GetByIdAsync(id, ct);
                return updatedData == null
                    ? OperationResult<GetManufacturingFormulaAdjustment>.Fail("Cap nhat me thanh cong nhung khong lay lai duoc du lieu.")
                    : OperationResult<GetManufacturingFormulaAdjustment>.Ok(updatedData, "Da cap nhat me chinh mau.");
            }

            var batch = MapBatch(
                request,
                entity.ManufacturingFormulaAdjustmentId,
                entity.Batches.Count + 1,
                DateTime.Now,
                _currentUser.EmployeeId);

            await _repository.AddBatchAsync(batch, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            var data = await GetByIdAsync(id, ct);
            return data == null
                ? OperationResult<GetManufacturingFormulaAdjustment>.Fail("Them me thanh cong nhung khong lay lai duoc du lieu.")
                : OperationResult<GetManufacturingFormulaAdjustment>.Ok(data, "Da them me chinh mau.");
        }

        public async Task<OperationResult<GetManufacturingFormulaAdjustment>> AddBatchRangeAsync(
            Guid id,
            PostManufacturingFormulaAdjustmentBatchRange request,
            CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Id khong hop le.");

            if (request == null)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Request khong duoc de trong.");

            if (request.BatchFrom <= 0)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("BatchFrom phai lon hon 0.");

            if (request.BatchTo < request.BatchFrom)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("BatchTo phai lon hon hoac bang BatchFrom.");

            if (request.BatchTo - request.BatchFrom + 1 > 100)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Chi duoc tao toi da 100 me moi lan.");

            var entity = await _repository.GetTrackedWithBatchesOnlyAsync(id, _currentUser.CompanyId, ct);
            if (entity == null)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Khong tim thay phieu chinh mau.");

            var batches = Enumerable.Range(request.BatchFrom, request.BatchTo - request.BatchFrom + 1)
                .Select(batchNo => ToBatchRequest(request, batchNo))
                .ToList();

            var validation = await ValidateReferencesAndBatchesAsync(entity.ManufacturingFormulaId, batches, ct);
            if (!validation.Success)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail(validation.Message ?? "Du lieu khong hop le.");

            var existingBatchNos = entity.Batches
                .Where(x =>
                    x.IsActive &&
                    x.TrialNo == request.TrialNo &&
                    x.BatchNo >= request.BatchFrom &&
                    x.BatchNo <= request.BatchTo)
                .Select(x => x.BatchNo)
                .OrderBy(x => x)
                .ToList();

            if (existingBatchNos.Count > 0)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail(
                    $"Cac me da ton tai trong phieu: {string.Join(", ", existingBatchNos)}.");

            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId;
            foreach (var batchRequest in batches)
            {
                var batch = MapBatch(
                    batchRequest,
                    entity.ManufacturingFormulaAdjustmentId,
                    batchRequest.BatchNo,
                    now,
                    userId);

                await _repository.AddBatchAsync(batch, ct);
            }

            await _unitOfWork.SaveChangesAsync(ct);
            await EnsureNotificationAsync(entity, _groupId, ct);

            var data = await GetByIdAsync(id, ct);
            return data == null
                ? OperationResult<GetManufacturingFormulaAdjustment>.Fail("Them day me thanh cong nhung khong lay lai duoc du lieu.")
                : OperationResult<GetManufacturingFormulaAdjustment>.Ok(data, "Da them day me chinh mau.");
        }

        public async Task<OperationResult<GetManufacturingFormulaAdjustment>> PatchBatchAsync(
            Guid id,
            Guid batchId,
            PostManufacturingFormulaAdjustmentBatchRange request,
            CancellationToken ct = default)
        {
            if (id == Guid.Empty || batchId == Guid.Empty)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Id khong hop le.");

            if (request == null)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Request khong duoc de trong.");

            if (request.BatchFrom <= 0)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("BatchFrom phai lon hon 0.");

            if (request.BatchTo < request.BatchFrom)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("BatchTo phai lon hon hoac bang BatchFrom.");

            if (request.BatchTo - request.BatchFrom + 1 > 100)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Chi duoc cap nhat toi da 100 me moi lan.");

            var entity = await _repository.GetTrackedWithBatchesAsync(id, _currentUser.CompanyId, ct);
            if (entity == null)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Khong tim thay phieu chinh mau.");

            var batch = entity.Batches.FirstOrDefault(x =>
                x.ManufacturingFormulaAdjustmentBatchId == batchId &&
                x.IsActive);

            if (batch == null)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Khong tim thay me chinh mau.");

            var batches = Enumerable.Range(request.BatchFrom, request.BatchTo - request.BatchFrom + 1)
                .Select(batchNo => ToBatchRequest(request, batchNo))
                .ToList();

            var validation = await ValidateReferencesAndBatchesAsync(entity.ManufacturingFormulaId, batches, ct);
            if (!validation.Success)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail(validation.Message ?? "Du lieu khong hop le.");

            var oldGroup = FindContiguousBatchGroup(entity.Batches.Where(x => x.IsActive).ToList(), batch);
            var oldGroupIds = oldGroup
                .Select(x => x.ManufacturingFormulaAdjustmentBatchId)
                .ToHashSet();

            var newBatchNos = batches
                .Select(x => x.BatchNo)
                .ToHashSet();

            var conflicts = entity.Batches
                .Where(x =>
                    x.IsActive &&
                    !oldGroupIds.Contains(x.ManufacturingFormulaAdjustmentBatchId) &&
                    x.TrialNo == request.TrialNo &&
                    newBatchNos.Contains(x.BatchNo))
                .Select(x => x.BatchNo)
                .OrderBy(x => x)
                .ToList();

            if (conflicts.Count > 0)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail(
                    $"Cac me da ton tai ngoai nhom dang sua: {string.Join(", ", conflicts)}.");

            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId;

            foreach (var oldBatch in oldGroup.Where(x => !newBatchNos.Contains(x.BatchNo)))
            {
                oldBatch.IsActive = false;
                oldBatch.UpdatedDate = now;
                oldBatch.UpdatedBy = userId;
            }

            foreach (var batchRequest in batches)
            {
                var existingBatch = oldGroup.FirstOrDefault(x => x.BatchNo == batchRequest.BatchNo);
                if (existingBatch != null)
                {
                    await ApplyBatchPatchAsync(existingBatch, batchRequest, ct);
                    continue;
                }

                var newBatch = MapBatch(
                    batchRequest,
                    entity.ManufacturingFormulaAdjustmentId,
                    batchRequest.BatchNo,
                    now,
                    userId);

                await _repository.AddBatchAsync(newBatch, ct);
            }

            entity.UpdatedDate = now;
            entity.UpdatedBy = userId;

            await _unitOfWork.SaveChangesAsync(ct);
            await EnsureNotificationAsync(entity, _groupId, ct);

            var data = await GetByIdAsync(id, ct);
            return data == null
                ? OperationResult<GetManufacturingFormulaAdjustment>.Fail("Cap nhat me thanh cong nhung khong lay lai duoc du lieu.")
                : OperationResult<GetManufacturingFormulaAdjustment>.Ok(data, "Da cap nhat me chinh mau.");
        }

        public async Task<OperationResult<GetManufacturingFormulaAdjustment>> DeleteBatchAsync(
            Guid id,
            Guid batchId,
            CancellationToken ct = default)
        {
            if (id == Guid.Empty || batchId == Guid.Empty)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Id khong hop le.");

            var entity = await _repository.GetTrackedWithBatchesAsync(id, _currentUser.CompanyId, ct);
            if (entity == null)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Khong tim thay phieu chinh mau.");

            var batch = entity.Batches.FirstOrDefault(x =>
                x.ManufacturingFormulaAdjustmentBatchId == batchId &&
                x.IsActive);

            if (batch == null)
                return OperationResult<GetManufacturingFormulaAdjustment>.Fail("Khong tim thay me chinh mau.");

            batch.IsActive = false;
            batch.UpdatedDate = DateTime.Now;
            batch.UpdatedBy = _currentUser.EmployeeId;

            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedBy = _currentUser.EmployeeId;

            await _unitOfWork.SaveChangesAsync(ct);

            var data = await GetByIdAsync(id, ct);
            return data == null
                ? OperationResult<GetManufacturingFormulaAdjustment>.Fail("Xoa me thanh cong nhung khong lay lai duoc du lieu.")
                : OperationResult<GetManufacturingFormulaAdjustment>.Ok(data, "Da xoa me chinh mau.");
        }

        public async Task<List<GetManufacturingFormulaAdjustment>> GetAllAsync(
            ManufacturingFormulaAdjustmentQuery query,
            CancellationToken ct = default)
        {
            query ??= new ManufacturingFormulaAdjustmentQuery();

            var db = ApplyQuery(_repository.Query(), query);

            return await ProjectToDto(db)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync(ct);
        }

        public async Task<OperationResult> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return OperationResult.Fail("Id khong hop le.");

            var entity = await _repository.GetTrackedAsync(id, _currentUser.CompanyId, ct);
            if (entity == null)
                return OperationResult.Fail("Khong tim thay phieu chinh mau.");

            entity.IsActive = false;
            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedBy = _currentUser.EmployeeId;

            await _unitOfWork.SaveChangesAsync(ct);
            return OperationResult.Ok("Da xoa phieu chinh mau.");
        }


        // ===================================================== Helper 

        private async Task EnsureNotificationAsync(
            ManufacturingFormulaAdjustment adjustment,
            Guid? notifyGroupId,
            CancellationToken ct = default)
        {
            if (!notifyGroupId.HasValue || notifyGroupId.Value == Guid.Empty)
                return;

            var groupId = notifyGroupId.Value;

            var productionOrderExternalId = await _mfgProductionOrderRepository.Query(track: false)
                .Where(x => x.MfgProductionOrderId == adjustment.MfgProductionOrderId)
                .Select(x => x.ExternalId)
                .FirstOrDefaultAsync(ct);

            var payload = new
            {
                manufacturingFormulaAdjustmentId = adjustment.ManufacturingFormulaAdjustmentId,
                mfgProductionOrderId = adjustment.MfgProductionOrderId,
                mfgProductionOrderExternalId = productionOrderExternalId,
                manufacturingFormulaId = adjustment.ManufacturingFormulaId,
                notifyGroupId = groupId,
                batchCount = adjustment.Batches.Count(x => x.IsActive),
                createdBy = _currentUser.EmployeeId,
                createdByName = _currentUser.personName,
                createdDate = adjustment.CreatedDate
            };

            var targetGroupIds = new[] { notifyGroupId, _PLPUPlan }
    .Where(x => x.HasValue && x.Value != Guid.Empty)
    .Select(x => x!.Value)
    .Distinct()
    .ToList();

            var targetUserIds = await _unitOfWork.MemberInGroupRepository.Query()
                .Where(x =>
                    targetGroupIds.Contains(x.GroupId) &&
                    x.IsActive &&
                    x.Profile.HasValue &&
                    x.ProfileNavigation != null &&
                    x.ProfileNavigation.CompanyId == _currentUser.CompanyId &&
                    x.ProfileNavigation.IsActive == true)
                .Select(x => x.Profile!.Value)
                .Distinct()
                .ToListAsync(ct);

            var orderText = string.IsNullOrWhiteSpace(productionOrderExternalId)
                ? string.Empty
                : $" cho lenh san xuat {productionOrderExternalId}";

            await _notificationService.PublishAsync(new PublishNotificationRequest
            {
                CompanyId = _currentUser.CompanyId,
                CreatedBy = _currentUser.EmployeeId,
                CreatedByNameSnapshot = _currentUser.personName,
                Topic = TopicNotifications.ManufacturingFormulaAdjustmentCreated,
                Severity = NotificationSeverity.Info,
                Title = "Phiếu chỉnh màu được cập nhật",
                Message = $"{_currentUser.personName} đã tạo phiểu chỉnh màu {orderText}.",
                Link = $"/plpu/mfgformulaadjustments/{adjustment.ManufacturingFormulaAdjustmentId}",
                PayloadJson = JsonSerializer.Serialize(payload),
                TargetUserIds = targetUserIds
            }, ct);
        }
    }
}

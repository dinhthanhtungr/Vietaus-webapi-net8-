using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Posts;
using VietausWebAPI.Core.Application.Features.Sales.Helpers.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerCrmFeatures
{
    public partial class CustomerCrmService
    {
        private const string CustomerInteractionAiPromptVersion = "v1";
        private const int CustomerInteractionAiMaxItems = 80;
        private const int CustomerInteractionAiBatchMaxCustomers = 100;
        private const int CustomerInteractionAiBatchChunkSize = 10;

        /// <summary>
        /// Táº¡o hoáº·c táº¡o láº¡i báº£n tÃ³m táº¯t AI cho lá»‹ch sá»­ tÆ°Æ¡ng tÃ¡c cá»§a má»™t khÃ¡ch hÃ ng trong má»™t khoáº£ng thá»i gian xÃ¡c Ä‘á»‹nh.
        /// Method nÃ y kiá»ƒm tra quyá»n xem CRM, xÃ¡c Ä‘á»‹nh sale trong pháº¡m vi Ä‘Æ°á»£c phÃ©p, láº¥y lá»‹ch sá»­ tÆ°Æ¡ng tÃ¡c,
        /// láº¥y summary trÆ°á»›c Ä‘Ã³ lÃ m ngá»¯ cáº£nh, gá»i Gemini Ä‘á»ƒ sinh summary má»›i vÃ  lÆ°u káº¿t quáº£ vÃ o báº£ng AI summary.
        /// </summary>
        /// <param name="customerId">Id cá»§a khÃ¡ch hÃ ng cáº§n táº¡o tÃ³m táº¯t AI.</param>
        /// <param name="request">
        /// ThÃ´ng tin yÃªu cáº§u táº¡o summary, bao gá»“m scope Monthly/Yearly/Lifetime/CustomRange,
        /// nÄƒm, thÃ¡ng, khoáº£ng ngÃ y, sale cáº§n lá»c vÃ  tÃ¹y chá»n ForceRegenerate.
        /// </param>
        /// <param name="ct">Cancellation token dÃ¹ng Ä‘á»ƒ há»§y tÃ¡c vá»¥ báº¥t Ä‘á»“ng bá»™.</param>
        /// <returns>
        /// Tráº£ vá» <see cref="OperationResult{T}"/> chá»©a <see cref="CustomerInteractionAiSummaryDto"/>.
        /// Náº¿u Ä‘Ã£ cÃ³ summary thÃ nh cÃ´ng vÃ  khÃ´ng yÃªu cáº§u táº¡o láº¡i, method tráº£ vá» summary hiá»‡n cÃ³.
        /// Náº¿u khÃ´ng cÃ³ tÆ°Æ¡ng tÃ¡c trong ká»³, method lÆ°u tráº¡ng thÃ¡i skipped.
        /// Náº¿u gá»i AI lá»—i, method lÆ°u lá»—i vÃ o AiErrorMessage.
        /// </returns>
        public async Task<OperationResult<CustomerInteractionAiSummaryDto>> GenerateCustomerInteractionAiSummaryAsync(
            Guid customerId,
            GenerateCustomerInteractionAiSummaryRequest request,
            CancellationToken ct = default)
        {
            request ??= new GenerateCustomerInteractionAiSummaryRequest();

            var now = DateTime.Now;
            var companyId = _currentUser.CompanyId;
            var employeeId = _currentUser.EmployeeId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var customer = await _unitOfWork.CustomerRepository.Query()
                .FirstOrDefaultAsync(x =>
                    x.CustomerId == customerId
                    && x.CompanyId == companyId
                    && x.IsActive == true
                    && visibleCustomerIds.Contains(x.CustomerId),
                    ct);

            if (customer == null)
                return OperationResult<CustomerInteractionAiSummaryDto>.Fail("Không tìm thấy khách hàng.");

            var scopedSaleEmployeeId = ResolveScopedEmployeeId(viewer, request.SaleEmployeeId, onlyMine: false);
            if (scopedSaleEmployeeId == Guid.Empty)
                return OperationResult<CustomerInteractionAiSummaryDto>.Fail("Bạn không có quyền tạo tóm tắt cho sale ngoài phạm vi xem.");

            var (periodFrom, periodToExclusive, year, month) = ResolveAiSummaryPeriod(request, now);
            var periodTo = periodToExclusive.AddTicks(-1);

            var interactionQ = _interactionRepository.Query()
                .Where(x =>
                    x.CompanyId == companyId
                    && x.CustomerId == customerId
                    && x.IsActive
                    && x.InteractionAt >= periodFrom
                    && x.InteractionAt < periodToExclusive);

            //if (scopedSaleEmployeeId.HasValue)
            //    interactionQ = interactionQ.Where(x => x.AssignedSaleEmployeeId == scopedSaleEmployeeId.Value || x.CreatedBy == scopedSaleEmployeeId.Value);

            var interactionCount = await interactionQ.CountAsync(ct);

            var existingSummary = await _aiSummaryRepository.Query(track: true)
                .FirstOrDefaultAsync(x =>
                    x.CompanyId == companyId
                    && x.CustomerId == customerId
                    && x.SaleEmployeeId == scopedSaleEmployeeId
                    && x.SummaryScope == request.SummaryScope
                    && x.PeriodFrom == periodFrom
                    && x.PeriodTo == periodTo
                    && x.IsActive,
                    ct);

            if (existingSummary != null && existingSummary.IsAiSuccess && !request.ForceRegenerate)
            {
                var existingDto = MapAiSummaryDto(existingSummary);
                existingDto.RateLimit = _geminiRateLimitService.GetCurrent(_geminiCustomerSummaryClient.Model);
                return OperationResult<CustomerInteractionAiSummaryDto>.Ok(existingDto, "ÄÃ£ cÃ³ tÃ³m táº¯t AI trong ká»³, khÃ´ng gá»i Gemini láº¡i.");
            }

            var previousSummary = await _aiSummaryRepository.Query()
                .Where(x =>
                    x.CompanyId == companyId
                    && x.CustomerId == customerId
                    && x.SaleEmployeeId == scopedSaleEmployeeId
                    && x.IsActive
                    && x.IsAiSuccess
                    && x.PeriodTo < periodFrom)
                .OrderByDescending(x => x.PeriodTo)
                .Select(x => x.Summary)
                .FirstOrDefaultAsync(ct) ?? string.Empty;

            var summary = existingSummary ?? new CustomerInteractionAiSummary
            {
                Id = Guid.CreateVersion7(),
                CustomerId = customerId,
                SaleEmployeeId = scopedSaleEmployeeId,
                CompanyId = companyId,
                SummaryScope = request.SummaryScope,
                Year = year,
                Month = month,
                PeriodFrom = periodFrom,
                PeriodTo = periodTo,
                CreatedDate = now,
                CreatedBy = employeeId,
                IsActive = true
            };

            summary.Year = year;
            summary.Month = month;
            summary.PeriodFrom = periodFrom;
            summary.PeriodTo = periodTo;
            summary.InteractionCount = interactionCount;
            summary.PreviousSummary = previousSummary;
            summary.SourceModel = _geminiCustomerSummaryClient.Model;
            summary.PromptVersion = CustomerInteractionAiPromptVersion;
            summary.UpdatedDate = existingSummary == null ? null : now;
            summary.UpdatedBy = existingSummary == null ? null : employeeId;

            if (interactionCount == 0)
            {
                summary.Summary = "Không có lịch sử tương tác trong kỳ.";
                summary.CustomerNeed = string.Empty;
                summary.CurrentStage = string.Empty;
                summary.NextAction = string.Empty;
                summary.Risk = string.Empty;
                summary.Sentiment = "Neutral";
                summary.IsAiSuccess = false;
                summary.IsAiSkipped = true;
                summary.AiErrorMessage = "Không có lịch sử tương tác trong kỳ.";
                summary.AiGeneratedDate = now;

                if (existingSummary == null)
                    await _aiSummaryRepository.AddAsync(summary, ct);

                await _unitOfWork.SaveChangesAsync(ct);
                var skippedDto = MapAiSummaryDto(summary);
                skippedDto.RateLimit = _geminiRateLimitService.GetCurrent(_geminiCustomerSummaryClient.Model);
                return OperationResult<CustomerInteractionAiSummaryDto>.Ok(
                    MapAiSummaryDto(summary),
                    "Không có lịch sử tương tác trong kỳ để tóm tắt.");
            }

            var interactions = await interactionQ
                .OrderByDescending(x => x.InteractionAt)
                .Take(CustomerInteractionAiMaxItems)
                .OrderBy(x => x.InteractionAt)
                .Select(x => new CustomerInteractionPromptRow
                {
                    InteractionAt = x.InteractionAt,
                    InteractionType = x.InteractionType,
                    Subject = x.Subject,
                    Content = x.Content,
                    Outcome = x.Outcome,
                    NextAction = x.NextAction,
                    AssignedSaleName = x.AssignedSaleEmployee != null ? x.AssignedSaleEmployee.FullName : null,
                    CreatedByName = x.CreatedByNavigation.FullName
                })
                .ToListAsync(ct);

            var rateLimit = _geminiRateLimitService.TryConsume(_geminiCustomerSummaryClient.Model);
            if (!rateLimit.CanRequest)
            {
                var rateLimitedDto = MapAiSummaryDto(summary);
                rateLimitedDto.CustomerCode = customer.ExternalId ?? string.Empty;
                rateLimitedDto.CustomerName = customer.CustomerName ?? string.Empty;
                rateLimitedDto.RateLimit = rateLimit;

                return OperationResult<CustomerInteractionAiSummaryDto>.Fail(
                    rateLimitedDto,
                    rateLimit.Message);
            }

            try
            {
                var prompt = BuildCustomerInteractionAiPrompt(customer, previousSummary, interactions, request.SummaryScope, periodFrom, periodTo);
                var aiResult = await _geminiCustomerSummaryClient.GenerateSummaryAsync(prompt, ct);

                summary.Summary = NormalizeAiField(aiResult.Summary);
                summary.CustomerNeed = NormalizeAiField(aiResult.CustomerNeed);
                summary.CurrentStage = NormalizeAiField(aiResult.CurrentStage);
                summary.NextAction = NormalizeAiField(aiResult.NextAction);
                summary.Risk = NormalizeAiField(aiResult.Risk);
                summary.Sentiment = NormalizeAiField(aiResult.Sentiment);
                summary.SourceModel = _geminiCustomerSummaryClient.Model;
                summary.PromptVersion = CustomerInteractionAiPromptVersion;
                summary.IsAiSuccess = true;
                summary.IsAiSkipped = false;
                summary.AiErrorMessage = null;
                summary.AiGeneratedDate = now;
            }
            catch (GeminiRateLimitException ex)
            {
                summary.Summary = string.Empty;
                summary.CustomerNeed = string.Empty;
                summary.CurrentStage = string.Empty;
                summary.NextAction = string.Empty;
                summary.Risk = string.Empty;
                summary.Sentiment = string.Empty;
                summary.IsAiSuccess = false;
                summary.IsAiSkipped = false;
                summary.AiErrorMessage = ex.Message;
                summary.AiGeneratedDate = now;

                if (ex.RetryAfterSeconds.HasValue && ex.RetryAfterSeconds.Value > rateLimit.RetryAfterSeconds)
                {
                    rateLimit.CanRequest = false;
                    rateLimit.RetryAfterSeconds = ex.RetryAfterSeconds.Value;
                    rateLimit.RetryAt = DateTime.Now.AddSeconds(ex.RetryAfterSeconds.Value);
                    rateLimit.Message = $"Gemini đang giới hạn request. Vui lòng thử lại sau {ex.RetryAfterSeconds.Value} giây.";
                }
            }
            catch (Exception ex)
            {
                summary.Summary = string.Empty;
                summary.CustomerNeed = string.Empty;
                summary.CurrentStage = string.Empty;
                summary.NextAction = string.Empty;
                summary.Risk = string.Empty;
                summary.Sentiment = string.Empty;
                summary.IsAiSuccess = false;
                summary.IsAiSkipped = false;
                summary.AiErrorMessage = ex.Message;
                summary.AiGeneratedDate = now;
            }

            if (existingSummary == null)
                await _aiSummaryRepository.AddAsync(summary, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            var dto = await GetAiSummaryDtoQuery()
                .FirstAsync(x => x.Id == summary.Id, ct);
            dto.RateLimit = summary.IsAiSuccess
                ? _geminiRateLimitService.GetCurrent(_geminiCustomerSummaryClient.Model)
                : rateLimit;

            return summary.IsAiSuccess
                ? OperationResult<CustomerInteractionAiSummaryDto>.Ok(dto, "Tạo tóm tắt AI thành công.")
                : OperationResult<CustomerInteractionAiSummaryDto>.Fail(dto, "Tạo tóm tắt AI thất bại.");
        }

        /// <summary>
        /// Láº¥y báº£n tÃ³m táº¯t AI má»›i nháº¥t cá»§a má»™t khÃ¡ch hÃ ng mÃ  ngÆ°á»i dÃ¹ng hiá»‡n táº¡i cÃ³ quyá»n xem.
        /// Káº¿t quáº£ Ä‘Æ°á»£c sáº¯p xáº¿p theo PeriodTo, AiGeneratedDate vÃ  CreatedDate Ä‘á»ƒ Æ°u tiÃªn summary má»›i nháº¥t.
        /// </summary>
        /// <param name="customerId">Id cá»§a khÃ¡ch hÃ ng cáº§n láº¥y summary má»›i nháº¥t.</param>
        /// <param name="ct">Cancellation token dÃ¹ng Ä‘á»ƒ há»§y tÃ¡c vá»¥ báº¥t Ä‘á»“ng bá»™.</param>
        /// <returns>
        /// Tráº£ vá» summary AI má»›i nháº¥t cá»§a khÃ¡ch hÃ ng.
        /// Náº¿u chÆ°a cÃ³ summary hoáº·c ngÆ°á»i dÃ¹ng khÃ´ng cÃ³ quyá»n xem khÃ¡ch hÃ ng, tráº£ vá» káº¿t quáº£ tháº¥t báº¡i.
        /// </returns>
        /// <summary>
        /// Generates AI summaries for multiple customers in one request while storing each customer summary separately.
        /// The method limits batch size to protect Gemini quota and stops early when the local quota is exhausted.
        /// </summary>
        /// <param name="request">Batch request containing customer ids, summary period, optional sale filter, and regenerate flag.</param>
        /// <param name="ct">Cancellation token used to cancel the asynchronous operation.</param>
        /// <returns>A batch result containing one item per processed customer and the current quota state.</returns>
        public async Task<OperationResult<CustomerInteractionAiSummaryBatchDto>> GenerateCustomerInteractionAiSummaryBatchAsync(
            GenerateCustomerInteractionAiSummaryBatchRequest request,
        CancellationToken ct = default)
        {
            request ??= new GenerateCustomerInteractionAiSummaryBatchRequest();

            var now = DateTime.Now;
            var companyId = _currentUser.CompanyId;
            var employeeId = _currentUser.EmployeeId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var customerIds = request.CustomerIds
                .Where(x => x != Guid.Empty)
                .Distinct()
                .Take(CustomerInteractionAiBatchMaxCustomers + 1)
                .ToList();

            if (customerIds.Count == 0)
                return OperationResult<CustomerInteractionAiSummaryBatchDto>.Fail("Vui lòng chọn ít nhất một khách hàng.");

            if (customerIds.Count > CustomerInteractionAiBatchMaxCustomers)
                return OperationResult<CustomerInteractionAiSummaryBatchDto>.Fail($"Chỉ được tạo tối đa {CustomerInteractionAiBatchMaxCustomers} khách hàng mỗi lần.");

            var batch = new CustomerInteractionAiSummaryBatchDto
            {
                TotalRequested = customerIds.Count
            };

            var (periodFrom, periodToExclusive, year, month) = ResolveAiSummaryPeriod(request, now);
            var periodTo = periodToExclusive.AddTicks(-1);
            var pendingItems = new List<CustomerInteractionAiBatchPendingItem>();

            foreach (var customerId in customerIds)
            {
                var customer = await _unitOfWork.CustomerRepository.Query()
                    .FirstOrDefaultAsync(x =>
                        x.CustomerId == customerId
                        && x.CompanyId == companyId
                        && x.IsActive == true
                        && visibleCustomerIds.Contains(x.CustomerId),
                        ct);

                if (customer == null)
                {
                    batch.Items.Add(new CustomerInteractionAiSummaryBatchItemDto
                    {
                        CustomerId = customerId,
                        Success = false,
                        Message = "Không tìm thấy khách hàng."
                    });
                    continue;
                }

                var scopedSaleEmployeeId = await ResolveAiSummarySaleEmployeeIdAsync(viewer, customerId, request.SaleEmployeeId, now, ct);
                if (scopedSaleEmployeeId == Guid.Empty)
                {
                    batch.Items.Add(new CustomerInteractionAiSummaryBatchItemDto
                    {
                        CustomerId = customerId,
                        Success = false,
                        Message = "Bạn không có quyền tạo tóm tắt cho sale ngoài phạm vi xem."
                    });
                    continue;
                }

                var interactionQ = _interactionRepository.Query()
                    .Where(x =>
                        x.CompanyId == companyId
                        && x.CustomerId == customerId
                        && x.IsActive
                        && x.InteractionAt >= periodFrom
                        && x.InteractionAt < periodToExclusive);

                if (scopedSaleEmployeeId.HasValue)
                    interactionQ = interactionQ.Where(x => x.AssignedSaleEmployeeId == scopedSaleEmployeeId.Value || x.CreatedBy == scopedSaleEmployeeId.Value);

                var interactionCount = await interactionQ.CountAsync(ct);

                var existingSummary = await _aiSummaryRepository.Query(track: true)
                    .FirstOrDefaultAsync(x =>
                        x.CompanyId == companyId
                        && x.CustomerId == customerId
                        && x.SaleEmployeeId == scopedSaleEmployeeId
                        && x.SummaryScope == request.SummaryScope
                        && x.PeriodFrom == periodFrom
                        && x.PeriodTo == periodTo
                        && x.IsActive,
                        ct);

                if (existingSummary != null && existingSummary.IsAiSuccess && !request.ForceRegenerate)
                {
                    var existingDto = MapAiSummaryDto(existingSummary);
                    existingDto.CustomerCode = customer.ExternalId ?? string.Empty;
                    existingDto.CustomerName = customer.CustomerName ?? string.Empty;
                    existingDto.RateLimit = _geminiRateLimitService.GetCurrent(_geminiCustomerSummaryClient.Model);

                    batch.Items.Add(new CustomerInteractionAiSummaryBatchItemDto
                    {
                        CustomerId = customerId,
                        Success = true,
                        Message = "Đã có tóm tắt AI trong kỳ, không gọi Gemini lại.",
                        Summary = existingDto
                    });
                    continue;
                }

                var previousSummary = await _aiSummaryRepository.Query()
                    .Where(x =>
                        x.CompanyId == companyId
                        && x.CustomerId == customerId
                        && x.SaleEmployeeId == scopedSaleEmployeeId
                        && x.IsActive
                        && x.IsAiSuccess
                        && x.PeriodTo < periodFrom)
                    .OrderByDescending(x => x.PeriodTo)
                    .Select(x => x.Summary)
                    .FirstOrDefaultAsync(ct) ?? string.Empty;

                var summary = existingSummary ?? new CustomerInteractionAiSummary
                {
                    Id = Guid.CreateVersion7(),
                    CustomerId = customerId,
                    SaleEmployeeId = scopedSaleEmployeeId,
                    CompanyId = companyId,
                    SummaryScope = request.SummaryScope,
                    CreatedDate = now,
                    CreatedBy = employeeId,
                    IsActive = true
                };

                summary.Year = year;
                summary.Month = month;
                summary.PeriodFrom = periodFrom;
                summary.PeriodTo = periodTo;
                summary.InteractionCount = interactionCount;
                summary.PreviousSummary = previousSummary;
                summary.SourceModel = _geminiCustomerSummaryClient.Model;
                summary.PromptVersion = CustomerInteractionAiPromptVersion;
                summary.UpdatedDate = existingSummary == null ? null : now;
                summary.UpdatedBy = existingSummary == null ? null : employeeId;

                if (interactionCount == 0)
                {
                    summary.Summary = "Không có lịch sử tương tác trong kỳ.";
                    summary.CustomerNeed = string.Empty;
                    summary.CurrentStage = string.Empty;
                    summary.NextAction = string.Empty;
                    summary.Risk = string.Empty;
                    summary.Sentiment = "Neutral";
                    summary.IsAiSuccess = false;
                    summary.IsAiSkipped = true;
                    summary.AiErrorMessage = "Không có lịch sử tương tác trong kỳ.";
                    summary.AiGeneratedDate = now;

                    if (existingSummary == null)
                        await _aiSummaryRepository.AddAsync(summary, ct);

                    var skippedDto = MapAiSummaryDto(summary);
                    skippedDto.CustomerCode = customer.ExternalId ?? string.Empty;
                    skippedDto.CustomerName = customer.CustomerName ?? string.Empty;
                    skippedDto.RateLimit = _geminiRateLimitService.GetCurrent(_geminiCustomerSummaryClient.Model);

                    batch.Items.Add(new CustomerInteractionAiSummaryBatchItemDto
                    {
                        CustomerId = customerId,
                        Success = true,
                        Message = "Không có lịch sử tương tác trong kỳ để tóm tắt.",
                        Summary = skippedDto
                    });
                    continue;
                }

                var interactions = await interactionQ
                    .OrderByDescending(x => x.InteractionAt)
                    .Take(CustomerInteractionAiMaxItems)
                    .OrderBy(x => x.InteractionAt)
                    .Select(x => new CustomerInteractionPromptRow
                    {
                        InteractionAt = x.InteractionAt,
                        InteractionType = x.InteractionType,
                        Subject = x.Subject,
                        Content = x.Content,
                        Outcome = x.Outcome,
                        NextAction = x.NextAction,
                        AssignedSaleName = x.AssignedSaleEmployee != null ? x.AssignedSaleEmployee.FullName : null,
                        CreatedByName = x.CreatedByNavigation.FullName
                    })
                    .ToListAsync(ct);

                if (existingSummary == null)
                    await _aiSummaryRepository.AddAsync(summary, ct);

                pendingItems.Add(new CustomerInteractionAiBatchPendingItem
                {
                    Customer = customer,
                    Summary = summary,
                    PreviousSummary = previousSummary,
                    Interactions = interactions
                });
            }

            foreach (var chunk in pendingItems.Chunk(CustomerInteractionAiBatchChunkSize))
            {
                var rateLimit = _geminiRateLimitService.TryConsume(_geminiCustomerSummaryClient.Model);
                if (!rateLimit.CanRequest)
                {
                    foreach (var item in chunk)
                    {
                        ApplyAiSummaryFailure(item.Summary, rateLimit.Message, now);
                        batch.Items.Add(BuildBatchItem(item, false, rateLimit.Message, rateLimit));
                    }

                    break;
                }

                try
                {
                    var prompt = BuildCustomerInteractionAiBatchPrompt(chunk, request.SummaryScope, periodFrom, periodTo);
                    var aiBatchResult = await _geminiCustomerSummaryClient.GenerateBatchSummaryAsync(prompt, ct);
                    var aiItems = aiBatchResult.Items
                        .Where(x => x.CustomerId != Guid.Empty)
                        .GroupBy(x => x.CustomerId)
                        .ToDictionary(x => x.Key, x => x.First());

                    foreach (var item in chunk)
                    {
                        if (!aiItems.TryGetValue(item.Customer.CustomerId, out var aiResult))
                        {
                            ApplyAiSummaryFailure(item.Summary, "Gemini không trả summary cho khách hàng này.", now);
                            batch.Items.Add(BuildBatchItem(item, false, "Tạo tóm tắt AI thất bại.", rateLimit));
                            continue;
                        }

                        item.Summary.Summary = NormalizeAiField(aiResult.Summary);
                        item.Summary.CustomerNeed = NormalizeAiField(aiResult.CustomerNeed);
                        item.Summary.CurrentStage = NormalizeAiField(aiResult.CurrentStage);
                        item.Summary.NextAction = NormalizeAiField(aiResult.NextAction);
                        item.Summary.Risk = NormalizeAiField(aiResult.Risk);
                        item.Summary.Sentiment = NormalizeAiField(aiResult.Sentiment);
                        item.Summary.SourceModel = _geminiCustomerSummaryClient.Model;
                        item.Summary.PromptVersion = CustomerInteractionAiPromptVersion;
                        item.Summary.IsAiSuccess = string.IsNullOrWhiteSpace(aiResult.ErrorMessage);
                        item.Summary.IsAiSkipped = false;
                        item.Summary.AiErrorMessage = NormalizeAiField(aiResult.ErrorMessage);
                        item.Summary.AiGeneratedDate = now;

                        batch.Items.Add(BuildBatchItem(
                            item,
                            item.Summary.IsAiSuccess,
                            item.Summary.IsAiSuccess ? "Tạo tóm tắt AI thành công." : "Tạo tóm tắt AI thất bại.",
                            _geminiRateLimitService.GetCurrent(_geminiCustomerSummaryClient.Model)));
                    }
                }
                catch (GeminiRateLimitException ex)
                {
                    if (ex.RetryAfterSeconds.HasValue && ex.RetryAfterSeconds.Value > rateLimit.RetryAfterSeconds)
                    {
                        rateLimit.CanRequest = false;
                        rateLimit.RetryAfterSeconds = ex.RetryAfterSeconds.Value;
                        rateLimit.RetryAt = DateTime.Now.AddSeconds(ex.RetryAfterSeconds.Value);
                        rateLimit.Message = $"Gemini đang giới hạn request. Vui lòng thử lại sau {ex.RetryAfterSeconds.Value} giây.";
                    }

                    foreach (var item in chunk)
                    {
                        ApplyAiSummaryFailure(item.Summary, rateLimit.Message, now);
                        batch.Items.Add(BuildBatchItem(item, false, rateLimit.Message, rateLimit));
                    }

                    break;
                }
                catch (Exception ex)
                {
                    foreach (var item in chunk)
                    {
                        ApplyAiSummaryFailure(item.Summary, ex.Message, now);
                        batch.Items.Add(BuildBatchItem(item, false, ex.Message, rateLimit));
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync(ct);

            batch.SuccessCount = batch.Items.Count(x => x.Success);
            batch.FailedCount = batch.Items.Count - batch.SuccessCount;
            batch.RateLimit = _geminiRateLimitService.GetCurrent(_geminiCustomerSummaryClient.Model);

            return batch.SuccessCount > 0
                ? OperationResult<CustomerInteractionAiSummaryBatchDto>.Ok(batch, "Tạo tóm tắt AI theo batch hoàn tất.")
                : OperationResult<CustomerInteractionAiSummaryBatchDto>.Fail(batch, "Không tạo được tóm tắt AI cho khách hàng nào.");
        }
        /// <summary>
        /// Gets the latest stored AI summary for a customer without calling Gemini or consuming quota.
        /// </summary>
        /// <param name="customerId">Customer id that owns the AI summary.</param>
        /// <param name="ct">Cancellation token used to cancel the asynchronous operation.</param>
        /// <returns>The latest stored AI summary when the current user can view the customer.</returns>
        public async Task<OperationResult<CustomerInteractionAiSummaryDto>> GetLatestCustomerInteractionAiSummaryAsync(
            Guid customerId,
            CancellationToken ct = default)
        {
            var companyId = _currentUser.CompanyId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var dto = await GetAiSummaryDtoQuery()
                .Where(x =>
                    x.CompanyId == companyId
                    && x.CustomerId == customerId
                    && visibleCustomerIds.Contains(x.CustomerId)
                    && x.IsActive)
                .OrderByDescending(x => x.PeriodTo)
                .ThenByDescending(x => x.AiGeneratedDate)
                .ThenByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync(ct);

            if (dto != null)
                dto.RateLimit = _geminiRateLimitService.GetCurrent(_geminiCustomerSummaryClient.Model);

            return dto == null
                ? OperationResult<CustomerInteractionAiSummaryDto>.Fail("Chưa có tóm tắt AI cho khách hàng này.")
                : OperationResult<CustomerInteractionAiSummaryDto>.Ok(dto);
        }

        /// <summary>
        /// Láº¥y tráº¡ng thÃ¡i quota local hiá»‡n táº¡i cho chá»©c nÄƒng táº¡o tÃ³m táº¯t AI mÃ  khÃ´ng gá»i Gemini vÃ  khÃ´ng tiÃªu tá»‘n request.
        /// FE cÃ³ thá»ƒ dÃ¹ng dá»¯ liá»‡u nÃ y Ä‘á»ƒ hiá»ƒn thá»‹ sá»‘ request cÃ²n láº¡i vÃ  thá»i gian cáº§n chá» trÆ°á»›c khi cho táº¡o láº¡i summary.
        /// </summary>
        /// <param name="ct">Cancellation token dÃ¹ng Ä‘á»ƒ giá»¯ cÃ¹ng signature async vá»›i service contract.</param>
        /// <returns>ThÃ´ng tin RPM/RPD cÃ²n láº¡i vÃ  thá»i Ä‘iá»ƒm cÃ³ thá»ƒ request tiáº¿p.</returns>
        public Task<OperationResult<AiRateLimitInfoDto>> GetCustomerInteractionAiSummaryQuotaAsync(CancellationToken ct = default)
        {
            var quota = _geminiRateLimitService.GetCurrent(_geminiCustomerSummaryClient.Model);
            return Task.FromResult(OperationResult<AiRateLimitInfoDto>.Ok(quota));
        }

        /// <summary>
        /// Resolves the sale employee that should own an AI summary.
        /// When the request specifies a sale, the method validates that sale against the viewer scope.
        /// When the request does not specify a sale, the method uses the active customer assignment first,
        /// then falls back to the latest active non-expired customer claim.
        /// </summary>
        /// <param name="viewer">Current CRM viewer scope used to validate requested sale access.</param>
        /// <param name="customerId">Customer id that is being summarized.</param>
        /// <param name="requestedSaleEmployeeId">Optional sale employee id supplied by the caller.</param>
        /// <param name="now">Current local server time used to validate claim expiration.</param>
        /// <param name="ct">Cancellation token used to cancel database queries.</param>
        /// <returns>The resolved sale employee id, null when the customer has no assignment/claim, or Guid.Empty when access is denied.</returns>
        private async Task<Guid?> ResolveAiSummarySaleEmployeeIdAsync(
            ViewerScope viewer,
            Guid customerId,
            Guid? requestedSaleEmployeeId,
            DateTime now,
            CancellationToken ct)
        {
            if (requestedSaleEmployeeId.HasValue)
                return ResolveScopedEmployeeId(viewer, requestedSaleEmployeeId, onlyMine: false);

            var companyId = _currentUser.CompanyId;

            var assignedSaleId = await _unitOfWork.CustomerAssignmentRepository.Query()
                .Where(x =>
                    x.CompanyId == companyId
                    && x.CustomerId == customerId
                    && x.IsActive)
                .OrderByDescending(x => x.UpdatedDate)
                .ThenByDescending(x => x.CreatedDate)
                .Select(x => (Guid?)x.EmployeeId)
                .FirstOrDefaultAsync(ct);

            if (assignedSaleId.HasValue)
                return ResolveScopedEmployeeId(viewer, assignedSaleId, onlyMine: false);

            var claimedSaleId = await _unitOfWork.CustomerClaimRepository.Query()
                .Where(x =>
                    x.CompanyId == companyId
                    && x.CustomerId == customerId
                    && x.IsActive
                    && x.ExpiresAt >= now)
                .OrderByDescending(x => x.ExpiresAt)
                .Select(x => (Guid?)x.EmployeeId)
                .FirstOrDefaultAsync(ct);

            return claimedSaleId.HasValue
                ? ResolveScopedEmployeeId(viewer, claimedSaleId, onlyMine: false)
                : null;
        }

        private IQueryable<CustomerInteractionAiSummaryDto> GetAiSummaryDtoQuery()
        {
            return _aiSummaryRepository.Query()
                .Select(x => new CustomerInteractionAiSummaryDto
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    CustomerCode = x.Customer.ExternalId,
                    CustomerName = x.Customer.CustomerName,
                    SaleEmployeeId = x.SaleEmployeeId,
                    SaleEmployeeCode = x.SaleEmployee != null ? x.SaleEmployee.ExternalId : null,
                    SaleEmployeeName = x.SaleEmployee != null ? x.SaleEmployee.FullName : null,
                    CompanyId = x.CompanyId,
                    SummaryScope = x.SummaryScope,
                    Year = x.Year,
                    Month = x.Month,
                    PeriodFrom = x.PeriodFrom,
                    PeriodTo = x.PeriodTo,
                    InteractionCount = x.InteractionCount,
                    PreviousSummary = x.PreviousSummary,
                    Summary = x.Summary,
                    CustomerNeed = x.CustomerNeed,
                    CurrentStage = x.CurrentStage,
                    NextAction = x.NextAction,
                    Risk = x.Risk,
                    Sentiment = x.Sentiment,
                    SourceModel = x.SourceModel,
                    PromptVersion = x.PromptVersion,
                    IsAiSuccess = x.IsAiSuccess,
                    IsAiSkipped = x.IsAiSkipped,
                    AiErrorMessage = x.AiErrorMessage,
                    AiGeneratedDate = x.AiGeneratedDate,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate,
                    IsActive = x.IsActive
                });
        }

        private static CustomerInteractionAiSummaryDto MapAiSummaryDto(CustomerInteractionAiSummary summary)
        {
            return new CustomerInteractionAiSummaryDto
            {
                Id = summary.Id,
                CustomerId = summary.CustomerId,
                SaleEmployeeId = summary.SaleEmployeeId,
                CompanyId = summary.CompanyId,
                SummaryScope = summary.SummaryScope,
                Year = summary.Year,
                Month = summary.Month,
                PeriodFrom = summary.PeriodFrom,
                PeriodTo = summary.PeriodTo,
                InteractionCount = summary.InteractionCount,
                PreviousSummary = summary.PreviousSummary,
                Summary = summary.Summary,
                CustomerNeed = summary.CustomerNeed,
                CurrentStage = summary.CurrentStage,
                NextAction = summary.NextAction,
                Risk = summary.Risk,
                Sentiment = summary.Sentiment,
                SourceModel = summary.SourceModel,
                PromptVersion = summary.PromptVersion,
                IsAiSuccess = summary.IsAiSuccess,
                IsAiSkipped = summary.IsAiSkipped,
                AiErrorMessage = summary.AiErrorMessage,
                AiGeneratedDate = summary.AiGeneratedDate,
                CreatedDate = summary.CreatedDate,
                UpdatedDate = summary.UpdatedDate,
                IsActive = summary.IsActive
            };
        }

        private static (DateTime PeriodFrom, DateTime PeriodToExclusive, int? Year, int? Month) ResolveAiSummaryPeriod(
            GenerateCustomerInteractionAiSummaryRequest request,
            DateTime now)
        {
            return ResolveAiSummaryPeriodCore(
                request.SummaryScope,
                request.From,
                request.To,
                request.Year,
                request.Month,
                now);
        }

        private static (DateTime PeriodFrom, DateTime PeriodToExclusive, int? Year, int? Month) ResolveAiSummaryPeriod(
            GenerateCustomerInteractionAiSummaryBatchRequest request,
            DateTime now)
        {
            return ResolveAiSummaryPeriodCore(
                request.SummaryScope,
                request.From,
                request.To,
                request.Year,
                request.Month,
                now);
        }

        private static (DateTime PeriodFrom, DateTime PeriodToExclusive, int? Year, int? Month) ResolveAiSummaryPeriodCore(
            CustomerInteractionSummaryScope summaryScope,
            DateTime? fromDate,
            DateTime? toDate,
            int? requestYear,
            int? requestMonth,
            DateTime now)
        {
            if (summaryScope == CustomerInteractionSummaryScope.CustomRange)
            {
                var from = fromDate?.Date ?? now.Date;
                var toExclusive = toDate?.Date.AddDays(1) ?? from.AddDays(1);

                if (toExclusive <= from)
                    toExclusive = from.AddDays(1);

                return (from, toExclusive, null, null);
            }

            if (summaryScope == CustomerInteractionSummaryScope.Yearly)
            {
                var year = requestYear.GetValueOrDefault(now.Year);

                if (year < 2000 || year > now.Year + 1)
                    year = now.Year;

                var from = new DateTime(year, 1, 1);
                return (from, from.AddYears(1), year, null);
            }

            if (summaryScope == CustomerInteractionSummaryScope.Lifetime)
            {
                var from = fromDate?.Date ?? new DateTime(2026, 1, 1);
                var toExclusive = toDate?.Date.AddDays(1) ?? now.Date.AddDays(1);

                if (toExclusive <= from)
                    toExclusive = now.Date.AddDays(1);

                return (from, toExclusive, null, null);
            }

            var monthlyYear = requestYear.GetValueOrDefault(now.Year);
            var monthlyMonth = requestMonth.GetValueOrDefault(now.Month);

            if (monthlyYear < 2000 || monthlyYear > now.Year + 1 || monthlyMonth < 1 || monthlyMonth > 12)
            {
                monthlyYear = now.Year;
                monthlyMonth = now.Month;
            }

            var monthFrom = new DateTime(monthlyYear, monthlyMonth, 1);
            return (monthFrom, monthFrom.AddMonths(1), monthlyYear, monthlyMonth);
        }

        private CustomerInteractionAiSummaryBatchItemDto BuildBatchItem(
            CustomerInteractionAiBatchPendingItem item,
            bool success,
            string message,
            AiRateLimitInfoDto rateLimit)
        {
            var dto = MapAiSummaryDto(item.Summary);
            dto.CustomerCode = item.Customer.ExternalId ?? string.Empty;
            dto.CustomerName = item.Customer.CustomerName ?? string.Empty;
            dto.RateLimit = rateLimit;

            return new CustomerInteractionAiSummaryBatchItemDto
            {
                CustomerId = item.Customer.CustomerId,
                Success = success,
                Message = message,
                Summary = dto
            };
        }

        private void ApplyAiSummaryFailure(CustomerInteractionAiSummary summary, string? message, DateTime now)
        {
            summary.Summary = string.Empty;
            summary.CustomerNeed = string.Empty;
            summary.CurrentStage = string.Empty;
            summary.NextAction = string.Empty;
            summary.Risk = string.Empty;
            summary.Sentiment = string.Empty;
            summary.SourceModel = _geminiCustomerSummaryClient.Model;
            summary.PromptVersion = CustomerInteractionAiPromptVersion;
            summary.IsAiSuccess = false;
            summary.IsAiSkipped = false;
            summary.AiErrorMessage = string.IsNullOrWhiteSpace(message) ? "Tạo tóm tắt AI thất bại." : message.Trim();
            summary.AiGeneratedDate = now;
        }

        private static string BuildCustomerInteractionAiBatchPrompt(
            IReadOnlyList<CustomerInteractionAiBatchPendingItem> customers,
            CustomerInteractionSummaryScope scope,
            DateTime periodFrom,
            DateTime periodTo)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Bạn là trợ lý CRM cho đội sale.");
            sb.AppendLine("Hãy đọc lịch sử tương tác của nhiều khách hàng và trả về đúng một JSON object hợp lệ.");
            sb.AppendLine("Không thêm markdown, không thêm ```json, không thêm giải thích ngoài JSON.");
            sb.AppendLine("Mỗi item phải tương ứng đúng một customerId được cung cấp. Không được bỏ sót khách hàng.");
            sb.AppendLine();
            sb.AppendLine("Schema bắt buộc:");
            sb.AppendLine("""
            {
              "items": [
                {
                  "customerId": "guid",
                  "summary": "",
                  "customerNeed": "",
                  "currentStage": "",
                  "nextAction": "",
                  "risk": "",
                  "sentiment": "Positive|Neutral|Negative",
                  "errorMessage": ""
                }
              ]
            }
            """);
            sb.AppendLine();
            sb.AppendLine($"Loại tóm tắt: {scope}");
            sb.AppendLine($"Kỳ: {periodFrom:dd-MM-yyyy} đến {periodTo:dd-MM-yyyy HH:mm:ss}");
            sb.AppendLine();

            foreach (var customerItem in customers)
            {
                var customer = customerItem.Customer;
                sb.AppendLine("--- CUSTOMER START ---");
                sb.AppendLine($"customerId: {customer.CustomerId}");
                sb.AppendLine($"Khách hàng: {customer.ExternalId} - {customer.CustomerName}");
                sb.AppendLine($"Tóm tắt trước đó: {(string.IsNullOrWhiteSpace(customerItem.PreviousSummary) ? "Không có" : customerItem.PreviousSummary)}");
                sb.AppendLine("Lịch sử tương tác trong kỳ:");

                foreach (var item in customerItem.Interactions)
                {
                    sb.AppendLine(
                        $"- [{item.InteractionAt:dd-MM-yyyy HH:mm}] " +
                        $"Type={item.InteractionType}; " +
                        $"Sale={item.AssignedSaleName ?? item.CreatedByName}; " +
                        $"Subject={item.Subject ?? ""}; " +
                        $"Content={item.Content ?? ""}; " +
                        $"Outcome={item.Outcome ?? ""}; " +
                        $"NextAction={item.NextAction ?? ""}");
                }

                sb.AppendLine("--- CUSTOMER END ---");
                sb.AppendLine();
            }

            sb.AppendLine("Yêu cầu cho từng khách hàng:");
            sb.AppendLine("- summary: tóm tắt ngắn gọn diễn biến chính trong quá trình chăm sóc khách.");
            sb.AppendLine("- customerNeed: nhu cầu hiện tại của khách; nếu chưa rõ thì ghi \"Chưa đủ thông tin\".");
            sb.AppendLine("- currentStage: format gợi ý: \"Giai đoạn: ...; Mức độ tiếp cận: ...; Căn cứ: ...\".");
            sb.AppendLine("- nextAction: format gợi ý: \"Việc cần làm: ...; Mục tiêu: ...; Thời hạn đề xuất: ...\".");
            sb.AppendLine("- risk: format bắt buộc: \"Rủi ro: ...; Mức độ: Không có/Thấp/Trung bình/Cao; Lý do: ...; Căn cứ: ...; Thời điểm phát hiện: ...\".");
            sb.AppendLine("- sentiment: chỉ một trong ba giá trị: Positive, Neutral, Negative.");
            sb.AppendLine("- errorMessage: để chuỗi rỗng nếu tạo được summary cho khách đó.");
            sb.AppendLine("- Không bịa thông tin không có trong dữ liệu.");
            sb.AppendLine("- Viết nội dung bằng tiếng Việt, nhưng giữ sentiment bằng tiếng Anh theo enum yêu cầu.");
            sb.AppendLine("- Nếu dữ liệu tương tác quá ít, hãy nói rõ là chưa đủ dữ liệu thay vì suy đoán.");

            return sb.ToString();
        }
        /// <summary>
        /// Xây dựng prompt gửi cho Gemini để tóm tắt lịch sử tương tác khách hàng.
        /// Prompt yêu cầu AI trả về đúng một JSON object hợp lệ theo schema cố định,
        /// đồng thời giải thích rõ giai đoạn tiếp cận, rủi ro, căn cứ và hành động tiếp theo.
        /// </summary>
        private static string BuildCustomerInteractionAiPrompt(
            Customer customer,
            string previousSummary,
            IReadOnlyList<CustomerInteractionPromptRow> interactions,
            CustomerInteractionSummaryScope scope,
            DateTime periodFrom,
            DateTime periodTo)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Bạn là trợ lý CRM cho đội sale.");
            sb.AppendLine("Hãy đọc lịch sử tương tác khách hàng và trả về đúng một JSON object hợp lệ.");
            sb.AppendLine("Không thêm markdown, không thêm ```json, không thêm giải thích ngoài JSON.");
            sb.AppendLine();
            sb.AppendLine("Schema bắt buộc:");
            sb.AppendLine("""
            {
              "summary": "",
              "customerNeed": "",
              "currentStage": "",
              "nextAction": "",
              "risk": "",
              "sentiment": "Positive|Neutral|Negative"
            }
            """);
            sb.AppendLine();

            sb.AppendLine($"Khách hàng: {customer.ExternalId} - {customer.CustomerName}");
            sb.AppendLine($"Loại tóm tắt: {scope}");
            sb.AppendLine($"Kỳ: {periodFrom:dd-MM-yyyy} đến {periodTo:dd-MM-yyyy HH:mm:ss}");
            sb.AppendLine($"Tóm tắt trước đó: {(string.IsNullOrWhiteSpace(previousSummary) ? "Không có" : LimitText(previousSummary, 3000))}");
            sb.AppendLine();

            sb.AppendLine("Lịch sử tương tác trong kỳ:");

            foreach (var item in interactions) 
            { 
                sb.AppendLine($"- [{item.InteractionAt:dd-MM-yyyy HH:mm}] " 
                    + $"Type={item.InteractionType}; " 
                    + $"Sale={item.AssignedSaleName ?? item.CreatedByName}; " 
                    + $"Subject={item.Subject ?? ""}; " 
                    + $"Content={item.Content ?? ""}; " 
                    + $"Outcome={item.Outcome ?? ""}; " 
                    + $"NextAction={item.NextAction ?? ""}"); 
            }

            sb.AppendLine();
            sb.AppendLine("Yêu cầu cho từng field:");
            sb.AppendLine("- summary: tóm tắt ngắn gọn diễn biến chính trong quá trình chăm sóc khách.");
            sb.AppendLine("- customerNeed: nhu cầu hiện tại của khách; nếu chưa rõ thì ghi \"Chưa đủ thông tin\".");

            sb.AppendLine("- currentStage: cho biết khách hàng đã tiếp cận đến giai đoạn nào.");
            sb.AppendLine("  Format gợi ý: \"Giai đoạn: ...; Mức độ tiếp cận: ...; Căn cứ: ...\".");
            sb.AppendLine("  Ví dụ giai đoạn: mới tiếp cận, đang tìm hiểu nhu cầu, đang tư vấn, đang gửi mẫu, đang báo giá, đang thương lượng, chờ phản hồi, có khả năng chốt, đã chốt, có nguy cơ mất khách, chưa rõ.");

            sb.AppendLine("- nextAction: hành động sale nên làm tiếp theo, càng cụ thể càng tốt.");
            sb.AppendLine("  Phải nêu hướng xử lý nếu có rủi ro.");
            sb.AppendLine("  Format gợi ý: \"Việc cần làm: ...; Mục tiêu: ...; Thời hạn đề xuất: ...\".");
            sb.AppendLine("  Nếu không có đủ dữ liệu về thời hạn thì ghi \"Thời hạn đề xuất: Chưa rõ\".");

            sb.AppendLine("- risk: nêu rủi ro/chướng ngại nếu có, đồng thời giải thích vì sao xác định rủi ro đó.");
            sb.AppendLine("  Format bắt buộc: \"Rủi ro: ...; Mức độ: Không có/Thấp/Trung bình/Cao; Lý do: ...; Căn cứ: ...; Thời điểm phát hiện: ...\".");
            sb.AppendLine("  Nếu chưa thấy rủi ro rõ ràng thì ghi: \"Rủi ro: Chưa thấy rủi ro rõ ràng; Mức độ: Không có; Lý do: Dữ liệu chưa thể hiện vấn đề đáng chú ý; Căn cứ: Không có; Thời điểm phát hiện: Không có\".");

            sb.AppendLine("- sentiment: chỉ một trong ba giá trị: Positive, Neutral, Negative.");
            sb.AppendLine("  Positive nếu khách phản hồi tích cực, quan tâm rõ, đồng ý bước tiếp theo hoặc có khả năng chốt.");
            sb.AppendLine("  Neutral nếu khách chỉ trao đổi thông tin, đang chờ phản hồi, chưa thể hiện tích cực hoặc tiêu cực rõ.");
            sb.AppendLine("  Negative nếu khách không hài lòng, chê giá/chất lượng/dịch vụ, từ chối, không phản hồi nhiều lần hoặc có dấu hiệu rời bỏ.");

            sb.AppendLine("- Không bịa thông tin không có trong dữ liệu.");
            sb.AppendLine("- Viết nội dung bằng tiếng Việt, nhưng giữ sentiment bằng tiếng Anh theo enum yêu cầu.");
            sb.AppendLine("- Nếu dữ liệu tương tác quá ít, hãy nói rõ là chưa đủ dữ liệu thay vì suy đoán.");
            sb.AppendLine("- Nếu có mâu thuẫn giữa summary trước đó và tương tác mới, ưu tiên dữ liệu tương tác mới hơn.");

            return sb.ToString();
        }

        /// <summary>
        /// Giới hạn độ dài text đưa vào prompt để tránh prompt quá dài và giảm tốn token.
        /// </summary>
        private static string LimitText(string? value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            value = value.Trim();

            return value.Length <= maxLength
                ? value
                : value[..maxLength] + "...";
        }

        private static string NormalizeAiField(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : value.Trim();
        }

        private sealed class CustomerInteractionAiBatchPendingItem
        {
            public Customer Customer { get; set; } = null!;
            public CustomerInteractionAiSummary Summary { get; set; } = null!;
            public string PreviousSummary { get; set; } = string.Empty;
            public IReadOnlyList<CustomerInteractionPromptRow> Interactions { get; set; } = Array.Empty<CustomerInteractionPromptRow>();
        }
        private sealed class CustomerInteractionPromptRow
        {
            public DateTime InteractionAt { get; set; }
            public CustomerInteractionType InteractionType { get; set; }
            public string? Subject { get; set; }
            public string? Content { get; set; }
            public string? Outcome { get; set; }
            public string? NextAction { get; set; }
            public string? AssignedSaleName { get; set; }
            public string CreatedByName { get; set; } = string.Empty;
        }
    }
}





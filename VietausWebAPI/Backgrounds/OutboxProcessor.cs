using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Domain.Entities.Notifications;
using VietausWebAPI.WebAPI.Backgrounds.Helpers;
using VietausWebAPI.WebAPI.Hubs;

namespace VietausWebAPI.WebAPI.Background
{
    public sealed class OutboxProcessor : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<OutboxProcessor> _logger;

        public OutboxProcessor(IServiceProvider sp, ILogger<OutboxProcessor> logger)
            => (_sp, _logger) = (sp, logger);

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            // vòng lặp
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    using var scope = _sp.CreateScope();
                    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var hub = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();

                    // Lấy batch outbox chưa xử lý
                    var batch = uow.OutboxMessages.Query(track: true)
                                 .Where(m => m.ProcessedAt == null)
                                 .OrderBy(m => m.CreatedAt)
                                 .Take(50)                              // batch size tùy bạn
                                 .ToListSafe();                         // helper bên dưới

                    if (batch.Count == 0)
                    {
                        await Task.Delay(800, ct);
                        continue;
                    }

                    foreach (var msg in batch)
                    {
                        try
                        {
                            switch (msg.Type)
                            {
                                case "InAppPush": // I/O ra ngoài SignalR
                                    {
                                        var env = JsonSerializer.Deserialize<OutboxEnvelope>(msg.PayloadJson);
                                        if (env is null || env.NotificationId is null)
                                            throw new InvalidOperationException("Envelope null hoặc thiếu NotificationId");

                                        var data = new { NotificationId = env.NotificationId.Value };

                                        // Gửi theo Role
                                        if (env.TargetRoles?.Count > 0)
                                        {
                                            var cKey = env.CompanyId.ToString("N");
                                            foreach (var role in env.TargetRoles ?? Enumerable.Empty<string>())
                                            {
                                                var rKey = role.Trim().ToUpperInvariant();
                                                await hub.Clients.Group($"role:{cKey}:{rKey}")
                                                    .SendAsync("notify", new { NotificationId = env.NotificationId!.Value }, ct);
                                            }
                                        }
                                        // Gửi theo User
                                        if (env.TargetUserIds?.Count > 0)
                                        {
                                            foreach (var uid in env.TargetUserIds.Where(g => g != Guid.Empty))
                                            {
                                                await hub.Clients.Group($"user:{uid}")
                                                    .SendAsync("notify", data, ct);
                                            }
                                        }

                                        // Gửi theo Team
                                        if (env.TargetTeamIds?.Count > 0)
                                        {
                                            foreach (var tid in env.TargetTeamIds.Where(g => g != Guid.Empty))
                                            {
                                                await hub.Clients.Group($"team:{tid}")
                                                    .SendAsync("notify", data, ct);
                                            }
                                        }

                                        // Composite (nếu bạn dùng)
                                        if (!string.IsNullOrWhiteSpace(env.TargetComposite))
                                        {
                                            await hub.Clients.Group($"combo:{env.CompanyId}:{env.TargetComposite}")
                                                .SendAsync("notify", data, ct);
                                        }

                                        // Hoàn tất
                                        msg.ProcessedAt = DateTime.Now;
                                        msg.Error = null;
                                        msg.Attempts++;
                                        break;
                                    }

                                case "Notification.Build":
                                    {
                                        var req = JsonSerializer.Deserialize<PublishNotificationRequest>(msg.PayloadJson)
                                                  ?? throw new InvalidOperationException("Build payload null");

                                        var companyId = req.CompanyId;

                                        // Idempotency nhẹ
                                        var exists = uow.Notifications.Query()
                                            .Any(n => n.CompanyId == companyId
                                                   && n.Topic == req.Topic
                                                   && n.Title == req.Title
                                                   && n.Link == req.Link
                                                   && n.PayloadJson == req.PayloadJson);

                                        if (!exists)
                                        {
                                            var now = DateTime.Now;
                                            // 2) Persist notification + recipients + user states
                                            var n = new Notification
                                            {
                                                Id = Guid.CreateVersion7(),
                                                CompanyId = companyId,
                                                Topic = req.Topic,
                                                Severity = req.Severity,
                                                Title = req.Title,
                                                Message = req.Message,
                                                Link = req.Link,
                                                PayloadJson = req.PayloadJson,
                                                CreatedDate = now,
                                                CreatedBy = req.CreatedBy,                     // ✅ dùng giá trị thật
                                                CreatedByNameSnapshot = req.CreatedByNameSnapshot
                                            };

                                            await uow.Notifications.AddAsync(n, ct);

                                            if (req.TargetUserIds?.Any() == true)
                                                foreach (var uid in req.TargetUserIds.Where(g => g != Guid.Empty).Distinct())
                                                    await uow.NotificationRecipients.AddAsync(new NotificationRecipient
                                                    {
                                                        Id = Guid.CreateVersion7(),
                                                        NotificationId = n.Id,
                                                        TargetUserId = uid
                                                    }, ct);

                                            if (req.TargetRoles?.Any() == true)
                                                foreach (var role in req.TargetRoles
                                                            .SelectMany(r => (r ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries))
                                                            .Select(r => r.Trim().ToUpperInvariant()).Distinct())
                                                    await uow.NotificationRecipients.AddAsync(new NotificationRecipient
                                                    {
                                                        Id = Guid.CreateVersion7(),
                                                        NotificationId = n.Id,
                                                        TargetRole = role
                                                    }, ct);

                                            if (req.TargetTeamIds?.Any() == true)
                                                foreach (var tid in req.TargetTeamIds.Where(g => g != Guid.Empty).Distinct())
                                                    await uow.NotificationRecipients.AddAsync(new NotificationRecipient
                                                    {
                                                        Id = Guid.CreateVersion7(),
                                                        NotificationId = n.Id,
                                                        TargetTeamId = tid
                                                    }, ct);

                                            var targets = await NotificationTargetResolver.ResolveEmployeeIdsAsync(uow, companyId, req, ct);
                                            foreach (var eid in targets)
                                                await uow.NotificationUserStates.AddAsync(new NotificationUserState
                                                {
                                                    NotificationId = n.Id,
                                                    UserId = eid,
                                                    IsRead = false
                                                }, ct);

                                            await uow.SaveChangesAsync();

                                            // 3) PUSH NGAY TẠI ĐÂY (không cần InAppPush)
                                            var data = new { NotificationId = n.Id };

                                            if (req.TargetRoles?.Any() == true)
                                            {
                                                var cKey = companyId.ToString("N");
                                                foreach (var role in req.TargetRoles)
                                                {
                                                    var rKey = role.Trim().ToUpperInvariant();
                                                    await hub.Clients.Group($"role:{cKey}:{rKey}").SendAsync("notify", data, ct);
                                                }
                                            }
                                            if (req.TargetUserIds?.Any() == true)
                                                foreach (var uid in req.TargetUserIds.Where(g => g != Guid.Empty))
                                                    await hub.Clients.Group($"user:{uid}").SendAsync("notify", data, ct);

                                            if (req.TargetTeamIds?.Any() == true)
                                                foreach (var tid in req.TargetTeamIds.Where(g => g != Guid.Empty))
                                                    await hub.Clients.Group($"team:{tid}").SendAsync("notify", data, ct);
                                        }

                                        // 4) Hoàn tất outbox BUILD
                                        msg.ProcessedAt = DateTime.Now;
                                        msg.Error = null;
                                        msg.Attempts++;
                                        break;
                                    }


                                default:
                                    {
                                        // Không có handler tương ứng → đánh lỗi và đánh dấu processed để không kẹt hàng đợi
                                        msg.Attempts++;
                                        msg.Error = $"No handler for type {msg.Type}";
                                        msg.ProcessedAt = DateTime.Now;
                                        break;
                                    }
                            }
                        }
                        catch (Exception exMsg)
                        {
                            // Retry kiểu đơn giản dựa vào Attempts (schema hiện tại của bạn)
                            msg.Attempts++;
                            msg.Error = exMsg.Message;

                            // Nếu muốn “bỏ qua” sau N lần, bạn có thể đặt ProcessedAt để tránh lặp vô hạn:
                            if (msg.Attempts >= 5)
                                msg.ProcessedAt = DateTime.Now;
                        }
                    }

                    await uow.SaveChangesAsync();
                }
                catch (OperationCanceledException) { /* shutting down */ }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "OutboxProcessor loop error");
                    await Task.Delay(1500, ct);
                }
            }
        }
    }

    // Helpers giữ style "Safe" của bạn (không kéo Microsoft.EntityFrameworkCore)
    static class EfSafeExt
    {
        public static System.Collections.Generic.List<T> ToListSafe<T>(this IQueryable<T> q)
        {
            var list = new System.Collections.Generic.List<T>();
            foreach (var i in q) list.Add(i);
            return list;
        }
    }
}



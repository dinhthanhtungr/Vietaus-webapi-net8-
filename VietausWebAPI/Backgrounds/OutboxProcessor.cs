using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
using VietausWebAPI.Core.Repositories_Contracts;
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
                                case "InAppPush":
                                    {
                                        var env = JsonSerializer.Deserialize<OutboxEnvelope>(msg.PayloadJson);
                                        if (env is null || env.NotificationId is null)
                                            throw new InvalidOperationException("Envelope null hoặc thiếu NotificationId");

                                        var data = new { NotificationId = env.NotificationId.Value };

                                        // Gửi theo Role
                                        if (env.TargetRoles?.Count > 0)
                                        {
                                            foreach (var role in env.TargetRoles.Where(r => !string.IsNullOrWhiteSpace(r)))
                                            {
                                                await hub.Clients.Group($"role:{env.CompanyId}:{role}")
                                                    .SendAsync("notify", data, ct);
                                            }
                                        }

                                        //if (msg.Type == "InAppPush")
                                        //{

                                        //    await hub.Clients.All
                                        //        .SendAsync("notify", new { NotificationId = env.NotificationId });


                                        //}

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

















//using System;
//using System.Text.Json;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using VietausWebAPI.Core.Repositories_Contracts;
//using VietausWebAPI.WebAPI.Hubs;

//namespace VietausWebAPI.WebAPI.Background
//{
//    public sealed class OutboxProcessor : BackgroundService
//    {
//        private readonly IServiceProvider _sp;
//        private readonly ILogger<OutboxProcessor> _logger;

//        public OutboxProcessor(IServiceProvider sp, ILogger<OutboxProcessor> logger)
//        {
//            _sp = sp;
//            _logger = logger;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            // vòng lặp đơn giản
//            while (!stoppingToken.IsCancellationRequested)
//            {
//                try
//                {
//                    using var scope = _sp.CreateScope();
//                    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
//                    var hub = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();

//                    // lấy 1 outbox chưa xử lý (ví dụ)
//                    var msg = await uow.OutboxMessages
//                        .Query(track: true)
//                        .FirstOrDefaultAsyncSafe(m => m.ProcessedAt == null, stoppingToken);

//                    if (msg == null)
//                    {
//                        await Task.Delay(800, stoppingToken);
//                        continue;
//                    }

//                    if (msg.Type == "InAppPush")
//                    {
//                        var payload = JsonSerializer.Deserialize<InAppPayload>(msg.PayloadJson);
//                        if (payload != null)
//                        {
//                            // tối giản: push event "notify" vào group company
//                            //await hub.Clients.Group($"company:{payload.CompanyId}")
//                            //    .SendAsync("notify", new { NotificationId = payload.NotificationId }, stoppingToken);

//                            await hub.Clients.All
//                                .SendAsync("notify", new { NotificationId = payload.NotificationId }, stoppingToken);

//                        }
//                    }

//                    msg.ProcessedAt = DateTime.UtcNow;
//                    msg.Attempts++;
//                    await uow.SaveChangesAsync();
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "OutboxProcessor loop error");
//                    await Task.Delay(1500, stoppingToken);
//                }
//            }
//        }

//        private sealed class InAppPayload
//        {
//            public Guid NotificationId { get; set; }
//            public Guid CompanyId { get; set; }
//        }
//    }

//    static class EfSafeExt
//    {
//        public static async Task<T?> FirstOrDefaultAsyncSafe<T>(this IQueryable<T> q, System.Linq.Expressions.Expression<Func<T, bool>> pred, CancellationToken ct)
//        {
//            // tránh lôi các extension khác — bạn thay bằng EF Core FirstOrDefaultAsync nếu có using Microsoft.EntityFrameworkCore
//            foreach (var item in q.Where(pred))
//                return await Task.FromResult(item);
//            return default;
//        }
//    }
//}

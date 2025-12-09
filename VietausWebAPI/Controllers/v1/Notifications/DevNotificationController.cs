    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json;
    using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
    using VietausWebAPI.Core.Application.Features.Notifications.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Domain.Enums.Notifications;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;

namespace VietausWebAPI.WebAPI.Controllers.v1.Notifications
    {
        [ApiController]
        [Route("dev/notify")]             // <— THÊM
        public class DevNotificationController : ControllerBase
        {
            private readonly INotificationService _noti;

            public DevNotificationController(INotificationService noti)
            {
                _noti = noti;
            }

            // POST /dev/notify/price-exceeded
            [HttpPost("price-exceeded")]
            public async Task<IActionResult> PriceExceeded([FromBody] PriceExceededDto dto, CancellationToken ct)
            {
                var payload = JsonSerializer.Serialize(new
                {
                    dto.FormulaId,
                    dto.FormulaExternalId,
                    dto.TotalCost,
                    dto.TargetPrice,
                    dto.MpoId
                });

                var id = await _noti.PublishAsync(new PublishNotificationRequest
                {
                    Topic = TopicNotifications.PriceOverSellCreated,
                    Severity = NotificationSeverity.Warning,
                    Title = $"Cảnh báo giá: {dto.FormulaExternalId}",
                    Message = $"Tổng chi phí {dto.TotalCost:N0} > Giá bán {dto.TargetPrice:N0}",
                    Link = $"/manufacturing/formula/{dto.FormulaId}",
                    PayloadJson = payload,
                    TargetRoles = new() { AppRoles.Leader } // ví dụ gửi cho vai trò Lãnh đạo
                }, ct);

                return Ok(new { NotificationId = id });
            }

            [HttpGet("{id:guid}")]
            public async Task<IActionResult> Get([FromRoute] Guid id)
            {
                var result = await _noti.GetByIdAsync(id);
                return result is null ? NotFound() : Ok(result);
            }

            [HttpGet("feed")]
            public async Task<ActionResult<IReadOnlyList<NotificationDto>>> Feed(
                [FromQuery] int take = 20,
                [FromQuery] DateTime? afterCreated = null,
                [FromQuery] Guid? afterId = null,
                CancellationToken ct = default)
            {
                var result = await _noti.GetFeedAsync(take, afterId, afterCreated, ct);
                return Ok(result);
            }

            [HttpGet("unread-count")]
            public async Task<ActionResult<int>> UnreadCount(CancellationToken ct)
                 => Ok(await _noti.GetUnreadCountAsync(ct));

            [HttpPost("{id:guid}/read")]
            public async Task<IActionResult> MarkRead([FromRoute] Guid id, CancellationToken ct)
            {
                await _noti.MarkReadAsync(id, ct);
                return NoContent();
            }

            [HttpPost("mark-all-read")]
            public async Task<IActionResult> MarkAllRead(CancellationToken ct)
            {
                await _noti.MarkAllReadAsync(ct);
                return NoContent();
            }


        public sealed class PriceExceededDto
            {
                public Guid CompanyId { get; set; }
                public Guid FormulaId { get; set; }
                public string FormulaExternalId { get; set; } = default!;
                public decimal TotalCost { get; set; }
                public decimal TargetPrice { get; set; }
                public Guid? MpoId { get; set; }
            }
        }
    }

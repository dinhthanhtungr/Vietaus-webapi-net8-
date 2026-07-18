using System;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets
{
    /// <summary>
    /// Describes the current local Gemini quota state that the UI can use to show remaining requests and retry countdowns.
    /// </summary>
    public class AiRateLimitInfoDto
    {
        public bool CanRequest { get; set; }
        public string Model { get; set; } = string.Empty;
        public int RetryAfterSeconds { get; set; }
        public DateTime? RetryAt { get; set; }
        public int RpmLimit { get; set; }
        public int RpmUsed { get; set; }
        public int RpmRemaining { get; set; }
        public int RpdLimit { get; set; }
        public int RpdUsed { get; set; }
        public int RpdRemaining { get; set; }
        public DateTime MinuteWindowStartsAt { get; set; }
        public DateTime MinuteWindowEndsAt { get; set; }
        public DateTime DayWindowStartsAt { get; set; }
        public DateTime DayWindowEndsAt { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

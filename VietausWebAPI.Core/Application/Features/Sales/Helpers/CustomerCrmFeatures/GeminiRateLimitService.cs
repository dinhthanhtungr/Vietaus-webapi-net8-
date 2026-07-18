using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets;

namespace VietausWebAPI.Core.Application.Features.Sales.Helpers.CustomerCrmFeatures
{
    /// <summary>
    /// Tracks local Gemini RPM and RPD usage in memory so the UI can show when users may request AI summaries again.
    /// </summary>
    public class GeminiRateLimitService : IGeminiRateLimitService
    {
        private static readonly ConcurrentDictionary<string, GeminiRateLimitBucket> Buckets = new();
        private readonly GeminiOptions _options;

        public GeminiRateLimitService(IOptions<GeminiOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Gets the current local quota state without consuming a request.
        /// </summary>
        /// <param name="model">Gemini model name used as the quota bucket key.</param>
        /// <returns>The current rate-limit state for the selected model.</returns>
        public AiRateLimitInfoDto GetCurrent(string model)
        {
            var now = DateTime.Now;
            var bucket = GetBucket(model, now);

            lock (bucket.SyncRoot)
            {
                ResetExpiredWindows(bucket, now);
                return BuildInfo(bucket, model, now);
            }
        }

        /// <summary>
        /// Attempts to consume one Gemini request from the local quota bucket.
        /// </summary>
        /// <param name="model">Gemini model name used as the quota bucket key.</param>
        /// <returns>The quota state after the consume attempt. CanRequest is false when the request should be blocked.</returns>
        public AiRateLimitInfoDto TryConsume(string model)
        {
            var now = DateTime.Now;
            var bucket = GetBucket(model, now);

            lock (bucket.SyncRoot)
            {
                ResetExpiredWindows(bucket, now);
                var info = BuildInfo(bucket, model, now);

                if (!info.CanRequest)
                    return info;

                bucket.MinuteUsed++;
                bucket.DayUsed++;

                return BuildInfo(bucket, model, now);
            }
        }

        /// <summary>
        /// Gets or creates the in-memory quota bucket for a Gemini model.
        /// </summary>
        /// <param name="model">Gemini model name used as the quota bucket key.</param>
        /// <param name="now">Current local server time.</param>
        /// <returns>The in-memory bucket used to track local usage.</returns>
        private GeminiRateLimitBucket GetBucket(string model, DateTime now)
        {
            var modelKey = NormalizeModel(model);
            return Buckets.GetOrAdd(modelKey, _ => new GeminiRateLimitBucket
            {
                MinuteWindowStartsAt = TruncateToMinute(now),
                DayWindowStartsAt = now.Date
            });
        }

        /// <summary>
        /// Resets minute and day counters when their windows have expired.
        /// </summary>
        /// <param name="bucket">The quota bucket being checked.</param>
        /// <param name="now">Current local server time.</param>
        private static void ResetExpiredWindows(GeminiRateLimitBucket bucket, DateTime now)
        {
            var minuteStart = TruncateToMinute(now);
            if (bucket.MinuteWindowStartsAt != minuteStart)
            {
                bucket.MinuteWindowStartsAt = minuteStart;
                bucket.MinuteUsed = 0;
            }

            if (bucket.DayWindowStartsAt.Date != now.Date)
            {
                bucket.DayWindowStartsAt = now.Date;
                bucket.DayUsed = 0;
            }
        }

        /// <summary>
        /// Builds a DTO that contains remaining requests, used requests, and retry timing for the UI.
        /// </summary>
        /// <param name="bucket">The quota bucket being projected.</param>
        /// <param name="model">Gemini model name shown to the user.</param>
        /// <param name="now">Current local server time.</param>
        /// <returns>A DTO containing the current local rate-limit state.</returns>
        private AiRateLimitInfoDto BuildInfo(GeminiRateLimitBucket bucket, string model, DateTime now)
        {
            var rpmLimit = Math.Max(1, _options.RateLimit.RequestsPerMinute);
            var rpdLimit = Math.Max(1, _options.RateLimit.RequestsPerDay);
            var rpmRemaining = Math.Max(0, rpmLimit - bucket.MinuteUsed);
            var rpdRemaining = Math.Max(0, rpdLimit - bucket.DayUsed);
            var minuteEndsAt = bucket.MinuteWindowStartsAt.AddMinutes(1);
            var dayEndsAt = bucket.DayWindowStartsAt.AddDays(1);
            var canRequest = rpmRemaining > 0 && rpdRemaining > 0;
            var retryAt = canRequest
                ? (DateTime?)null
                : rpmRemaining <= 0 ? minuteEndsAt : dayEndsAt;
            var retryAfterSeconds = retryAt.HasValue
                ? Math.Max(1, (int)Math.Ceiling((retryAt.Value - now).TotalSeconds))
                : 0;

            return new AiRateLimitInfoDto
            {
                CanRequest = canRequest,
                Model = string.IsNullOrWhiteSpace(model) ? _options.Model : model,
                RetryAfterSeconds = retryAfterSeconds,
                RetryAt = retryAt,
                RpmLimit = rpmLimit,
                RpmUsed = bucket.MinuteUsed,
                RpmRemaining = rpmRemaining,
                RpdLimit = rpdLimit,
                RpdUsed = bucket.DayUsed,
                RpdRemaining = rpdRemaining,
                MinuteWindowStartsAt = bucket.MinuteWindowStartsAt,
                MinuteWindowEndsAt = minuteEndsAt,
                DayWindowStartsAt = bucket.DayWindowStartsAt,
                DayWindowEndsAt = dayEndsAt,
                Message = BuildMessage(canRequest, retryAfterSeconds, rpmRemaining, rpdRemaining, rpmLimit, rpdLimit)
            };
        }

        /// <summary>
        /// Builds a user-facing quota message based on the current remaining request counters.
        /// </summary>
        /// <param name="canRequest">Whether a new Gemini request is allowed now.</param>
        /// <param name="retryAfterSeconds">Seconds until the next allowed request when blocked.</param>
        /// <param name="rpmRemaining">Remaining requests in the current minute window.</param>
        /// <param name="rpdRemaining">Remaining requests in the current day window.</param>
        /// <param name="rpmLimit">Configured requests-per-minute limit.</param>
        /// <param name="rpdLimit">Configured requests-per-day limit.</param>
        /// <returns>A short message that can be displayed directly in the UI.</returns>
        private static string BuildMessage(
            bool canRequest,
            int retryAfterSeconds,
            int rpmRemaining,
            int rpdRemaining,
            int rpmLimit,
            int rpdLimit)
        {
            if (canRequest)
                return $"Còn {rpmRemaining}/{rpmLimit} request trong phút này và {rpdRemaining}/{rpdLimit} request trong ngày.";

            if (rpmRemaining <= 0)
                return $"Bạn đã dùng hết {rpmLimit} request/phút. Vui lòng thử lại sau {retryAfterSeconds} giây.";

            return $"Bạn đã dùng hết {rpdLimit} request/ngày. Vui lòng thử lại sau {retryAfterSeconds} giây.";
        }

        /// <summary>
        /// Normalizes a Gemini model name before using it as a quota bucket key.
        /// </summary>
        /// <param name="model">Raw model name from configuration.</param>
        /// <returns>A normalized model key.</returns>
        private string NormalizeModel(string model)
        {
            return string.IsNullOrWhiteSpace(model)
                ? _options.Model.Trim().ToUpperInvariant()
                : model.Trim().ToUpperInvariant();
        }

        /// <summary>
        /// Truncates a DateTime to the start of its minute.
        /// </summary>
        /// <param name="value">DateTime value to truncate.</param>
        /// <returns>The same DateTime rounded down to the current minute.</returns>
        private static DateTime TruncateToMinute(DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0, value.Kind);
        }

        private sealed class GeminiRateLimitBucket
        {
            public object SyncRoot { get; } = new();
            public DateTime MinuteWindowStartsAt { get; set; }
            public int MinuteUsed { get; set; }
            public DateTime DayWindowStartsAt { get; set; }
            public int DayUsed { get; set; }
        }
    }
}

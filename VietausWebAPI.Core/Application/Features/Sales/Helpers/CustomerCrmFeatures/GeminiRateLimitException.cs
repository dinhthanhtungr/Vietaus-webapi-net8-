using System;

namespace VietausWebAPI.Core.Application.Features.Sales.Helpers.CustomerCrmFeatures
{
    /// <summary>
    /// Represents a Gemini quota error returned by the Gemini API.
    /// </summary>
    public class GeminiRateLimitException : Exception
    {
        public GeminiRateLimitException(string message, int? retryAfterSeconds = null)
            : base(message)
        {
            RetryAfterSeconds = retryAfterSeconds;
        }

        public int? RetryAfterSeconds { get; }
    }
}

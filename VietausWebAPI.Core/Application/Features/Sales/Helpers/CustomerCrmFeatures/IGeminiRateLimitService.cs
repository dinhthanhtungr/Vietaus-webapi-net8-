using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets;

namespace VietausWebAPI.Core.Application.Features.Sales.Helpers.CustomerCrmFeatures
{
    /// <summary>
    /// Provides application-side Gemini quota checks before the system calls Gemini.
    /// </summary>
    public interface IGeminiRateLimitService
    {
        /// <summary>
        /// Gets the current local quota state without consuming a request.
        /// </summary>
        /// <param name="model">Gemini model name used as the quota bucket key.</param>
        /// <returns>The current rate-limit state for the selected model.</returns>
        AiRateLimitInfoDto GetCurrent(string model);

        /// <summary>
        /// Attempts to consume one Gemini request from the local quota bucket.
        /// </summary>
        /// <param name="model">Gemini model name used as the quota bucket key.</param>
        /// <returns>The quota state after the consume attempt. CanRequest is false when the request should be blocked.</returns>
        AiRateLimitInfoDto TryConsume(string model);
    }
}

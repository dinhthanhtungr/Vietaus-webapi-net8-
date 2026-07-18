namespace VietausWebAPI.Core.Application.Features.Sales.Helpers.CustomerCrmFeatures
{
    /// <summary>
    /// Defines Gemini integration settings used by CRM AI summary features.
    /// </summary>
    public class GeminiOptions
    {
        public bool Enabled { get; set; }
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://generativelanguage.googleapis.com";
        public string Model { get; set; } = "gemini-2.5-flash-lite";
        public int TimeoutSeconds { get; set; } = 120;
        public GeminiRateLimitOptions RateLimit { get; set; } = new();
    }

    /// <summary>
    /// Defines the local application-side Gemini request quota used to protect the real Gemini quota.
    /// These values should be aligned with the RPM/RPD limits configured for the selected Gemini model.
    /// </summary>
    public class GeminiRateLimitOptions
    {
        public int RequestsPerMinute { get; set; } = 15;
        public int RequestsPerDay { get; set; } = 500;
    }
}


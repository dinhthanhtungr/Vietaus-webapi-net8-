using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.Helpers.CustomerCrmFeatures
{
    public class GeminiCustomerSummaryClient : IGeminiCustomerSummaryClient
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
        private readonly HttpClient _httpClient;
        private readonly GeminiOptions _options;

        public GeminiCustomerSummaryClient(HttpClient httpClient, IOptions<GeminiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public string Model => _options.Model;

        /// <summary>
        /// Calls Gemini to generate a structured CRM interaction summary from the supplied prompt.
        /// </summary>
        /// <param name="prompt">The full prompt containing customer context and interaction history.</param>
        /// <param name="ct">Cancellation token used to cancel the Gemini request.</param>
        /// <returns>The structured summary returned by Gemini.</returns>
        /// <exception cref="GeminiRateLimitException">Thrown when Gemini returns a quota or rate-limit response.</exception>
        /// <exception cref="InvalidOperationException">Thrown when configuration is invalid or Gemini returns an unexpected error.</exception>
        public async Task<CustomerInteractionAiSummaryResult> GenerateSummaryAsync(string prompt, CancellationToken ct = default)
        {
            if (!_options.Enabled)
                throw new InvalidOperationException("Gemini is disabled.");

            if (string.IsNullOrWhiteSpace(_options.ApiKey))
                throw new InvalidOperationException("Gemini ApiKey is missing.");

            if (string.IsNullOrWhiteSpace(_options.Model))
                throw new InvalidOperationException("Gemini Model is missing.");

            var baseUrl = string.IsNullOrWhiteSpace(_options.BaseUrl)
                ? "https://generativelanguage.googleapis.com"
                : _options.BaseUrl.TrimEnd('/');

            var requestUrl = $"{baseUrl}/v1beta/models/{Uri.EscapeDataString(_options.Model)}:generateContent?key={Uri.EscapeDataString(_options.ApiKey)}";

            var request = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.2,
                    responseMimeType = "application/json"
                }
            };

            using var response = await _httpClient.PostAsJsonAsync(requestUrl, request, JsonOptions, ct);
            var responseText = await response.Content.ReadAsStringAsync(ct);

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
                throw new GeminiRateLimitException(
                    $"Gemini đã giới hạn request. Vui lòng thử lại sau.",
                    ReadRetryAfterSeconds(response));

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Gemini request failed: {(int)response.StatusCode} {responseText}");

            var contentText = ExtractCandidateText(responseText);
            return ParseSingleSummaryResult(contentText);
        }

        public async Task<CustomerInteractionAiSummaryBatchResult> GenerateBatchSummaryAsync(string prompt, CancellationToken ct = default)
        {
            if (!_options.Enabled)
                throw new InvalidOperationException("Gemini is disabled.");

            if (string.IsNullOrWhiteSpace(_options.ApiKey))
                throw new InvalidOperationException("Gemini ApiKey is missing.");

            if (string.IsNullOrWhiteSpace(_options.Model))
                throw new InvalidOperationException("Gemini Model is missing.");

            var baseUrl = string.IsNullOrWhiteSpace(_options.BaseUrl)
                ? "https://generativelanguage.googleapis.com"
                : _options.BaseUrl.TrimEnd('/');

            var requestUrl = $"{baseUrl}/v1beta/models/{Uri.EscapeDataString(_options.Model)}:generateContent?key={Uri.EscapeDataString(_options.ApiKey)}";

            var request = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.2,
                    responseMimeType = "application/json"
                }
            };

            using var response = await _httpClient.PostAsJsonAsync(requestUrl, request, JsonOptions, ct);
            var responseText = await response.Content.ReadAsStringAsync(ct);

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
                throw new GeminiRateLimitException(
                    $"Gemini đã giới hạn request. Vui lòng thử lại sau.",
                    ReadRetryAfterSeconds(response));

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Gemini request failed: {(int)response.StatusCode} {responseText}");

            var contentText = ExtractCandidateText(responseText);
            return ParseBatchSummaryResult(contentText);
        }
        private static CustomerInteractionAiSummaryResult ParseSingleSummaryResult(string contentText)
        {
            var jsonText = ExtractJsonValue(contentText);

            try
            {
                using var document = JsonDocument.Parse(jsonText);
                var root = document.RootElement;

                if (root.ValueKind == JsonValueKind.Object
                    && root.TryGetProperty("items", out var items)
                    && items.ValueKind == JsonValueKind.Array)
                {
                    var first = items.EnumerateArray().FirstOrDefault();
                    if (first.ValueKind != JsonValueKind.Undefined)
                    {
                        return JsonSerializer.Deserialize<CustomerInteractionAiSummaryResult>(first.GetRawText(), JsonOptions)
                            ?? new CustomerInteractionAiSummaryResult { Summary = contentText };
                    }
                }

                if (root.ValueKind == JsonValueKind.Array)
                {
                    var first = root.EnumerateArray().FirstOrDefault();
                    if (first.ValueKind != JsonValueKind.Undefined)
                    {
                        return JsonSerializer.Deserialize<CustomerInteractionAiSummaryResult>(first.GetRawText(), JsonOptions)
                            ?? new CustomerInteractionAiSummaryResult { Summary = contentText };
                    }
                }

                return JsonSerializer.Deserialize<CustomerInteractionAiSummaryResult>(root.GetRawText(), JsonOptions)
                    ?? new CustomerInteractionAiSummaryResult { Summary = contentText };
            }
            catch (JsonException)
            {
                return new CustomerInteractionAiSummaryResult
                {
                    Summary = contentText
                };
            }
        }

        private static CustomerInteractionAiSummaryBatchResult ParseBatchSummaryResult(string contentText)
        {
            var jsonText = ExtractJsonValue(contentText);

            try
            {
                using var document = JsonDocument.Parse(jsonText);
                var root = document.RootElement;

                if (root.ValueKind == JsonValueKind.Array)
                {
                    var items = root.Deserialize<List<CustomerInteractionAiSummaryBatchResultItem>>(JsonOptions) ?? new();
                    return new CustomerInteractionAiSummaryBatchResult { Items = items };
                }

                if (root.ValueKind == JsonValueKind.Object
                    && !root.TryGetProperty("items", out _))
                {
                    var item = root.Deserialize<CustomerInteractionAiSummaryBatchResultItem>(JsonOptions);
                    return new CustomerInteractionAiSummaryBatchResult
                    {
                        Items = item is null ? new List<CustomerInteractionAiSummaryBatchResultItem>() : new List<CustomerInteractionAiSummaryBatchResultItem> { item }
                    };
                }

                return JsonSerializer.Deserialize<CustomerInteractionAiSummaryBatchResult>(root.GetRawText(), JsonOptions)
                    ?? new CustomerInteractionAiSummaryBatchResult();
            }
            catch (JsonException)
            {
                return new CustomerInteractionAiSummaryBatchResult();
            }
        }
        /// <summary>
        /// Extracts Retry-After seconds from a Gemini HTTP response when Google provides the header.
        /// </summary>
        /// <param name="response">Gemini HTTP response.</param>
        /// <returns>The number of seconds to wait before retrying, or null when the header is not available.</returns>
        private static int? ReadRetryAfterSeconds(HttpResponseMessage response)
        {
            if (response.Headers.RetryAfter == null)
                return null;

            if (response.Headers.RetryAfter.Delta.HasValue)
                return Math.Max(1, (int)Math.Ceiling(response.Headers.RetryAfter.Delta.Value.TotalSeconds));

            if (response.Headers.RetryAfter.Date.HasValue)
            {
                var seconds = (response.Headers.RetryAfter.Date.Value - DateTimeOffset.Now).TotalSeconds;
                return Math.Max(1, (int)Math.Ceiling(seconds));
            }

            return null;
        }

        /// <summary>
        /// Extracts generated text from the first Gemini candidate.
        /// </summary>
        /// <param name="responseText">Raw Gemini JSON response text.</param>
        /// <returns>The generated text from Gemini, or the raw response when no candidate text is available.</returns>
        private static string ExtractCandidateText(string responseText)
        {
            using var document = JsonDocument.Parse(responseText);
            var root = document.RootElement;

            if (!root.TryGetProperty("candidates", out var candidates) || candidates.ValueKind != JsonValueKind.Array)
                return responseText;

            var firstCandidate = candidates.EnumerateArray().FirstOrDefault();
            if (firstCandidate.ValueKind == JsonValueKind.Undefined)
                return responseText;

            if (!firstCandidate.TryGetProperty("content", out var content))
                return responseText;

            if (!content.TryGetProperty("parts", out var parts) || parts.ValueKind != JsonValueKind.Array)
                return responseText;

            var texts = parts
                .EnumerateArray()
                .Where(x => x.TryGetProperty("text", out _))
                .Select(x => x.GetProperty("text").GetString())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            return texts.Count == 0
                ? responseText
                : string.Join(Environment.NewLine, texts);
        }

        /// <summary>
        /// Extracts the first JSON object from Gemini text, or wraps plain text into the summary schema.
        /// </summary>
        /// <param name="text">Generated Gemini text.</param>
        /// <returns>A JSON object string that can be deserialized into the summary DTO.</returns>
        private static string ExtractJsonValue(string text)
        {
            var trimmed = text.Trim();
            if (trimmed.StartsWith("{") && trimmed.EndsWith("}"))
                return trimmed;

            var start = trimmed.IndexOf('{');
            var end = trimmed.LastIndexOf('}');

            if (start >= 0 && end > start)
                return trimmed.Substring(start, end - start + 1);

            return JsonSerializer.Serialize(new CustomerInteractionAiSummaryResult
            {
                Summary = trimmed
            }, JsonOptions);
        }
    }
}







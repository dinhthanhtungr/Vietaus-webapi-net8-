using System.Threading;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.Helpers.CustomerCrmFeatures
{
    public interface IGeminiCustomerSummaryClient
    {
        string Model { get; }
        Task<CustomerInteractionAiSummaryResult> GenerateSummaryAsync(string prompt, CancellationToken ct = default);
        Task<CustomerInteractionAiSummaryBatchResult> GenerateBatchSummaryAsync(string prompt, CancellationToken ct = default);
    }
}


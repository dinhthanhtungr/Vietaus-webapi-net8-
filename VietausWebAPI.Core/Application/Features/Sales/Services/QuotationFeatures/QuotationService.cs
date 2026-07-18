using System.Text.Json;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Sales.Helpers.QuotationFeatures;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.QuotationFeatures;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.QuotationFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.Shared.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.QuotationFeatures
{
    public sealed partial class QuotationService : IQuotationService
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        private readonly IQuotationRepository _quotationRepository;
        private readonly IQuotationLineRepository _quotationLineRepository;
        private readonly IQuotationStatusHistoryRepository _statusHistoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IVisibilityHelper _visibilityHelper;
        private readonly IExternalIdService _externalIdService;
        private readonly IQuotationPdf _quotationPdf;

        public QuotationService(
            IQuotationRepository quotationRepository,
            IQuotationLineRepository quotationLineRepository,
            IQuotationStatusHistoryRepository statusHistoryRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IVisibilityHelper visibilityHelper,
            IExternalIdService externalIdService,
            IQuotationPdf quotationPdf)
        {
            _quotationRepository = quotationRepository;
            _quotationLineRepository = quotationLineRepository;
            _statusHistoryRepository = statusHistoryRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _visibilityHelper = visibilityHelper;
            _externalIdService = externalIdService;
            _quotationPdf = quotationPdf;
        }
    }
}

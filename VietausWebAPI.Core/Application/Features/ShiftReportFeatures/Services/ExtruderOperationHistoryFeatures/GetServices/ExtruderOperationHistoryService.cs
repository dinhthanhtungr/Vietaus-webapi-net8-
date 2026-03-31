using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.DTOs.ExtruderOperationHistoryDTOs;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.Querys.ExtruderOperationHistoryQuery;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.RepositoriesContracts.ExtruderOperationHistoryRepositories;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.ServiceContracts.ExtruderOperationHistoryFeatures.GetServices;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.ShiftReportSchema;

namespace VietausWebAPI.Core.Application.Features.ShiftReportFeatures.Services.ExtruderOperationHistoryFeatures.GetServices
{
    public class ExtruderOperationHistoryService : IExtruderOperationHistoryService
    {
        private readonly IExtruderOperationHistoryReadRepositories _repository;

        public ExtruderOperationHistoryService(IExtruderOperationHistoryReadRepositories repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<OperationHistoryMd>> GetPagedAsync(
            ExtruderOperationHistoryQuery query,
            CancellationToken cancellationToken = default)
        {
            query ??= new ExtruderOperationHistoryQuery();

            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 20;

            return await _repository.GetPagedAsync(query, cancellationToken);
        }

        public async Task<List<OperationHistoryMd>> GetByExternalIdAsync(
            string externalId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(externalId))
                    return new List<OperationHistoryMd>();

                return await _repository.GetByExternalIdAsync(externalId, cancellationToken);
            }
            catch (Exception Ex)
            {
                // Log the exception if necessary
                return new List<OperationHistoryMd>();
            }
        }

        public async Task<OperationHistoryMd?> GetLatestByExternalIdAsync(
            string externalId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(externalId))
                return null;

            return await _repository.GetLatestByExternalIdAsync(externalId, cancellationToken);
        }

        public async Task<ExtruderOperationHistoryHasData> GetIsValidAsync(
            ExtruderOperationHistoryQuery query,
            CancellationToken cancellationToken = default)
        {
            if (query == null || (string.IsNullOrWhiteSpace(query.ProductCode) && string.IsNullOrWhiteSpace(query.ExternalId)))
                return null;
            return await _repository.GetIsValidAsync(query, cancellationToken);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.DTOs.ExtruderOperationHistoryDTOs;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.Querys.ExtruderOperationHistoryQuery;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.RepositoriesContracts.ExtruderOperationHistoryRepositories;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.ShiftReportSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.ShiftReportFeatures.ExtruderOperationHistoryRepositories
{
    public class ExtruderOperationHistoryReadRepositories : IExtruderOperationHistoryReadRepositories
    {
        private readonly ApplicationDbContext _context;

        public ExtruderOperationHistoryReadRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<OperationHistoryMd>> GetPagedAsync(
            ExtruderOperationHistoryQuery query,
            CancellationToken cancellationToken = default)
        {
            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 20;

            var source = _context.OperationHistoryMds
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.ProductCode))
            {
                var productCode = query.ProductCode.Trim().ToLower();
                source = source.Where(x =>
                    x.ProductCode != null &&
                    x.ProductCode.ToLower().Contains(productCode));
            }

            if (!string.IsNullOrWhiteSpace(query.ExternalId))
            {
                var externalId = query.ExternalId.Trim().ToLower();
                source = source.Where(x =>
                    x.ExternalId != null &&
                    x.ExternalId.ToLower().Contains(externalId));
            }

            var totalCount = await source.CountAsync(cancellationToken);

            var items = await source
                .OrderByDescending(x => x.CreatedAt)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<OperationHistoryMd>(
                items,
                totalCount,
                query.PageNumber,
                query.PageSize
            );
        }

        public async Task<List<OperationHistoryMd>> GetByExternalIdAsync(
            string externalId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(externalId))
                return new List<OperationHistoryMd>();

            externalId = externalId.Trim();

            return await _context.OperationHistoryMds
                .AsNoTracking()
                .Where(x => x.ExternalId == externalId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<OperationHistoryMd?> GetLatestByExternalIdAsync(
            string externalId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(externalId))
                return null;

            externalId = externalId.Trim();

            return await _context.OperationHistoryMds
                .AsNoTracking()
                .Where(x => x.ExternalId == externalId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ExtruderOperationHistoryHasData> GetIsValidAsync(
            ExtruderOperationHistoryQuery query,
            CancellationToken cancellationToken = default)
        {
            var source = _context.OperationHistoryMds
                .AsNoTracking()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.ProductCode))
            {
                var productCode = query.ProductCode.Trim().ToLower();
                source = source.Where(x =>
                    x.ProductCode != null &&
                    x.ProductCode.ToLower().Contains(productCode));
            }
            if (!string.IsNullOrWhiteSpace(query.ExternalId))
            {
                var externalId = query.ExternalId.Trim().ToLower();
                source = source.Where(x =>
                    x.ExternalId != null &&
                    x.ExternalId.ToLower().Contains(externalId));
            }
            DateTime? latestCreatedAt = null;
            var count = await source.CountAsync(cancellationToken);
            if (count > 0)
            {
                latestCreatedAt = await source.MaxAsync(x => (DateTime?)x.CreatedAt, cancellationToken);
            }

            return new ExtruderOperationHistoryHasData
            {
                HasHistory = count > 0,
                Count = count,
                LatestCreatedAt = latestCreatedAt
            };
        }
    }
}
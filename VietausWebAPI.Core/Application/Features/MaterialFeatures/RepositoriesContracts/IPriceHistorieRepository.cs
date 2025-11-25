using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts
{
    public interface IPriceHistorieRepository
    {
        IQueryable<PriceHistory> Query(bool track = false);
        Task AddAsync(PriceHistory newPrice, CancellationToken ct = default);
    }
}

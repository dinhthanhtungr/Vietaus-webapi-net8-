

using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts
{
    public interface ICategoryRepository
    {
        IQueryable<Category> Query();
    }
}

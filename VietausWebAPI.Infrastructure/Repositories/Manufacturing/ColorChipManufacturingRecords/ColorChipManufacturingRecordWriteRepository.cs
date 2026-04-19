using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts.ColorChipManufacturingRecords;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Manufacturing.ColorChipManufacturingRecords
{
    public class ColorChipManufacturingRecordWriteRepository : Repository<ColorChipManufacturingRecord>, IColorChipManufacturingRecordWriteRepository
    {
        public ColorChipManufacturingRecordWriteRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ColorChipManufacturingRecord> CreateAsync(
            ColorChipManufacturingRecord entity,
            CancellationToken ct = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await AddAsync(entity, ct);
            return entity;
        }
    }
}

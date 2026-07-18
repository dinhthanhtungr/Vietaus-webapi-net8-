using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.PrintectFeatures.RepositoryContracts;
using VietausWebAPI.Core.Domain.Entities.PrintectSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Printects
{
    public sealed class LabelTemplateRepositories : Repository<LabelTemplate>, ILabelTemplateRepository
    {
        public LabelTemplateRepositories(ApplicationDbContext context) : base(context) { }
    }

    public sealed class LabelElementRepositories : Repository<LabelElement>, ILabelElementRepository
    {
        public LabelElementRepositories(ApplicationDbContext context) : base(context) { }
    }

    public sealed class HistoryPrintLabelForAllRepositories : Repository<HistoryPrintLabelForAll>, IHistoryPrintLabelForAllRepository
    {
        public HistoryPrintLabelForAllRepositories(ApplicationDbContext context) : base(context) { }
    }
}

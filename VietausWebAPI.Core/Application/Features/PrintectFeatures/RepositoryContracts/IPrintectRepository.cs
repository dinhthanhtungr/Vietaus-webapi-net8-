using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities.PrintectSchema;

namespace VietausWebAPI.Core.Application.Features.PrintectFeatures.RepositoryContracts
{
    public interface ILabelTemplateRepository : IRepository<LabelTemplate> { }
    public interface ILabelElementRepository : IRepository<LabelElement> { }
    public interface IHistoryPrintLabelForAllRepository : IRepository<HistoryPrintLabelForAll> { }

}

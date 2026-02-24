using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities.HrSchema;
using VietausWebAPI.Core.Identity;

namespace VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts
{
    public interface IPartRepository : IRepository<Part>
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Usecases.SupplyRequests.RepositoriesContracts
{
    public interface IMaterialsRepository
    {
        Task <List<MaterialsMaterialDatum>> SearchByNameAsync(string name, Guid materialGroupId);
    }
}

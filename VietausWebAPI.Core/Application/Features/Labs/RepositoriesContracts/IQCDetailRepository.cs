using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts
{
    public interface IQCDetailRepository
    {
        Task AddQCDetail(QCDetail qCDetail);
        //Task DeleteQcOutput(QCDetail qCDetail);
    }
}

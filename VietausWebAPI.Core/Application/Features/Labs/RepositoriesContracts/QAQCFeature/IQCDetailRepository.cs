using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature
{
    public interface IQCDetailRepository
    {
        Task AddQCDetail(Qcdetail qCDetail);
        //Task DeleteQcOutput(QCDetail qCDetail);
    }
}

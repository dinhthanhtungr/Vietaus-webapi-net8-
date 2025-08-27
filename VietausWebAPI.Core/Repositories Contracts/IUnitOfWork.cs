using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Planning.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeesRepository EmployeesCommonRepository { get; }

        //HR
        IEmployeesRepository EmployeesRepository { get; }
        IGroupRepository GroupRepository { get; }
        IMemberInGroupRepository MemberInGroupRepository { get; }
        //Sale
        ICustomerRepository CustomerRepository { get; }
        ITransferCustomerRepository TransferCustomerRepository { get; }
        ICustomerAssignmentRepository CustomerAssignmentRepository { get; }
        ICustomerTransferLogRepository CustomerTransferLogRepository { get; }
        // Labs
        IProductStandardRepository ProductStandardRepository { get; }
        IProductInspectionRepository ProductInspectionRepository { get; }
        IProductTestRepository ProductTestRepository { get; }
        IProductRepository ProductRepository { get; }
        ISampleRequestRepository SampleRequestRepository { get; }
        //IMfgProductionOrdersPlanRepository MfgProductionOrdersPlanRepository { get; }
        IQCDetailRepository IQCDetailRepository { get; }
        ISampleRequestImageRepository SampleRequestImageRepository { get; }

        //Planning
        IScheduealRepository ScheduealRepository { get; }


        // MfgProductionOrder
        IMfgProductionOrdersPlanRepository IMfgProductionOrdersPlanRepository { get; }


        // Thêm các repository khác nếu cần
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> SaveChangesAsync();
    }
}

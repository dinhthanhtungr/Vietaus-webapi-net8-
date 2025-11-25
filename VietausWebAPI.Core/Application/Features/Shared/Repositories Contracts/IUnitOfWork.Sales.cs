using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.MerchandiseOrderFeatures;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        ICustomerRepository CustomerRepository { get; }
        ITransferCustomerRepository TransferCustomerRepository { get; }
        ICustomerAssignmentRepository CustomerAssignmentRepository { get; }
        ICustomerTransferLogRepository CustomerTransferLogRepository { get; }
        IMerchandiseOrderRepository MerchandiseOrderRepository { get; }
    }
}

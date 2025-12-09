using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        public ICustomerRepository CustomerRepository { get; }
        public ITransferCustomerRepository TransferCustomerRepository { get; }
        public ICustomerAssignmentRepository CustomerAssignmentRepository { get; }
        public ICustomerClaimRepository CustomerClaimRepository { get; }
        public ICustomerNoteRepository CustomerNoteRepository { get; }
        public ICustomerTransferLogRepository CustomerTransferLogRepository { get; }
        public IMerchandiseOrderRepository MerchandiseOrderRepository { get; }
    }
}

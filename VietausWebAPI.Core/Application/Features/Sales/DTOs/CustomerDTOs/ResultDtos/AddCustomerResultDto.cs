using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs.ResultDtos
{
    public sealed record AddCustomerResultDto(
        Guid CustomerId,
        string ExternalId,
        string Name,
        string TaxCode,
        Guid EmployeeId,
        string EmployeeName,
        Guid GroupId,
        string GroupName
    );

}

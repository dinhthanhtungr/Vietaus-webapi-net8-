using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.Queries;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.CompanyFeatures.ServiceContracts
{
    public interface ICompanyService
    {
        Task<OperationResult<PagedResult<GetCompanies>>> GetCompaniesAsync(CompaniesQuery quer, CancellationToken ct);
    }
}

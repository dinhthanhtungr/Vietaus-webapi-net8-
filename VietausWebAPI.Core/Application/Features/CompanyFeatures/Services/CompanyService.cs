using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.Queries;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.CompanyFeatures.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<PagedResult<GetCompanies>>> GetCompaniesAsync(CompaniesQuery query, CancellationToken token)
        {
            try
            {
                var companies = _unitOfWork.CompanyRepository.Query();
               
                // Pagination
                var totalItems = companies.Count();
                var items = companies
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(c => new GetCompanies
                    {
                        CompanyId = c.CompanyId,
                        CompanyName = c.Name,
                    })
                    .ToList();

                // Replace the instantiation of PagedResult<GetCompanies> with the correct constructor arguments
                var pagedResult = new PagedResult<GetCompanies>(
                    items,
                    totalItems,
                    query.PageNumber,
                    query.PageSize
                );
                return OperationResult<PagedResult<GetCompanies>>.Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return OperationResult<PagedResult<GetCompanies>>.Fail($"An error occurred while retrieving companies: {ex.Message}");
            }
        }
    }
}

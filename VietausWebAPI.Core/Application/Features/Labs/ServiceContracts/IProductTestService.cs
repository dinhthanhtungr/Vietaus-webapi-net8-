using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductStandardFeature;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductTestFeature;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductTestFeature;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.ServiceContracts
{
    public interface IProductTestService
    {
        Task<PagedResult<ProductTestDTO>> GetAllAsync(ProductTestQuery productTest);
        Task<ProductTestDTO> GetPagedByIdAsync(string ExternalId);
    }
}

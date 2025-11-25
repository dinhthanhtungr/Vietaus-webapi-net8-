//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.ProductInspectionFeature;
//using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductInspectionFeature;
//using VietausWebAPI.Core.Application.Shared.Models.PageModels;
//using VietausWebAPI.Core.Domain.Entities;

//namespace VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.QAQCFeatures
//{
//    public interface IProductInspectionService
//    {
//        Task<OperationResult> PostProductInspectionServiceAsync(PostProductInspectionRequest productInspection);
//        Task<PagedResult<ProductInspectionSummary>> GetProductInspectionPagedAsync(ProductInspectionQuery? query);

//        Task<ProductInspectionInformation> GetProductInspectionByIdAsync(Guid id);
//        Task DeleteCOAService(Guid id);
//        Task<byte[]> GeneralPdfService(Guid id);
//        Task<byte[]> GeneralQCPdfService(StatisticalReportQuery query);
//    }
//}

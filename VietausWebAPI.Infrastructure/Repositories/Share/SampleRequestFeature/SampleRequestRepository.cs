using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Share.SampleRequestFeature
{
    public class SampleRequestRepository : ISampleRequestRepository
    {
        private readonly ApplicationDbContext _context; 
        public SampleRequestRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        /// <summary>
        /// Thêm một yêu cầu mẫu mới vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="sampleRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task AddAsync(SampleRequest sampleRequest, CancellationToken ct = default)
        {
            await _context.SampleRequests.AddAsync(sampleRequest, ct);
        }

        /// <summary>
        /// Xóa mẫu theo điều kiện (thực chất là cập nhật IsActive = false)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteSampleRequestAsync(Guid id)
        {
            return await _context.SampleRequests
                .Where(e => e.SampleRequestId == id)
                .ExecuteUpdateAsync(
                setters => setters.SetProperty(e => e.IsActive, false)
                );

        }

        /// <summary>
        /// Lấy ExternalId mới nhất bắt đầu bằng tiền tố cho trước.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string?> GetLatestExternalIdStartsWithAsync(string prefix)
        {
            return await _context.SampleRequests
                .Where(e => e.ExternalId.StartsWith(prefix))
                .OrderByDescending(e => e.ExternalId.Length)   // dài hơn => số lớn hơn
                .ThenByDescending(e => e.ExternalId)           // cùng độ dài thì so chuỗi
                .Select(e => e.ExternalId)
                .FirstOrDefaultAsync();
        }


        /// <summary>
        /// Tạo lệnh query để truy vấn yêu cầu mẫu từ cơ sở dữ liệu.
        /// </summary>
        /// <returns></returns>
        public IQueryable<SampleRequest> Query(bool track = false)
        {
            var db = _context.SampleRequests.AsQueryable();
            return track ? db : db.AsNoTracking();
        }

        //public async Task<int> UpdateSampleRequestAsync(UpdateSampleRequest sampleRequest, CancellationToken ct = default)
        //{

        //    static DateTime? AsUnspecified(DateTime? dt)
        //            => dt.HasValue ? DateTime.SpecifyKind(dt.Value, DateTimeKind.Unspecified) : null;


        //    var sampleRequestInt = await _context.SampleRequests
        //        .Where(e => e.SampleRequestId == sampleRequest.SampleRequestId)
        //        .ExecuteUpdateAsync(setters => setters
        //            //.SetProperty(e => e.ProductId, e => sampleRequest.ProductId )
        //            .SetProperty(e => e.CustomerId, e => sampleRequest.CustomerId)
        //            //.SetProperty(e => e.RealDeliveryDate, e => sampleRequest.RealDeliveryDate ?? e.RealDeliveryDate)
        //            //.SetProperty(e => e.RequestTestSampleDate, e => sampleRequest.RequestTestSampleDate ?? e.RequestTestSampleDate)
        //            //.SetProperty(e => e.ExpectedDeliveryDate, e => sampleRequest.ExpectedDeliveryDate ?? e.ExpectedDeliveryDate)
        //            //.SetProperty(e => e.RequestDeliveryDate, e => sampleRequest.RequestDeliveryDate ?? e.RequestDeliveryDate)
        //            //.SetProperty(e => e.ResponseDeliveryDate, e => sampleRequest.ResponseDeliveryDate ?? e.ResponseDeliveryDate)
        //            //.SetProperty(e => e.RealPriceQuoteDate, e => sampleRequest.RealPriceQuoteDate ?? e.RealPriceQuoteDate)
        //            .SetProperty(e => e.RealDeliveryDate, e => sampleRequest.RealDeliveryDate)
        //            .SetProperty(e => e.RequestTestSampleDate, e => sampleRequest.RequestTestSampleDate )
        //            .SetProperty(e => e.ExpectedDeliveryDate, e => sampleRequest.ExpectedDeliveryDate )
        //            .SetProperty(e => e.RequestDeliveryDate, e => sampleRequest.RequestDeliveryDate )
        //            .SetProperty(e => e.ResponseDeliveryDate, e => sampleRequest.ResponseDeliveryDate)
        //            .SetProperty(e => e.RealPriceQuoteDate, e => sampleRequest.RealPriceQuoteDate )
        //            .SetProperty(e => e.ExpectedPriceQuoteDate, e => sampleRequest.ExpectedPriceQuoteDate)
        //            .SetProperty(e => e.AdditionalComment, e => sampleRequest.AdditionalComment ?? e.AdditionalComment)
        //            .SetProperty(e => e.Status, e => sampleRequest.Status ?? e.Status)
        //            .SetProperty(e => e.RequestType, e => sampleRequest.RequestType ?? e.RequestType)
        //            .SetProperty(e => e.ExpectedQuantity, e => sampleRequest.ExpectedQuantity ?? e.ExpectedQuantity)
        //            .SetProperty(e => e.ExpectedPrice, e => sampleRequest.ExpectedPrice ?? e.ExpectedPrice)
        //            .SetProperty(e => e.SampleQuantity, e => sampleRequest.SampleQuantity ?? e.SampleQuantity)
        //            .SetProperty(e => e.OtherComment, e => sampleRequest.OtherComment ?? e.OtherComment)
        //            .SetProperty(e => e.InfoType, e => sampleRequest.InfoType ?? e.InfoType)
        //            .SetProperty(e => e.CustomerProductCode, e => sampleRequest.CustomerProductCode ?? e.CustomerProductCode)
        //            .SetProperty(e => e.FormulaId, e => sampleRequest.FormulaId ?? e.FormulaId)
        //            .SetProperty(e => e.SaleComment, e => sampleRequest.SaleComment ?? e.SaleComment)
        //            .SetProperty(e => e.BranchId, e => sampleRequest.BranchId ?? e.BranchId)
        //            .SetProperty(e => e.Package, e => sampleRequest.Package ?? e.Package)
        //            .SetProperty(e => e.UpdatedBy, e => sampleRequest.UpdatedBy ?? e.UpdatedBy)
        //            .SetProperty(e => e.UpdatedDate, e => DateTime.Now)
        //        , ct);

        //    var ProductInt = await _context.Products
        //        .Where(e => e.ProductId == sampleRequest.ProductId)
        //        .ExecuteUpdateAsync(setters => setters
        //            .SetProperty(p => p.Requirement, p => sampleRequest.Product.Requirement ?? p.Requirement)
        //            .SetProperty(p => p.Name, p => sampleRequest.Product.Name ?? p.Name)
        //            .SetProperty(p => p.ColourCode, p => sampleRequest.Product.ColourCode ?? p.ColourCode)
        //            .SetProperty(p => p.ColourName, p => sampleRequest.Product.ColourName ?? p.ColourName)
        //            .SetProperty(p => p.ExpiryType, p => sampleRequest.Product.ExpiryType ?? p.ExpiryType)
        //            .SetProperty(p => p.StorageCondition, p => sampleRequest.Product.StorageCondition ?? p.StorageCondition)
        //            .SetProperty(p => p.LabComment, p => sampleRequest.Product.LabComment ?? p.LabComment)
        //            .SetProperty(p => p.ProductType, p => sampleRequest.Product.ProductType ?? p.ProductType)
        //            .SetProperty(p => p.Procedure, p => sampleRequest.Product.Procedure ?? p.Procedure)
        //            .SetProperty(p => p.Application, p => sampleRequest.Product.Application ?? p.Application)
        //            .SetProperty(p => p.ProductUsage, p => sampleRequest.Product.ProductUsage ?? p.ProductUsage)
        //            .SetProperty(p => p.PolymerMatchedIn, p => sampleRequest.Product.PolymerMatchedIn ?? p.PolymerMatchedIn)
        //            .SetProperty(p => p.Code, p => sampleRequest.Product.Code ?? p.Code)
        //            .SetProperty(p => sampleRequest.Product.EndUser, p => sampleRequest.Product.EndUser ?? p.EndUser)
        //            .SetProperty(p => sampleRequest.Product.OtherComment, p => sampleRequest.Product.OtherComment ?? p.OtherComment)
        //            .SetProperty(p => p.Unit, p => sampleRequest.Product.Unit ?? p.Unit)
        //            // ====== Số liệu / thông số ======
        //            .SetProperty(p => p.UsageRate, p => sampleRequest.Product.UsageRate ?? p.UsageRate)
        //            .SetProperty(p => p.DeltaE, p => sampleRequest.Product.DeltaE ?? p.DeltaE)
        //            .SetProperty(p => p.RecycleRate, p => sampleRequest.Product.RecycleRate ?? p.RecycleRate)
        //            .SetProperty(p => p.TaicalRate, p => sampleRequest.Product.TaicalRate ?? p.TaicalRate)
        //            .SetProperty(p => p.MaxTemp, p => sampleRequest.Product.MaxTemp ?? p.MaxTemp)
        //            .SetProperty(p => p.Weight, p => sampleRequest.Product.Weight ?? p.Weight)
        //            // ====== Chuẩn / kiểm tra / flags ======
        //            .SetProperty(p => p.FoodSafety, p => sampleRequest.Product.FoodSafety ?? p.FoodSafety)
        //            .SetProperty(p => p.RohsStandard, p => sampleRequest.Product.RohsStandard ?? p.RohsStandard)
        //            .SetProperty(p => p.WeatherResistance, p => sampleRequest.Product.WeatherResistance ?? p.WeatherResistance)
        //            .SetProperty(p => p.LightCondition, p => sampleRequest.Product.LightCondition ?? p.LightCondition)
        //            .SetProperty(p => p.VisualTest, p => sampleRequest.Product.VisualTest ?? p.VisualTest)
        //            .SetProperty(p => p.ReturnSample, p => sampleRequest.Product.ReturnSample ?? p.ReturnSample)
        //            // ====== Phân loại ======
        //            .SetProperty(p => p.CategoryId, p => sampleRequest.Product.CategoryId ?? p.CategoryId)
        //            // ====== Audit ======
        //            .SetProperty(p => p.UpdatedBy, p => sampleRequest.UpdatedBy ?? p.UpdatedBy)
        //            .SetProperty(p => p.CreatedBy, p => sampleRequest.CreatedBy ?? p.CreatedBy)
        //            .SetProperty(p => p.UpdatedDate, p => DateTime.Now) // dùng UTC để tránh Unspecified
        //        , ct);


        //    return sampleRequestInt + ProductInt;
        //}
    }
}

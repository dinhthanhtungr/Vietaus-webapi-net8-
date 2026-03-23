using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.ProductFeatures;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;

namespace VietausWebAPI.Core.Application.Features.Labs.Services.ProductFeatures
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<OperationResult<int>> ChangeCustomerByProductAsync(ChangeCustomerForProductRequest req, CancellationToken ct = default)
        {
            if (req.ProductId == Guid.Empty) return OperationResult<int>.Fail("ProductId không hợp lệ.");
            if (req.NewCustomerId == Guid.Empty) return OperationResult<int>.Fail("CustomerId mới không hợp lệ.");

            var companyId = _currentUser.CompanyId;
            var userId = _currentUser.EmployeeId;
            var now = DateTime.Now;

            await using var tx = await _unitOfWork.BeginTransactionAsync(ct);
            try 
            {
                // (1) Check product tồn tại trong company (optional nhưng nên có)
                var productExists = await _unitOfWork.ProductRepository.Query()
                    .AnyAsync(p => p.ProductId == req.ProductId && p.CompanyId == companyId && p.IsActive, ct);

                if (!productExists)
                    return OperationResult<int>.Fail("Không tìm thấy Product hoặc Product không thuộc Company hiện tại.");

                // (2) Check customer tồn tại trong company (optional nhưng nên có)
                var customerExists = await _unitOfWork.CustomerRepository.Query()
                    .AnyAsync(c => c.CustomerId == req.NewCustomerId && c.IsActive == true, ct);

                if (!customerExists)
                    return OperationResult<int>.Fail("Customer không tồn tại hoặc không thuộc Company hiện tại.");

                // (3) Update tất cả SampleRequest của product này
                var srQuery = _unitOfWork.SampleRequestRepository.Query()
                    .Where(sr => sr.CompanyId == companyId
                              && sr.IsActive
                              && sr.ProductId == req.ProductId);

                // Nếu bạn dùng EF Core 7+ (khuyên dùng)
                var affected = await srQuery.ExecuteUpdateAsync(setters => setters
                        .SetProperty(x => x.CustomerId, req.NewCustomerId)
                        .SetProperty(x => x.UpdatedDate, now)
                        .SetProperty(x => x.UpdatedBy, userId),
                    ct);

                await tx.CommitAsync(ct);
                return OperationResult<int>.Ok(affected);
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                return OperationResult<int>.Fail("Có lỗi xảy ra khi đổi khách hàng cho Product. " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách Product có phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PagedResult<GetProduct>> GetAllAsync(ProductQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 10;

                var q = _unitOfWork.ProductRepository.Query();
                var sampleRequestQ = _unitOfWork.SampleRequestRepository.Query();

                // 1) Ẩn product không có tên HOẶC không có colour code
                q = q.Where(x => !string.IsNullOrWhiteSpace(x.Name)
                              && !string.IsNullOrWhiteSpace(x.ColourCode));



                // 2) Keyword
                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim();
                    q = q.Where(x =>
                        (x.Name ?? string.Empty).Contains(keyword) ||
                        (x.ColourCode ?? string.Empty).Contains(keyword));
                }



                // 3) Filter khác
                if (query.CompanyId is Guid companyId && companyId != Guid.Empty )
                    q = q.Where(p => p.CompanyId == companyId);

                if (query.ProductId is Guid productId && productId != Guid.Empty)
                    q = q.Where(p => p.ProductId == productId);

                if (query.CustomerId is Guid customerId && customerId != Guid.Empty && query.CustomerId!= Guid.Parse("019bd983-28a1-7231-810a-14c03e090b75"))
                {
                    q = q.Where(p => sampleRequestQ
                        .Any(sr => sr.ProductId == p.ProductId && (sr.CustomerId == customerId || sr.Customer.IsLead)));
                }

                // 4) Count sau khi đã filter
                var totalCount = await q.CountAsync(ct);

                // 5) Select thủ công
                var items = await q
                    .OrderByDescending(p => p.CreatedDate)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(p => new GetProduct
                    {
                        ProductId = p.ProductId,
                        ColourCode = p.ColourCode,
                        Name = p.Name,
                        ColourName = p.ColourName,
                        Additive = p.Additive,
                        UsageRate = p.UsageRate,
                        DeltaE = p.DeltaE,
                        Requirement = p.Requirement,
                        ExpiryType = p.ExpiryType,
                        StorageCondition = p.StorageCondition,
                        LabComment = p.LabComment,
                        Procedure = p.Procedure,
                        RecycleRate = p.RecycleRate,
                        TaicalRate = p.TaicalRate,
                        Application = p.Application,
                        ProductUsage = p.ProductUsage,
                        PolymerMatchedIn = p.PolymerMatchedIn,
                        Code = p.Code,
                        EndUser = p.EndUser,
                        FoodSafety = p.FoodSafety,
                        RohsStandard = p.RohsStandard,
                        ReachStandard = p.ReachStandard,
                        MaxTemp = p.MaxTemp,
                        WeatherResistance = p.WeatherResistance,
                        LightCondition = p.LightCondition,
                        VisualTest = p.VisualTest,
                        ReturnSample = p.ReturnSample,
                        OtherComment = p.OtherComment,
                        CategoryId = p.CategoryId,
                        Weight = p.Weight,
                        Unit = p.Unit,
                        IsRecycle = p.IsRecycle,

                        // Map formulas -> List<GetSampleFormula>
                        // (Nếu muốn nhẹ hơn: chỉ lấy Top N hoặc chỉ lấy formula "current/latest" — xem phần dưới)
                        SampleFormula = p.Formulas
                            .Where(f => f.Status != "Draft" && f.Status != "Inprocess")   // hoặc "InProcess"
                            .OrderByDescending(f => f.CreatedDate)
                            .Select(f => new GetSampleFormula
                            {
                                FormulaId = f.FormulaId,
                                ExternalId = f.ExternalId,
                                Name = f.Name,
                                Status = f.Status,
                                TotalPrice = f.TotalPrice,
                                ProductionPrice = f.ProductionPrice,
                                PresidentPrice = f.PresidentPrice,
                                ProfitMarginPrice = f.ProfitMarginPrice,
                            })
                            .ToList()
                    })
                    .ToListAsync(ct);

                return new PagedResult<GetProduct>(items, totalCount, query.PageNumber, query.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetProductInformation> GetInformationByIdAsync(Guid query, CancellationToken ct = default)
        {        
            throw new Exception($"Error retrieving product information");
        }
    }
}

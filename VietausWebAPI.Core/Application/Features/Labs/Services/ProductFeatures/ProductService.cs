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
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Labs.Services.ProductFeatures
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

                var result = _unitOfWork.ProductRepository.Query();

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim();
                    // Tìm theo tên/mã NV hoặc tên/mã khách trong batch
                    result = result.Where(x =>
                        (x.Name ?? "").Contains(keyword) ||
                        (x.ColourCode ?? "").Contains(keyword) 
                    );
                }

                if (query.CompanyId.HasValue && query.CompanyId.Value != Guid.Empty)
                {
                    result = result.Where(p => p.CompanyId == query.CompanyId.Value);
                }

                if (query.ProductId.HasValue && query.ProductId.Value != Guid.Empty)
                {
                    result = result.Where(p => p.ProductId == query.ProductId.Value);
                }

                int totalCount = await result.CountAsync(ct);

                var items = await result
                    .OrderByDescending(c => c.CreatedDate) // "F1" -> "F0000000001"
                    .ProjectTo<GetProduct>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return new PagedResult<GetProduct>(items, totalCount, query.PageNumber, query.PageSize);
            }

            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tạo mới sản phẩm
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        //public async Task<ProductDTO> CreateAsync(CreateProductRequest req, CancellationToken ct = default)
        //{
        //    await using var tx = await _unitOfWork.BeginTransactionAsync();
        //    try
        //    {
        //        var entity = _mapper.Map<Product>(req);

        //        await _unitOfWork.ProductRepository.AddAsync(entity, ct);
        //        await _unitOfWork.SaveChangesAsync();

        //        // (Ví dụ) thêm các bước khác cũng nằm chung transaction ở đây
        //        // await _uow.AuditLogs.AddAsync(...);
        //        // await _uow.SaveChangesAsync(ct);

        //        await tx.CommitAsync(ct);

        //        return _mapper.Map<ProductDTO>(entity);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        await tx.RollbackAsync(ct);
        //        throw; // giữ nguyên stack trace
        //    }
        //    catch
        //    {
        //        await tx.RollbackAsync(ct);
        //        throw;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetProductInformation> GetInformationByIdAsync(Guid query, CancellationToken ct = default)
        {
            // Validate tối thiểu
            //if (query == Guid.Empty) throw new ArgumentNullException(nameof(query));
            //try
            //{
            //    var res = _unitOfWork.ProductRepository.Query()
            //        .Where(p => p.IsActive && p.ProductId == query);

            //    var dto = await res
            //        .Select(p => new GetProductInformation
            //        {
            //            ProductId = p.ProductId,
            //            ColourCode = p.ColourCode,
            //            Name = p.Name,
            //            ColourName = p.ColourName,
            //            Additive = p.Additive,
            //            UsageRate = p.UsageRate,
            //            DeltaE = p.DeltaE,
            //            Requirement = p.Requirement,
            //            ExpiryType = p.ExpiryType,
            //            StorageCondition = p.StorageCondition,
            //            LabComment = p.LabComment,
            //            //ProductType = p.ProductType,
            //            Procedure = p.Procedure,
            //            RecycleRate = p.RecycleRate,
            //            TaicalRate = p.TaicalRate,
            //            Application = p.Application,
            //            ProductUsage = p.ProductUsage,
            //            PolymerMatchedIn = p.PolymerMatchedIn,
            //            Code = p.Code,
            //            EndUser = p.EndUser,
            //            FoodSafety = p.FoodSafety,
            //            RohsStandard = p.RohsStandard,
            //            MaxTemp = p.MaxTemp,
            //            WeatherResistance = p.WeatherResistance,
            //            LightCondition = p.LightCondition,
            //            VisualTest = p.VisualTest,
            //            ReturnSample = p.ReturnSample,
            //            IsRecycle = p.IsRecycle,
            //            OtherComment = p.OtherComment,
            //            CategoryId = p.CategoryId,
            //            Weight = p.Weight,
            //            Unit = p.Unit,
            //            CreatedDate = p.CreatedDate,
            //            CreatedBy = p.CreatedBy,
            //            UpdatedDate = p.UpdatedDate,
            //            UpdatedBy = p.UpdatedBy,
            //            CompanyName = p.Company != null ? p.Company.Name : null,
            //            LastProductionDate = p.MfgProductionOrders
            //                .OrderByDescending(m => m.ManufacturingDate)
            //                .Select(m => m.ManufacturingDate)
            //                .FirstOrDefault()
            //        })
            //        .FirstOrDefaultAsync(ct);

            //    if (dto == null) throw new Exception("Product not found.");

            //    dto.LastVUFormulaUse = await _unitOfWork.FormulaRepository.Query()
            //      .Where(f => f.ProductId == query)
            //      // chọn "last" theo CheckDate rồi tới SentDate rồi CreatedDate
            //      .OrderByDescending(f => f.CheckDate ?? f.SentDate ?? f.CreatedDate)
            //      .Select(f => new GetFormula
            //      {
            //          FormulaId = f.FormulaId,
            //          ExternalId = f.ExternalId,
            //          Note = f.Note,
            //          Name = f.Name,
            //          ProductCode = f.Product.Code, // nếu có
            //          Status = f.Status,
            //          TotalPrice = f.TotalPrice,
            //          CheckDate = f.CheckDate,
            //          CheckNameSnapshot = f.CheckByNavigation != null ? f.CheckByNavigation.FullName : null,     // đổi theo tên field thực tế
            //          SentDate = f.SentDate,
            //          SentByNameSnapshot = f.SentByNavigation != null ? f.SentByNavigation.FullName : null,     // đổi theo tên field thực tế
            //          CreatedDate = f.CreatedDate,
            //          CreatedByName = f.CreatedByNavigation != null ? f.CreatedByNavigation.FullName : null,       // đổi theo tên field thực tế
            //          IsSelect = f.IsSelect,
            //          materialFormulas = f.FormulaMaterials
            //              .OrderBy(m => m.MaterialNameSnapshot)
            //              .Select(m => new GetMaterialFormula
            //              {
            //                  MaterialId = m.MaterialId,
            //                  CategoryId = m.CategoryId,
            //                  Quantity = m.Quantity,
            //                  UnitPrice = m.UnitPrice,
            //                  TotalPrice = m.TotalPrice,
            //                  MaterialNameSnapshot = m.MaterialNameSnapshot,
            //                  MaterialExternalIdSnapshot = m.MaterialExternalIdSnapshot,
            //                  Unit = m.Unit
            //              })
            //              .ToList()
            //      })
            //      .FirstOrDefaultAsync(ct);


            //    dto.lastVAFormulaUse = await _unitOfWork.ManufacturingFormulaRepository.Query()
            //        .Where(mf => mf.VUFormula != null && mf.VUFormula.ProductId == query && mf.IsStandard)
            //        .OrderByDescending(mf => mf.CreatedDate) // hoặc LastUsedDate nếu có
            //        .Select(mf => new GetSummaryMfgFormula
            //        {
            //            ManufacturingFormulaId = mf.ManufacturingFormulaId,
            //            ExternalId = mf.ExternalId,
            //            Name = mf.Name,
            //            TotalPrice = mf.TotalPrice,
            //            isStandard = mf.IsStandard,
            //            IsSelect = mf.IsSelect,
            //            ManufacturingFormulaMaterials = mf.ManufacturingFormulaMaterials
            //                .OrderBy(mm => mm.MaterialNameSnapshot)
            //                .Select(mm => new GetSampleMfgFormulaMaterial
            //                {
            //                    MaterialId = mm.MaterialId,
            //                    CategoryId = mm.CategoryId,
            //                    Quantity = mm.Quantity,
            //                    UnitPrice = mm.UnitPrice,
            //                    MaterialNameSnapshot = mm.MaterialNameSnapshot,
            //                    MaterialExternalIdSnapshot = mm.MaterialExternalIdSnapshot,
            //                    Unit = mm.Unit
            //                })
            //                .ToList()
            //        })
            //        .FirstOrDefaultAsync(ct);

            //    return dto;
            //}
            //catch (Exception ex)
            //{
                //throw new Exception($"Error retrieving product information: {ex.Message}", ex);
                throw new Exception($"Error retrieving product information");
            //}
        }
    }
}

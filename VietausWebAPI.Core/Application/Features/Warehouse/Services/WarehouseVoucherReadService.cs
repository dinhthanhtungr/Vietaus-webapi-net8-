using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices;
using VietausWebAPI.Core.Application.Features.Warehouse.Queries;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.Warehouse.Services
{
    public class WarehouseVoucherReadService : IWarehouseVoucherReadService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WarehouseVoucherReadService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<PagedResult<WarehouseVoucherDto>>> GetWarehouseVouchersAsync(WarehouseVoucherReadQuery query)
        {
            try
            {
                var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
                var pageSize = query.PageSize <= 0 ? 15 : query.PageSize;

                var voucherQuery = _unitOfWork.WarehouseVoucherReadRepository.Query()
                    .AsNoTracking()
                    .Where(x => x.RequestId != null);

                if (query.CompanyId.HasValue)
                    voucherQuery = voucherQuery.Where(x => x.CompanyId == query.CompanyId.Value);

                if (query.VoucherType.HasValue)
                    voucherQuery = voucherQuery.Where(x => x.VoucherType == query.VoucherType.Value);

                if (!string.IsNullOrWhiteSpace(query.Status))
                {
                    var status = query.Status.Trim();
                    voucherQuery = voucherQuery.Where(x => x.Status != null && x.Status.Contains(status));
                }

                if (query.FromDate.HasValue)
                {
                    var from = query.FromDate.Value.Date;
                    voucherQuery = voucherQuery.Where(x => x.CreatedDate >= from);
                }

                if (query.ToDate.HasValue)
                {
                    var to = query.ToDate.Value.Date.AddDays(1).AddTicks(-1);
                    voucherQuery = voucherQuery.Where(x => x.CreatedDate <= to);
                }

                var detailKeywordQuery = _unitOfWork.WarehouseVoucherDetailReadRepository.Query()
                    .AsNoTracking();

                var headerQuery =
                    from v in voucherQuery
                    join r in _unitOfWork.WarehouseRequestRepository.Query().AsNoTracking()
                        on v.RequestId equals r.RequestId into requestGroup
                    from r in requestGroup.DefaultIfEmpty()
                    select new
                    {
                        v.VoucherId,
                        v.VoucherCode,
                        v.VoucherType,
                        v.Status,
                        v.CreatedDate,
                        v.RequestId,
                        v.CompanyId,
                        v.CreatedBy,

                        RequestCode = r != null ? r.RequestCode : "",
                        RequestName = r != null ? r.RequestName : "",
                        ReqStatus = r != null ? (WarehouseRequestStatus?)r.ReqStatus : null,
                        ReqType = r != null ? (WareHouseRequestType?)r.ReqType : null,
                        CodeFromRequest = r != null ? r.codeFromRequest : ""
                    };

                if (query.ReqType.HasValue)
                {
                    headerQuery = headerQuery.Where(x => x.ReqType == query.ReqType.Value);
                }

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var kw = query.Keyword.Trim();

                    headerQuery = headerQuery.Where(x =>
                        (!string.IsNullOrEmpty(x.VoucherCode) && x.VoucherCode.Contains(kw)) ||
                        (!string.IsNullOrEmpty(x.RequestCode) && x.RequestCode.Contains(kw)) ||
                        (!string.IsNullOrEmpty(x.RequestName) && x.RequestName.Contains(kw)) ||
                        (!string.IsNullOrEmpty(x.CodeFromRequest) && x.CodeFromRequest.Contains(kw)) ||
                        detailKeywordQuery.Any(d =>
                            d.VoucherId == x.VoucherId &&
                            (
                                (d.ProductCode != null && d.ProductCode.Contains(kw)) ||
                                (d.ProductName != null && d.ProductName.Contains(kw)) ||
                                (d.LotNumber != null && d.LotNumber.Contains(kw))
                            ))
                    );
                }

                var totalCount = await headerQuery.CountAsync();

                var headers = await headerQuery
                    .OrderByDescending(x => x.CreatedDate)
                    .ThenByDescending(x => x.VoucherId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (!headers.Any())
                {
                    var empty = new PagedResult<WarehouseVoucherDto>(
                        new List<WarehouseVoucherDto>(),
                        totalCount,
                        pageNumber,
                        pageSize);

                    return OperationResult<PagedResult<WarehouseVoucherDto>>.Ok(empty);
                }

                var voucherIds = headers.Select(x => x.VoucherId).ToList();
                var companyIds = headers.Select(x => x.CompanyId).Distinct().ToList();
                var employeeIds = headers.Select(x => x.CreatedBy).Distinct().ToList();

                var companyMap = await _unitOfWork.CompanyRepository.Query()
                    .AsNoTracking()
                    .Where(x => companyIds.Contains(x.CompanyId))
                    .Select(x => new
                    {
                        x.CompanyId,
                        x.Name
                    })
                    .ToDictionaryAsync(x => x.CompanyId, x => x.Name);

                var employeeMap = await _unitOfWork.EmployeesRepository.Query()
                    .AsNoTracking()
                    .Where(x => employeeIds.Contains(x.EmployeeId))
                    .Select(x => new
                    {
                        x.EmployeeId,
                        Name = x.FullName
                    })
                    .ToDictionaryAsync(x => x.EmployeeId, x => x.Name);

                var details = await _unitOfWork.WarehouseVoucherDetailReadRepository.Query()
                    .AsNoTracking()
                    .Where(x => voucherIds.Contains(x.VoucherId))
                    .OrderBy(x => x.VoucherId)
                    .ThenBy(x => x.LineNo)
                    .Select(x => new WarehouseVoucherDetailDto
                    {
                        VoucherDetailId = x.VoucherDetailId,
                        VoucherId = x.VoucherId,
                        LineNo = x.LineNo,
                        ProductCode = x.ProductCode,
                        ProductName = x.ProductName,
                        LotNumber = x.LotNumber,
                        QtyKg = x.QtyKg,
                        Bags = x.Bags,
                        SlotId = x.SlotId,
                        PurposeId = x.PurposeId,
                        IsIncrease = x.IsIncrease,
                        ExpiryDate = x.ExpiryDate,
                        VoucherType = x.VoucherType,
                        Note = x.Note
                    })
                    .ToListAsync();



                var detailMap = details
                    .GroupBy(x => x.VoucherId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                var requestExternalIds = headers
                    .Where(x => x.ReqType == WareHouseRequestType.ImportOther) // sửa theo enum thật của bạn
                    .Select(x => x.CodeFromRequest)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .ToList();

                var supplierMap = await (
                    from po in _unitOfWork.PurchaseOrderRepository.Query().AsNoTracking()
                    join pos in _unitOfWork.PurchaseOrderSnapshotRepository.Query().AsNoTracking()
                        on po.PurchaseOrderSnapshotId equals pos.PurchaseOrderSnapshotId
                    where po.ExternalId != null && requestExternalIds.Contains(po.ExternalId)
                    select new
                    {
                        po.ExternalId,
                        pos.SupplierNameSnapshot,
                        pos.SupplierExternalIdSnapshot
                    })
                    .ToDictionaryAsync(
                        x => x.ExternalId!,
                        x => new
                        {
                            x.SupplierNameSnapshot,
                            x.SupplierExternalIdSnapshot
                        });

                var items = headers.Select(x =>
                {
                    supplierMap.TryGetValue(x.CodeFromRequest ?? "", out var supplierInfo);

                    return new WarehouseVoucherDto
                    {
                        VoucherId = x.VoucherId,
                        VoucherCode = x.VoucherCode,
                        VoucherType = x.VoucherType,
                        Status = x.Status,
                        CreatedDate = x.CreatedDate,

                        RequestId = x.RequestId,
                        RequestCode = x.RequestCode,
                        RequestName = x.RequestName,
                        ReqStatus = x.ReqStatus,
                        ReqType = x.ReqType,
                        CodeFromRequest = x.CodeFromRequest,

                        CompanyId = x.CompanyId,
                        CompanyName = companyMap.TryGetValue(x.CompanyId, out var companyName)
                            ? companyName
                            : string.Empty,

                        CreatedBy = x.CreatedBy,
                        CreatedByName = employeeMap.TryGetValue(x.CreatedBy, out var employeeName)
                            ? employeeName
                            : string.Empty,

                        SupplierName = supplierInfo?.SupplierNameSnapshot ?? string.Empty,
                        SupplierExternalId = supplierInfo?.SupplierExternalIdSnapshot ?? string.Empty,

                        Details = detailMap.TryGetValue(x.VoucherId, out var voucherDetails)
                            ? voucherDetails
                            : new List<WarehouseVoucherDetailDto>()
                    };
                }).ToList();

                var result = new PagedResult<WarehouseVoucherDto>(items, totalCount, pageNumber, pageSize);
                return OperationResult<PagedResult<WarehouseVoucherDto>>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<PagedResult<WarehouseVoucherDto>>.Fail(ex.Message);
            }
        }

        public async Task<OperationResult<WarehouseVoucherDto>> GetWarehouseVoucherByIdAsync(long voucherId)
        {
            try
            {
                var header =
                    await (
                        from v in _unitOfWork.WarehouseVoucherReadRepository.Query().AsNoTracking()
                        join r in _unitOfWork.WarehouseRequestRepository.Query().AsNoTracking()
                            on v.RequestId equals r.RequestId into requestGroup
                        from r in requestGroup.DefaultIfEmpty()
                        where v.VoucherId == voucherId
                        select new
                        {
                            v.VoucherId,
                            v.VoucherCode,
                            v.VoucherType,
                            v.Status,
                            v.CreatedDate,
                            v.RequestId,
                            v.CompanyId,
                            v.CreatedBy,

                            RequestCode = r != null ? r.RequestCode : "",
                            RequestName = r != null ? r.RequestName : "",
                            ReqStatus = r != null ? (WarehouseRequestStatus?)r.ReqStatus : null,
                            ReqType = r != null ? (WareHouseRequestType?)r.ReqType : null,
                            CodeFromRequest = r != null ? r.codeFromRequest : ""
                        }
                    ).FirstOrDefaultAsync();

                if (header == null)
                    return OperationResult<WarehouseVoucherDto>.Fail("Không tìm thấy phiếu kho.");

                var companyName = await _unitOfWork.CompanyRepository.Query()
                    .AsNoTracking()
                    .Where(x => x.CompanyId == header.CompanyId)
                    .Select(x => x.Name)
                    .FirstOrDefaultAsync() ?? string.Empty;

                var employeeName = await _unitOfWork.EmployeesRepository.Query()
                    .AsNoTracking()
                    .Where(x => x.EmployeeId == header.CreatedBy)
                    .Select(x => x.FullName)
                    .FirstOrDefaultAsync() ?? string.Empty;

                var details = await _unitOfWork.WarehouseVoucherDetailReadRepository.Query()
                    .AsNoTracking()
                    .Where(x => x.VoucherId == voucherId)
                    .OrderBy(x => x.LineNo)
                    .Select(x => new WarehouseVoucherDetailDto
                    {
                        VoucherDetailId = x.VoucherDetailId,
                        VoucherId = x.VoucherId,
                        LineNo = x.LineNo,
                        ProductCode = x.ProductCode,
                        ProductName = x.ProductName,
                        LotNumber = x.LotNumber,
                        QtyKg = x.QtyKg,
                        Bags = x.Bags,
                        SlotId = x.SlotId,
                        PurposeId = x.PurposeId,
                        IsIncrease = x.IsIncrease,
                        ExpiryDate = x.ExpiryDate,
                        VoucherType = x.VoucherType,
                        Note = x.Note
                    })
                    .ToListAsync();

                var result = new WarehouseVoucherDto
                {
                    VoucherId = header.VoucherId,
                    VoucherCode = header.VoucherCode,
                    VoucherType = header.VoucherType,
                    Status = header.Status,
                    CreatedDate = header.CreatedDate,

                    RequestId = header.RequestId,
                    RequestCode = header.RequestCode,
                    RequestName = header.RequestName,
                    ReqStatus = header.ReqStatus,
                    ReqType = header.ReqType,
                    CodeFromRequest = header.CodeFromRequest,

                    CompanyId = header.CompanyId,
                    CompanyName = companyName,

                    CreatedBy = header.CreatedBy,
                    CreatedByName = employeeName,

                    Details = details
                };

                return OperationResult<WarehouseVoucherDto>.Ok(result);
            }
            catch (Exception ex)
            {
                return OperationResult<WarehouseVoucherDto>.Fail(ex.Message);
            }
        }
    }
}

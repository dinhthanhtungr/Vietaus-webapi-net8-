using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Employees;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs.ResultDtos;
using VietausWebAPI.Core.Application.Features.Sales.Helpers.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.Shared.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.HrSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static QuestPDF.Helpers.Colors;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerFeatures
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _CurrentUser;
        private readonly IVisibilityHelper _visibilityHelper;

        public CustomerService (IUnitOfWork unitOfWork
            , IMapper mapper
            , ICurrentUser currentUser
            , IVisibilityHelper visibilityHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _CurrentUser = currentUser;
            _visibilityHelper = visibilityHelper;
        }


        // ======================================================================== Get ========================================================================

        /// <summary>
        /// Xem danh sách tất cả khách hàng có phân trang
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<OperationResult<PagedResult<GetReviewCustomer>>> GetAllAsync(CustomerQuery? query)
        {
            var pagedResult = await _unitOfWork.CustomerRepository.GetAllAsync(query);
            try
            {
                var pagedResultMapped = _mapper.Map<PagedResult<GetReviewCustomer>>(pagedResult);
                return OperationResult<PagedResult<GetReviewCustomer>>.Ok(pagedResultMapped);
            }
            catch (Exception ex)
            {
                return OperationResult<PagedResult<GetReviewCustomer>>.Fail($"Lỗi khi lấy danh sách nhân viên: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách khách hàng được phân công cho nhân viên cụ thể 
        /// và yêu cầu đặt biệt của khách hàng ở đơn hàng gần nhất
        /// Service sử dụng cho chọn khách hàng để tạo đơn hàng 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult<PagedResult<GetReviewCustomer>>> GetCustomerByEmployeeAssignment(CustomerQuery? query, CancellationToken ct = default)
        {
            try
            {
                // ----- Input & context -----
                query ??= new CustomerQuery();
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                // Lấy scope từ helper (đã biết EmployeeId/Group/Role/NowUtc)
                var viewer = await _visibilityHelper.BuildViewerScopeAsync(ct);
                var now = viewer.Now;
                var empId = Guid.Empty;

                // ----- Base (tenant, active) -----
                var baseQuery = _unitOfWork.CustomerRepository.Query();

                // ----- Search & date filters (áp dụng TRƯỚC khi shape) -----
                if (!string.IsNullOrWhiteSpace(query.keyword))
                {
                    var kw = query.keyword.Trim();
                    baseQuery = baseQuery.Where(c => c.CustomerName.Contains(kw) || c.ExternalId.Contains(kw));
                    // Nếu dùng PostgreSQL, có thể đổi sang EF.Functions.ILike(...)
                }

                if (query.From.HasValue)
                    baseQuery = baseQuery.Where(c => c.CreatedDate >= query.From.Value);

                if (query.To.HasValue)
                {
                    var toEnd = query.To.Value.Date.AddDays(1).AddTicks(-1);
                    baseQuery = baseQuery.Where(c => c.CreatedDate <= toEnd);
                }

                // ----- Visibility filter qua helper -----
                baseQuery = _visibilityHelper.ApplyCustomer(baseQuery, viewer);

                var shapedQuery = baseQuery
                    .Select(c => new
                    {
                        Entity = c,

                        HasActiveAssignment = c.CustomerAssignments.Any(a => a.IsActive),
                        IsLead = c.IsLead,

                        // "độ mới" trong sắp xếp
                        LatestClaimExpiresAt = c.CustomerClaims
                            .Where(cl => cl.IsActive && cl.Type == ClaimType.Work && cl.ExpiresAt > now)
                            .Select(cl => (DateTime?)cl.ExpiresAt)
                            .Max(),

                        ManagedByCurrentScope =
                            c.CustomerAssignments.Any(a =>
                                a.IsActive && (a.EmployeeId == viewer.EmployeeId
                                               || (viewer.IsLeader && a.GroupId == viewer.GroupId))
                            )
                            ||
                            c.CustomerClaims.Any(cl =>
                                cl.IsActive && cl.Type == ClaimType.Work && cl.ExpiresAt > now
                                && (cl.EmployeeId == viewer.EmployeeId
                                    || (viewer.IsLeader && cl.GroupId == viewer.GroupId))
                            ),

                        LatestAssignmentScope = (!viewer.IsLeader
                            ? c.CustomerAssignments.Where(a => a.IsActive)
                            : c.CustomerAssignments.Where(a => a.IsActive && a.GroupId == viewer.GroupId)
                        )
                        .OrderByDescending(a => a.CreatedDate)
                        .Select(a => new
                        {
                            EmpId = (Guid?)a.EmployeeId,
                            EmpName = a.Employee.FullName,
                            GroupId = (Guid?)a.GroupId
                        })
                        .FirstOrDefault(),
                    })
                    .Select(x => new
                    {
                        x.Entity,

                        IsSaled = x.HasActiveAssignment && !x.IsLead,
                        IsLead = x.IsLead && !x.HasActiveAssignment,

                        x.LatestClaimExpiresAt,

                        ManagerEmpIdScope = x.LatestAssignmentScope != null ? x.LatestAssignmentScope.EmpId : null,
                        ManagerEmpNameScope = x.LatestAssignmentScope != null ? x.LatestAssignmentScope.EmpName : null,
                        GroupIdScope = x.LatestAssignmentScope != null ? x.LatestAssignmentScope.GroupId : null,

                        x.ManagedByCurrentScope,

                        SortBucket = (x.HasActiveAssignment && !x.IsLead) ? 0 : (x.IsLead && !x.HasActiveAssignment ? 1 : 2)
                    });


                // ----- Lọc theo nhân viên (dropdown) -----
                if (query.EmployeeId.HasValue)
                {
                    empId = query.EmployeeId.Value;
                    shapedQuery = shapedQuery.Where(x =>
                        x.Entity.CustomerAssignments.Any(a => a.IsActive && a.EmployeeId == empId)
                    );
                }

                // ----- Lọc theo loại KH (dropdown) -----
                if (query.type == CustomerType.Saled)
                    shapedQuery = shapedQuery.Where(x => x.IsSaled && !x.Entity.IsLead);
                else if (query.type == CustomerType.Lead)
                    shapedQuery = shapedQuery.Where(x => x.IsLead);
                // All -> không filter thêm

                // ----- Count & Page dùng CÙNG query -----
                var totalItems = await shapedQuery.CountAsync(ct);

                var pageCustomers = await shapedQuery
                    .OrderBy(x => x.SortBucket)
                    .ThenBy(x => x.GroupIdScope == null ? 1 : 0)
                    .ThenBy(x => x.GroupIdScope)
                    .ThenBy(x => x.ManagerEmpNameScope)
                    .ThenByDescending(x => x.LatestClaimExpiresAt ?? x.Entity.CreatedDate)
                    .ThenByDescending(x => x.Entity.CustomerId)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)

                    // Bước 1: lấy LatestOrder 1 lần
                    .Select(x => new
                    {
                        x.Entity.CustomerId,
                        x.Entity.ExternalId,
                        CustomerName = x.Entity.CustomerName,
                        x.Entity.RegistrationNumber,
                        x.Entity.CustomerGroup,
                        AssigneeEmpId_Ind = x.ManagerEmpIdScope,
                        AssigneeEmpName_Ind = x.ManagerEmpNameScope,

                        LatesrClaim = x.Entity.CustomerClaims
                            .Where(cl => cl.IsActive && cl.Type == ClaimType.Work && cl.ExpiresAt > now)
                            .OrderByDescending(cl => cl.ExpiresAt)
                            .Select(cl => new { EmpId = (Guid?)cl.EmployeeId, EmpName = cl.Employee.FullName })
                            .FirstOrDefault(),


                        PrimaryContact = x.Entity.Contacts
                            .OrderByDescending(co => co.IsPrimary)
                            .Select(co => new { co.Phone, FullName = (co.FirstName + " " + co.LastName).Trim() })
                            .FirstOrDefault(),

                        AddressFromAddress = x.Entity.Addresses
                            .OrderByDescending(a => a.IsPrimary)
                            .Select(a => a.AddressLine)
                            .FirstOrDefault(),

                        LatestOrder = x.Entity.MerchandiseOrders
                            .OrderByDescending(o => o.CreateDate)
                            .Select(o => new { o.Note, o.PaymentType, o.ShippingMethod })
                            .FirstOrDefault(),

                        IsLeadOnly = x.IsLead,
                        ManagedByCurrentScope = x.ManagedByCurrentScope
                    })
                    .Select(x => new
                    {
                        x.CustomerId,
                        x.ExternalId,
                        x.CustomerName,
                        x.RegistrationNumber,
                        x.CustomerGroup,
                        x.AssigneeEmpId_Ind,
                        x.AssigneeEmpName_Ind,

                        ClaimEmpId_Ind = x.LatesrClaim != null ? x.LatesrClaim.EmpId : null,
                        ClaimEmpName_Ind = x.LatesrClaim != null ? x.LatesrClaim.EmpName : null,

                        PhoneFromContact = x.PrimaryContact != null ? x.PrimaryContact.Phone : null,
                        ReceiverFromContact = x.PrimaryContact != null ? x.PrimaryContact.FullName : null,
                        AddressFromAddress = x.AddressFromAddress,

                        CustomerSpecialRequirement = x.LatestOrder != null ? x.LatestOrder.Note : null,
                        PaymentType = x.LatestOrder != null ? x.LatestOrder.PaymentType : null,
                        DeliveryType = x.LatestOrder != null ? x.LatestOrder.ShippingMethod : null,

                        x.IsLeadOnly,
                        x.ManagedByCurrentScope
                    })
                    .ToListAsync(ct);

                var leadIds = pageCustomers
                    .Where(p => p.IsLeadOnly)
                    .Select(p => p.CustomerId)
                    .Distinct()
                    .ToList();

                var leadClaimNamesRows = await _unitOfWork.CustomerRepository.Query()
                    .Where(c => leadIds.Contains(c.CustomerId))
                    .SelectMany(c => c.CustomerClaims
                        .Where(cl => cl.IsActive && cl.Type == ClaimType.Work && cl.ExpiresAt > now)
                        .Select(cl => new { c.CustomerId, Name = cl.Employee.FullName })
                    )
                    .ToListAsync(ct);

                var leadClaimNamesMap = leadClaimNamesRows
                    .GroupBy(x => x.CustomerId)
                    .ToDictionary(g => g.Key, g => g.Select(z => z.Name).Distinct().ToArray());


                // ----- Map DTO -----
                var result = pageCustomers.Select(x => new GetReviewCustomer
                {
                    CustomerId = x.CustomerId,
                    ExternalId = x.ExternalId,
                    Name = x.CustomerName,
                    RegNo = x.RegistrationNumber,
                    Group = x.CustomerGroup,

                    // EmployeeId ưu tiên theo phạm vi như cũ
                    EmployeeId = x.AssigneeEmpId_Ind ?? x.ClaimEmpId_Ind,

                    EmployeeName = x.IsLeadOnly
                        ? string.Join(", ", leadClaimNamesMap.TryGetValue(x.CustomerId, out var names) ? names : Array.Empty<string>())
                        : (x.AssigneeEmpName_Ind ?? x.ClaimEmpName_Ind ?? string.Empty),

                    Phone = x.PhoneFromContact,
                    Address = x.AddressFromAddress,
                    DeliveryName = x.ReceiverFromContact,
                    CustomerSpectialRequirement = x.CustomerSpecialRequirement ?? string.Empty,
                    paymentType = x.PaymentType ?? string.Empty,
                    delivieryType = x.DeliveryType ?? string.Empty,
                    IsLead = x.IsLeadOnly,
                    IsManagedByCurrent = x.ManagedByCurrentScope,
                    CanOpenDetail = !x.IsLeadOnly || (x.IsLeadOnly && x.ManagedByCurrentScope)
                }).ToList();

                return OperationResult<PagedResult<GetReviewCustomer>>.Ok(
                    new PagedResult<GetReviewCustomer>(result, totalItems, query.PageNumber, query.PageSize)
                );
            }
            catch (Exception ex)
            {
                return OperationResult<PagedResult<GetReviewCustomer>>.Fail($"Lỗi khi lấy danh sách khách hàng: {ex.Message}");
            }
        }


        /// <summary>
        /// Lấy thông tin chi tiết khách hàng theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<GetCustomer> GetCustomerByIdAsync(Guid id, CancellationToken ct = default)
        {
            return _unitOfWork.CustomerRepository.GetCustomerByIdAsync(id);
        }

        /// <summary>
        /// Lây thông tin khách hàng theo ID cho phòng Sales
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult<GetCustomer>> GetCustomerByIdForSalesAsync(Guid id, CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var userId = _CurrentUser.EmployeeId;
            var companyId = _CurrentUser.CompanyId;

            // 1) Leader scope (nếu có)
            var leaderGroupId = await _unitOfWork.MemberInGroupRepository.Query()
                .Where(g => g.Profile == userId && g.IsAdmin == true && g.IsActive == true)
                .Select(g => (Guid?)g.GroupId)
                .FirstOrDefaultAsync(ct);

            var isLeader = leaderGroupId.HasValue && leaderGroupId.Value != Guid.Empty;
            var scopeGroupId = leaderGroupId ?? Guid.Empty;

            // 2) Kiểm tra tồn tại + công ty + lấy cờ IsLead
            var exist = await _unitOfWork.CustomerRepository.Query()
                .Where(c => c.CustomerId == id && c.CompanyId == companyId)
                .Select(c => new { c.CustomerId, c.IsLead })
                .FirstOrDefaultAsync(ct);


            if (exist is null)
                return OperationResult<GetCustomer>.Fail("Không tìm thấy khách hàng.");

            // 3) Tính quyền
            // 3.1 Assignment (nhân viên / nhóm)
            var managedByEmployee = await _unitOfWork.CustomerAssignmentRepository.Query()
                .AnyAsync(a => a.IsActive && a.CustomerId == id && a.EmployeeId == userId, ct);

            var managedByGroup = isLeader && await _unitOfWork.CustomerAssignmentRepository.Query()
                .AnyAsync(a => a.IsActive && a.CustomerId == id && a.GroupId == scopeGroupId, ct);

            // 3.2 Claim Work còn hạn (cho non-lead)
            var hasActiveClaim = await _unitOfWork.CustomerClaimRepository.Query()
                .AnyAsync(cl => cl.IsActive
                             && cl.Type == ClaimType.Work
                             && cl.ExpiresAt > now
                             && cl.CustomerId == id
                             && (cl.EmployeeId == userId || (isLeader && cl.GroupId == scopeGroupId)), ct);

            bool canOpenDetail =
                exist.IsLead
                    ? (managedByEmployee || managedByGroup || hasActiveClaim)
                    : (managedByEmployee || managedByGroup || hasActiveClaim);


            if (!canOpenDetail)
                return OperationResult<GetCustomer>.Fail("Bạn không có quyền xem khách hàng này.");

            // 4) Có quyền → lấy chi tiết bằng Repository (giữ nguyên code cũ)
            var dto = await _unitOfWork.CustomerRepository.GetCustomerByIdAsync(id);

            if (dto is null)
                return OperationResult<GetCustomer>.Fail("Không tải được chi tiết khách hàng.");


            dto.Notes = isLeader
                ? await GetNotesForLeaderAsync(id, scopeGroupId, companyId, userId, ct)
                : await GetNotesForSaleAsync(id, userId, companyId, ct);

            return OperationResult<GetCustomer>.Ok(dto);
        }

        /// <summary>
        /// Lấy 
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult<IReadOnlyList<GetCustomerLeadOwner>>> GetCustomerLeadOwner(Guid CustomerId, CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var companyId = _CurrentUser.CompanyId;

            var list = await _unitOfWork.CustomerClaimRepository.Query()
                .Where(cl => cl.CustomerId == CustomerId
                          && cl.IsActive
                          && cl.Type == ClaimType.Work
                          && cl.ExpiresAt > now
                          && cl.CompanyId == companyId)
                .Select(cl => new GetCustomerLeadOwner
                {
                    EmployeeId = cl.EmployeeId,
                    EmployeeName = cl.Employee.FullName,
                    EmployeeCode = cl.Employee.ExternalId,
                    GroupId = cl.GroupId,
                    GroupName = cl.Group.Name,
                    ExpiresAt = cl.ExpiresAt,
                    DaysLeft = (cl.ExpiresAt - now).Days
                })
                .OrderBy(cl => cl.ExpiresAt)
                .ToListAsync(ct);

            return OperationResult<IReadOnlyList<GetCustomerLeadOwner>>.Ok(list);
        }

        /// <summary>
        /// Hàm lấy notes khi là SALE
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="employeeId"></param>
        /// <param name="companyId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<List<GetCustomerNoteItem>> GetNotesForSaleAsync(Guid customerId, Guid employeeId, Guid companyId, CancellationToken ct)
        {
            var effectiveGroupIds = _unitOfWork.MemberInGroupRepository.Query()
                .Where(g => g.Profile == employeeId && g.IsActive == true)
                .Select(g => g.GroupId);

            var leaderIds = _unitOfWork.MemberInGroupRepository.Query()
                .Where(m => m.IsActive == true && m.IsAdmin == true && effectiveGroupIds.Contains(m.GroupId))
                .Select(m => m.Profile)
                .Distinct();

            return await _unitOfWork.CustomerNoteRepository.Query(false).AsNoTracking()
                .Where(n => n.CustomerId == customerId
                         && n.CompanyId == companyId
                         && n.Visibility == NoteVisibility.Group
                         && (n.AuthorEmployeeId == employeeId || leaderIds.Contains(n.AuthorEmployeeId)))
                .OrderByDescending(n => n.AuthorEmployeeId == employeeId) // của tôi trước
                .ThenByDescending(n => n.CreatedAt)
                .Select(n => new GetCustomerNoteItem
                {
                    Id = n.Id,
                    Content = n.Content,
                    CreatedAt = n.CreatedAt,
                    AuthorEmployeeId = n.AuthorEmployeeId,
                    AuthorName = n.AuthorEmployee.FullName,   // EF JOIN Employees
                    AuthorGroupId = n.AuthorGroupId,
                    AuthorGroupName = n.AuthorGroup.Name      // EF JOIN Groups
                })
                .ToListAsync(ct);
        }

        /// <summary>
        /// Hàm lấy notes khi là LEADER
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="leaderGroupId"></param>
        /// <param name="companyId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<List<GetCustomerNoteItem>> GetNotesForLeaderAsync(Guid customerId, Guid leaderGroupId, Guid companyId, Guid currentUserId, CancellationToken ct)
        {
            return await _unitOfWork.CustomerNoteRepository.Query(false).AsNoTracking()
                .Where(n => n.CustomerId == customerId
                         && n.CompanyId == companyId
                         && n.Visibility == NoteVisibility.Group
                         && n.AuthorGroupId == leaderGroupId)
                .OrderByDescending(n => n.AuthorEmployeeId == currentUserId) // leader viết trước
                .ThenByDescending(n => n.CreatedAt)
                .Select(n => new GetCustomerNoteItem
                {
                    Id = n.Id,
                    Content = n.Content,
                    CreatedAt = n.CreatedAt,
                    AuthorEmployeeId = n.AuthorEmployeeId,
                    AuthorName = n.AuthorEmployee.FullName,
                    AuthorGroupId = n.AuthorGroupId,
                    AuthorGroupName = n.AuthorGroup.Name
                })
                .ToListAsync(ct);
        }

        // ======================================================================== Post ========================================================================

        /// <summary>
        /// Thêm một khách hàng mới
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<OperationResult<AddCustomerResultDto>> AddNewCustomer(PostCustomer customer)
            {
                // 0) Context & thời gian UTC
                var now = DateTime.Now;
                var companyId = _CurrentUser.CompanyId;
                var userId = _CurrentUser.EmployeeId;

                // 1) ExternalId
                if (string.IsNullOrWhiteSpace(customer.ExternalId))
                {
                    customer.ExternalId = await ExternalIdGenerator.GenerateCode(
                        "KH",
                        prefix => _unitOfWork.CustomerRepository.GetLatestExternalIdStartsWithAsync(prefix)
                    );
                }

                // 2) Validate tối thiểu: kiểm tra MST trùng (giữ code của bạn)
                var taxNorm = NormalizeTaxCode(customer.TaxNumber);
                if (!string.IsNullOrEmpty(taxNorm))
                {
                    var existedInfo = await _unitOfWork.CustomerRepository.Query()
                        .Where(c =>
                            ((c.TaxNumber ?? "")
                                .Replace("-", "")
                                .Replace(".", "")
                                .Replace(" ", "")
                                .ToUpper()) == taxNorm
                            && c.CompanyId == companyId)
                        .Select(c => new
                        {
                            c.ExternalId,
                            c.CustomerId,
                            c.CustomerName,
                            c.TaxNumber,
                            Assignment = c.CustomerAssignments
                                .Where(a => a.IsActive)
                                .OrderByDescending(a => a.CreatedDate)
                                .Select(a => new
                                {
                                    a.EmployeeId,
                                    EmployeeName = a.Employee.FullName,
                                    a.GroupId,
                                    GroupName = a.Group.Name
                                })
                                .FirstOrDefault()
                        })
                        .FirstOrDefaultAsync();

                    if (existedInfo != null)
                    {
                        var dto = new AddCustomerResultDto(
                            existedInfo.CustomerId,
                            existedInfo.ExternalId,
                            existedInfo.CustomerName,
                            existedInfo.TaxNumber ?? string.Empty,
                            existedInfo.Assignment?.EmployeeId ?? Guid.Empty,
                            existedInfo.Assignment?.EmployeeName ?? string.Empty,
                            existedInfo.Assignment?.GroupId ?? Guid.Empty,
                            existedInfo.Assignment?.GroupName ?? string.Empty
                        );

                        return OperationResult<AddCustomerResultDto>.Fail(dto,
                            $"Mã số thuế {customer.TaxNumber} đã tồn tại cho khách hàng \"{dto.Name}\" " +
                            $"và hiện đang do {(dto.EmployeeName ?? "chưa gán")} quản lý" +
                            $"{(dto.GroupName is null ? "" : $" ({dto.GroupName})")}."
                        );
                    }
                }

                // 3) Lấy group hiện tại của người tạo (để auto-claim / assign)
                var groupId = await _unitOfWork.MemberInGroupRepository.Query()
                    .Where(m => m.Profile == userId && m.IsActive == true)
                    .Select(m => (Guid?)m.GroupId)
                    .FirstOrDefaultAsync();

                if (groupId is null)
                    return OperationResult<AddCustomerResultDto>.Fail("Nhân viên sale chưa thuộc nhóm nào (Group).");

                // 4) Transaction – bắt đầu ghi
                await _unitOfWork.BeginTransactionAsync();
                try
                {
                    // 4.1) Tạo Customer (lead-first by default)
                    var customerEntity = new Customer
                    {
                        CustomerId = Guid.CreateVersion7(),
                        CustomerName = customer.CustomerName,
                        ExternalId = customer.ExternalId,
                        TaxNumber = customer.TaxNumber,
                        RegistrationNumber = customer.RegistrationNumber,
                        RegistrationAddress = customer.RegistrationAddress,
                        CustomerGroup = customer.CustomerGroup,
                        CompanyId = companyId,
                        IsActive = true,

                        CreatedBy = userId,
                        CreatedDate = now,
                        UpdatedBy = userId,
                        UpdatedDate = now,

                        // Lead-first mặc định
                        //IsLead = !customer.AssignNow || (customer.AssignNow && !customer.ConvertNow),
                        //LeadStatus = !customer.AssignNow
                        //    ? LeadStatus.Open                // default lead
                        //    : (customer.ConvertNow ? LeadStatus.Converted : LeadStatus.Qualified),

                        IsLead = true,
                        LeadStatus = LeadStatus.Claimed,

                        Addresses = customer.Addresses.Select(a => new Address
                        {
                            AddressLine = a.AddressLine,
                            City = a.City,
                            District = a.District,
                            PostalCode = a.PostalCode,
                            Country = a.Country,
                            IsPrimary = a.IsPrimary,
                            IsActive = true,
                        }).ToList(),

                        Contacts = customer.Contacts.Select(c => new Contact
                        {
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            Email = c.Email,
                            Phone = c.Phone,
                            IsPrimary = c.IsPrimary,
                            IsActive = true,
                        }).ToList(),
                    };

                    // 4.2) Nếu assign ngay → thêm assignment active
                    //if (customer.AssignNow)
                    //{
                    //    customerEntity.CustomerAssignments = new List<CustomerAssignment>
                    //    {
                    //        new CustomerAssignment
                    //        {
                    //            EmployeeId = userId,   // hoặc customer.CustomerAssignment.EmployeeId nếu cho phép chọn
                    //            GroupId = groupId.Value,
                    //            IsActive = true,

                    //            CompanyId = companyId,
                    //            CreatedBy  = userId,
                    //            CreatedDate= now,
                    //            UpdatedBy  = userId,
                    //            UpdatedDate= now
                    //        }
                    //    };
                    //}

                    await _unitOfWork.CustomerRepository.AddNewCustomer(customerEntity);
                    if (!string.IsNullOrWhiteSpace(customer.Notes))
                    {
                        var authorGroupId = groupId.Value; // group của người tạo (đã lấy trước đó)
                        await AddCustomerNoteAsync(customerEntity.CustomerId, customer.Notes.Trim());
                    }


                    await _unitOfWork.SaveChangesAsync(); // => có CustomerId

                    // 4.3) Nếu KHÔNG assign ngay → auto-claim TTL cho người tạo
                    if (!customer.AssignNow)
                    {
                        // Chặn tranh chấp: DB có unique partial 1 claim Work active / Customer
                        var claim = new CustomerClaim
                        {
                            Id = Guid.CreateVersion7(),
                            CustomerId = customerEntity.CustomerId,
                            EmployeeId = userId,
                            GroupId = groupId.Value,
                            Type = ClaimType.Work,
                            ExpiresAt = now.AddHours(Math.Max(1, customer.ClaimTtlHours)),
                            IsActive = true,
                            CompanyId = companyId
                        };
                        await _unitOfWork.CustomerClaimRepository.AddAsync(claim);
                        await _unitOfWork.SaveChangesAsync();

                        // Đồng bộ trạng thái “Claimed” cho funnel (tùy policy bạn giữ)
                        customerEntity.LeadStatus = LeadStatus.Claimed;
                        await _unitOfWork.SaveChangesAsync();
                    }

                    await _unitOfWork.CommitTransactionAsync();

                    return OperationResult<AddCustomerResultDto>.Ok(
                        customer.AssignNow ? "Tạo khách hàng & giao quản lý thành công."
                                  : $"Tạo khách hàng tìm năng thành công và đã cho {customer.ClaimTtlHours} giờ."
                    );
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return OperationResult<AddCustomerResultDto>.Fail($"Lỗi khi tạo khách hàng: {ex.Message}");
                }
            }
      
        /// <summary>
        /// Thêm Note cho Customer.
        /// - AuthorGroupId lấy từ CustomerAssignment hiện tại của người viết.
        /// - Visibility mặc định GROUP; yêu cầu: chỉ cùng Group mới xem được.
        /// </summary>
        public async Task<OperationResult<Guid>> AddCustomerNoteAsync(Guid customerId, string content, CancellationToken ct = default)
        {
            try
            {
                var companyId = _CurrentUser.CompanyId;
                var employeeId = _CurrentUser.EmployeeId;

                var GroupId = await _unitOfWork.MemberInGroupRepository.Query()
                    .Where(g => g.Profile == employeeId && g.IsActive == true)
                    .Select(g => (Guid)g.GroupId)
                    .FirstOrDefaultAsync(ct);

                if (GroupId == Guid.Empty)
                    return OperationResult<Guid>.Fail("Không xác định được Group hiện tại của người viết cho khách hàng này.");

                var note = new CustomerNote
                {
                    Id = Guid.CreateVersion7(),
                    CustomerId = customerId,
                    AuthorEmployeeId = employeeId,
                    AuthorGroupId = GroupId,
                    Content = content,
                    Visibility = NoteVisibility.Group, // đảm bảo mặc định private
                    IsApprovedShare = false,
                    CreatedAt = DateTime.Now,
                    CompanyId = companyId
                };

                await _unitOfWork.CustomerNoteRepository.AddAsync(note, ct);

                return OperationResult<Guid>.Ok(note.Id, "Đã thêm ghi chú.");
            }
            catch (Exception ex)
            {
                return OperationResult<Guid>.Fail($"Không thể thêm ghi chú: {ex.Message}");
            }
        }

        // ======================================================================== Patch ========================================================================

        private async Task<OperationResult<Guid>> UpsertCustomerNoteInline(Guid customerId, PatchCustomerNote reqNote, CancellationToken ct = default)
        {
            if (reqNote == null || string.IsNullOrWhiteSpace(reqNote.Content))
                return OperationResult<Guid>.Ok(Guid.Empty, "Bỏ qua note trống.");

            var companyId = _CurrentUser.CompanyId;
            var employeeId = _CurrentUser.EmployeeId;

            // ===== CASE 1: UPDATE theo NoteId =====
            if (reqNote.NoteId.HasValue && reqNote.NoteId.Value != Guid.Empty)
            {
                var note = await _unitOfWork.CustomerNoteRepository.Query(track: true)
                    .FirstOrDefaultAsync(n =>
                            n.Id == reqNote.NoteId.Value
                         && n.CustomerId == customerId
                         && n.CompanyId == companyId, ct);

                if (note == null)
                    return OperationResult<Guid>.Fail("Không tìm thấy ghi chú để sửa.");

                if (note.AuthorEmployeeId != employeeId)
                    return OperationResult<Guid>.Fail("Bạn chỉ có thể sửa ghi chú của chính mình.");

                // Không đổi group/visibility khi sửa
                note.Content = reqNote.Content.Trim();
                note.IsApprovedShare = false; // nếu có workflow duyệt

                return OperationResult<Guid>.Ok(note.Id, "Đã cập nhật ghi chú.");
            }

            // ===== CASE 2: KHÔNG có NoteId → SỬA NOTE CŨ CỦA CHÍNH TÁC GIẢ (thay vì tạo mới) =====
            // (Bản 'today-only'): chỉ gộp vào note cùng ngày của chính tác giả để tránh "đè" note cũ quan trọng
            var today = DateTime.Today;
            var lastOwnNoteToday = await _unitOfWork.CustomerNoteRepository.Query(track: true)
                .Where(n => n.CustomerId == customerId
                         && n.CompanyId == companyId
                         && n.AuthorEmployeeId == employeeId
                         && n.CreatedAt >= today)                 // cùng ngày
                .OrderByDescending(n => n.CreatedAt)
                .FirstOrDefaultAsync(ct);

            if (lastOwnNoteToday != null)
            {
                // Chỉ cập nhật nội dung; KHÔNG đổi group/visibility
                lastOwnNoteToday.Content = reqNote.Content.Trim();
                lastOwnNoteToday.IsApprovedShare = false;

                return OperationResult<Guid>.Ok(lastOwnNoteToday.Id, "Đã cập nhật ghi chú trong ngày.");
            }

            // Không có note của chính tác giả trong ngày → fallback: tạo mới
            var addRes = await AddCustomerNoteAsync(customerId, reqNote.Content.Trim(), ct);
            return addRes;
        }

        /// <summary>
        /// Xóa mềm dữ liệu khách hàng này
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteCustomerByIdAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _unitOfWork.CustomerRepository.DeleteCustomerByIdAsync(id);
                var affected = await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return affected > 0
                    ? OperationResult.Ok("Xóa khách hàng thành công")
                    : OperationResult.Fail("Không tìm thấy khách hàng hoặc xóa không thành công.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi xóa khách hàng: {ex.Message}");

            }
        }

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateCustomerAsync(PatchCustomer req, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;
                var userId = _CurrentUser.EmployeeId;
                var companyId = _CurrentUser.CompanyId;

                // 1) Load customer + child collections (track = true)
                var existing = await _unitOfWork.CustomerRepository.Query(track: true)
                    .Include(c => c.Addresses)
                    .Include(c => c.Contacts)
                    .FirstOrDefaultAsync(c => c.CustomerId == req.CustomerId && c.CompanyId == companyId, ct);

                if (existing == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return OperationResult.Fail($"Không tìm thấy khách hàng với ID {req.CustomerId}");
                }

                // 2) Patch basic fields
                existing.UpdatedDate = now;
                existing.UpdatedBy = userId;

                PatchHelper.SetIfRef(req.CustomerName, () => existing.CustomerName, v => existing.CustomerName = v);
                PatchHelper.SetIfRef(req.Phone, () => existing.Phone, v => existing.Phone = v);
                PatchHelper.SetIfRef(req.Website, () => existing.Website, v => existing.Website = v);
                PatchHelper.SetIfRef(req.CustomerGroup, () => existing.CustomerGroup, v => existing.CustomerGroup = v);
                PatchHelper.SetIfRef(req.ApplicationName, () => existing.ApplicationName, v => existing.ApplicationName = v);
                PatchHelper.SetIfRef(req.RegistrationNumber, () => existing.RegistrationNumber, v => existing.RegistrationNumber = v);
                PatchHelper.SetIfRef(req.RegistrationAddress, () => existing.RegistrationAddress, v => existing.RegistrationAddress = v);
                PatchHelper.SetIfRef(req.TaxNumber, () => existing.TaxNumber, v => existing.TaxNumber = v);
                PatchHelper.SetIfNullable(req.IssueDate, () => existing.IssueDate, v => existing.IssueDate = v);
                PatchHelper.SetIfRef(req.IssuedPlace, () => existing.IssuedPlace, v => existing.IssuedPlace = v);
                PatchHelper.SetIfRef(req.FaxNumber, () => existing.FaxNumber, v => existing.FaxNumber = v);
                PatchHelper.SetIfNullable(req.IsActive, () => existing.IsActive, v => existing.IsActive = v);

                PatchHelper.SetIf(req.IsLead, () => existing.IsLead, v => existing.IsLead = v);
                if (req.LeadStatus.HasValue)
                    existing.LeadStatus = req.LeadStatus.Value;

                var addAddresses = CustomerAggregateSync.SyncAddresses(
                    existing,
                    req.Addresses,
                    treatMissingAsSoftDelete: false); // nếu FE gửi full list thì bật true

                var addContacts = CustomerAggregateSync.SyncContacts(
                    existing,
                    req.Contacts,
                    treatMissingAsSoftDelete: false);

                if (addAddresses.Count > 0) await _unitOfWork.CustomerRepository.AddAddressAsync(addAddresses, ct);
                if (addContacts.Count > 0) await _unitOfWork.CustomerRepository.AddContactAsync(addContacts, ct);


                // note
                if (req.PatchCustomerNote != null && !string.IsNullOrWhiteSpace(req.PatchCustomerNote.Content))
                {
                    var noteRes = await UpsertCustomerNoteInline(existing.CustomerId, req.PatchCustomerNote);
                    if (!noteRes.Success)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return OperationResult.Fail(noteRes.Message);
                    }
                }

                // 4) Persist
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return OperationResult.Ok("Cập nhật khách hàng thành công");
            }
            catch (InvalidOperationException ex) // stale check
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail("Dữ liệu khách hàng đã bị thay đổi bởi người khác. Vui lòng tải lại và thử lại.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi cập nhật khách hàng: {ex.Message}");
            }
        }


        private void UpsertAddressesAsync(Customer existing, IEnumerable<PatchAddress> incoming, CancellationToken ct)
        {
            if (incoming == null) return;

            var now = DateTime.Now;
            var userId = _CurrentUser.EmployeeId;

            var existingList = existing.Addresses?.ToList() ?? new List<Address>();
            var incomingIds = incoming
                .Select(a => a.AddressId)
                .Where(id => id != Guid.Empty)
                .ToHashSet();


            // 2) THÊM MỚI / CẬP NHẬT / REVIVE
            foreach (var inc in incoming)
            {
                if (inc.AddressId == Guid.Empty)
                {
                    // THÊM MỚI
                    var add = new Address
                    {
                        AddressId = Guid.CreateVersion7(),
                        CustomerId = existing.CustomerId,
                        IsActive = true,
                    };

                    PatchHelper.SetIfRef(inc.AddressLine, () => add.AddressLine, v => add.AddressLine = v);
                    PatchHelper.SetIfRef(inc.City, () => add.City, v => add.City = v);
                    PatchHelper.SetIfRef(inc.District, () => add.District, v => add.District = v);
                    PatchHelper.SetIfRef(inc.Province, () => add.Province, v => add.Province = v);
                    PatchHelper.SetIfRef(inc.Country, () => add.Country, v => add.Country = v);
                    PatchHelper.SetIfRef(inc.PostalCode, () => add.PostalCode, v => add.PostalCode = v);
                    PatchHelper.SetIfNullable(inc.IsPrimary, () => add.IsPrimary, v => add.IsPrimary = v);
                    PatchHelper.SetIf(inc.IsActive, () => add.IsActive, v => add.IsActive = v);

                    // Ensure Addresses collection is not null before adding
                    if (existing.Addresses == null)
                    {
                        existing.Addresses = new List<Address>();
                    }
                    existing.Addresses.Add(add);
                }
                else
                {
                    var target = existingList.FirstOrDefault(a => a.AddressId == inc.AddressId);

                    if (target == null)
                    {
                        // Có ID hợp lệ nhưng không thuộc customer này (hoặc chưa track) → bỏ qua hoặc log cảnh báo theo policy
                        // Hoặc an toàn hơn: tạo mới theo ID gửi lên (nếu bạn cho phép “re-attach”)
                        continue;
                    }

                    // CẬP NHẬT (CHO PHÉP SỬA) + REVIVE nếu trước đó bị soft delete
                    target.IsActive = true;

                    PatchHelper.SetIfRef(inc.AddressLine, () => target.AddressLine, v => target.AddressLine = v);
                    PatchHelper.SetIfRef(inc.City, () => target.City, v => target.City = v);
                    PatchHelper.SetIfRef(inc.District, () => target.District, v => target.District = v);
                    PatchHelper.SetIfRef(inc.Province, () => target.Province, v => target.Province = v);
                    PatchHelper.SetIfRef(inc.Country, () => target.Country, v => target.Country = v);
                    PatchHelper.SetIfRef(inc.PostalCode, () => target.PostalCode, v => target.PostalCode = v);
                    PatchHelper.SetIfNullable(inc.IsPrimary, () => target.IsPrimary, v => target.IsPrimary = v);
                }
            }

            // 3) Đảm bảo chỉ 1 địa chỉ Primary đang ACTIVE
            var activePrimaries = existing.Addresses?.Where(a => a.IsActive == true && a.IsPrimary == true).ToList() ?? new List<Address>();
            if (activePrimaries.Count > 1)
            {
                foreach (var a in activePrimaries.Skip(1))
                    a.IsPrimary = false;
            }
        }

        private void UpsertContactsAsync(Customer existing, IEnumerable<PatchContact> incoming, CancellationToken ct)
        {
            if (incoming == null) return;

            var now = DateTime.Now;
            var userId = _CurrentUser.EmployeeId;

            // tránh null ref
            var contacts = existing.Contacts ??= new List<Contact>();
            var existingList = contacts.ToList();
            var incomingIds = incoming
                .Select(a => a.ContactId)
                .Where(id => id != Guid.Empty)
                .ToHashSet();

            // 2) THÊM MỚI / CẬP NHẬT (CHO SỬA) / REVIVE
            foreach (var inc in incoming)
            {
                if (inc.ContactId == Guid.Empty)
                {
                    // THÊM MỚI
                    var contact = new Contact
                    {
                        ContactId = Guid.CreateVersion7(),
                        CustomerId = existing.CustomerId,
                        IsActive = true,         // active ngay
                    };

                    PatchHelper.SetIfRef(inc.FirstName, () => contact.FirstName, v => contact.FirstName = v);
                    PatchHelper.SetIfRef(inc.LastName, () => contact.LastName, v => contact.LastName = v);
                    PatchHelper.SetIfRef(inc.Email, () => contact.Email, v => contact.Email = v);
                    PatchHelper.SetIfRef(inc.Phone, () => contact.Phone, v => contact.Phone = v);
                    PatchHelper.SetIfRef(inc.Gender, () => contact.Gender, v => contact.Gender = v); // string?
                    PatchHelper.SetIfNullable(inc.IsPrimary, () => contact.IsPrimary, v => contact.IsPrimary = v);

                    contacts.Add(contact);
                }
                else
                {
                    var target = existingList.FirstOrDefault(c => c.ContactId == inc.ContactId);
                    if (target == null)
                    {
                        // ID lạ (không thuộc customer này) → bỏ qua hoặc log theo policy
                        continue;
                    }

                    // REVIVE nếu từng soft-delete + CHO SỬA
                    target.IsActive = true;

                    PatchHelper.SetIfRef(inc.FirstName, () => target.FirstName, v => target.FirstName = v);
                    PatchHelper.SetIfRef(inc.LastName, () => target.LastName, v => target.LastName = v);
                    PatchHelper.SetIfRef(inc.Email, () => target.Email, v => target.Email = v);
                    PatchHelper.SetIfRef(inc.Phone, () => target.Phone, v => target.Phone = v);
                    PatchHelper.SetIfRef(inc.Gender, () => target.Gender, v => target.Gender = v); // string?
                    PatchHelper.SetIfNullable(inc.IsPrimary, () => target.IsPrimary, v => target.IsPrimary = v);
                }
            }

            // 3) Đảm bảo chỉ 1 contact Primary đang ACTIVE
            var activePrimaries = contacts.Where(c => c.IsActive == true && c.IsPrimary == true).ToList();
            if (activePrimaries.Count > 1)
            {
                foreach (var c in activePrimaries.Skip(1))
                    c.IsPrimary = false;
            }
        }

        // ======================================================================== Helper ========================================================================

        private static string NormalizeTaxCode(string? tax)
        {
            if (string.IsNullOrWhiteSpace(tax)) return string.Empty;
            // chuẩn hoá để so sánh/uniqueness: bỏ khoảng trắng, gạch, viết hoa
            return new string(tax.Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();
        }
    }
}


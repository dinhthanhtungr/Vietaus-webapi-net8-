using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Infrastructure.Utilities;
using VietausWebAPI.WebAPI.DatabaseContext;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace VietausWebAPI.Infrastructure.Repositories
{
    public class RequestMaterialRepository : IRequestMaterialRepository
    {
        private readonly ApplicationDbContext  _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public RequestMaterialRepository (ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Thêm chi tiết đề xuất mua vật tư
        /// </summary>
        /// <param name="requestDetail"></param>
        /// <returns></returns>
        public async Task AddRequestDetailMaterialAsync(List<RequestDetailMaterialDatum> requestDetail)
        {
            await _context.RequestDetailMaterialData.AddRangeAsync(requestDetail);
        }
        /// <summary>
        /// Tạo một đề xuất mua vật tư với đầy đủ các thông số
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SupplyRequestsMaterialDatum> CreateRequestAsync(SupplyRequestsMaterialDatum request)
        {
            await _context.SupplyRequestsMaterialData.AddRangeAsync(request);
            return request;
        }
        /// <summary>
        /// Lấy ra mã đề xuất cuối cùng
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetLastRequestIdRepository()
        {
            // Lấy ngày hiện tại
            var today = DateTime.UtcNow;
            string datePart = today.ToString("ddMMyy"); // Ví dụ: "020225" cho 02/02/2025

            // Lấy bản ghi có RequestId lớn nhất cho ngày hiện tại
            var latestRequest = await _context.SupplyRequestsMaterialData
                .Where(x => x.RequestId.Contains(datePart))
                .OrderByDescending(x => x.RequestDate)
                .FirstOrDefaultAsync();

            int nextSequence;
            if (latestRequest == null)
            {
                // Nếu không có bản ghi nào cho ngày hôm nay, bắt đầu từ 01
                nextSequence = 1;
            }
            else
            {
                // Lấy số thứ tự từ RequestId lớn nhất (ví dụ: "020225_02" -> 2)
                string sequencePart = latestRequest.RequestId.Split('_').Last();
                if (int.TryParse(sequencePart, out int currentSequence))
                {
                    nextSequence = currentSequence + 1;
                }
                else
                {
                    throw new Exception("Invalid RequestId format.");
                }
            }

            // Tạo RequestId mới
            string newRequestId = $"{datePart}_{nextSequence:D2}"; // Ví dụ: "020225_03"

            // (Tùy chọn) Lưu bản ghi mới vào bảng nếu cần
            // var newRecord = new SupplyRequestMaterialData { RequestId = newRequestId, ... };
            // _context.SupplyRequestsMaterialData.Add(newRecord);
            // await _context.SaveChangesAsync();
            return newRequestId;
            //return await _context.SupplyRequestsMaterialData.OrderByDescending(x => x.RequestId).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Lấy ra danh sách đề xuất mua vật tư theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedResult<SupplyRequestsMaterialDatum>> GetRequestRepository(RequestMaterialQuery query)
        {
            var queryable = _context.SupplyRequestsMaterialData
                .AsNoTracking()
                .Include(r => r.RequestDetailMaterialData).ThenInclude(r => r.MaterialGroup)

                .Include(r => r.Employee)
                .Include(r => r.Employee.Part)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.KeyWord))
            {
                string keyword = query.KeyWord.ToLower();

                queryable = queryable.Where(x =>
                    (x.RequestId != null && x.RequestId.ToLower().Contains(keyword)) ||
                    (x.RequestStatus != null && x.RequestStatus.ToLower().Contains(keyword)) ||
                    (x.Employee != null && EF.Functions.Collate(x.Employee.FullName, "Latin1_General_CI_AI").ToLower().Contains(keyword)) ||
                    (x.EmployeeId != null && x.EmployeeId.ToLower().Contains(keyword)) ||

                    x.RequestDetailMaterialData.Any(y =>
                        (y.MaterialName != null && EF.Functions.Collate(y.MaterialName, "Latin1_General_CI_AI").ToLower().Contains(keyword)) ||
                        (y.MaterialGroupId != null && y.MaterialGroupId.ToLower().Contains(keyword)) ||
                        (y.MaterialGroup != null && y.MaterialGroup.MaterialGroupName.ToLower().Contains(keyword))
                    )
                );
            }

            if (query.PartId != null && query.PartId.Any())
            {
                queryable = queryable.Where(x => query.PartId.Contains(x.Employee.PartId));
            }

            if (query.StatusFilter != null && query.StatusFilter.Any())
            {
                queryable = queryable.Where(x => query.StatusFilter.Contains(x.RequestStatus));
            }
            // Lọc theo khoản thời gian
            if (query.RequestDateFrom != null && query.RequestDateTo != null)
            {
                queryable = queryable.Where(x => x.RequestDate >= query.RequestDateFrom && x.RequestDate <= query.RequestDateTo);
            }

            else if (query.RequestDateFrom.HasValue) 
            {
                queryable = queryable.Where(x => x.RequestDate.Date >= query.RequestDateFrom.Value);
            }
           
            else if (query.RequestDateTo.HasValue)
            {
                queryable = queryable.Where(x => x.RequestDate.Date <= query.RequestDateTo.Value);
            }
            else if (query.RequestDate != null)
            {
                queryable = queryable.Where(x => x.RequestDate == query.RequestDate);
            }
            else if (query.requestStatus != null)
            {
                queryable = queryable.Where(x => x.RequestStatus == query.requestStatus);
            }

            // Sắp xếp
            switch (query.SortBy)
            {
                case "RequestId":
                    queryable = query.SortAscending
                        ? queryable.OrderBy(x => x.RequestId) 
                        : queryable.OrderByDescending(x => x.RequestId);
                    break;
                case "RequestDate":
                    queryable = query.SortAscending
                        ? queryable.OrderBy(x => x.RequestDate)
                        : queryable.OrderByDescending(x => x.RequestDate);
                    break;
                case "RequestStatus":
                    queryable = query.SortAscending
                        ? queryable.OrderBy(x => x.RequestStatus)
                        : queryable.OrderByDescending(x => x.RequestStatus);
                    break;
                default:
                    queryable = queryable.OrderBy(x => x.RequestDate);
                    break;
            }

            return await QueryableExtensions.GetPagedAsync(queryable, query);
        }
        /// <summary>
        /// Rollback transaction
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task RollbackAsync(IDbContextTransaction transaction)
        {
            await transaction.RollbackAsync();
        }

        public async Task<PagedResult<FlatRequestMaterialDto>> FlatRequestMaterialRepository(RequestMaterialQuery query)
        {
            var queryable = _context.SupplyRequestsMaterialData
                .AsNoTracking()
                .Include(r => r.RequestDetailMaterialData)
                .Include(r => r.Employee)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.KeyWord))
            {
                string keyword = query.KeyWord.ToLower();

                queryable = queryable.Where(x =>
                    (x.RequestId != null && x.RequestId.ToLower().Contains(keyword)) ||
                    (x.RequestStatus != null && x.RequestStatus.ToLower().Contains(keyword)) ||
                    (x.Employee != null && EF.Functions.Collate(x.Employee.FullName, "Latin1_General_CI_AI").ToLower().Contains(keyword)) ||
                    (x.EmployeeId != null && x.EmployeeId.ToLower().Contains(keyword)) ||

                    x.RequestDetailMaterialData.Any(y =>
                        (y.MaterialName != null && EF.Functions.Collate(y.MaterialName, "Latin1_General_CI_AI").ToLower().Contains(keyword)) ||
                        (y.MaterialGroupId != null && y.MaterialGroupId.ToLower().Contains(keyword)) ||
                        (y.MaterialGroup != null && EF.Functions.Collate(y.MaterialGroup.MaterialGroupName, "Latin1_General_CI_AI").ToLower().Contains(keyword))
                    )
                );
            }

            // Lọc theo khoản thời gian
            if (query.RequestDateFrom != null && query.RequestDateTo != null)
            {
                queryable = queryable.Where(x => x.RequestDate >= query.RequestDateFrom && x.RequestDate <= query.RequestDateTo);
            }
            if (query.PartId != null)
            {
                queryable = queryable.Where(x => x.Employee.PartId == query.PartId);
            }

            else if (query.RequestDateFrom.HasValue)
            {
                queryable = queryable.Where(x => x.RequestDate.Date >= query.RequestDateFrom.Value);
            }

            else if (query.RequestDateTo.HasValue)
            {
                queryable = queryable.Where(x => x.RequestDate.Date <= query.RequestDateTo.Value);
            }
            else if (query.RequestDate != null)
            {
                queryable = queryable.Where(x => x.RequestDate == query.RequestDate);
            }


                var flatQuery = queryable
                    .SelectMany(request =>
                        request.RequestDetailMaterialData
                            .Where(detail =>
                                string.IsNullOrWhiteSpace(query.KeyWord) ||
                                (detail.MaterialName != null && EF.Functions.Collate(detail.MaterialName, "Latin1_General_CI_AI").ToLower().Contains(query.KeyWord.ToLower())) ||
                                (detail.MaterialGroupId != null && detail.MaterialGroupId.ToLower().Contains(query.KeyWord.ToLower())) ||
                                (detail.MaterialGroup != null && EF.Functions.Collate(detail.MaterialGroup.MaterialGroupName, "Latin1_General_CI_AI").ToLower().Contains(query.KeyWord.ToLower())) ||
                                (detail.Request.Employee != null && EF.Functions.Collate(detail.Request.Employee.FullName, "Latin1_General_CI_AI").ToLower().Contains(query.KeyWord.ToLower())) ||
                                (detail.Request.EmployeeId != null && detail.Request.EmployeeId.ToLower().Contains(query.KeyWord.ToLower())) ||
                                (detail.RequestId != null && detail.RequestId.ToLower().Contains(query.KeyWord.ToLower()))

                                ),
                                (request, detail) => new FlatRequestMaterialDto
                                {
                                    RequestId = request.RequestId,
                                    RequestDate = request.RequestDate,
                                    RequestStatus = request.RequestStatus,
                                    EmployeeId = request.EmployeeId,
                                    EmployeeName = request.Employee.FullName,
                                    MaterialGroupName = detail.MaterialGroup.MaterialGroupName,
                                    MaterialName = detail.MaterialName,
                                    RequestQuantity = detail.RequestedQuantity,
                                    Unit = detail.Unit
                                });

            return await QueryableExtensions.GetPagedAsync(flatQuery, query);
        }
    }
}

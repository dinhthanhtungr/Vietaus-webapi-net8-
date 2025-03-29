using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.WebAPI.DatabaseContext;

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
        public async Task<SupplyRequestsMaterialDatum> GetLastRequestIdRepository()
        {
            return await _context.SupplyRequestsMaterialData.OrderByDescending(x => x.RequestId).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Lấy ra danh sách đề xuất mua vật tư theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SupplyRequestsMaterialDatum>> GetRequestRepository(RequestMaterialQuery query)
        {
            var queryable = _context.SupplyRequestsMaterialData
                .AsNoTracking()
                .Include(r => r.RequestDetailMaterialData)
                .Include(r => r.Employee)
                .AsQueryable();
            // Lọc theo RequestId
            if (!string.IsNullOrEmpty(query.RequestId))
            {
                queryable = queryable.Where(x => x.RequestId == query.RequestId);
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

            // Lọc theo EmployeeId
            if (!string.IsNullOrEmpty(query.EmployeeId))
            {
                queryable = queryable.Where(x => x.EmployeeId == query.EmployeeId);
            }

            // Lọc theo RequestStatus
            if (!string.IsNullOrEmpty(query.RequestStatus))
            {
                queryable = queryable.Where(x => x.RequestStatus == query.RequestStatus);
            }

            // Lọc theo MaterialName
            if (!string.IsNullOrEmpty(query.MaterialName))
            {
                queryable = queryable.Where(x => x.RequestDetailMaterialData.Any(y => y.MaterialName.Contains(query.MaterialName)));
            }

            // Lọc theo MaterialGroupID
            if (!string.IsNullOrEmpty(query.MaterialGroupID))
            {
                queryable = queryable.Where(x => x.RequestDetailMaterialData.Any(y => y.MaterialGroupId.Contains(query.MaterialGroupID)));
            }

            // Lọc theo Department
            if (!string.IsNullOrEmpty(query.Department))
            {
                queryable = queryable.Where(x => x.Employee.PartId == query.Department);
            }

            // Lọc theo EmployeeName
            if (!string.IsNullOrEmpty(query.EmployeeName))
            {
                queryable = queryable.Where(x => x.Employee.FullName.Contains(query.EmployeeName));
            }

            // Sắp xếp
            switch (query.SortBy?.ToLower())
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

            // Tính tổng số trang trước khi nhận trang
            int totalItems = await queryable.CountAsync();
            
            int pageNumber = Math.Max(1, query.PageNumber);
            int pageSize = Math.Max(1, query.PageSize);
            queryable = queryable
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await queryable.ToListAsync();
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
    }
}

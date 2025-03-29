using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories
{
    public class SupplyRequestsMaterialDatumRepository : ISupplyRequestsMaterialDatumRepository
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public SupplyRequestsMaterialDatumRepository (ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Thêm đề xuất mới
        /// </summary>
        /// <param name="supplyRequestsMaterialData"></param>
        /// <returns></returns>
        public async Task AddSupplyRequestsMaterialDatumRepository(List<SupplyRequestsMaterialDatum> supplyRequestsMaterialData)
        {
            await _context.SupplyRequestsMaterialData.AddRangeAsync(supplyRequestsMaterialData);
        }

        /// <summary>
        /// Lấy đề xuất theo id
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public async Task<SupplyRequestsMaterialDatum> GetWithId(string requestId)
        {
            return await _context.SupplyRequestsMaterialData.FirstOrDefaultAsync(x => x.RequestId == requestId);         
        }

        /// <summary>
        /// Lấy tất cả đề xuất
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SupplyRequestsMaterialDatum>> GetAllSupplyRequestsMaterialDatumRepository()
        {
            return await _context.SupplyRequestsMaterialData.ToListAsync();
        }
        /// <summary>
        /// Cập nhật trạng thái đề xuất
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="requestStatus"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdateRequestStatusAsyncRepository(string requestId, string requestStatus)
        {
            var request = await _context.SupplyRequestsMaterialData
                .FirstOrDefaultAsync(x => x.RequestId == requestId);

            if (request == null)
            {
                throw new Exception($"Request with ID {requestId} not found");
            }
            else
            {
                request.RequestStatus = requestStatus;
            }
        }

    }
}

using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.PLPUReports;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.ReportFeatures.PLPUReports
{
    public class FinishPLPUReportRepository : IFinishPLPUReportRepository
    {
        private readonly ApplicationDbContext _context;

        public FinishPLPUReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FinishRow>> GetFinishReportAsync(DateTime from, DateTime to, CancellationToken ct)
        {
            var fromDate = from;
            var toDate = to.AddDays(1);

            var rawData =
                from dod in _context.DeliveryOrderDetails.AsNoTracking()
                join d in _context.DeliveryOrders.AsNoTracking()
                    on dod.DeliveryOrderId equals d.Id
                join mod in _context.MerchandiseOrderDetails.AsNoTracking()
                    on dod.MerchandiseOrderDetailId equals mod.MerchandiseOrderDetailId
                join mo in _context.MerchandiseOrders.AsNoTracking()
                    on mod.MerchandiseOrderId equals mo.MerchandiseOrderId
                join mop in _context.MfgOrderPOs.AsNoTracking().Where(x => x.IsActive)
                    on mod.MerchandiseOrderDetailId equals mop.MerchandiseOrderDetailId into mopJoin
                from mop in mopJoin.DefaultIfEmpty()
                join mfg in _context.MfgProductionOrders.AsNoTracking()
                    on mop.MfgProductionOrderId equals mfg.MfgProductionOrderId into mfgJoin
                from mfg in mfgJoin.DefaultIfEmpty()
                where dod.IsActive
                      && d.IsActive
                      && mod.IsActive
                      && mo.IsActive
                      && d.CreatedDate >= fromDate
                      && d.CreatedDate < toDate
                select new
                {
                    DeliveryOrderCode = d.ExternalId,
                    MerchandiseOrderCode = mo.ExternalId,
                    MfgOrderCode = mfg != null ? mfg.ExternalId : null,

                    CustomerName = mo.CustomerNameSnapshot,

                    ProductCode = !string.IsNullOrWhiteSpace(dod.ProductExternalIdSnapShot)
                        ? dod.ProductExternalIdSnapShot
                        : mod.ProductExternalIdSnapshot,

                    ProductName = !string.IsNullOrWhiteSpace(dod.ProductNameSnapShot)
                        ? dod.ProductNameSnapShot
                        : mod.ProductNameSnapshot,

                    BatchNo = dod.LotNoList,

                    OrderReceivedDate = mfg != null ? mfg.CreatedDate : (DateTime?)null,
                    DeliveryRequestDate = mod.DeliveryRequestDate,
                    ActualDeliveryDate = d.CreatedDate,

                    OrderedQuantity = mod.ExpectedQuantity,
                    DeliveredQuantity = dod.Quantity,

                    Address = !string.IsNullOrWhiteSpace(d.DeliveryAddress)
                        ? d.DeliveryAddress
                        : mo.DeliveryAddress,

                    Note = !string.IsNullOrWhiteSpace(d.Note)
                        ? d.Note
                        : mod.Comment
                };

            var data = await rawData.ToListAsync(ct);

            var rows = data
                .Select((x, index) => new FinishRow
                {
                    Stt = index + 1,
                    DeliveryOrderCode = x.DeliveryOrderCode ?? string.Empty,
                    MerchandiseOrderCode = x.MerchandiseOrderCode ?? string.Empty,
                    MfgOrderCode = x.MfgOrderCode ?? string.Empty,
                    CustomerName = x.CustomerName ?? string.Empty,
                    ProductCode = x.ProductCode ?? string.Empty,
                    ProductName = x.ProductName ?? string.Empty,
                    BatchNo = x.BatchNo ?? string.Empty,
                    OrderReceivedDate = x.OrderReceivedDate,
                    DeliveryRequestDate = x.DeliveryRequestDate,
                    ActualDeliveryDate = x.ActualDeliveryDate,
                    LateDays = x.ActualDeliveryDate.HasValue
                        ? (x.ActualDeliveryDate.Value.Date - x.DeliveryRequestDate.Date).Days
                        : null,
                    OrderedQuantity = x.OrderedQuantity,
                    DeliveredQuantity = x.DeliveredQuantity,
                    Address = x.Address ?? string.Empty,
                    Note = x.Note ?? string.Empty
                })
                .OrderBy(x => x.ActualDeliveryDate)
                .ThenBy(x => x.CustomerName)
                .ThenBy(x => x.MerchandiseOrderCode)
                .ThenBy(x => x.ProductCode)
                .ToList();

            return rows;
        }
    }
}
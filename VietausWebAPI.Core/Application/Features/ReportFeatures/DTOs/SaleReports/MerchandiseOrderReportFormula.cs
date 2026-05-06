using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports
{
    public static class MerchandiseOrderReportFormula
    {
        /// <summary>
        /// Tính tỉ lệ giao hàng theo số lượng.
        /// Tỉ lệ giao hàng = Đã giao / Số lượng đặt * 100
        /// Formula: DeliveredQuantity / OrderedQuantity * 100
        /// </summary>
        public static decimal CalculateFulfillmentRate(decimal deliveredQuantity, decimal orderedQuantity)
        {
            return orderedQuantity > 0m
                ? Math.Round(deliveredQuantity / orderedQuantity * 100m, 2)
                : 0m;
        }

        /// <summary>
        /// Tính tỉ lệ thực bán theo giá trị tiền.
        /// Tỉ lệ thực bán = Doanh thu thực bán / Tổng giá trị đơn đặt * 100
        /// Formula: ActualSoldAmount / TotalOrderAmount * 100
        /// </summary>
        public static decimal CalculateActualSoldRate(decimal actualSoldAmount, decimal totalOrderAmount)
        {
            return totalOrderAmount > 0m
                ? Math.Round(actualSoldAmount / totalOrderAmount * 100m, 2)
                : 0m;
        }

        /// <summary>
        /// Tính số lượng còn lại chưa giao.
        /// Formula: OrderedQuantity - DeliveredQuantity
        /// </summary>
        public static decimal CalculateRemainingQuantity(decimal orderedQuantity, decimal deliveredQuantity)
        {
            return orderedQuantity - deliveredQuantity;
        }

        /// <summary>
        /// Tính giá trị còn lại chưa giao / chưa bán.
        /// Formula: TotalOrderAmount - ActualSoldAmount
        /// </summary>
        public static decimal CalculateRemainingAmount(decimal totalOrderAmount, decimal actualSoldAmount)
        {
            return totalOrderAmount - actualSoldAmount;
        }

        /// <summary>
        /// Tính doanh thu thực bán.
        /// Formula: DeliveredQuantity * UnitPriceAgreed
        /// </summary>
        public static decimal CalculateActualSoldAmount(decimal deliveredQuantity, decimal unitPriceAgreed)
        {
            return deliveredQuantity * unitPriceAgreed;
        }

        /// <summary>
        /// Tính số tiền chưa thanh toán theo logic hiện tại của report.
        /// Formula:
        /// - IsPaid = true  => 0
        /// - IsPaid = false => ActualSoldAmount
        /// </summary>
        public static decimal CalculateUnpaidAmount(bool isPaid, decimal actualSoldAmount)
        {
            return isPaid ? 0m : actualSoldAmount;
        }
    }
}

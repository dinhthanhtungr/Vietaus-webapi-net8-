using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

namespace VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature.ColorChipRecordFeatures
{
    public interface IColorChipRecordWriteRepositories
    {
        /// <summary>
        /// Tạo mới một bản ghi ColorChipRecord trong hệ thống. Phương thức này sẽ nhận vào một đối tượng ColorChipRecord chứa thông tin chi tiết của bản ghi cần tạo, sau đó lưu trữ nó vào cơ sở dữ liệu và trả về đối tượng ColorChipRecord đã được tạo thành công, bao gồm cả ID và các trường liên quan khác. Nếu có lỗi xảy ra trong quá trình tạo, phương thức sẽ ném ra một ngoại lệ thích hợp để thông báo về vấn đề đã gặp phải.
        /// </summary>
        /// <param name="entity">Đối tượng ColorChipRecord cần tạo.</param>
        /// <param name="cancellationToken">Token hủy bỏ tác vụ.</param>
        /// <returns>Đối tượng ColorChipRecord đã được tạo thành công.</returns>
        Task<ColorChipRecord> CreateAsync(ColorChipRecord entity, CancellationToken cancellationToken = default);
    }
}

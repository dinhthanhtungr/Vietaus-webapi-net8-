using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Shared.Helper.FileStorage
{
    /// <summary>
    /// Lưu Thư mục “hậu trường”
    /// trả về storageKey (chuỗi để ghi DB)
    /// Tải Qua API của bạn: controller mở file (OpenReadAsync) rồi return File(...)
    /// Phù hợp Tài liệu riêng tư/có phân quyền (hợp đồng, chứng từ)
    /// Bảo mặt Không public URL ⇒ an toàn hơn
    /// phụ thuộc IOptions<StorageOptions>
    /// </summary>
    public interface IFileShareStorage
    {
        // Lưu file, trả về đường dẫn TƯƠNG ĐỐI để ghi DB (vd: "orders/{orderId}/Contract/xxxx.pdf")
        Task<string> SaveAsync(Stream stream, string contentType, string fileName,
                               string relativeFolder, CancellationToken ct = default);

        // Đọc file (dùng cho API download)
        Task<(Stream Stream, string ContentType, string FileName)> OpenReadAsync(string relativePath,
                               CancellationToken ct = default);

        // (tuỳ chọn) Xoá file
        Task DeleteAsync(string relativePath, CancellationToken ct = default);
    }
}

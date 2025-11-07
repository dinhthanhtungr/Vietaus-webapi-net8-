//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace VietausWebAPI.Core.Application.Shared.Helper.ImageStorage
//{
//    /// <summary>
//    /// Lưu wwwroot/... của website
//    /// trả về publicUrl (đường link cho browser) + storageKey (đường dẫn nội bộ để xoá)
//    /// Tải bằng Trực tiếp bằng URL tĩnh: <img src="/uploads/...">
//    /// Phù hợp cho Ảnh/icon/banner công khai (hình sản phẩm)
//    /// Bảo mặt mặt định Có URL public ⇒ ai có link là xem
//    /// phụ thuộc IWebHostEnvironment (lấy wwwroot)
//    /// </summary>
//    public interface IFileStorage
//    {
//        // Trả về (publicUrl, storageKey). storageKey là đường dẫn nội bộ/xoá file dựa vào đây
//        Task<(string publicUrl, string storageKey)> SaveAsync(Stream stream, string contentType, string fileName, string folder);
//        Task DeleteAsync(string storageKey);
//    }
//}

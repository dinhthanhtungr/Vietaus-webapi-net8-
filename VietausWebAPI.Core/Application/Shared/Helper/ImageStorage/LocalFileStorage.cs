using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Shared.Helper.ImageStorage
{
    /// <summary>
    /// Lưu wwwroot/... của website
    /// trả về publicUrl (đường link cho browser) + storageKey (đường dẫn nội bộ để xoá)
    /// Tải bằng Trực tiếp bằng URL tĩnh: <img src="/uploads/...">
    /// Phù hợp cho Ảnh/icon/banner công khai (hình sản phẩm)
    /// Bảo mặt mặt định Có URL public ⇒ ai có link là xem
    /// phụ thuộc IWebHostEnvironment (lấy wwwroot)
    /// </summary>
    public class LocalFileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _http; // nếu muốn build absolute URL

        public LocalFileStorage(IWebHostEnvironment env, IHttpContextAccessor http)
        {
            _env = env;
            _http = http;
        }

        public async Task<(string publicUrl, string storageKey)> SaveAsync(Stream stream, string contentType, string fileName, string folder)
        {
            var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var physDir = Path.Combine(webRoot, folder.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(physDir);

            var fullPath = Path.Combine(physDir, fileName);
            using (var fs = File.Create(fullPath))
                await stream.CopyToAsync(fs);

            // storageKey dùng để xoá: folder/fileName
            var storageKey = $"{folder.TrimEnd('/')}/{fileName}";

            // publicUrl tương đối cho UseStaticFiles
            var publicUrl = $"/{storageKey}";

            return (publicUrl, storageKey);
        }

        public Task DeleteAsync(string storageKey)
        {
            var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var fullPath = Path.Combine(webRoot, storageKey.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(fullPath)) File.Delete(fullPath);
            return Task.CompletedTask;
        }
    }
}

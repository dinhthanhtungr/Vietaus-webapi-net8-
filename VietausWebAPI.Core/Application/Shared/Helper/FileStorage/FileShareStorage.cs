using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Shared.Helper.FileStorage
{
    public class FileShareStorage : IFileShareStorage
    {
        private readonly StorageOptions _opt;

        public FileShareStorage(IOptions<StorageOptions> opt) => _opt = opt.Value;

        public async Task<string> SaveAsync(Stream stream, string contentType, string fileName,
                                            string relativeFolder, CancellationToken ct = default)
        {
            // relativeFolder ví dụ: $"orders/{orderId}/Contract"
            var folder = Path.Combine(_opt.RootPath, relativeFolder);
            Directory.CreateDirectory(folder);

            var safe = Path.GetFileName(fileName); // chống path traversal
            var key = $"{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}_{safe}";
            var full = Path.Combine(folder, key);

            await using (var fs = new FileStream(full, FileMode.CreateNew, FileAccess.Write, FileShare.Read, 81920, true))
            {
                await stream.CopyToAsync(fs, ct);
            }

            // Trả đường dẫn tương đối (không “dính” RootPath) để lưu trong DB
            return Path.Combine(relativeFolder, key).Replace('\\', '/');
        }

        public Task<(Stream Stream, string ContentType, string FileName)> OpenReadAsync(string relativePath, CancellationToken ct = default)
        {
            var full = Path.Combine(_opt.RootPath, relativePath);
            var fs = (Stream)new FileStream(full, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, true);
            var name = Path.GetFileName(full);
            var ctType = GetContentType(name);
            return Task.FromResult((fs, ctType, name));
        }

        public Task DeleteAsync(string relativePath, CancellationToken ct = default)
        {
            var full = Path.Combine(_opt.RootPath, relativePath);
            if (File.Exists(full)) File.Delete(full);
            return Task.CompletedTask;
        }

        private static string GetContentType(string name) => Path.GetExtension(name).ToLowerInvariant() switch
        {
            ".pdf" => "application/pdf",
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }
}

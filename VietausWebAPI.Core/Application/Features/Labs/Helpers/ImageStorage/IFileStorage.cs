using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.Helpers.ImageStorage
{
    public interface IFileStorage
    {
        // Trả về (publicUrl, storageKey). storageKey là đường dẫn nội bộ/xoá file dựa vào đây
        Task<(string publicUrl, string storageKey)> SaveAsync(Stream stream, string contentType, string fileName, string folder);
        Task DeleteAsync(string storageKey);
    }
}

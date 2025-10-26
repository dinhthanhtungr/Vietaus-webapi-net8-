using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Shared.Helper.FileStorage
{
    public class StorageOptions
    {
        public string RootPath { get; set; } = null!;
        public string? PublicBaseUrl { get; set; }  // nếu bạn muốn tạo URL public (không bắt buộc)
    }
}

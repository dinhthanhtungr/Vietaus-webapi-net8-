using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Shared.Helper
{
    public class ExternalIdGenerator
    {
        public static async Task<string> GenerateExternalId(
            string prefix,
            Func<string, Task<string?>> getLatestIdFunc)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var basePrefix = $"{prefix}{year}{month}";

            string? lastId = await getLatestIdFunc(basePrefix);

            int nextIdNumber = 1;
            if (!string.IsNullOrWhiteSpace(lastId) && lastId.Length >= basePrefix.Length + 5)
            {
                var lastNumberPart = lastId.Substring(basePrefix.Length);
                if (int.TryParse(lastNumberPart, out int lastNumber))
                {
                    nextIdNumber = lastNumber + 1;
                }
            }
            
            string nextId = $"{basePrefix}{nextIdNumber:D5}"; // Ensure 5 digits for the number part
            return nextId;
        }

        public static async Task<string> GenerateCode(
            string prefix,
            Func<string, Task<string?>> getLatestCodeFunc)
        {
            // Tạo basePrefix: ví dụ "CUS_"
            var basePrefix = $"{prefix}_";

            // Lấy mã cuối cùng từ DB theo tiền tố
            string? lastCode = await getLatestCodeFunc(basePrefix);

            int nextNumber = 1;

            if (!string.IsNullOrWhiteSpace(lastCode) && lastCode.StartsWith(basePrefix))
            {
                // Tách phần số ra
                string numberPart = lastCode.Substring(basePrefix.Length);

                if (int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"{basePrefix}{nextNumber}";
        }

    }
}

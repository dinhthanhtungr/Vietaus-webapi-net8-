using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Shared.Helper.IdCounter
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


        public static async Task<string> GenerateCode(
            string prefix,                                // "NVL"
            string? group,                                // "BB"
            Func<string, Task<string?>> getLatestCodeFunc,
            int width = 3)                                // số chữ số đệm
        {
            var basePrefix = string.IsNullOrWhiteSpace(group)
                ? $"{prefix}_"
                : $"{prefix}_{group}_";                   // "NVL_BB_"

            var lastCode = await getLatestCodeFunc(basePrefix);

            int next = 1;
            if (!string.IsNullOrWhiteSpace(lastCode) && lastCode.StartsWith(basePrefix))
            {
                var numberPart = lastCode.Substring(basePrefix.Length);
                if (int.TryParse(numberPart, out var last)) next = last + 1;
            }

            return $"{basePrefix}{next.ToString().PadLeft(width, '0')}";
        }


        public static async Task<string> GenerateFormulaCode(
            string prefix,
            Func<string, Task<string?>> getLatestCodeFunc)
        {
            var basePrefix = prefix ?? string.Empty;

            // Lấy mã cuối cùng theo tiền tố (ví dụ: "F" → "F009")
            string? lastCode = await getLatestCodeFunc(basePrefix);

            int nextNumber = 1;

            if (!string.IsNullOrWhiteSpace(lastCode))
            {
                // Chỉ chấp nhận dạng <prefix><digits>, ví dụ: F009, CUS_012
                var m = Regex.Match(lastCode, $"^{Regex.Escape(basePrefix)}(\\d+)$");
                if (m.Success && int.TryParse(m.Groups[1].Value, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            // Zero-pad theo padWidth: D3 → 001, 012, 123
            return $"{basePrefix}{nextNumber.ToString($"D{3}")}";
        }

        public static async Task<string> GenerateCodeFromTemplateAsync(
                                            string template,
                                            IQueryable<string> existingCodes,
                                            Func<IQueryable<string>, string, string, CancellationToken, Task<string?>> getLatestCodeFunc,
                                            int padWidth = 0,
                                            CancellationToken ct = default)
        {
            var (left, right) = SplitTemplate(template);
            var maxTail = await getLatestCodeFunc(existingCodes, left, right, ct); // giờ mới đúng chữ ký

            var next = string.IsNullOrEmpty(maxTail) ? 1 : int.Parse(maxTail) + 1;
            var numberPart = padWidth > 0 ? next.ToString($"D{padWidth}") : next.ToString();
            return $"{left}{numberPart}{right}";
        }


        private static (string Left, string Right) SplitTemplate(string template)
        {
            if (string.IsNullOrWhiteSpace(template)) throw new ArgumentException("Template rỗng");
            var i = template.IndexOf('_');
            if (i < 0) throw new ArgumentException("Template phải chứa '_' làm chỗ số");
            return (template[..i], template[(i + 1)..]); // ("LL71", "C")
        }
    }
}

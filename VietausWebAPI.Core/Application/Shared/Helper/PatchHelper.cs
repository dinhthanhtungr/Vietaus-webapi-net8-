using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Shared.Helper
{
    public static class PatchHelper
    {
        // cho kiểu tham chiếu (string, object…)
        public static bool SetIfRef<T>(T? incoming, Func<T?> current, Action<T?> apply)
            where T : class
        {
            var cur = current();
            var changed =
                incoming != null && // chỉ apply khi có giá trị mới
                !EqualityComparer<T?>.Default.Equals(incoming, cur);

            if (changed) apply(incoming);
            return changed;
        }

        // cho kiểu value (int, decimal, bool...) với input nullable (int?, bool?…)
        // HỖ TRỢ cả việc set về null nếu bạn dùng phiên bản Nullable bên dưới.
        public static bool SetIf<T>(T? incoming, Func<T> current, Action<T> apply)
            where T : struct, IEquatable<T>
        {
            if (!incoming.HasValue) return false;
            var cur = current();
            var changed = !incoming.Value.Equals(cur);
            if (changed) apply(incoming.Value);
            return changed;
        }

        // Nếu bạn cần hỗ trợ so sánh và set được cả null (ví dụ CheckBy từ int? -> null)
        public static bool SetIfNullable<T>(T? incoming, Func<T?> current, Action<T?> apply)
            where T : struct, IEquatable<T>
        {
            var cur = current();
            var changed = !Nullable.Equals(incoming, cur);
            if (changed) apply(incoming);
            return changed;
        }


    }
}

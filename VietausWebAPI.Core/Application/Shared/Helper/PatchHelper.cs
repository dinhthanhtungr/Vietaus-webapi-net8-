using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Shared.Helper
{
    public static class PatchHelper
    {
        // Reference type: null = bỏ qua (KHÔNG patch)
        public static bool SetIfRef<T>(T? incoming, Func<T?> current, Action<T?> apply)
            where T : class
        {
            if (incoming is null) return false;

            var cur = current();
            var changed = !EqualityComparer<T?>.Default.Equals(incoming, cur);
            if (changed) apply(incoming);
            return changed;
        }

        // Value type (non-nullable entity): null = bỏ qua
        public static bool SetIf<T>(
            T? incoming,
            Func<T> current,
            Action<T> apply,
            Func<T, bool>? isValid = null
        ) where T : struct, IEquatable<T>
        {
            if (!incoming.HasValue) return false;
            if (isValid != null && !isValid(incoming.Value)) return false;

            var cur = current();
            var changed = !incoming.Value.Equals(cur);
            if (changed) apply(incoming.Value);
            return changed;
        }

        public static bool SetIfGuid(Guid? incoming, Func<Guid> current, Action<Guid> apply, bool ignoreEmpty = true)
            => SetIf(incoming, current, apply, g => !ignoreEmpty || g != Guid.Empty);

        // Nullable value type entity: null = patch (set null)
        public static bool SetIfNullable<T>(
            T? incoming,
            Func<T?> current,
            Action<T?> apply
        ) where T : struct
        {
            var cur = current();
            var changed = !Nullable.Equals(incoming, cur);
            if (changed) apply(incoming); // incoming có thể null => set null
            return changed;
        }

        // Reference type: null = patch (set null)
        public static bool SetIfRefNullable<T>(
            T? incoming,
            Func<T?> current,
            Action<T?> apply
        ) where T : class
        {
            var cur = current();
            var changed = !EqualityComparer<T?>.Default.Equals(incoming, cur);
            if (changed) apply(incoming); // incoming có thể null => set null
            return changed;
        }

        public static bool SetIfEnum<T>(
            T? incoming,
            Func<T> current,
            Action<T> apply
        ) where T : struct, Enum
        {
            if (!incoming.HasValue) return false;

            var cur = current();
            var changed = !EqualityComparer<T>.Default.Equals(incoming.Value, cur);
            if (changed) apply(incoming.Value);
            return changed;
        }

    }
}

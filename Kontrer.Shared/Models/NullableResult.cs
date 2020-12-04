using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.Models
{
    public struct NullableResult<T>
    {
        public T Value { get; set; }
        public bool WasFound { get; init; }
        public readonly T DefaultValue { get; init; }


        public NullableResult(T value, bool wasFound, T defaultValue = default)
        {
            Value = value;
            WasFound = wasFound;
            DefaultValue = defaultValue;
        }
    }
}

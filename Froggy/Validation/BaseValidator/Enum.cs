using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation.BaseValidator
{
    public enum IntervalValidatorType
    {
        Equal = 1,
        MinimumInclusive = 2,
        MaximumInclusive = 4,
        IntervalInclusive = MinimumInclusive | MaximumInclusive,
        MinimumExclusive = 8,
        MaximumExclusive = 16,
        IntervalExclusive = MinimumExclusive | MaximumExclusive
    }
}

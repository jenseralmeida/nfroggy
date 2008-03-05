using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation.BaseValidator
{
    public enum ComparableValidatorType
    {
        Equal = 0,
        MinimumInclusive = 1,
        MaximumInclusive = 2,
        IntervalInclusive = MinimumInclusive | MaximumInclusive,
        MinimumExclusive = 4,
        MaximumExclusive = 8,
        IntervalExclusive = MinimumExclusive | MaximumExclusive
    }
}

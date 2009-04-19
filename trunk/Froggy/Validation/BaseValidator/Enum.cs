using System;

namespace Froggy.Validation.BaseValidator
{
	[Flags]
	[Serializable]
    public enum IntervalValidatorType
    {
    	None = 0,
        Equal = 1,
        MinimumInclusive = 2,
        MaximumInclusive = 4,
        IntervalInclusive = MinimumInclusive | MaximumInclusive,
        MinimumExclusive = 8,
        MaximumExclusive = 16,
        IntervalExclusive = MinimumExclusive | MaximumExclusive
    }

    /// <summary>
    /// Configure how null values are validated by LenghtValidator
    /// </summary>
    [Serializable]
    public enum NullValueLength
    {
        Zero = 0,
        Ignore = 1
    }
}

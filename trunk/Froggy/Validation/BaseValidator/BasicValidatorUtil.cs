using System;

namespace Froggy.Validation.BaseValidator
{
	[Serializable]
    internal static class BasicValidatorUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static bool ContainsValueInEnum(int value, int source)
        {
            return (source & value) == value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Froggy.Validation.BaseValidator;

namespace Froggy.Validation.BaseValidator
{
    internal static class BasicValidatorUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="contains"></param>
        /// <returns></returns>
        internal static bool ContainsValueInEnum(int value, int source)
        {
            return (source & value) == value;
        }
    }
}

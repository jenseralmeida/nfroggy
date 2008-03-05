using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation.ValidatorUnits
{
    public class ValueValidatorUnit<T> : IValidatorUnit<T>
    {
        public bool Execute(T value)
        {
            T result;
            return ValueValidatorUnit<T>.TryChangeType(value, out result);
        }

        public bool ExecuteWithConvert(object value, out T result)
        {
            return ValueValidatorUnit<T>.TryChangeType(value, out result);
        }

        public static bool TryChangeType(object value, out T result)
        {
            bool isNullValue = (value == null) || Convert.IsDBNull(value);
            if (isNullValue)
            {
                result = default(T);
                return true;
            }
            Type realTypeOfGenericParameter = GetRealTypeOfGenericParameter();
            if (realTypeOfGenericParameter == typeof(char))
            {
                value = value.ToString();
            }
            if (realTypeOfGenericParameter.IsInstanceOfType(value))
            {
                result = (T)value;
                return true;
            }

            try
            {
                result = (T)Convert.ChangeType(value, realTypeOfGenericParameter);
                return true;
            }
            catch (InvalidCastException)
            {
                result = default(T);
                return false;
            }
            catch (ArgumentNullException)
            {
                result = default(T);
                return false;
            }
        }

        #region ChangeType

        private static Type GetRealTypeOfGenericParameter()
        {
            Type type = typeof(T);
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return Nullable.GetUnderlyingType(type);
            }
            else
            {
                return type;
            }
        }

        #endregion ChangeType
    }
}

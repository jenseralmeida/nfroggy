using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation.BaseValidator
{
    public sealed class SystemTypeValidator<T> : IValidatorType<T>
    {
        #region IvalidatorType<T>

        public bool Execute(object value, out T result, out string errorMessageTemplate)
        {
            if (SystemTypeValidator<T>.TryChangeType(value, out result))
            {
                errorMessageTemplate = "";
                return true;
            }
            else
            {
                errorMessageTemplate = "Invalid data type";
                return false;
            }
        }

        #endregion IvalidatorType<T>
        private Type _RealType;
        private bool _IsNullable;

        public Type RealType
        {
            get { return _RealType; }
        }

        public bool IsNullable
        {
            get { return _IsNullable; }
            set { _IsNullable = value; }
        }

        public SystemTypeValidator()
        {
            _RealType = GetRealTypeOfGenericParameter(out _IsNullable);
        }

        public SystemTypeValidator(bool isNullable)
            : this()
        {
            IsNullable = isNullable;
        }

        public static bool TryChangeType(object value, out T result)
        {
            bool isNullValue;
            Type realType = GetRealTypeOfGenericParameter(out isNullValue);
            return PrivateTryChangeType(value, realType, out isNullValue, out result);
        }

        private static bool PrivateTryChangeType(object value, Type realType, out bool isNullValue, out T result)
        {
            isNullValue = (value == null) || Convert.IsDBNull(value);
            if (isNullValue)
            {
                result = default(T);
                return true;
            }
            // For char type convert it for string, so the behaviour for business is more consistent
            if (realType == typeof(char))
            {
                value = value.ToString();
            }
            // If the type is of same type of the value the return it
            if (realType.IsInstanceOfType(value))
            {
                result = (T)value;
                return true;
            }

            try
            {
                result = (T)Convert.ChangeType(value, realType);
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

        #region PrivateTryChangeType

        private static Type GetRealTypeOfGenericParameter(out bool isNullable)
        {
            Type realType = typeof(T);
            isNullable = IsNullableGeneric(realType);
            if (isNullable)
            {
                realType = Nullable.GetUnderlyingType(realType);
            }
            return realType;
        }

        private static bool IsNullableGeneric(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        #endregion ChangeType
    }
}

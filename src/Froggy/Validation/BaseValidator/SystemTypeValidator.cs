using System;
using System.Globalization;

namespace Froggy.Validation.BaseValidator
{
	[Serializable]
    public sealed class SystemTypeValidator<T> : ITypeValidator<T>
    {
        #region ITypeValidator

        public bool Execute(object value, out T result, out string errorMessageTemplate)
        {
            if (TryChangeType(value, IsNullable, out result))
            {
                errorMessageTemplate = "";
                return true;
            }
            errorMessageTemplate = "Invalid data type";
            return false;
        }

        #endregion ITypeValidator
        private readonly Type _RealType;
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

        /// <summary>
        /// Try change type, with the restriction of null value of the type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryChangeType(object value, out T result)
        {
            bool isNullableType;
            Type realType = GetRealTypeOfGenericParameter(out isNullableType);
            return PrivateTryChangeType(value, realType, isNullableType, out result);
        }

        /// <summary>
        /// Try change type, validating if the value is null, with a custom restriction of null value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isNullable"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryChangeType(object value, bool isNullable, out T result)
        {
            bool isNullableType;
            Type realType = GetRealTypeOfGenericParameter(out isNullableType);
            return PrivateTryChangeType(value, realType, isNullable, out result);
        }

        private static bool PrivateTryChangeType(object value, Type realType, bool isNullable, out T result)
        {
            bool isNullValue = (value == null) || Convert.IsDBNull(value);
            if (isNullValue)
            {
                result = default(T);
                return isNullable;
            }
            if ((value is String) && (value.ToString() == ""))
            {
                result = default(T);
                return isNullable;
            }
            // For char type convert it for string, so the behaviour for business is more consistent
            if (value is char)
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
                result = (T)Convert.ChangeType(value, realType, CultureInfo.CurrentCulture);
                return true;
            }
            catch (InvalidCastException)
            {
                result = default(T);
                return false;
            }
            catch (FormatException)
            {
                result = default(T);
                return false;
            }
            catch (OverflowException)
            {
                result = default(T);
                return false;
            }
            catch (ArgumentNullException)
            {
                result = default(T);
                return false;
            }
            catch (ArgumentOutOfRangeException)
            {
                result = default(T);
                return false;
            }
        }

        #region PrivateTryChangeType

        private static Type GetRealTypeOfGenericParameter(out bool isNullable)
        {
            Type realType = typeof(T);
            bool isGeneric;
            isNullable = IsNullValueAllowedForType(realType, out isGeneric);
            if (isNullable && isGeneric)
            {
                realType = Nullable.GetUnderlyingType(realType);
            }
            return realType;
        }

        private static bool IsNullValueAllowedForType(Type type, out bool isGeneric)
        {
            isGeneric = type.IsGenericType;
            if (isGeneric)
            {
                return type.GetGenericTypeDefinition() == typeof(Nullable<>);
            }
            return !type.IsValueType;
        }

        #endregion ChangeType
    }
}

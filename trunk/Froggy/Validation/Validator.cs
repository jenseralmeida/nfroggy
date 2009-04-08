using System;
using System.Collections.Generic;
using Froggy.Validation.BaseValidator;

namespace Froggy.Validation
{
    [Serializable]
    public class Validator<T> : IValidation, IValidatorConfiguration
    {
        #region Constructors

        public Validator()
        {
            _TypeValidator = new SystemTypeValidator<T>();
            _TestValidators = new Dictionary<Type, ITestValidator>();
        }

        public Validator(string errorMessageLabel)
            : this()
        {
            SetUpErrorMessageLabel(errorMessageLabel);
        }

        public Validator(string errorMessageLabel, bool isNullable)
            : this()
		{
            SetUpErrorMessageLabel(errorMessageLabel);
            SetUpNullable(isNullable);
		}

        #endregion Constructors

        #region IValidatorConfiguration

        string _ErrorMessageLabel;
        string _CustomErrorMessage;
        ITypeValidator<T> _TypeValidator;
        readonly Dictionary<Type, ITestValidator> _TestValidators;

        public string ErrorMessageLabel
        {
            get
            {
                return _ErrorMessageLabel;
            }
            set
            {
                _ErrorMessageLabel = value;
            }
        }

        public string CustomErrorMessage
        {
            get
            {
                return _CustomErrorMessage;
            }
            set
            {
                _CustomErrorMessage = value;
            }
        }

        public bool IsNullable
        {
            get
            {
                return _TypeValidator.IsNullable;
            }
            set
            {
                _TypeValidator.IsNullable = value;
            }
        }

        public void AddTestValidator(ITestValidator testValidator)
        {
            Type typeOfTestValidator = testValidator.GetType();
            _TestValidators[typeOfTestValidator] = testValidator;
        }

        #endregion

        public ITypeValidator<T> TypeValidator
        {
            get { return _TypeValidator; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "The validator type proport must not be null");
                }
                _TypeValidator = value;
            }
        }

        #region Basic SetUp

        public Validator<T> SetUpErrorMessageLabel(string errorMessageLabel)
        {
            _ErrorMessageLabel = errorMessageLabel;
            return this;
        }

        public Validator<T> SetUpCustomMessage(string customErrorMessage)
        {
            _CustomErrorMessage = customErrorMessage;
            return this;
        }

        public Validator<T> SetUpNullable(bool isNullable)
        {
            _TypeValidator.IsNullable = isNullable;
            return this;
        }

        public Validator<T> SetUp(ITestValidator testValidator)
        {
            AddTestValidator(testValidator);
            return this;
        }

        public Validator<T> SetUp(ITypeValidator<T> typeValidator)
        {
            TypeValidator = typeValidator;
            return this;
        }

        #endregion Basic SetUp

        #region IValidation

        public void Validate(object value)
        {
            Convert(value);
        }

        public bool IsValid(object value)
        {
            string errorMessage;
            return IsValid(value, out errorMessage);
        }

        public bool IsValid(object value, out string errorMessage)
        {
            T result;
            return TryConvert(value, out result, out errorMessage);
        }

        #endregion IValidation

        #region Conversion

        public T Convert(object value)
        {
            T result;
            string errorMessage;
            if (!TryConvert(value, out result, out errorMessage))
            {
                throw new ValidateException(errorMessage);
            }
            return result;
        }

        public bool TryConvert(object value, out T result)
        {
            string errorMessage;
            return TryConvert(value, out result, out errorMessage);
        }

        public bool TryConvert(object value, out T result, out string errorMessage)
        {
            bool sucess = TypeValidator.Execute(value, out result, out errorMessage);
            if (!sucess)
            {
                return false;
            }
            foreach (ITestValidator testsValidator in _TestValidators.Values)
            {
                if (!testsValidator.Execute(result, out errorMessage))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion Conversion
    }
}
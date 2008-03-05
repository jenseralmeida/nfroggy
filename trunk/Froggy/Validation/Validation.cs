using System;
using System.Collections.Generic;
using System.Text;
using Froggy.Validation.BaseValidator;

namespace Froggy.Validation
{
    public class Validation<T>
    {
        #region Class method

        public static Validation<T> SetUp(string errorMessageLabel)
        {
            return Validation<T>
                .Create()
                .SetUpErrorMessageLabel(errorMessageLabel);
        }

        public static Validation<T> SetUp(string errorMessageLabel, bool isNullable)
        {
            return Validation<T>
                .Create()
                .SetUpErrorMessageLabel(errorMessageLabel)
                .SetUpNullable(isNullable);
        }

        public static Validation<T> Create()
        {
            return new Validation<T>();
        }

        #endregion Class method

        #region Constructors

        private Validation()
        {
            _ValidatorType = new SystemTypeValidator<T>();
            _testValidators = new Dictionary<Type, ITestValidator<T>>();
        }

        #endregion Constructors

        #region Fields

        string _ErrorMessageLabel;
        string _CustomErrorMessage;
        IValidatorType<T> _ValidatorType;
        Dictionary<Type, ITestValidator<T>> _testValidators;

        #endregion Fields

        #region Properties

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

        public IValidatorType<T> TypeValidator
        {
            get { return _ValidatorType; }
            set 
            {
                if (value == null)
                {
                    throw new ArgumentNullException("TypeValidator", "The validator type proport must not be null");
                }
                _ValidatorType = value; 
            }
        }

        #endregion Properties

        #region Basic SetUp

        public Validation<T> SetUpErrorMessageLabel(string errorMessageLabel)
        {
            _ErrorMessageLabel = errorMessageLabel;
            return this;
        }

        public Validation<T> SetUpCustomMessage(string customErrorMessage)
        {
            _CustomErrorMessage = customErrorMessage;
            return this;
        }

        public Validation<T> SetUpNullable(bool isNullable)
        {
            _ValidatorType.IsNullable = isNullable;
            return this;
        }

        public Validation<T> SetUp(ITestValidator<T> testValidator)
        {
            Type validatorType = testValidator.GetType();
            _testValidators[validatorType] = testValidator;
            return this;
        }

        public Validation<T> SetUp(IValidatorType<T> validatorType)
        {
            _ValidatorType = validatorType;
            return this;
        }

        #endregion Basic SetUp

        #region Advanced SetUp

        public Validation<T> SetUpLength(int equal)
        {
            return this.SetUp(new LengthValidator<T>(equal));
        }

        public Validation<T> SetUpLength(int minimum, int maximum)
        {
            return this.SetUp(new LengthValidator<T>(minimum, maximum));
        }

        public Validation<T> SetUpLength(int minimum, int maximum, LengthValidatorType lengthValidatorType)
        {
            return this.SetUp(new LengthValidator<T>(minimum, maximum, lengthValidatorType));
        }

        public Validation<T> SetUpComparable(T equal)
        {
            return this.SetUp(new ComparableValidator<T>(equal));
        }

        public Validation<T> SetUpComparable(T minimum, T maximum)
        {
            return this.SetUp(new ComparableValidator<T>(minimum, maximum));
        }

        public Validation<T> SetUpComparable(T minimum, T maximum, ComparableValidatorType comparableValidatorType)
        {
            return this.SetUp(new ComparableValidator<T>(minimum, maximum, comparableValidatorType));
        }

        #endregion Advanced SetUp

        #region Validation

        public void Validate(object value)
        {
            this.Convert(value);
        }

        public bool IsValid(object value)
        {
            string errorMessage;
            return this.IsValid(value, out errorMessage);
        }

        public bool IsValid(object value, out string errorMessage)
        {
            T result;
            return this.TryConvert(value, out result, out errorMessage);
        }

        public T Convert(object value)
        {
            T result;
            string errorMessage;
            if (!this.TryConvert(value, out result, out errorMessage))
            {
                throw new ValidateException(errorMessage);
            }
            return result;
        }

        public bool TryConvert(object value, out T result)
        {
            string errorMessage;
            return this.TryConvert(value, out result, out errorMessage);
        }

        public bool TryConvert(object value, out T result, out string errorMessage)
        {
            bool sucess = TypeValidator.Execute(value, out result, out errorMessage);
            if (!sucess)
            {
                return false;
            }
            foreach (ITestValidator<T> testsValidator in _testValidators.Values)
            {
                if (!testsValidator.Execute(result, out errorMessage))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion Validation
    }
}

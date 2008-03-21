using System;
using System.Collections.Generic;
using System.Text;
using Froggy.Validation.BaseValidator;

namespace Froggy.Validation
{
    public class Validator<T>
    {
        #region Class method

        public static Validator<T> SetUp(string errorMessageLabel)
        {
            return Validator<T>
                .Create()
                .SetUpErrorMessageLabel(errorMessageLabel);
        }

        public static Validator<T> SetUp(string errorMessageLabel, bool isNullable)
        {
            return Validator<T>
                .Create()
                .SetUpErrorMessageLabel(errorMessageLabel)
                .SetUpNullable(isNullable);
        }

        public static Validator<T> Create()
        {
            return new Validator<T>();
        }

        #endregion Class method

        #region Constructors

        private Validator()
        {
            _TypeValidator = new SystemTypeValidator<T>();
            _TestValidators = new Dictionary<Type, ITestValidator<T>>();
        }

        #endregion Constructors

        #region Fields

        string _ErrorMessageLabel;
        string _CustomErrorMessage;
        ITypeValidator<T> _TypeValidator;
        Dictionary<Type, ITestValidator<T>> _TestValidators;

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

        public ITypeValidator<T> TypeValidator
        {
            get { return _TypeValidator; }
            set 
            {
                if (value == null)
                {
                    throw new ArgumentNullException("TypeValidator", "The validator type proport must not be null");
                }
                _TypeValidator = value; 
            }
        }

        #endregion Properties

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

        public Validator<T> SetUp(ITestValidator<T> testValidator)
        {
            Type typeOfTestValidator = testValidator.GetType();
            _TestValidators[typeOfTestValidator] = testValidator;
            return this;
        }

        public Validator<T> SetUp(ITypeValidator<T> typeValidator)
        {
            _TypeValidator = typeValidator;
            return this;
        }

        #endregion Basic SetUp

        #region Advanced SetUp

        public Validator<T> SetUpLength(int equal)
        {
            return this.SetUp(new LengthValidator<T>(equal));
        }

        public Validator<T> SetUpLength(int minimum, int maximum)
        {
            return this.SetUp(new LengthValidator<T>(minimum, maximum));
        }

        public Validator<T> SetUpLength(int minimum, int maximum, IntervalValidatorType lengthValidatorType)
        {
            return this.SetUp(new LengthValidator<T>(minimum, maximum, lengthValidatorType));
        }

        public Validator<T> SetUpComparable(T equal)
        {
            return this.SetUp(new ComparableValidator<T>(equal));
        }

        public Validator<T> SetUpComparable(T minimum, T maximum)
        {
            return this.SetUp(new ComparableValidator<T>(minimum, maximum));
        }

        public Validator<T> SetUpComparable(T minimum, T maximum, IntervalValidatorType comparableValidatorType)
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
            foreach (ITestValidator<T> testsValidator in _TestValidators.Values)
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

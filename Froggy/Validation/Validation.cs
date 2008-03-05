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

        string _ErrorMessageLabel;
        string _CustomErrorMessage;
        IValidatorType<T> _ValidatorType;

        Dictionary<Type, IValidatorTest<T>> _ValidatorsTest;


        private Validation()
        {
            _ValidatorType = new SystemTypeValidator<T>();
            _ValidatorsTest = new Dictionary<Type, IValidatorTest<T>>();
        }

        #region Validation<T> Members

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

        public Validation<T> SetUp(IValidatorTest<T> validatorTest)
        {
            Type validatorType = validatorTest.GetType();
            _ValidatorsTest[validatorType] = validatorTest;
            return this;
        }

        public Validation<T> SetUp(IValidatorType<T> validatorType)
        {
            _ValidatorType = validatorType;
            return this;
        }

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

        public IValidatorType<T> ValidatorType
        {
            get { return _ValidatorType; }
            set 
            {
                if (value == null)
                {
                    throw new ArgumentNullException("ValidatorType", "The validator type proport must not be null");
                }
                _ValidatorType = value; 
            }
        }

        public void Validate(object value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsValid(object value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsValid(object value, out string errorMessage)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public T Convert(object value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public T Convert(object value, out string errorMessage)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool TryConvert(object value, out T result)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool TryConvert(object value, out T result, out string errorMessage)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

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

        public Validation<T> SetUpInterval(T equal)
        {
            return this.SetUp(new ComparableValidator<T>(equal));
        }

        public Validation<T> SetUpInterval(T minimum, T maximum)
        {
            return this.SetUp(new ComparableValidator<T>(minimum, maximum));
        }

        public Validation<T> SetUpInterval(T minimum, T maximum, ComparableValidatorType comparableValidatorType)
        {
            return this.SetUp(new ComparableValidator<T>(minimum, maximum, comparableValidatorType));
        }

    }
}

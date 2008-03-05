using System;
using System.Collections.Generic;
using System.Text;
using Froggy.Validation.BaseValidator;

namespace Froggy.Validation
{
    public class Validation<T> : IValidation<T>
    {
        string _ErrorMessageLabel;
        string _CustomErrorMessage;
        IValidatorType<T> _ValidatorType;

        Dictionary<Type, IValidatorTest<T>> _ValidatorsTest;


        private Validation()
        {
            _ValidatorType = new SystemTypeValidator<T>();
            _ValidatorsTest = new Dictionary<Type, IValidatorTest<T>>();
        }

        #region IValidation<T> Members

        public IValidation<T> SetUpErrorMessageLabel(string errorMessageLabel)
        {
            _ErrorMessageLabel = errorMessageLabel;
            return this;
        }

        public IValidation<T> SetUpCustomMessage(string customErrorMessage)
        {
            _CustomErrorMessage = customErrorMessage;
            return this;
        }

        public IValidation<T> SetUpNullable(bool isNullable)
        {
            _ValidatorType.IsNullable = isNullable;
            return this;
        }

        public IValidation<T> SetUp(IValidatorTest<T> validatorTest)
        {
            Type validatorType = validatorTest.GetType();
            _ValidatorsTest[validatorType] = validatorTest;
            return this;
        }

        public IValidation<T> SetUp(IValidatorType<T> validatorType)
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


        public IValidation<T> SetUpLength(int equal)
        {
            return this.SetUp(new LengthValidator<T>(equal));
        }

        public IValidation<T> SetUpLength(int minimum, int maximum)
        {
            return this.SetUp(new LengthValidator<T>(minimum, maximum));
        }

        public IValidation<T> SetUpLength(int minimum, int maximum, LengthValidatorType lengthValidatorType)
        {
            return this.SetUp(new LengthValidator<T>(minimum, maximum, lengthValidatorType));
        }

        public IValidation<T> SetUpInterval(T equal)
        {
            return this.SetUp(new ComparableValidator<T>(equal));
        }

        public IValidation<T> SetUpInterval(T minimum, T maximum)
        {
            return this.SetUp(new ComparableValidator<T>(minimum, maximum));
        }

        public IValidation<T> SetUpInterval(T minimum, T maximum, ComparableValidatorType comparableValidatorType)
        {
            return this.SetUp(new ComparableValidator<T>(minimum, maximum, comparableValidatorType));
        }

    }
}

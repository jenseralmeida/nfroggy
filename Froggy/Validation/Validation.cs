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
        TypeValidator<T> _TypeValidator;

        Dictionary<Type, IValidatorTest<T>> _ValidatorsTest;


        private Validation()
        {
            _TypeValidator = new TypeValidator<T>();
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
            _TypeValidator.IsNullable = isNullable;
            return this;
        }

        public IValidation<T> SetUpLength(int equalLength)
        {
            throw new Exception("The method or operation is not implemented.");
            return this;
        }

        public IValidation<T> SetUpLength(int minimumLength, int maximumLength)
        {
            throw new Exception("The method or operation is not implemented.");
            return this;
        }

        public IValidation<T> SetUpInterval(T equal)
        {
            return this.SetUp(new ComparableValidator<T>(equal));
        }

        public IValidation<T> SetUpInterval(T minimum, T maximum)
        {
            return this.SetUp(new ComparableValidator<T>(minimum, maximum));
        }

        public IValidation<T> SetUpInterval(T minimum, T maximum, ComparableValidatorType intervalValidatorType)
        {
            return this.SetUp(new ComparableValidator<T>(minimum, maximum, intervalValidatorType));
        }

        public IValidation<T> SetUp(IValidatorTest<T> validatorTest)
        {
            Type validatorType = validatorTest.GetType();
            _ValidatorsTest[validatorType] = validatorTest;
            return this;
        }

        public string ErrorMessageLabel
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public string CustomMessage
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
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
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Froggy.Validation.Object
{
    public class ObjectValidatorUtil: IValidation
    {
        private static Type _ValidatorType;

        static ObjectValidatorUtil()
        {
            _ValidatorType = typeof(Validator<>);
        }

        Type objType;
        Dictionary<PropertyInfo, IValidation> validationsByPropertyInfo = new Dictionary<PropertyInfo, IValidation>();
        Dictionary<string, IValidation> validationsByName = new Dictionary<string, IValidation>();

        public ObjectValidatorUtil(Type objType)
        {
            // verify nullable type
            Validator<Type>
                .Create("object type")
                .SetUpNullable(false)
                .Validate(objType);

            foreach (var propertyInfo in objType.GetProperties())
            {
                Type validatorType = _ValidatorType.MakeGenericType(propertyInfo.PropertyType);
                IValidation validation = (IValidation)Activator.CreateInstance(validatorType);
                var validatorAttributes = propertyInfo.GetCustomAttributes(typeof(ValidatorAttribute), true);
                switch (validatorAttributes.Length)
                {
                    case 0:
                        AddValidation(propertyInfo, validation);
                        break;
                    case 1:
                        AddValidation(propertyInfo, validation, (ValidatorAttribute)validatorAttributes[0]);
                        break;
                    default:
                        throw new InvalidOperationException("Only one validator is allowed for one property");
                }
            }
            this.objType = objType;
        }

        private void AddValidation(PropertyInfo propertyInfo, IValidation validation, ValidatorAttribute validatorAttrib)
        {
            IValidatorConfiguration config = (IValidatorConfiguration)validation;
            config.ErrorMessageLabel = validatorAttrib.ErrorMessageLabel;
            config.CustomErrorMessage = validatorAttrib.CustomErrorMessage;
            config.IsNullable = validatorAttrib.IsNullable;
            if (validatorAttrib.CustomTestValidators != null)
            {
                Array.ForEach<ITestValidator>(validatorAttrib.CustomTestValidators, t => config.AddTestValidator(t));
            }
            AddValidation(propertyInfo, validation);
        }

        private void AddValidation(PropertyInfo propertyInfo, IValidation validation)
        {
            validationsByPropertyInfo.Add(propertyInfo, validation);
            validationsByName.Add(propertyInfo.Name, validation);
        }

        public bool IsValid(object value, string memberName)
        {
            IValidation validation = validationsByName[memberName];
            if (validation != null)
            {
                return validation.IsValid(value);
            }
            else
            {
                return true;
            }
        }

        #region IValidation Members

        public bool IsValid(object obj)
        {
            foreach (PropertyInfo propertyInfo in validationsByPropertyInfo.Keys)
            {
                bool existsValidator;
                object value = GetValueOfProperty(obj, propertyInfo, out existsValidator);
                if (existsValidator)
                {
                    bool isValueOfPropertyValid = validationsByPropertyInfo[propertyInfo].IsValid(value);
                    if (!isValueOfPropertyValid)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool IsValid(object obj, out string errorMessage)
        {
            errorMessage = "";
            foreach (PropertyInfo propertyInfo in validationsByPropertyInfo.Keys)
            {
                bool existsValidator;
                object value = GetValueOfProperty(obj, propertyInfo, out existsValidator);
                if (existsValidator)
                {
                    string tempErrorMessage;
                    bool isValueOfPropertyValid = validationsByPropertyInfo[propertyInfo].IsValid(value, out tempErrorMessage);
                    if (!isValueOfPropertyValid)
                    {
                        errorMessage += tempErrorMessage + "\n";
                    }
                }
            }
            return String.IsNullOrEmpty(errorMessage);
        }

        public void Validate(object obj)
        {
            foreach (PropertyInfo propertyInfo in validationsByPropertyInfo.Keys)
            {
                bool existsValidator;
                object value = GetValueOfProperty(obj, propertyInfo, out existsValidator);
                if (existsValidator)
                {
                    IValidation validation = validationsByPropertyInfo[propertyInfo];
                    validation.Validate(value);
                }
            }
        }

        #endregion

        private object GetValueOfProperty(object obj, PropertyInfo propertyInfo, out bool existsValidator)
        {
            IValidation validation = validationsByPropertyInfo[(PropertyInfo)propertyInfo];
            existsValidator = validation != null;
            if (!existsValidator)
            {
                return null;
            }
            return propertyInfo.GetValue(obj, null);
        }
    }
}

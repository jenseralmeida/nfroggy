using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Froggy.Validation.Object
{
    public class ObjectValidatorUtil: IValidation
    {
        private static Type _ValidatorBaseGenericType;

        static ObjectValidatorUtil()
        {
            _ValidatorBaseGenericType = typeof(Validator<>);
        }

        Type objType;
        Dictionary<PropertyInfo, IValidation> validationsByPropertyInfo = new Dictionary<PropertyInfo, IValidation>();
        Dictionary<string, IValidation> validationsByName = new Dictionary<string, IValidation>();

        public ObjectValidatorUtil(Type objType)
        {
            // verify if the type is nullable
            Validator<Type>
                .Create("object type")
                .SetUpNullable(false)
                .Validate(objType);

            foreach (var propertyInfo in objType.GetProperties())
            {
                var validatorAttributes = propertyInfo.GetCustomAttributes(typeof(IValidatorAttribute), true);
                if (validatorAttributes.Length == 0)
                {
                    CreateValidator(propertyInfo);
                }
                else
                {
                    IValidatorConfiguration validatorConfiguration = CreateValidator(propertyInfo);
                    foreach (IValidatorAttribute attribute in validatorAttributes)
                    {
                        ConfigValidator(validation, attribute);
                    }
                }
            }
            this.objType = objType;
        }

        private void ConfigValidator(IValidatorConfiguration validatorConfiguration, IValidatorAttribute attribute)
        {
            throw new NotImplementedException();
        }

        private void AddValidation(PropertyInfo propertyInfo, IValidation validation, ValidatorAttribute validatorAttrib)
        {
            CreateValidator(propertyInfo);
            IValidatorConfiguration config = (IValidatorConfiguration)validation;
            config.ErrorMessageLabel = validatorAttrib.ErrorMessageLabel;
            config.CustomErrorMessage = validatorAttrib.CustomErrorMessage;
            config.IsNullable = validatorAttrib.IsNullable;
            if (validatorAttrib.CustomTestValidators != null)
            {
                Array.ForEach<ITestValidator>(validatorAttrib.CustomTestValidators, t => config.AddTestValidator(t));
            }
            CreateValidator(propertyInfo);
        }

        private IValidatorConfiguration CreateValidator(PropertyInfo propertyInfo)
        {
            Type validatorType = _ValidatorBaseGenericType.MakeGenericType(propertyInfo.PropertyType);
            IValidatorConfiguration validatorConfiguration = (IValidatorConfiguration)Activator.CreateInstance(validatorType);
            validationsByPropertyInfo.Add(propertyInfo, (IValidation)validatorConfiguration);
            validationsByName.Add(propertyInfo.Name, (IValidation)validatorConfiguration);
            return validatorConfiguration;
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

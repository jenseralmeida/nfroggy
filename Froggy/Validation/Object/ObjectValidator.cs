using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Froggy.Validation.Object
{
    [Serializable]
    public class ObjectValidator : IValidation
    {
        private static Type _ValidatorBaseGenericType;
        private static readonly Type _MessageValidatorAttributeType;
        private static readonly Type _TypeValidatorAttributeType;
        private static readonly Type _TestValidatorAttributeType;

        static ObjectValidator()
        {
            _ValidatorBaseGenericType = typeof(Validator<>);
            _MessageValidatorAttributeType = typeof(MessageValidatorAttribute);
            _TypeValidatorAttributeType = typeof(TypeValidatorAttribute);
            _TestValidatorAttributeType = typeof(TestValidatorAttribute);
        }

        Type objType;
        Dictionary<PropertyInfo, IValidation> validationsByPropertyInfo = new Dictionary<PropertyInfo, IValidation>();
        Dictionary<string, IValidation> validationsByName = new Dictionary<string, IValidation>();

        public ObjectValidator(Type objType)
        {
            // verify if the type is nullable
            new Validator<Type>("object type")
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
                        ConfigureValidator(validatorConfiguration, attribute);
                    }
                }
            }
            this.objType = objType;
        }

        /// <summary>
        /// Create a new instance of a <see cref="Validator´1"/> extract from the <see cref="PropertyInfo"/>
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private IValidatorConfiguration CreateValidator(PropertyInfo propertyInfo)
        {
            Type validatorType = _ValidatorBaseGenericType.MakeGenericType(propertyInfo.PropertyType);
            IValidatorConfiguration validatorConfiguration = (IValidatorConfiguration)Activator.CreateInstance(validatorType);
            // TODO: Change default name to a more smart rule for extract of the property name
            validatorConfiguration.ErrorMessageLabel = propertyInfo.Name.ToLower();
            validationsByPropertyInfo.Add(propertyInfo, (IValidation)validatorConfiguration);
            validationsByName.Add(propertyInfo.Name, (IValidation)validatorConfiguration);
            return validatorConfiguration;
        }

        private void ConfigureValidator(IValidatorConfiguration validatorConfiguration, IValidatorAttribute attribute)
        {

            Type attributeType = attribute.GetType();
            if (attributeType.IsSubclassOf(_MessageValidatorAttributeType))
            {
                MessageValidatorAttribute message = (MessageValidatorAttribute)attribute;
                validatorConfiguration.ErrorMessageLabel = message.ErrorMessageLabel;
                validatorConfiguration.CustomErrorMessage = message.CustomErrorMessage;
            }
            else if (attributeType.IsSubclassOf(_TypeValidatorAttributeType))
            {
                TypeValidatorAttribute type = (TypeValidatorAttribute)attribute;
                validatorConfiguration.IsNullable = type.IsNullable;
            }
            else if (attributeType.IsSubclassOf(_TestValidatorAttributeType))
            {
                TestValidatorAttribute test = (TestValidatorAttribute)attribute;
                validatorConfiguration.AddTestValidator(test.Create());
            }
            else
            {
                throw new InvalidProgramException("The validator attribute must be of the types: MessageValidatorAttribute, TypeValidatorValidator or TestValidatorAttribute");
            }
        }

        public bool IsPropertyValid(object value, string propertyName)
        {
            IValidation validation = validationsByName[propertyName];
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

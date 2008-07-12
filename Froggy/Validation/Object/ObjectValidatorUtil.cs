using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Froggy.Validation.Object
{
    public class ObjectValidatorUtil: IValidation
    {
        Type objType;
        Dictionary<PropertyInfo, IValidation> validationsByPropertyInfo = new Dictionary<PropertyInfo, IValidation>();
        Dictionary<string, IValidation> validationsByName = new Dictionary<string, IValidation>();

        public ObjectValidatorUtil(Type objType)
        {
            // verify nullable type
            Validator<object>
                .Create("object type")
                .SetUpNullable(false)
                .Validate(objType);

            foreach (var propertyInfo in objType.GetProperties())
            {
                var validatorAttributes = propertyInfo.GetCustomAttributes(typeof(ValidatorAttribute), true);
                foreach (ValidatorAttribute validatorAttrib in validatorAttributes )
                {
                    // Inicialmente apenas um validator por membro é suportado. ver se vale a pena criar estrutura pra ter mais de um validator por membro
                    validationsByPropertyInfo.Add(propertyInfo, validatorAttrib.Validator);
                    validationsByName.Add(propertyInfo.Name, validatorAttrib.Validator);
                }
            }

            this.objType = objType;
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

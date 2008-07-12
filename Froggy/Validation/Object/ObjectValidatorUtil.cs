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
        Dictionary<MemberInfo, IValidation> validationsByMemberInfo = new Dictionary<MemberInfo, IValidation>();
        Dictionary<string, IValidation> validationsByName = new Dictionary<string, IValidation>();

        public ObjectValidatorUtil(Type objType)
        {
            // verify nullable type
            Validator<object>
                .Create("object type")
                .SetUpNullable(false)
                .Validate(objType);

            foreach (var memberInfo in objType.GetMembers())
            {
                var validatorAttributes = memberInfo.GetCustomAttributes(typeof(ValidatorAttribute), true);
                foreach (ValidatorAttribute validatorAttrib in validatorAttributes )
                {
                    // Inicialmente apenas um validator por membro é suportado. ver se vale a pena criar estrutura pra ter mais de um validator por membro
                    validationsByMemberInfo.Add(memberInfo, validatorAttrib.Validator);
                    validationsByName.Add(memberInfo.Name, validatorAttrib.Validator);
                }
            }

            this.objType = objType;
        }

        public bool IsValid(string memberName)
        {
            PropertyInfo memberInfo = validationsByName[memberName];
            return IsMemberInfoValid(memberInfo);
        }

        #region IValidation Members

        public bool IsValid(object obj)
        {
            foreach (PropertyInfo memberInfo in validationsByMemberInfo.Keys)
            {
                bool isMemberInfoValid = IsMemberInfoValid(memberInfo);
                if (!isMemberInfoValid)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsValid(object value, out string errorMessage)
        {
            IValidation validation;
            string errorMessages = "";
            foreach (PropertyInfo memberInfo in validationsByMemberInfo.Keys)
            {
                validation = validationsByMemberInfo[(MemberInfo)memberInfo];
                object value = memberInfo.GetValue(obj, null);
                string tempErrorMessage;
                bool isMemberInfoValid = validation.IsValid(value, out tempErrorMessage);
                if (!isMemberInfoValid)
                {
                    errorMessages += tempErrorMessage + "\n";
                }
            }
            return String.IsNullOrEmpty(errorMessage);
        }

        public void Validate(object value)
        {
            foreach (PropertyInfo memberInfo in validationsByMemberInfo.Keys)
            {
                IValidation validation = validationsByMemberInfo[(MemberInfo)memberInfo];
                object value = memberInfo.GetValue(obj, null);
                validation.Validate(value);
            }
            return true;
        }

        #endregion

        private bool IsMemberInfoValid(PropertyInfo memberInfo)
        {
            IValidation validation = validationsByMemberInfo[(MemberInfo)memberInfo];
            object value = memberInfo.GetValue(obj, null);
            return validation.IsValid(value);
        }
    }
}

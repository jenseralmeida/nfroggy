using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Froggy.Validation.Object
{
    public class ObjectValidatorUtil
    {
        Type objType;
        ArrayList list = new ArrayList();

        public ObjectValidatorUtil(Type objType)
        {
            // verify nullable type
            Validator<object>
                .Create("object type")
                .SetUpNullable(false)
                .Validate(objType);

            foreach (var mi in objType.GetMethods())
            {
                foreach (ValidatorAttribute attrib in mi.GetCustomAttributes(typeof(ValidatorAttribute), true) )
                {
                    list.Add(attrib.Validator);
                }
            }

            this.objType = objType;
        }

        public bool IsValid(object obj)
        {
            // retorna se o obj é valido
        }
    }
}

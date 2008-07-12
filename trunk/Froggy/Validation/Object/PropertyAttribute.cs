using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy.Validation.Object
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyAttribute : ValidatorAttribute
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Froggy.Validation.Object
{
    public interface ITestValidatorAttribute
    {
        ITestValidator Create();
    }
}

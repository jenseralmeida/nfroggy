using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation
{
    [Serializable]
    public class ValidateException: FroggyException
    {
        public ValidateException(string message)
            : base (message)
        {
        }
    }
}

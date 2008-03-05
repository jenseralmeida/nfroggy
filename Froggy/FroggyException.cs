using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy
{
    [Serializable]
    public class FroggyException: Exception
    {
        public FroggyException(string message)
            : base (message)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Froggy.Validation
{
    [Serializable]
    public class ValidateException: FroggyException
    {
		public ValidateException()
			: base ()
		{
		}
		
		protected ValidateException(SerializationInfo info, StreamingContext context)
			: base (info, context)
		{
		}
		
		public ValidateException(string message, Exception exception)
			: base (message, exception)
		{
		}
		
		public ValidateException(string message)
			: base (message)
		{
		}
    }
}

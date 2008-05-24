using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Froggy
{
	[Serializable]
	public class FroggyException: Exception
	{
		public FroggyException()
			: base ()
		{
		}
		
		protected FroggyException(SerializationInfo info, StreamingContext context)
			: base (info, context)
		{
		}
		
		public FroggyException(string message, Exception exception)
			: base (message, exception)
		{
		}
		
		public FroggyException(string message)
			: base (message)
		{
		}
	}
}

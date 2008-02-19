using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation.System
{
    class Int32Validator: ITypeValidator<Int32?>
    {
        #region ITypeValidator<T> Members

        public int? Parse(object value)
        {
            if ( (value == null) || (Convert.IsDBNull(value) ) )
            {
                return null;
            }
            else
            {
                return Int32.Parse(value.ToString());
            }
        }

        #endregion
    }
}

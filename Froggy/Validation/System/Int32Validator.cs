using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation.System
{
    class NullableInt32Validator: ITypeValidator<Int32?>
    {
        #region ITypeValidator<T> Members

        public bool TryParse(object value, out int? result)
        {
            if ( (value == null) || (Convert.IsDBNull(value) ) )
            {
                result = null;
                return true;
            }
            else
            {
                int temporaryResult;
                bool suces = Int32.TryParse(value.ToString(), out temporaryResult);

            }
        }

        #endregion
    }
}

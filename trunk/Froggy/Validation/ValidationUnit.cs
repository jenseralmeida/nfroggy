using System;
using System.Collections.Generic;
using System.Text;

namespace Froggy.Validation
{
    public class ValidationUnit: IValidationUnit
    {
        string _ErrorMessageLabel;
        string _CustomMessage;
        List<IValidator> _Validators;

        #region IValidationUnit Members

        public string ErrorMessageLabel
        {
            get
            {
                return _ErrorMessageLabel; 
            }
            set
            {
                _ErrorMessageLabel = value;
            }
        }

        public string CustomMessage
        {
            get
            {
                return _CustomMessage;
            }
            set
            {
                _CustomMessage = value;
            }
        }

        public List<IValidator> Validators
        {
            get { _Validators; }
        }

        #endregion
    }
}

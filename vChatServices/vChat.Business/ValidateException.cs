using System;
using System.Collections.Generic;

namespace vChat.Business
{
    public class ValidateException : System.Exception
    {
        private static List<String> _errors;

        public List<String> Errors
        {
            get
            {
                if (_errors == null)
                    _errors = new List<String>();

                return _errors;
            }

            set { _errors = value; }
        }

        public ValidateException(String Message) : base(Message)
        {
        }

        public ValidateException(List<String> errors)
        {
            this.Errors.AddRange(errors);
        }
    }
}

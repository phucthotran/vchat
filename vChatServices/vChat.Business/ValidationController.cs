using System;
using System.Collections.Generic;

namespace vChat.Business
{
    public class ValidationController
    {
        private static List<String> _errors;

        private static List<String> Errors
        {
            get
            {
                if (_errors == null)
                    _errors = new List<String>();

                return _errors;
            }
            set { _errors = value; }
        }

        public static void Prepare()
        {
            Errors.Clear();
        }

        public static void NewError(String Error)
        {
            Errors.Add(Error);
        }

        public static void Validate()
        {
            if (Errors.Count > 0)
                throw new ValidateException(Errors);
        }
    }
}

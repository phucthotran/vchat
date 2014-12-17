using System;
using System.Collections.Generic;

namespace vChat.Business.Validations
{
    public class Validation<T> where T : class
    {
        public T Value { get; set; }
        public String ArgName { get; set; }
        public List<String> Errors { get; set; }

        public Validation(T value, String argName)
        {
            this.Value = value;
            this.ArgName = argName;
        }
    }

    public class ValidationWithStruct<S> where S : struct
    {
        public S Value { get; set; }
        public String ArgName { get; set; }

        public ValidationWithStruct(S value, String argName)
        {
            this.Value = value;
            this.ArgName = argName;
        }
    }
}

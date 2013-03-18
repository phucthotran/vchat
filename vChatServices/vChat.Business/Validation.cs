using System;

namespace vChat.Business.Validations
{
    public class Validation<T> where T : class
    {
        public T Value { get; set; }
        public String ArgName { get; set; }

        public Validation(T value, String argName)
        {
            this.Value = value;
            this.ArgName = argName;
        }
    }
}

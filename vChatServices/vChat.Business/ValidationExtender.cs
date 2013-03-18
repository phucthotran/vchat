using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vChat.Business.Validations
{
    public static class ValidationExtender
    {
        private static bool ValidationOn = System.Configuration.ConfigurationManager.AppSettings.Get("ValidationOn").ToUpper() == "ON";

        public static Validation<T> RequiredArgument<T>(this T item, string argName) where T : class
        {            
            return new Validation<T>(item, argName);
        }

        public static Validation<T> NotNull<T>(this Validation<T> item) where T : class
        {
            if (ValidationOn && item.Value == null)
                throw new ArgumentNullException(item.ArgName);

            return item;
        }

        public static Validation<String> ShorterThan(this Validation<String> item, int limit)
        {
            if (ValidationOn && item.Value.Length >= limit)
                throw new ArgumentException(String.Format("Parameter {0} must be shorter than {1} chars", item.ArgName, limit));

            return item;
        }

        public static Validation<String> LongerThan(this Validation<String> item, int limit)
        {
            if (ValidationOn && item.Value.Length <= limit)
                throw new ArgumentException(String.Format("Parameter {0} must be longer than {1} chars", item.ArgName, limit));

            return item;
        }

        public static Validation<String> Between(this Validation<String> item, int from, int to)
        {
            if (ValidationOn && (item.Value.Length < from || item.Value.Length > to))
                throw new ArgumentException(String.Format("Parameter {0} must between {1} and {2} chars", item.ArgName, from, to));

            return item;
        }

        //public static Validation<int> BeginFrom(this Validation<int> item, int begin)
        //{
        //    if (ValidationOn && item.Value < begin)
        //        throw new ArgumentException(String.Format("Parameter {0} must be begin from {1}", item.ArgName, begin));

        //    return item;
        //}
    }
}

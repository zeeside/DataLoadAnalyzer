using System;

namespace DataLoadAnalyzer.Common
{
    public class Check
    {
        public static bool IsNotNullOrEmpty(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException($"Value {value} cannot be null");
            return true;
        }

        public static bool IsNotNull<T>(T value, string errorMessage="")
        {
            errorMessage = string.IsNullOrEmpty(errorMessage) ? $"Argument of type {typeof(T)} cannot be null" : errorMessage;

            if (value == null) throw new ArgumentNullException(errorMessage);

            return true;
        }
    }
}

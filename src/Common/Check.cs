using System;
using System.Collections.Generic;
using System.Linq;

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

        public static bool IsValidFilePathType(string value, string errorMessage = "")
        {
            Check.IsNotNull<string>(value, "FilePathType configuration cannot be null");

            var validTypes = new List<string> { "absolute", "relative" };

            errorMessage = string.IsNullOrEmpty(errorMessage) ? $"FilePathType {value} was not valid" : errorMessage;

            if (!validTypes.Any(v => v.ToLower() == value)) throw new Exception(errorMessage);

            return true;
        }
    }
}

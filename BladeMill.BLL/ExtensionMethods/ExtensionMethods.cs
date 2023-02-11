using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BladeMill.BLL.ExtentionsMethod
{
    public static class ExtensionMethods
    {
        public static IEnumerable<T> DoAction<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection is null)
            {
                return collection;
            }
            foreach (var element in collection)
            {
                action(element);
            }

            return collection;
        }

        public static string ThrowIfNullOrEmpty(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value is empty");
            }

            return value;
        }
        public static string ThrowIfTooLong(this string value, int maxLength)
        {
            if (value is not null && value.Length > maxLength)
            {
                throw new ArgumentException("Value to long");
            }

            return value;
        }
        public static string ThrowIfTooShort(this string value, int minLength)
        {
            if (value is not null && value.Length < minLength)
            {
                throw new ArgumentException("Value to short");
            }

            return value;
        }
        public static string CheckFileIfNotExistThrowException(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException(file);
            return string.Empty;
        }

        public static string CheckDirectoryIfNotExistThrowException(string dir)
        {
            if (!Directory.Exists(dir))
                throw new DirectoryNotFoundException(dir);
            return string.Empty;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

namespace AI_Assistant_Win.Utils
{
    public class HttpHelper
    {
        public static string ToQueryString<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var parameters = new List<string>();

            foreach (var property in properties)
            {
                var value = property.GetValue(obj, null);
                if (value != null)
                {
                    string encodedKey = HttpUtility.UrlEncode(property.Name);
                    string encodedValue = HttpUtility.UrlEncode(value.ToString());
                    parameters.Add($"{encodedKey}={encodedValue}");
                }
            }

            return string.Join("&", parameters);
        }
    }
}

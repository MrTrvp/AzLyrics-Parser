using System;
using System.Collections.Specialized;
using System.Linq;                      

namespace AzLyricParser.Core.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static string ToQueryString(this NameValueCollection collection)
        {
            var concatedValues = 
                (from key in collection.AllKeys
                 from value in collection.GetValues(key)
                    select $"{Escape(key)}={Escape(value)}")
                .ToArray();

            return "?" + string.Join("&", concatedValues);
        }

        private static string Escape(string value)
        {
            return Uri.EscapeDataString(value);
        }
    }
}            
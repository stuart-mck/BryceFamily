using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Extensions
{
    public static class StringExtensions
    {
        private static TextInfo _textInfo = new CultureInfo("en-AU", false).TextInfo;

        public static string ToTitleCase(this string input)
        {
            return _textInfo.ToTitleCase(input.Trim().ToLower());
        }
    }
}

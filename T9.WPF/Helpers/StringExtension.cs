using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace T9.WPF.Helpers
{
    static class StringExtension
    {
        public static string ReplaceAt(this string source, int startIndex, int count, string newString)
        {
            return source
                .Remove(startIndex, count)
                .Insert(startIndex, newString);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OISC_Compiler
{
    public static class StringExtensions
    {
        public static String RemoveMultipleSpaces(this String data)
        {
            return System.Text.RegularExpressions.Regex.Replace(data, @"\s+", " ");
        }
    }
}

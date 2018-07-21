using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.Utility
{
    public static class Encoder
    {

        public static string Encode(string s)
        {
            return s
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }
    }
}

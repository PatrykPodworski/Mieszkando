using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery
{
    public class MlUri : Expression
    {
        public readonly string ValueWithExtension;

        public MlUri(string valueWithExtension)
        {
            ValueWithExtension = valueWithExtension;
        }

        public MlUri(string value, MlUriDocumentType type)
        {
            var extension = type == MlUriDocumentType.Json ? "json" : "xml";
            ValueWithExtension = $"{value}.{extension}";
        }

        public override string Query => $"'/{ValueWithExtension}'";
    }
}

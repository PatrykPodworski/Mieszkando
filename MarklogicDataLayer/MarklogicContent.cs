using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer
{
    public class MarklogicContent
    {
        public static MarklogicContent Xml(string documentName, string content, string[] collections = null) =>
            new MarklogicContent($"/{documentName}{(documentName.EndsWith(".xml") ? string.Empty : ".xml")}", content);

        public string DocumentName { get; }
        public string Content { get; }
        public string[] Collections { get; }
        public string Media { get; }

        public MarklogicContent(string documentName, string content, string[] collections = null)
        {
            DocumentName = documentName;
            Content = content;
            Collections = collections;
            Media = documentName.EndsWith(".xml") ? MediaType.Xml : MediaType.Json;
        }
    }
}

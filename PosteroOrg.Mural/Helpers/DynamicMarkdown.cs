using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MarkdownSharp;

namespace PosteroOrg.Mural
{
    public static class DynamicMarkdown
    {
        public static string MarkdownTransform(this string content)
        {
            return new Markdown().Transform(content);
        }
    }
}
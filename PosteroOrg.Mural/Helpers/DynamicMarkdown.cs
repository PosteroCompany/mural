using MarkdownSharp;

namespace PosteroOrg.Mural
{
    public static class DynamicMarkdown
    {
        private static MarkdownOptions Options = new MarkdownOptions
        {
            AutoHyperlink = true
        };

        public static string MarkdownTransform(this string content)
        {
            return new Markdown(Options).Transform(content);
        }
    }
}
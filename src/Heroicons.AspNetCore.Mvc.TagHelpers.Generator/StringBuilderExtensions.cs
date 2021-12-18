using System;
using System.Text;

namespace Heroicons.AspNetCore.Mvc.TagHelpers.Generator
{
    internal static class StringBuilderExtensions
    {
        public static void AppendIndentedLine(this StringBuilder sb, int indent, string line)
        {
            sb.AppendIndent(indent);
            sb.AppendLine(line);
        }

        public static void AppendIndent(this StringBuilder sb, int indent) => sb.Append(' ', indent * 4);

        /// <remarks>
        /// https://stackoverflow.com/a/14087738/108374
        /// </remarks>
        public static void AppendStringLiteral(this StringBuilder sb, string input)
        {
            sb.Append("\"");
            foreach (var c in input)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append(@"\\");
                        break;
                    case '\0':
                        sb.Append(@"\0");
                        break;
                    case '\a':
                        sb.Append(@"\a");
                        break;
                    case '\b':
                        sb.Append(@"\b");
                        break;
                    case '\f':
                        sb.Append(@"\f");
                        break;
                    case '\n':
                        sb.Append(@"\n");
                        break;
                    case '\r':
                        sb.Append(@"\r");
                        break;
                    case '\t':
                        sb.Append(@"\t");
                        break;
                    case '\v':
                        sb.Append(@"\v");
                        break;
                    default:
                        // ASCII printable character
                        if (c >= 0x20 && c <= 0x7e)
                        {
                            sb.Append(c);
                            // As UTF16 escaped character
                        }
                        else
                        {
                            sb.Append(@"\u");
                            sb.Append(((int)c).ToString("x4"));
                        }

                        break;
                }
            }

            sb.Append("\"");
        }

        public static void AppendCodeBlock(this StringBuilder sb, int indent, string line, Action<StringBuilder, int> inner, bool semicolon = false)
        {
            sb.AppendIndentedLine(indent, line);
            sb.AppendIndentedLine(indent, "{");
            inner(sb, indent + 1);
            sb.AppendIndentedLine(indent, semicolon ? "};" : "}");
        }

        public static void AppendNamespace(this StringBuilder sb, int indent, string @namespace, Action<StringBuilder, int> inner)
            => sb.AppendCodeBlock(indent, $"namespace {@namespace}", inner);

        public static void AppendPublicEnum(this StringBuilder sb, int indent, string typeName, Action<StringBuilder, int> inner)
            => sb.AppendCodeBlock(indent, $"public enum {typeName}", inner);

        public static void AppendInternalStaticClass(this StringBuilder sb, int indent, string typeName, Action<StringBuilder, int> inner)
            => sb.AppendCodeBlock(indent, $"internal static class {typeName}", inner);

        public static void AppendXmlDocSummary(this StringBuilder sb, int indent, string summary)
        {
            sb.AppendIndentedLine(indent, "/// <summary>");
            sb.AppendIndentedLine(indent, $"/// {summary}");
            sb.AppendIndentedLine(indent, "/// </summary>");
        }
    }
}

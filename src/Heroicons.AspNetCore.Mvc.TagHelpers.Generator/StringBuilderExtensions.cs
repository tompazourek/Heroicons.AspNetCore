using System;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;

namespace Heroicons.AspNetCore.Mvc.TagHelpers.Generator;

internal static class StringBuilderExtensions
{
    public static void AppendIndentedLine(this StringBuilder sb, int indent, string line)
    {
        sb.AppendIndent(indent);
        sb.AppendLine(line);
    }

    public static void AppendIndent(this StringBuilder sb, int indent) => sb.Append(' ', indent * 4);

    public static void AppendStringLiteral(this StringBuilder sb, string input)
    {
        using var sw = new StringWriter(sb);
        SyntaxFactory.Literal(input).WriteTo(sw);
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

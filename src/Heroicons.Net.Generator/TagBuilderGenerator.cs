using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Humanizer;
using static System.IO.Path;

namespace Heroicons.Net.Generator
{
    public class TagBuilderGenerator
    {
        public void GenerateMethods(StringBuilder sb, int indent, IReadOnlyList<FileInfo> iconFiles)
        {
            for (var index = 0; index < iconFiles.Count; index++)
            {
                var iconFile = iconFiles[index];
                GenerateMethod(sb, indent, iconFile);
                if (index + 1 < iconFiles.Count)
                {
                    sb.AppendLine();
                }
            }
        }

        public void GenerateHeroiconKindFile(StringBuilder sb, int indent, IReadOnlyList<FileInfo> iconFiles)
        {
            AppendIndentedLine(sb, indent, "namespace Heroicons.Net");
            AppendIndentedLine(sb, indent, "{");
            GenerateHeroiconKindEnum(sb, indent + 1, iconFiles);
            AppendIndentedLine(sb, indent, "}");
        }

        private static void GenerateHeroiconKindEnum(StringBuilder sb, int indent, IReadOnlyList<FileInfo> iconFiles)
        {
            AppendIndentedLine(sb, indent, "public enum HeroiconKind");
            AppendIndentedLine(sb, indent, "{");
            GenerateHeroiconKindEnumItems(sb, indent + 1, iconFiles);
            AppendIndentedLine(sb, indent, "}");
        }

        private static void GenerateHeroiconKindEnumItems(StringBuilder sb, int indent, IReadOnlyList<FileInfo> iconFiles)
        {
            var kinds = iconFiles.Select(GetKindIdentifier).Distinct().OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();
            foreach (var kind in kinds)
            {
                AppendIndentedLine(sb, indent, $"{kind},");
            }
        }

        public void GenerateHeroiconNameFile(StringBuilder sb, int indent, IReadOnlyList<FileInfo> iconFiles)
        {
            AppendIndentedLine(sb, indent, "namespace Heroicons.Net");
            AppendIndentedLine(sb, indent, "{");
            GenerateHeroiconNameEnum(sb, indent + 1, iconFiles);
            AppendIndentedLine(sb, indent, "}");
        }

        private static void GenerateHeroiconNameEnum(StringBuilder sb, int indent, IReadOnlyList<FileInfo> iconFiles)
        {
            AppendIndentedLine(sb, indent, "public enum HeroiconName");
            AppendIndentedLine(sb, indent, "{");
            GenerateHeroiconNameEnumItems(sb, indent + 1, iconFiles);
            AppendIndentedLine(sb, indent, "}");
        }

        private static void GenerateHeroiconNameEnumItems(StringBuilder sb, int indent, IReadOnlyList<FileInfo> iconFiles)
        {
            var kinds = iconFiles.Select(GetNameIdentifier).Distinct().OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();
            foreach (var kind in kinds)
            {
                AppendIndentedLine(sb, indent, $"{kind},");
            }
        }

        public IReadOnlyList<FileInfo> GetIconFiles(DirectoryInfo heroiconsRoot)
            => heroiconsRoot.GetFiles("*.svg", SearchOption.AllDirectories).OrderBy(x => x.FullName, StringComparer.OrdinalIgnoreCase).ToList();

        public void GenerateMethod(StringBuilder sb, int indent, FileInfo iconFile)
        {
            var methodName = GenerateMethodName(iconFile);
            AppendIndentedLine(sb, indent, $"private static Microsoft.AspNetCore.Mvc.Rendering.TagBuilder {methodName}()");
            AppendIndentedLine(sb, indent, "{");
            GenerateMethodContents(sb, indent + 1, iconFile);
            AppendIndentedLine(sb, indent, "}");
        }

        private static string GenerateMethodName(FileInfo iconFile) => $"{GetKindIdentifier(iconFile)}_{GetNameIdentifier(iconFile)}";
        private static string GetKindIdentifier(FileInfo iconFile) => ProcessIdentifier(iconFile.Directory!.Name);
        private static string GetNameIdentifier(FileInfo iconFile) => ProcessIdentifier(GetFileNameWithoutExtension(iconFile.Name));
        private static string ProcessIdentifier(string identifier) => identifier.Replace('-', ' ').Pascalize();

        private static void GenerateMethodContents(StringBuilder sb, int indent, FileInfo iconFile)
        {
            var svgDoc = XDocument.Load(iconFile.FullName);
            var svgNode = svgDoc.Root!;

            AppendIndentedLine(sb, indent, $@"var tagBuilder = new Microsoft.AspNetCore.Mvc.Rendering.TagBuilder(""{svgNode.Name.LocalName}"")");
            AppendIndentedLine(sb, indent, "{");
            GenerateTagBuilderInitializerContents(sb, indent + 1, svgNode);
            AppendIndentedLine(sb, indent, "};");

            AppendIndent(sb, indent);
            sb.Append("tagBuilder.InnerHtml.AppendHtml(");
            GenerateEscapedString(sb, GetSvgNodeInner(svgNode));

            sb.AppendLine(");");

            AppendIndentedLine(sb, indent, "return tagBuilder;");
        }

        private static string GetSvgNodeInner(XContainer svgNode)
        {
            var result = new StringBuilder();
            foreach (var childNode in svgNode.Elements())
            {
                result.Append(childNode.IgnoreNamespace());
            }
            return result.ToString();
        }

        private static void GenerateTagBuilderInitializerContents(StringBuilder sb, int indent, XElement svgNode)
        {
            AppendIndentedLine(sb, indent, "Attributes =");
            AppendIndentedLine(sb, indent, "{");
            GenerateAttributesContents(sb, indent + 1, svgNode);
            AppendIndentedLine(sb, indent, "},");
        }

        private static void GenerateAttributesContents(StringBuilder sb, int indent, XElement svgNode)
        {
            foreach (var attribute in svgNode.Attributes())
            {
                AppendIndent(sb, indent);
                sb.Append("{ ");
                GenerateEscapedString(sb, attribute.Name.LocalName);
                sb.Append(", ");
                GenerateEscapedString(sb, attribute.Value);
                sb.AppendLine(" },");
            }
        }

        /// <remarks>
        /// https://stackoverflow.com/a/14087738/108374
        /// </remarks>
        private static void GenerateEscapedString(StringBuilder sb, string input)
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

        private static void AppendIndentedLine(StringBuilder sb, int indent, string line)
        {
            AppendIndent(sb, indent);
            sb.AppendLine(line);
        }

        private static void AppendIndent(StringBuilder sb, int indent)
        {
            sb.Append(' ', indent * 4);
        }
    }
}

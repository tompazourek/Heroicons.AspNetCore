using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Heroicons.AspNetCore.Mvc.TagHelpers.Generator
{
    [Generator]
    public class HeroiconSourceGenerator : ISourceGenerator
    {
        private const string NamespacePublic = "Heroicons.AspNetCore.Mvc.TagHelpers";
        private const string NamespaceInternal = NamespacePublic + ".Internal";
        private const string NamespaceIcons = NamespaceInternal + ".Icons";

        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var icons = Icon.GetAll();
            var kinds = icons.Select(x => x.Kind).Distinct().OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();
            var names = icons.Select(x => x.Name).Distinct().OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();

            // generate public enums
            GenerateFile(context, "HeroiconKind", sb => GenerateHeroiconKindType(sb, kinds));
            GenerateFile(context, "HeroiconName", sb => GenerateHeroiconNameType(sb, names));

            // generate tag builder for each icon
            foreach (var icon in icons)
            {
                GenerateFile(context, $"{icon.Kind}_{icon.Name}", sb => GenerateSingleTagBuilderFactory(sb, icon));
            }

            // generate tag builder factory by kind and name enums
            GenerateFile(context, "HeroiconTagBuilderFactory", sb => GenerateHeroiconTagBuilderFactoryType(sb, kinds, names));
        }

        private static void GenerateFile(GeneratorExecutionContext context, string fileName, Action<StringBuilder> generator)
        {
            var sb = new StringBuilder();
            generator(sb);
            context.AddSource($"{fileName}.Generated.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        internal static void GenerateHeroiconKindType(StringBuilder sb, IReadOnlyList<string> kinds)
            => sb.AppendNamespace(0, NamespacePublic, (sb2, indent2)
                => sb.AppendPublicEnum(indent2, "HeroiconKind", (sb3, indent3) =>
                {
                    foreach (var kind in kinds)
                    {
                        sb3.AppendIndentedLine(indent3, $"{kind},");
                    }
                }));

        internal static void GenerateHeroiconNameType(StringBuilder sb, IReadOnlyList<string> names)
            => sb.AppendNamespace(0, NamespacePublic, (sb2, indent2)
                => sb.AppendPublicEnum(indent2, "HeroiconName", (sb3, indent3) =>
                {
                    foreach (var name in names)
                    {
                        sb3.AppendIndentedLine(indent3, $"{name},");
                    }
                }));

        internal static void GenerateSingleTagBuilderFactory(StringBuilder sb, Icon icon)
            => sb.AppendNamespace(0, NamespaceIcons, (sb2, indent2)
                => sb.AppendInternalStaticClass(indent2, $"{icon.Kind}_{icon.Name}", (sb3, indent3)
                    => GenerateSingleTagBuilderFactoryMethod(sb3, indent3, icon)));

        private static void GenerateSingleTagBuilderFactoryMethod(StringBuilder sb, int indent, Icon icon)
            => sb.AppendCodeBlock(indent, "public static Microsoft.AspNetCore.Mvc.Rendering.TagBuilder CreateTagBuilder()", (sb2, indent2)
                => GenerateSingleTagBuilderFactoryMethodContents(sb2, indent2, icon));

        private static void GenerateSingleTagBuilderFactoryMethodContents(StringBuilder sb, int indent, Icon icon)
        {
            using var iconStream = icon.GetContentStream();
            var svgNode = XDocument.Load(iconStream).Root!;

            sb.AppendCodeBlock(indent, $@"var tagBuilder = new Microsoft.AspNetCore.Mvc.Rendering.TagBuilder(""{svgNode.Name.LocalName}"")", (sb2, indent2)
                => sb2.AppendCodeBlock(indent2, "Attributes =", (sb3, indent3)
                    =>
                {
                    foreach (var attribute in svgNode.Attributes())
                    {
                        sb3.AppendIndent(indent3);
                        sb3.Append("{ ");
                        sb3.AppendStringLiteral(attribute.Name.LocalName);
                        sb3.Append(", ");
                        sb3.AppendStringLiteral(attribute.Value);
                        sb3.AppendLine(" },");
                    }
                }), semicolon: true);


            var innerHtml = new StringBuilder();
            foreach (var childNode in svgNode.Elements())
            {
                innerHtml.Append(childNode.IgnoreNamespace());
            }

            sb.AppendIndent(indent);
            sb.Append("tagBuilder.InnerHtml.AppendHtml(");
            sb.AppendStringLiteral(innerHtml.ToString());
            sb.AppendLine(");");

            sb.AppendIndentedLine(indent, "return tagBuilder;");
        }

        public static void GenerateHeroiconTagBuilderFactoryType(StringBuilder sb, IReadOnlyList<string> kinds, IReadOnlyList<string> names)
            => sb.AppendNamespace(0, NamespaceInternal, (sb2, indent2)
                => sb.AppendInternalStaticClass(indent2, "HeroiconTagBuilderFactory", (sb3, indent3) =>
                {
                    GenerateSwitchByKindAndNameMethod(sb3, indent3, kinds);
                    sb3.AppendLine();

                    foreach (var kind in kinds)
                    {
                        GenerateSwitchByNameMethod(sb3, indent3, kind, names);
                        sb3.AppendLine();
                    }
                }));

        private static void GenerateSwitchByNameMethod(StringBuilder sb, int indent, string kind, IReadOnlyList<string> names)
            => sb.AppendCodeBlock(indent, $"private static Microsoft.AspNetCore.Mvc.Rendering.TagBuilder CreateTagBuilder_{kind}({NamespacePublic}.HeroiconName name)", (sb2, indent2)
                => sb2.AppendCodeBlock(indent2, "return name switch ", (sb3, indent3)
                    => GenerateSwitchByKindSwitchItems(sb3, indent3, kind, names), semicolon: true));

        private static void GenerateSwitchByKindSwitchItems(StringBuilder sb, int indent, string kind, IReadOnlyList<string> names)
        {
            foreach (var name in names)
            {
                sb.AppendIndentedLine(indent, $"{NamespacePublic}.HeroiconName.{name} => {NamespaceIcons}.{kind}_{name}.CreateTagBuilder(),");
            }

            sb.AppendIndentedLine(indent, "_ => throw new System.ArgumentOutOfRangeException(nameof(name))");
        }

        private static void GenerateSwitchByKindAndNameMethod(StringBuilder sb, int indent, IReadOnlyList<string> kinds)
            => sb.AppendCodeBlock(indent, $"public static Microsoft.AspNetCore.Mvc.Rendering.TagBuilder CreateTagBuilder({NamespacePublic}.HeroiconKind kind, {NamespacePublic}.{"HeroiconName"} name)", (sb2, indent2)
                => sb2.AppendCodeBlock(indent2, "return kind switch ", (sb3, indent3)
                    => GenerateSwitchByKindAndNameSwitchItems(sb3, indent3, kinds), semicolon: true));

        private static void GenerateSwitchByKindAndNameSwitchItems(StringBuilder sb, int indent, IReadOnlyList<string> kinds)
        {
            foreach (var kind in kinds)
            {
                sb.AppendIndentedLine(indent, $"{NamespacePublic}.HeroiconKind.{kind} => CreateTagBuilder_{kind}(name),");
            }

            sb.AppendIndentedLine(indent, "_ => throw new System.ArgumentOutOfRangeException(nameof(kind))");
        }
    }
}

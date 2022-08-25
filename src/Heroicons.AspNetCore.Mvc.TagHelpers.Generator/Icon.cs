using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Humanizer;

namespace Heroicons.AspNetCore.Mvc.TagHelpers.Generator
{
    internal class Icon
    {
        protected Icon(string manifestResourceName)
        {
            if (string.IsNullOrEmpty(manifestResourceName)) throw new ArgumentException("Value cannot be null or empty.", nameof(manifestResourceName));
            ManifestResourceName = manifestResourceName;

            var parts = manifestResourceName.Split('.');

            Kind = GetIdentifier(parts, parts.Length - 4) switch
            {
                "_20" => "Mini",
                "_24" => GetIdentifier(parts, parts.Length - 3),
                _ => throw new InvalidOperationException($"Unknown size name part in '{manifestResourceName}'")
            };

            Name = GetIdentifier(parts, parts.Length - 2);
        }

        private static string GetIdentifier(IReadOnlyList<string> parts, int index) => parts[index].Replace('-', ' ').Pascalize();

        protected string ManifestResourceName { get; }
        public string Kind { get; }
        public string Name { get; }

        private static readonly Assembly AssemblyWithResources = typeof(Icon).Assembly;
        public Stream GetContentStream() => AssemblyWithResources.GetManifestResourceStream(ManifestResourceName);

        public static IReadOnlyList<Icon> GetAll()
            => AssemblyWithResources.GetManifestResourceNames()
                .Where(x => x.EndsWith("svg"))
                .Select(x => new Icon(x))
                .OrderBy(x => x.Kind, StringComparer.OrdinalIgnoreCase)
                .ThenBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();

        public override string ToString() => $"{Kind}/{Name}";
    }
}

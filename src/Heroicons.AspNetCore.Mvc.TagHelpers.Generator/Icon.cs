using System;
using System.IO;
using Humanizer;
using Microsoft.CodeAnalysis.Text;

namespace Heroicons.AspNetCore.Mvc.TagHelpers.Generator
{
    internal class Icon
    {
        public Icon(string path, SourceText sourceText)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (sourceText == null) throw new ArgumentNullException(nameof(sourceText));

            var parts = path.Split('/', '\\');

            Name = GetIdentifier(Path.GetFileNameWithoutExtension(path));
            SourceText = sourceText;
            
            var partSize = parts[parts.Length - 3];
            if (partSize == "20")
            {
                Kind = "Mini";
            }
            else
            {
                var partType = parts[parts.Length - 2];
                Kind = GetIdentifier(partType);
            }
        }

        private static string GetIdentifier(string input) => input.Replace('-', ' ').Pascalize();

        public string Kind { get; }
        public string Name { get; }
        public SourceText SourceText { get; }

        public override string ToString() => $"{Kind}/{Name}";
    }
}

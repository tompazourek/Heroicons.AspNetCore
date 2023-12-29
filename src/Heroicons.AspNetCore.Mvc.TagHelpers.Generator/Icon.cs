using System.IO;
using Humanizer;
using Microsoft.CodeAnalysis.Text;

namespace Heroicons.AspNetCore.Mvc.TagHelpers.Generator;

internal class Icon
{
    public Icon(string path, SourceText sourceText)
    {
        var fileName = Path.GetFileNameWithoutExtension(path);
        Name = GetIdentifier(fileName);
        SourceText = sourceText;

        var parts = path.Split('/', '\\');
        var partSize = parts[parts.Length - 3];

        switch (partSize)
        {
            case "16":
                Kind = "Micro";
                break;
            case "20":
                Kind = "Mini";
                break;
            default:
            {
                var partType = parts[parts.Length - 2];
                Kind = GetIdentifier(partType);
                break;
            }
        }
    }

    private static string GetIdentifier(string input) => input.Replace('-', ' ').Pascalize();

    public string Kind { get; }
    public string Name { get; }
    public SourceText SourceText { get; }

    public bool IsDeprecated => Name is
        "ArrowLeftOnRectangle" or
        "ArrowRightOnRectangle" or
        "ArrowSmallDown" or
        "ArrowSmallLeft" or
        "ArrowSmallRight" or
        "ArrowSmallUp" or
        "MinusSmall" or
        "PlusSmall";

    public override string ToString() => $"{Kind}/{Name}";
}

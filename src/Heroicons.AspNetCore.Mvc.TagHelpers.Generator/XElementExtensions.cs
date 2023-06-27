using System.Linq;
using System.Xml.Linq;

namespace Heroicons.AspNetCore.Mvc.TagHelpers.Generator;

internal static class XElementExtensions
{
    /// <remarks>
    /// https://stackoverflow.com/a/11466336/108374
    /// </remarks>
    public static XElement IgnoreNamespace(this XElement element)
        => new(XNamespace.None + element.Name.LocalName, element.Elements().Select(IgnoreNamespace), element.Attributes());
}

using System.Linq;
using System.Xml.Linq;

namespace Heroicons.Net.Generator
{
    /// <remarks>
    /// https://stackoverflow.com/a/11466336/108374
    /// </remarks>
    public static class XElementExtensions
    {
        public static XElement IgnoreNamespace(this XElement xelem)
        {
            XNamespace xmlns = "";
            var name = xmlns + xelem.Name.LocalName;
            return new XElement(name,
                from e in xelem.Elements()
                select IgnoreNamespace(e),
                xelem.Attributes()
            );
        }
    }
}

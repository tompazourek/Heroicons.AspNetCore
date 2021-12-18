using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Heroicons.AspNetCore.Mvc.TagHelpers;

/// <summary>
/// Heroicon tag helper.
/// </summary>
[HtmlTargetElement("heroicon", TagStructure = TagStructure.WithoutEndTag)]
public class HeroiconTagHelper : TagHelper
{
    /// <summary>
    /// Style kind of the icon.
    /// </summary>
    [HtmlAttributeName("kind")]
    public HeroiconKind Kind { get; set; }

    /// <summary>
    /// Name of the icon.
    /// </summary>
    [HtmlAttributeName("name")]
    public HeroiconName Name { get; set; }

    /// <inheritdoc />
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(output);
        
        TagBuilder tagBuilder = Internal.HeroiconTagBuilderFactory.CreateTagBuilder(Kind, Name);
        output.TagName = tagBuilder.TagName;
        output.TagMode = TagMode.StartTagAndEndTag;
        output.MergeAttributes(tagBuilder);
        output.Content.AppendHtml(tagBuilder.InnerHtml);
    }
}

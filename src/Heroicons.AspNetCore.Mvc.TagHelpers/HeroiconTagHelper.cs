using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Heroicons.AspNetCore.Mvc.TagHelpers;

[HtmlTargetElement("heroicon", TagStructure = TagStructure.WithoutEndTag)]
public class HeroiconTagHelper : TagHelper
{
    [HtmlAttributeName("kind")]
    public HeroiconKind Kind { get; set; }

    [HtmlAttributeName("name")]
    public HeroiconName Name { get; set; }

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

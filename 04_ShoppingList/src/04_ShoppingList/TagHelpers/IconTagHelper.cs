using Microsoft.AspNetCore.Razor.TagHelpers;

namespace _04_ShoppingList.TagHelpers;

/// <summary>
/// A TagHelper to simplify writing FontAwesome icons in Razor Views.
/// Usage: &lt;fa-icon name="cart-shopping" class="btn-icon" /&gt;
/// Generates: &lt;i class="fa-solid fa-cart-shopping btn-icon"&gt;&lt;/i&gt;
/// </summary>
[HtmlTargetElement("fa-icon")]
public class IconTagHelper : TagHelper
{
    /// <summary>
    /// The name of the FontAwesome icon (e.g., "plus", "cart-shopping").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // Change the tag from <fa-icon> to <i>
        output.TagName = "i";
        
        // Remove the inner structure since it's just an icon tag
        output.TagMode = TagMode.StartTagAndEndTag;

        // Base FontAwesome solid classes
        var iconClasses = $"fa-solid fa-{Name}";

        // Preserve existing classes (like btn-icon, text-red-500)
        if (output.Attributes.TryGetAttribute("class", out var existingClassAttribute))
        {
            var existingClasses = existingClassAttribute.Value.ToString();
            output.Attributes.SetAttribute("class", $"{iconClasses} {existingClasses}");
        }
        else
        {
            output.Attributes.SetAttribute("class", iconClasses);
        }

        // Avoid rendering the "name" attribute in the final HTML
        output.Attributes.RemoveAll("name");
    }
}

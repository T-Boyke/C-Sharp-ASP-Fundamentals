using System.ComponentModel.DataAnnotations;

namespace _06_NewsApplication2.Domain.Entities;

/// <summary>
/// Repräsentiert einen News-Artikel.
/// </summary>
public class Article
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Die Überschrift darf maximal 100 Zeichen lang sein.")]
    public required string Headline { get; set; }

    [Required]
    public required string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Display(Name = "Author")]
    public int AuthorId { get; set; }

    public virtual Author? Author { get; set; }

    /// <summary>
    /// Generiert eine Vorschau des Inhalts (ca. 100 Zeichen), 
    /// wobei am Ende eines vollständigen Wortes abgeschnitten wird.
    /// </summary>
    public string ContentPreview
    {
        get
        {
            if (string.IsNullOrEmpty(Content) || Content.Length <= 100)
                return Content ?? string.Empty;

            var preview = Content.Substring(0, 100);
            var lastSpace = preview.LastIndexOf(' ');
            
            if (lastSpace > 0)
            {
                return preview.Substring(0, lastSpace) + "...";
            }
            
            return preview + "...";
        }
    }
}

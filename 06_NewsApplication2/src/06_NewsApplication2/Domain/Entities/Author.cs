using System.ComponentModel.DataAnnotations;

namespace _06_NewsApplication2.Domain.Entities;

/// <summary>
/// Repräsentiert einen Autor in der News-Anwendung.
/// </summary>
public class Author
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Firstname { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Lastname { get; set; }

    /// <summary>
    /// Ruft den vollständigen Namen des Autors ab.
    /// </summary>
    public string Fullname => $"{Firstname} {Lastname}".Trim();

    /// <summary>
    /// Navigations-Property für Artikel des Autors.
    /// </summary>
    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}

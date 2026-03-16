using System.ComponentModel.DataAnnotations;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Represents a critic review snippet from Metacritic.
/// </summary>
public class MetacriticReview
{
    public int MetacriticReviewID { get; set; }
    
    public int FilmID { get; set; }
    public virtual Film Film { get; set; } = null!;

    [MaxLength(200)]
    public string? Author { get; set; }

    [MaxLength(200)]
    public string? Publication { get; set; }

    public int? Score { get; set; }

    public string Content { get; set; } = string.Empty;

    public string? ReviewUrl { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace _07_Patienten.Domain.Entities;

/// <summary>
/// Repräsentiert einen Patienten in der Arztpraxis.
/// </summary>
public class Patient
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Firstname { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Lastname { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Birthdate { get; set; }

    [Required]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Die SVNR muss genau 10 Ziffern lang sein.")]
    public required string SocialSecurityNumber { get; set; }

    /// <summary>
    /// Berechnet das aktuelle Alter des Patienten.
    /// </summary>
    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - Birthdate.Year;
            if (Birthdate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }

    public string Fullname => $"{Firstname} {Lastname}";

    /// <summary>
    /// Navigations-Property für Untersuchungen.
    /// </summary>
    public virtual ICollection<Examination> Examinations { get; set; } = new List<Examination>();
}

using System.ComponentModel.DataAnnotations;

namespace _07_Patienten.Domain.Entities;

public class Doctor
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string? Title { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Firstname { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Lastname { get; set; }

    public string Fullname => string.IsNullOrWhiteSpace(Title) ? $"{Firstname} {Lastname}" : $"{Title} {Firstname} {Lastname}";

    // Navigation Property back to Patient
    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}

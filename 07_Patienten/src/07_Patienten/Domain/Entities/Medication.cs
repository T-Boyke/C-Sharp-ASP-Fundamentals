using System.ComponentModel.DataAnnotations;

namespace _07_Patienten.Domain.Entities;

/// <summary>
/// Repräsentiert ein verschriebenes Medikament für einen Patienten.
/// </summary>
public class Medication
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(50)]
    public string? Dosage { get; set; }

    [MaxLength(200)]
    public string? Instructions { get; set; }

    public DateTime PrescribedDate { get; set; }
    
    [MaxLength(20)]
    public string? Pzn { get; set; }

    public int PatientId { get; set; }
    public virtual Patient Patient { get; set; } = null!;
}

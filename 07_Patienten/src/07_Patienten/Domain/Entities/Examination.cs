using System.ComponentModel.DataAnnotations;

namespace _07_Patienten.Domain.Entities;

/// <summary>
/// Repräsentiert eine medizinische Untersuchung eines Patienten.
/// </summary>
public class Examination
{
    public int Id { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; } = DateTime.Now;

    [Required]
    public required string Findings { get; set; }

    [Required]
    public int PatientId { get; set; }

    public virtual Patient? Patient { get; set; }
}

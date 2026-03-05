using System.ComponentModel.DataAnnotations;

namespace _07_Patienten.Domain.Entities;

public class HealthInsurance
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    // Navigation Property back to Patient
    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}

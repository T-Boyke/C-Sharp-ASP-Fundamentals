using System.ComponentModel.DataAnnotations;

namespace _07_Patienten.Domain.Entities;

public class ContactInfo
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string? PhoneNumber { get; set; }

    [MaxLength(100)]
    [EmailAddress]
    public string? Email { get; set; }

    // Navigation Property back to Patient
    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}

using System.ComponentModel.DataAnnotations;

namespace _07_Patienten.Domain.Entities;

public class Address
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Street { get; set; }

    [Required]
    [MaxLength(20)]
    public required string HouseNumber { get; set; }

    [Required]
    [MaxLength(10)]
    public required string ZipCode { get; set; }

    [Required]
    [MaxLength(100)]
    public required string City { get; set; }

    // Navigation Property back to Patient
    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}

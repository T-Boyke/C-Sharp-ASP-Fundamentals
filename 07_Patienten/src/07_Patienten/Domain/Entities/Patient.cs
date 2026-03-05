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
    /// Gibt an, ob der Patient privat versichert ist (Prioritäts-Flag).
    /// </summary>
    public bool IsPrivatePatient { get; set; }

    /// <summary>
    /// Datum des nächsten geplanten Termins.
    /// </summary>
    [DataType(DataType.DateTime)]
    public DateTime? NextAppointmentDate { get; set; }

    /// <summary>
    /// Aktuelle Symptome oder Anamnese-Notizen.
    /// </summary>
    [MaxLength(1000)]
    public string? Symptoms { get; set; }

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

    // --- 3NF Relations ---

    // Krankenkasse
    public int? HealthInsuranceId { get; set; }
    public virtual HealthInsurance? HealthInsurance { get; set; }

    // Behandelnder Arzt
    public int? DoctorId { get; set; }
    public virtual Doctor? Doctor { get; set; }

    // Anschrift
    public int? AddressId { get; set; }
    public virtual Address? Address { get; set; }

    // Kontaktdaten
    public int? ContactInfoId { get; set; }
    public virtual ContactInfo? ContactInfo { get; set; }

    // --- Collections ---
    /// Navigations-Property für Untersuchungen.
    /// </summary>
    public virtual ICollection<Examination> Examinations { get; set; } = new List<Examination>();

    /// <summary>
    /// Navigations-Property für verschriebene Medikamente.
    /// </summary>
    public virtual ICollection<Medication> Medications { get; set; } = new List<Medication>();
}

using _08_Patienten2.Domain.ValueObjects;

namespace _08_Patienten2.Domain.Entities;

/// <summary>
/// Repräsentiert einen Patienten in der Arztpraxis (DDD Aggregate Root).
/// </summary>
public class Patient
{
    /// <summary>
    /// Initialisiert eine neue Instanz des Patienten.
    /// </summary>
    /// <param name="firstname">Vorname des Patienten.</param>
    /// <param name="lastname">Nachname des Patienten.</param>
    /// <param name="birthdate">Geburtsdatum.</param>
    /// <param name="ssn">Sozialversicherungsnummer (10 Ziffern).</param>
    public Patient(string firstname, string lastname, DateTime birthdate, string ssn)
    {
        Firstname = firstname;
        Lastname = lastname;
        Birthdate = birthdate;
        SocialSecurityNumber = ssn;
    }

    // EF Core Konstruktor
    protected Patient() { }

    public int Id { get; private set; }
    public string Firstname { get; private set; }
    public string Lastname { get; private set; }
    public DateTime Birthdate { get; private set; }
    public string SocialSecurityNumber { get; private set; }
    public bool IsPrivatePatient { get; private set; }
    public DateTime? NextAppointmentDate { get; private set; }
    public string? Symptoms { get; private set; }

    // Value Objects
    public Address? Address { get; private set; }
    public ContactInfo? ContactInfo { get; private set; }

    // Relations
    public int? HealthInsuranceId { get; private set; }
    public virtual HealthInsurance? HealthInsurance { get; private set; }

    public int? DoctorId { get; private set; }
    public virtual Doctor? Doctor { get; private set; }

    private readonly List<Examination> _examinations = [];
    public virtual IReadOnlyCollection<Examination> Examinations => _examinations.AsReadOnly();

    /// <summary>
    /// Aktualisiert die Adresse des Patienten.
    /// </summary>
    public void UpdateAddress(Address newAddress) => Address = newAddress;

    /// <summary>
    /// Setzt den nächsten Untersuchungstermin.
    /// </summary>
    public void ScheduleAppointment(DateTime appointmentDate) => NextAppointmentDate = appointmentDate;

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
}

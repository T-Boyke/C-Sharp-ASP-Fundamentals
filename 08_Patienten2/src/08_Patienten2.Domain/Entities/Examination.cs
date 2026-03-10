namespace _08_Patienten2.Domain.Entities;

/// <summary>
/// Repräsentiert eine medizinische Untersuchung eines Patienten.
/// </summary>
public class Examination
{
    public Examination(DateTime date, string description, string findings)
    {
        Date = date;
        Description = description;
        Findings = findings;
    }

    protected Examination() { }

    public int Id { get; private set; }
    public DateTime Date { get; private set; }
    public string Description { get; private set; }
    public string Findings { get; private set; }

    public int PatientId { get; private set; }
    public virtual Patient? Patient { get; private set; }
}

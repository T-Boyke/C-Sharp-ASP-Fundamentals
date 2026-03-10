namespace _08_Patienten2.Domain.Entities;

/// <summary>
/// Repräsentiert ein Medikament, das einem Patienten verschrieben wurde.
/// </summary>
public class Medication
{
    public Medication(string name, string dosage, string instructions)
    {
        Name = name;
        Dosage = dosage;
        Instructions = instructions;
    }

    protected Medication() { }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Dosage { get; private set; }
    public string Instructions { get; private set; }

    public int PatientId { get; private set; }
    public virtual Patient? Patient { get; private set; }
}

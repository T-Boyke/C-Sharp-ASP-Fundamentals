namespace _08_Patienten2.Domain.Entities;

/// <summary>
/// Repräsentiert einen behandelnden Arzt.
/// </summary>
public class Doctor
{
    public Doctor(string name, string specialty)
    {
        Name = name;
        Specialty = specialty;
    }

    protected Doctor() { }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Specialty { get; private set; }
}

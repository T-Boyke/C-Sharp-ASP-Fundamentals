namespace _08_Patienten2.Domain.Entities;

/// <summary>
/// Repräsentiert eine Krankenkasse.
/// </summary>
public class HealthInsurance
{
    public HealthInsurance(string name, string providerCode)
    {
        Name = name;
        ProviderCode = providerCode;
    }

    protected HealthInsurance() { }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string ProviderCode { get; private set; }
}

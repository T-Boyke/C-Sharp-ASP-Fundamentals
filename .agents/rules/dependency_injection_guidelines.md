# Dependency Injection Guidelines

Saubere Verwaltung von Abhängigkeiten für Testbarkeit und Wartbarkeit.

## 🏗️ Core Principles

- **Constructor Injection**: Ausschließlich Constructor-Injection verwenden. Mit C# 14 bevorzugt via **Primary Constructors**.
- **Interface-basiert**: Registriere Abhängigkeiten immer gegen ein Interface (`IService`), niemals gegen die Implementierung.
- **Service Locator vermeiden**: Nutze NIEMALS `IServiceProvider` direkt im Code, um Instanzen aufzulösen (Anti-Pattern).

## ⏱️ Service Lifetimes

Wähle die Lifetime mit Bedacht:

1.  **Transient**: Erstellt eine neue Instanz bei jeder Anfrage. Standard für leichtgewichtige, zustandslose Services.
2.  **Scoped**: Erstellt eine Instanz pro HTTP-Request. Ideal für Repositories oder Services, die mit der Datenbank interagieren (`DbContext` ist Scoped).
3.  **Singleton**: Eine Instanz für die gesamte Laufzeit der Anwendung. Nur für absolut zustandslose oder globale Cache-Services.

## 🚀 Implementierung (ASP.NET Core 10)

```csharp
// Registrierung in Program.cs
builder.Services.AddScoped<IPatientService, PatientService>();

// Nutzung via Primary Constructor (C# 14 Style)
public class PatientController(IPatientService patientService) : Controller
{
    // patientService ist hier direkt verfügbar
}
```

## 🛡️ Excellence Checklist
- [ ] Werden alle Abhängigkeiten über den Constructor injiziert?
- [ ] Ist die gewählte Lifetime korrekt (Vermeidung von Captive Dependencies)?
- [ ] Wird gegen Interfaces programmiert?

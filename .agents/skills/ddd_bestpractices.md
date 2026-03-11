# Skill: DDD Best Practices (Domain-Driven Design)

Richtlinien für eine saubere, fachlich orientierte Softwarearchitektur.

## 🏛️ Strategisches Design

- **Ubiquitous Language**: Verwende fachlich korrekte Begriffe (Deutsch/Englisch-Mix je nach Projektkontext) konsequent im Code und in der Dokumentation.
- **Bounded Context**: Halte die Domäne isoliert von Infrastruktur (Datenbank, API) und UI. Jede Unit sollte ihren eigenen klaren Fokus haben.

## 🏗️ Lösungs- & Ordnerstruktur

Projekte sollten konsequent in Layer unterteilt werden, um Separation of Concerns (SoC) sicherzustellen:

- **Src/**: Der Quellcode der Anwendung.
  - `{Project}.Domain`: Entitäten, Value Objects, Domain Services, Repositories-Interfaces (keine Abhängigkeiten!).
  - `{Project}.Application`: Use Cases (Commands/Queries), DTOs, Mapper, Application Logic.
  - `{Project}.Infrastructure`: DB-Kontext (EF Core), Repository-Implementierungen, externe Services (Mail, API).
  - `{Project}.Web`: MVC/WebAPI, Middlewares, Frontend-Assets, Controller.
- **Tests/**: Analog zum Src-Ordner.
  - `{Project}.UnitTests`: Tests für Domain & Application.
  - `{Project}.IntegrationTests`: Tests für Infrastructure & Web.

### Solution Management (.slnx)

- Nutze das moderne **.slnx** Format (Visual Studio 2022 v17.10+) für eine schlanke und git-freundliche Verwaltung statt klassischer `.sln` Dateien.

## 🧩 Taktisches Design (Muster)

### 1. Entities (Entitäten)

- Besitzen eine eindeutige Identität (ID).
- **Kapselung**: Nutze `private set` für Eigenschaften. Zustandsänderungen erfolgen nur über explizite Methoden (z.B. `AktiviereKonto()`).
- **Validierung**: Eine Entität sollte sich niemals in einem invaliden Zustand befinden.

### 2. Value Objects (Wertobjekte)

- Haben keine Identität, werden über ihre Werte definiert (z.B. `Addresse`, `Geldeinheit`).
- **Immutability**: Wertobjekte sind unveränderlich. Änderungen geben eine neue Instanz zurück.
- In EF Core 10 als **Owned Types** oder **Complex Types** abgebildet.

### 3. Aggregates (Aggregate)

- Eine Gruppe von Entitäten und Wertobjekten, die als Einheit betrachtet werden.
- Das **Aggregate Root** ist der einzige Zugriffspunkt von außen.

### 4. Domain Services

- Enthalten Logik, die nicht sinnvoll in eine einzelne Entität passt (z.B. koordinierte Berechnungen über mehrere Entitäten hinweg).

## 🚀 Implementierung in C# 14 & .NET 10

- **Primary Constructors**: Ideal für Value Objects oder Services.
- **Records**: Nutze `record` für Value Objects, um automatische Wert-Gleichheit und Immutability zu erhalten.
- **Encapsulated Collections**: Gib Listen nur als `IReadOnlyCollection<T>` oder `IEnumerable<T>` nach außen.

```csharp
public class Einkaufswagen
{
    private readonly List<Einkaufsposition> _positionen = [];

    /// <summary>
    /// Die Liste der aktuellen Positionen im Einkaufswagen (schreibgeschützt).
    /// </summary>
    public IReadOnlyCollection<Einkaufsposition> Positionen => _positionen.AsReadOnly();

    public void FuegePositionHinzu(Produkt produkt, int menge)
    {
        // Domänenlogik & Validierung hier...
    }
}
```

## 🛡️ IHK-Exzellenz-Check

- [ ] Ist die Fachlogik von der Technik (SQL, ASP.NET) getrennt?
- [ ] Sind alle Fachbegriffe konsistent dokumentiert?
- [ ] Verhindern die Entitäten illegale Zustände durch Kapselung?

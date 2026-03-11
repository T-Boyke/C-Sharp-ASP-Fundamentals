# Common Infrastructure

Geteilte Komponenten und Infrastruktur-Entscheidungen für alle Units.

## 💾 Datenhaltung
- **Datenbank**: SQL Server (LocalDB) oder SQLite für Unit-Beispiele.
- **ORM**: Entity Framework Core 10 (siehe `efcore_bestpractices.md`).
- **Migrations**: Werden pro Unit im `Infrastructure` Layer verwaltet.

## 🛠️ Tooling & Libs
- **Logging**: Serilog oder das standard Microsoft.Extensions.Logging.
- **Validierung**: FluentValidation für komplexe Fachlogik.
- **Visualisierung**: Tailwind CSS 4.2 als primärer Design-Stack.

## 🏗️ Gemeinsame Muster
- **Result Pattern**: Verwendung eines generischen `Result<T>` Objekts für Domain-Antworten.
- **Mapping**: Manuelles Mapping oder dedizierte Map-Methoden werden gegenüber "magischen" Automappern bevorzugt.
- **DI-Registrierung**: Findet zentral in der `Program.cs` der jeweiligen Unit statt.

## 📂 Dateipfade
- Quellcode: `src/[UnitName]`
- Tests: `tests/[UnitName].Tests`
- Dokumentation: `docs/adr` oder `docs/unit-docs`

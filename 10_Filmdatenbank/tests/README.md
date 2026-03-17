# 🧪 FilmDB - Test Suite (/tests)

Dieses Verzeichnis enthält die Tests für die Film-Management-Applikation. Wir verfolgen einen strikten **TDD-Ansatz** mit dem Ziel einer **100% Code-Abdeckung**.

## 🏗️ Test-Struktur

Die Tests sind in drei Hauptprojekte unterteilt, um eine klare Trennung zwischen reiner Logik und Infrastruktur-Abhängigkeiten zu gewährleisten:

### 1. [_10_Filmdatenbank.UnitTests](./_10_Filmdatenbank.UnitTests)

- **Fokus**: Testet die Geschäftslogik isoliert.
- **Inhalt**:
  - **Domain**: Validierung der Entitäten (`Film`, `Person`, etc.).
  - **Web**: Controller-Logik mit gemockten Abhängigkeiten via **Moq**.
- **Vorteil**: Extrem schnelle Ausführung, da keine Datenbank- oder I/O-Zugriffe erfolgen.

### 2. [_10_Filmdatenbank.IntegrationTests](./_10_Filmdatenbank.IntegrationTests)

- **Fokus**: Testet das Zusammenspiel der Komponenten mit echter Datenbank-Interaktion.
- **Inhalt**:
  - **Infrastructure**: Persistenz-Checks mit **EF Core In-Memory Database**.
  - **Seeding**: Validierung des automatischen Daten-Seeds via **Bogus**.
  - **Web**: Controller-Tests, die gegen eine reale (In-Memory) Datenbank laufen.
- **Vorteil**: Stellt sicher, dass die Anwendung auch mit realen Datenflüssen korrekt funktioniert.

### 3. [_10_Filmdatenbank.PlaywrightTests](./_10_Filmdatenbank.PlaywrightTests)

- **Fokus**: **Systemtests (End-to-End)** im echten Browser.
- **Inhalt**:
  - **UI/UX**: Testet Benutzer-Workflows (Login, Suche, Import, Admin-Bereich).
  - **Cross-Browser**: Unterstützt Chromium, Firefox und Webkit via **Playwright**.
  - **Self-Hosted**: Startet automatisch einen Test-Server auf Port 5016.
- **Vorteil**: Validiert das fertige System aus der Sicht des Endbenutzers.

## 🚀 Tests ausführen

Zum Ausführen aller Tests kannst du entweder den Test Explorer in Visual Studio nutzen oder die CLI verwenden:

```bash
# Alle Tests in der Solution ausführen
dotnet test ../10_Filmdatenbank.slnx

# Nur Unit Tests
dotnet test _10_Filmdatenbank.UnitTests

# Nur Integration Tests
dotnet test _10_Filmdatenbank.IntegrationTests

# Nur System Tests (E2E)
dotnet test _10_Filmdatenbank.PlaywrightTests
```

> [!IMPORTANT]
> Für die Systemtests müssen die Browser-Binärdateien installiert sein. Falls Fehler auftreten:
> `pwsh _10_Filmdatenbank.PlaywrightTests/bin/Debug/net10.0/playwright.ps1 install`

## 📈 Coverage Report

Wir nutzen `coverlet`, um die Testabdeckung zu messen. Für 100% Coverage wird jeder logische Pfad in den Controllern und Entity-Modellen geprüft.

---

> [!TIP]
> Neue Features sollten immer zuerst durch einen fehlschlagenden Test definiert werden (**Red**), bevor die Implementierung erfolgt (**Green**).

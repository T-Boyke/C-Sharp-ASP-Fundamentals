# 🛡️ FilmDB - Testing Infrastructure (/tests)

Dieses Verzeichnis enthält die automatisierte Test-Suite für das Film-Management-System. Wir setzen auf **xUnit**, um die Integrität unserer Clean Architecture sicherzustellen.

## 🧪 Test-Strategie

### 1. Unit Tests (Domäne & Applikation)
Wir testen die Kernelemente der Anwendung isoliert:
- **Entities**: Validierung von Properties und Geschäftsregeln in `Film`, `Person` etc.
- **Services**: Prüfung der Geschäftslogik in der Applikations-Schicht unter Verwendung von Mocks für die Infrastruktur.

### 2. Integration Tests (Infrastruktur)
Prüfung des Zusammenspiels mit der Datenbank:
- **Persistence**: Validierung der EF Core Konfigurationen und der Fluent API Mappings.
- **Seeding**: Sicherstellung, dass der `DbSeeder` die korrekte Anzahl an Datensätzen erzeugt.

## 🏃 Ausführung
Um alle Tests zu starten, führe folgenden Befehl im Hauptverzeichnis aus:
```bash
dotnet test
```

---
> [!TIP]
> Die Tests dienen nicht nur der Fehlersuche, sondern auch als Dokumentation für das erwartete Systemverhalten! 📖

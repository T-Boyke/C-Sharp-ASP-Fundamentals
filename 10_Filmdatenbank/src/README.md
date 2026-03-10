# 🛠️ FilmDB - Source Code (/src)

Dieses Verzeichnis enthält den Quellcode der Film-Management-Applikation, unterteilt in eine saubere, mehrschichtige Architektur (Clean Architecture / DDD).

## 🏗️ Architektur-Schichten

### 1. [_10_Filmdatenbank.Domain](./_10_Filmdatenbank.Domain)

Der Kern der Anwendung. Enthält die Geschäftslogik und die Datenmodelle.

* **Entitäten**: `Film`, `Person`, `Eigenschaft`.
* **Beziehungen**: Komplexe Viele-zu-Viele-zu-Viele-Verknüpfung über `PersonEigenschaftFilm`.
* **Eigenschaften**: Definition von Rollen (z.B. Regisseur, Schauspieler).

### 2. [_10_Filmdatenbank.Application](./_10_Filmdatenbank.Application)

Schnittstelle zwischen Domäne und Infrastruktur.

* **Interfaces**: Definition von Diensten.
* **DTOs**: Datenübertragungsobjekte für die Kommunikation nach außen.

### 3. [_10_Filmdatenbank.Infrastructure](./_10_Filmdatenbank.Infrastructure)

Die Anbindung an die Außenwelt (Datenbank, Dateisystem).

* **Persistence**: EF Core `ApplicationDbContext` und Fluent API Konfigurationen.
* **Identity**: Integration von ASP.NET Core Identity für die Benutzerverwaltung.
* **Migrations**: Datenbank-Versionierung.

### 4. [_10_Filmdatenbank.Web](./_10_Filmdatenbank.Web)

Das eigentliche Startprojekt (MVC).

* **Controllers**: Steuerung des FLusses (Film, Account, Home).
* **Views**: Premium UI mit Tailwind CSS 4, Font-Awesome und Inter-Font.
* **StaticAssets**: Konfiguriertes Asset-Management für moderne Webstandards.
* **Seeding**: Initialisierung der Datenbank mit 50+ Testdatensätzen via **Bogus**.

## 🚀 Ausführung

Die Anwendung kann direkt aus Visual Studio oder via CLI gestartet werden:

```bash
dotnet run --project src/_10_Filmdatenbank.Web
```

---

> [!IMPORTANT]
> Achte darauf, dass die ConnectionString in `appsettings.json` im Web-Projekt korrekt konfiguriert ist (Standard: LocalDB).

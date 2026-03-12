# 🛠️ FilmDB - Source Code (/src)

Dieses Verzeichnis enthält den Quellcode der Film-Management-Applikation, unterteilt in eine saubere, mehrschichtige Architektur (Clean Architecture / DDD).

## 🏗️ Architektur-Schichten

### 1. [_10_Filmdatenbank.Domain](./_10_Filmdatenbank.Domain)

Der Kern der Anwendung. Enthält die Geschäftslogik und die Datenmodelle.

* **Entitäten**: `Film`, `Person`, `Eigenschaft` (Erweitert um TMDB "Perfect Alignment" Felder).
* **Beziehungen**: Komplexe Viele-zu-Viele-zu-Viele-Verknüpfung über `PersonEigenschaftFilm`.

### 2. [_10_Filmdatenbank.Application](./_10_Filmdatenbank.Application)

Schnittstelle zwischen Domäne und Infrastruktur.

* **Interfaces**: `ITmdbService` für externe Datenanreicherung.
* **Services**: Implementierung der TMDB-Logik via `TMDbLib`.

### 3. [_10_Filmdatenbank.Web](./_10_Filmdatenbank.Web)

Das eigentliche Startprojekt (MVC).

* **Controllers**: `Film`, `Tmdb` (API für Echtzeit-Suche), `Account`, `Home`.
* **Views**: Premium UI mit Backdrop-Hero-Sektionen und dynamischer Cast-Auswahl.
* **Seeding**: Initialisierung mit **Bogus** und TMDB-kompatiblen Primärschlüsseln.

## 🚀 Ausführung

Die Anwendung kann direkt aus Visual Studio oder via CLI gestartet werden:

```bash
dotnet run --project src/_10_Filmdatenbank.Web
```

---

> [!IMPORTANT]
> Achte darauf, dass die ConnectionString in `appsettings.json` im Web-Projekt korrekt konfiguriert ist (Standard: LocalDB).

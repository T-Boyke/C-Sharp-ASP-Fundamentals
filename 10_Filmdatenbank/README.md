# 🎬 Unit 10: Film-Management-System (FilmDB)

Willkommen zur **Unit 10**. In diesem Modul haben wir ein professionelles Film-Management-System aufgebaut. Dieses Projekt demonstriert eine saubere, mehrschichtige Architektur (DDD) kombiniert mit einem modernen, performanten Frontend.

---

## 👨‍🏫 Was ist das hier? (Für absolute Anfänger)
Stell dir vor, du bist der Betreiber einer Videothek oder eines Streaming-Dienstes. Du musst Filme speichern, wissen welcher Schauspieler welche Rolle spielt und wer Regie geführt hat. Genau das macht diese App!

### Die wichtigsten Regeln, die hier genutzt werden:
1. **Clean Architecture (DDD)**: Das Programm ist in klare Schichten unterteilt. Der Kern (die Filme) ist streng getrennt von der Speicherung (Datenbank) und der Darstellung (Webseite).
2. **Identity & RBAC**: Es gibt verschiedene Rollen (Admin, Member). Nur wer angemeldet ist, sieht die Details, und nur Admins dürfen Daten ändern.
3. **Automatisches Seeding**: Beim ersten Start füllt sich die Datenbank automatisch mit über 50 realistischen Filmen und Personen – dank **Bogus**.
4. **Kein Bootstrap**: Wir setzen zu 100% auf **Tailwind CSS 4.2** für maximale Design-Freiheit und Performance. ✨

---

## 🏗️ Wie ist das Projekt aufgebaut?
Das Projekt folgt dem Clean Architecture Muster und besteht aus vier Schichten:

1. **[Domäne (Kernelemente)](./src/_10_Filmdatenbank.Domain/Entities)**: Enthält die Entitäten `Film.cs`, `Person.cs` und `Eigenschaft.cs`. Die Beziehung zwischen ihnen wird über die Link-Tabelle `PersonEigenschaftFilm.cs` (Many-to-Many-to-Many) abgebildet.
2. **[Infrastruktur (Speicher)](./src/_10_Filmdatenbank.Infrastructure)**: Hier wird Entity Framework Core genutzt, um die Daten sicher in einer SQL Server Datenbank zu speichern (In-Memory für Tests).
3. **[Applikation (Logik)](./src/_10_Filmdatenbank.Application)**: Hier liegen die Schnittstellen und Dienste für die Geschäftslogik.
4. **[Webseite (UI)](./src/_10_Filmdatenbank.Web)**: Ein modernes ASP.NET MVC Frontend mit Tailwind 4, Inter-Font und Glassmorphism-Effekten.
5. **[Tests (Qualitätssicherung)](./tests)**: Getrennte **Unit-** und **Integrations-Tests** mit 100% Abdeckung.

---

## 📂 Wo finde ich was?
* [**/src**](./src/README.md): Der eigentliche Quellcode, unterteilt in die vier Architektur-Schichten.
* [**/tests**](./tests/README.md): DDD-konforme Test-Suite (Unit & Integration).
* [**/docs/diagrams.md**](./docs/diagrams.md): **Technische Dokumentation (Big 5 UML + ERD).**
* [**/aufgabe**](./aufgabe/Aufgabe%20Filmdatenbank.pdf): Die ursprüngliche Aufgabenstellung als PDF.

---

## 🚀 Schnellstart: So startest du die App
1.  Öffne die Datei `10_Filmdatenbank.slnx` in **Visual Studio 2022** (Version 17.10+).
2.  Stelle sicher, dass `_10_Filmdatenbank.Web` als Startprojekt festgelegt ist.
3.  Drücke **F5**.
4.  Die Datenbank wird automatisch erstellt und mit **Testdaten** gefüllt.
5.  **Logins**:
    - **Admin**: `admin@film.de` (Passwort: `Admin123!`)
    - **Member**: Erstelle dir einfach einen eigenen Account!

---

## 🛡️ Qualität & Sicherheit
*   **Normalisierte Datenbank**: 3. Normalform für Personen, Rollen und Filme zur Vermeidung von Redundanz.
*   **Rollen-Management**: Klare Trennung zwischen Lese- und Schreibrechten via ASP.NET Identity.
*   **Premium Design**: Responsive Grid, hover-animierte Karten und moderne Typografie (Inter).
*   **Realistische Daten**: Bogus sorgt für glaubwürdige Test-Szenarien.
*   **Kein Altlasten**: Vollständiger Verzicht auf Bootstrap für ein modernes Asset-Management.

---
> [!TIP]
> Schau dir die `walkthrough.md` im Brain-Verzeichnis an, um Details zur Implementierung und den Design-Entscheidungen zu erfahren! 🗺️

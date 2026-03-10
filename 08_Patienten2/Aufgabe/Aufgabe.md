# Arbeitsauftrag: Modernisierung des Patienten-Management-Systems (08_Patienten2)

## 🎯 Zielsetzung
Das bestehende Patienten-Management-System soll in eine hochperformante, testbare und wartbare **DDD (Domain-Driven Design)** Architektur überführt werden. Dabei sind moderne Webstandards und strikte Sicherheitsvorgaben (IHK-Konformität) einzuhalten.

## 📋 Anforderungen

### 1. Architektur & Struktur
- Umstellung auf ein Multi-Projekt-Solution Layout (`.sln` oder `.slnx`).
- Unterteilung in die Layer: **Domain**, **Application**, **Infrastructure** und **Web**.
- Einhaltung des Clean Architecture Prinzips (Abhängigkeiten zeigen nach innen zum Domain-Layer).

### 2. Technologie-Stack
- **Frontend**: Verwendung von **Tailwind CSS 4.2** (CLI Version) und **Font-Awesome 7.2** (lokale Einbindung). Kein Bootstrap verwenden.
- **Backend**: .NET 10 (C# 14 Features nutzen).
- **ORM**: Entity Framework Core 10 mit SQL Server (LocalDB).

### 3. Sicherheit (IAM & RBAC)
- Implementierung von **ASP.NET Core Identity**.
- Konfiguration eines rollenbasierten Zugriffsschutzes (Roles: `Admin`, `User`).
- Automatisches Seeding eines Administrator-Accounts bei Systemstart.

### 4. Datenmodell & DDD
- Implementierung von aggregierten Wurzeln (Aggregate Roots) wie `Patient` und `Doctor`.
- Einsatz von **Value Objects** für komplexe Datentypen wie `Address` und `ContactInfo`.
- Verwendung des **Unit of Work** und **Repository Patterns**.

### 5. Qualitätssicherung (TDD)
- Erstellung von Unit-Tests für Domain-Logik und Application-Services.
- Einsatz von xUnit, FluentAssertions und NSubstitute.
- Ziel: 100% Abdeckung der kritischen Geschäftslogik.

### 6. Dokumentation
- Durchgängige **deutsche XML-Dokumentation** im Domain- und Application-Layer zur Sicherstellung der IHK-Konformität.

## 🛠️ Auslieferung
Die Lösung muss über eine zentrale Projektdatei startbar sein und alle Tests müssen erfolgreich durchlaufen.

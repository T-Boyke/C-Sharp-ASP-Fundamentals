# Arbeitsauftrag: Identity Management & Security (09_Identity)

## 🎯 Zielsetzung
Implementierung eines robusten Identitätsmanagements unter Verwendung von ASP.NET Core Identity in einer professionellen **DDD (Domain-Driven Design)** und **TDD (Test-Driven Development)** Architektur.

## 📋 Anforderungen

### 1. Architektur
- Aufbau einer 4-Layer-Struktur: **Domain**, **Application**, **Infrastructure**, **Web**.
- Einhaltung der Clean Architecture Prinzipien (Inward dependencies).

### 2. Identity & Security (IAM/RBAC)
- Integration von **ASP.NET Core Identity**.
- Implementierung von rollenbasiertem Zugriffsschutzes (RBAC).
- Automatischer Seed von Rollen (`Admin`, `User`) und einem Administrator-Account.

### 3. Frontend Standards
- Einsatz von **Tailwind CSS 4.2** CLI für ein modernes Premium-Design.
- Lokale Einbindung von **Font-Awesome 7.2**.
- Erstellung einer ansprechenden Login-Oberfläche und Error-States (Access Denied).

### 4. Qualitätssicherung
- Unit-Tests für Domain-Value-Objects und Application-Services.
- Sicherstellung von 100% Logic-Coverage.
- Verwendung von xUnit, FluentAssertions und NSubstitute.

### 5. Dokumentation
- **Deutsche XML-Dokumentation** zur Sicherstellung der IHK-Konformität.

## 🛠️ Auslieferung
Einreichung einer funktionsfähigen Solution, in der alle Tests grün sind und der Admin-Seed bei Start aktiv ist.

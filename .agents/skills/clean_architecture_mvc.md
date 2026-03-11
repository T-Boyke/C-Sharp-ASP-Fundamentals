# Skill: Clean Architecture MVC Workflow

Anleitung zur sauberen Trennung von Verantwortlichkeiten in Web-Projekten.

## 🏗️ Struktur eines Features

### 1. Domain Layer (Kern)
- Definiere die **Entities** und **Value Objects** (siehe `ddd_bestpractices.md`).
- Schreibe Logik in die Domäne, NIEMALS in den Controller.

### 2. Application Layer (Service)
- Erstelle **Application Services**, die Domänenlogik koordinieren.
- Nutze **DTOs** (Data Transfer Objects) für den Datenaustausch nach außen.

### 3. Web Layer (UI/MVC)
- **ViewModels**: Jede View bekommt ein exakt passendes ViewModel. Keine Domänen-Objekte in der View!
- **Mapping**: Nutze sauberes Mapping (manuell oder AutoMapper) zwischen DTOs und ViewModels.
- **Controller**: Halte sie "thin" (dünn). Er validiert die Eingabe und ruft den Service auf.

## 🔄 Workflow eines Requests
1. **Controller** empfängt Daten (POST/GET).
2. **Validator** (FluentValidation) prüft das ViewModel.
3. **Controller** mappt ViewModel auf DTO und ruft **Service** auf.
4. **Service** arbeitet mit dem **Repository** und der **Domain**.
5. **Service** gibt DTO zurück.
6. **Controller** mappt DTO auf ViewModel und gibt **View** zurück.

## 🛡️ Architektur-Checkliste
- [ ] Gibt es Logik im Controller? (Wenn ja: In Domain/Service verschieben).
- [ ] Greift die View direkt auf die Datenbank/Entität zu? (Wenn ja: ViewModel nutzen).
- [ ] Ist der Code testbar, ohne eine Datenbank zu starten? (Abhängigkeiten über Interfaces injiziert).

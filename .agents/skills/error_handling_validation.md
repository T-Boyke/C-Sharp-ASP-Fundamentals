# Skill: Error Handling & Validation

Workflow für ein sicheres System und exzellentes Benutzerfeedback.

## 🛡️ Validierung (FluentValidation)

- **Regeln**: Definiere Validierungsregeln in separaten `Validator`-Klassen.
- **Benutzerfeedback**: Fehlermeldungen müssen präzise und für Anfänger
  verständlich sein.
- **Client-Side**: Nutze jQuery Validation (oder Tailwind-basierte Feedback-UI)
  für schnelles Feedback.

## 🛑 Error Handling Strategie

### 1. Global Exception Handler

- Nutze Middleware, um alle ungefangenen Fehler zentral zu loggen und eine
  saubere "Oops"-Seite anzuzeigen.
- Zeige NIEMALS technische Stacktraces in der Produktion.

### 2. Result-Pattern (Domain-Ebene)

- Nutze ein `Result`-Objekt (Success/Failure) für geschäftliche Fehler statt
  Exceptions für den Programmfluss.
- Beispiel: `Result.Failure("Dieser Patient ist bereits registriert.")`.

### 3. IHK-Konformität (Logging)

- Logge Fehler mit ausreichend Kontext (Serilog/ILogger).
- Unterscheide zwischen `Warning` (Nutzerfehler) und `Error` (Systemfehler).

## 🚀 Implementierungs-Schritte

1. Erstelle einen **Validator** für das ViewModel.
2. Integeiere die Validierung im **Controller** (`ModelState.IsValid`).
3. Rückgabe von Fehlern an die View via `ValidationSummary` oder gezielte
   UI-Overlays.
4. Dokumentiere kritische Fehlerzustände mit `<exception>` XML-Tags.

## 🛡️ Excellence Checklist

- [ ] Sind alle Eingaben validiert, bevor sie verarbeitet werden?
- [ ] Gibt es eine freundliche Fehlermeldung für den User?
- [ ] Werden Fehler konsistent geloggt?

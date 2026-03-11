# Security Best Practices (IHK Standard)

Sicherheit ist nicht verhandelbar – ein Muss für jeden IHK-Prüfer.

## 🛡️ Defense in Depth

### 1. Input-Validierung

- Vertraue NIEMALS Benutzereingaben.
- Validierung auf **Client- und Server-Seite** (siehe
  `error_handling_validation.md`).

### 2. Schutz vor häufigen Angriffen

- **XSS (Cross-Site Scripting)**: Nutze die automatische HTML-Kodierung von
  Razor/Blazor. Vermeide `MarkupString`, wenn es nicht sicher ist.
- **SQL-Injection**: Nutze Entity Framework Core (parametrisierte Queries sind
  Standard).
- **CSRF (Cross-Site Request Forgery)**: Nutze automatische Antiforgery-Tokens
  in MVC-Formularen.

### 3. Sensible Daten

- **Connection Strings**: Nutze `Secret Manager` für die lokale Entwicklung und
  `Environment Variables` für die Produktion. NIEMALS Passwörter im Git
  hinterlegen.
- **Hashing**: Speichere Passwörter niemals im Klartext. Nutze `Identity` oder
  `BCrypt`.

## 🛑 Best Practices für die Prüfung

- [ ] Werden sensible Daten verschlüsselt oder gehasht?
- [ ] Gibt es klare Berechtigungsprüfungen (`[Authorize]`)?
- [ ] Werden Sicherheitsupdates für NuGet-Pakete eingespielt?
- [ ] Sind alle Fehlerseiten sicher und geben keine Details über das System
      preis?

# API Design Standards (REST)

Richtlinien für konsistente, robuste und entwicklerfreundliche Web-APIs.

## 🏗️ Struktur & Routing

- **Kebab-Case URLs**: Nutze `api/v1/patienten-daten` statt `api/v1/PatientenDaten`.
- **Ressourcen**: Nutze Substantive im Plural (`/produkte`, `/bestellungen`).
- **HTTP-Methoden**: 
    - `GET`: Abrufen von Ressourcen.
    - `POST`: Erstellen neuer Ressourcen.
    - `PUT`: Vollständiges Update einer Ressource.
    - `PATCH`: Teilweises Update einer Ressource.
    - `DELETE`: Löschen einer Ressource.

## 🚀 Status-Codes & Responses

- **Erfolg**: `200 OK`, `201 Created` (nach POST), `204 No Content` (nach DELETE/PUT ohne Body).
- **Fehler**: `400 Bad Request` (Validierung), `401 Unauthorized`, `403 Forbidden`, `404 Not Found`, `500 Internal Server Error`.
- **Besonderheit**: Nutze standardisierte Response-Objekte für Fehlermeldungen (z.B. RFC 7807 Problem Details).

## 🛡️ Best Practices

- **Versionierung**: Nutze URL-Versionierung (z.B. `/api/v1/...`).
- **DTOs**: Nutze DTOs für Input und Output. Gib NIEMALS Domain-Entities direkt zurück.
- **Idempotenz**: GET, PUT und DELETE müssen idempotent sein.

## 🛡️ Excellence Checklist
- [ ] Werden die korrekten HTTP-Verben verwendet?
- [ ] Sind die URLs in Kebab-Case formatiert?
- [ ] Werden DTOs für den Datenaustausch genutzt?
- [ ] Sind die Status-Codes für Erfolg und Fehler konsistent?

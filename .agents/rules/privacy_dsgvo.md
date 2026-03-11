---
description: DSGVO (GDPR) Compliance Standards & Privacy Rules
---

# Datenschutz & DSGVO (GDPR) Standards

Dieses Dokument definiert die verbindlichen Datenschutzstandards gemäß DSGVO
(GDPR) für alle Funktionen, Datenbankdesigns und Benutzeroberflächen in diesem
Projekt. **Privacy by Design** ist eine Grundvoraussetzung.

## 1. Datenminimierung (Data Minimization)

- **Erhebung**: Frage und speichere nur Daten, die für den jeweiligen
  Geschäftszweck absolut notwendig sind (Art. 5 Abs. 1 lit. c DSGVO).
- **Löschkonzept**: Stelle sicher, dass temporäre Daten, Logfiles und
  Trackingdaten regelmäßig automatisiert gelöscht oder anonymisiert werden (z.B.
  IP-Adressen in Web-Server Logs kürzen).
- **Recht auf Vergessenwerden**: Optimiere Datenbankstrukturen so, dass
  Nutzerdaten vollständig entfernt (Hard Delete) oder irreversibel anonymisiert
  (Soft Delete mit Anonymisierung) werden können.

## 2. Zustimmung & Transparenz (Consent & Transparency)

- **Cookie-Consent**: Setze keine nicht-essenziellen Cookies (z.B. Tracking,
  Analytics) vor der expliziten, freiwilligen (Opt-in) Zustimmung des Nutzers.
  Essenzielle Cookies (Sessions, CSRF-Token) erfordern keine Zustimmung, müssen
  aber in der Datenschutzerklärung aufgeführt sein.
- **Formulare**: Binde bei Kontakt-, Registrierungs- oder Kommentarformularen
  Checkboxen zur expliziten Zustimmung der Datenspeicherung und Verarbeitung
  gemäß Datenschutzrichtlinien ein.
- **Transparenz**: Verlinke die Datenschutzbestimmungen prominent aus dem Footer
  und direkt dort, wo Daten erhoben werden.

## 3. Externe Dienstleister & CDNs

- **Node-Free / Local Assets First**: Um die Weitergabe von
  Nutzer-Tracking-Daten (z.B. IP-Adressen) an amerikanische Server zu verhinden
  (Schrems II), verwende **niemals CDNs** (wie jsdelivr, unpkg, google fonts).
  Lade Schriften (z.B. Noto Sans Variable) und Icons (FontAwesome) immer lokal
  über LibMan (`/wwwroot/lib`).
- **Third-Party Services**: Wenn externe Services via API angebunden werden,
  anonymisiere oder pseudonymisiere die ausgehenden IDs und Nutzerdaten.

## 4. Sicherheit & Verschlüsselung (Security & Encryption)

- **Data in Rest**: Speichere Passwörter extrem sicher (ASP.NET Core Identity
  nutzt standardmäßig PBKDF2). Verzichte auf Klartextspeicherung.
- **Data in Transit**: Nutze ausnahmslos HTTPS (TLS 1.2+). Setze HSTS Header via
  ASP.NET Core Middleware (`app.UseHsts()`).
- **Anonymisierung in Logs**: Vermeide die Protokollierung von Personenbezogenen
  Daten (PII) wie E-Mail-Adressen, Klartext-Passwörtern oder Klarnamen in
  Applikationslogs (`ILogger`).

## 5. Berechtigungen (RBAC)

- Nutze Rollen- und Policy-basierte Zugriffskontrollen
  (`[Authorize(Roles = "...")]`), sodass nur berechtigtes Personal (z.B.
  Administratoren) auf vollständige Benutzerdaten Zugriff hat.

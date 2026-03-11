---
description: Accessibility (BTHG/WCAG 2.1 AA) and ARIA Guidelines
---

# Barrierefreiheit (100% A11y & BTHG) Standards

Diese Richtlinie sichert die 100%ige Einhaltung des
**Barrierefreiheitsstärkungsgesetzes (BTHG)** und der **WCAG 2.1 AA**
Konformität für sämtliche Razor Views, C#-Komponenten und Tailwind CSS Layouts
in diesem Projekt.

## 1. Semantisches HTML

- **Struktur**: Nutze korrekte HTML5 Tags (`<header>`, `<nav>`, `<main>`,
  `<article>`, `<section>`, `<footer>` und `<aside>`), um Screenreadern das
  Parsen zu erleichtern.
- **Überschriften**: Halte eine logische Überschriftenhierarchie ein (`<h1>` bis
  `<h6>`), ohne Stufen zu überspringen (z.B. kein `<h3>` direkt nach einem
  `<h1>`). Pro Seite gibt es genau ein `<h1>`.

## 2. ARIA Roles und Attributes (100% A11y Goal)

Wenn natives HTML nicht ausreicht oder eigene UI-Komponenten (Toggles,
Dropdowns) via Vanilla JS (Node-free) gebaut werden:

- **`aria-expanded`**: Für Dropdowns, Modals und Toggles (wie den
  Dark-Mode-Switch). Muss bei Klick via JavaScript aktualisiert werden.
- **`aria-hidden="true"`**: Blende alle rein dekorativen Objekte (insbesondere
  FontAwesome Icons `<i class="fa-solid fa-*"></i>`) konsequent vor
  Screenreadern aus.
- **`aria-label` & `aria-labelledby`**: Gib Buttons, die nur aus Icons bestehen
  (z.B. Löschen-Button, Toggle-Button), immer ein klares, aussagekräftiges
  `aria-label` oder nutze visuell versteckten Begleittext (`sr-only` Klasse von
  Tailwind).
- **`role="..."`**: Weist dynamischen Widgets explizite Rollen zu
  (`role="alert"`, `role="dialog"`, `role="button"`, `role="status"`,
  `role="tablist"`).

## 3. Tastaturnavigation (Keyboard Navigation)

- Jedes interaktive Element (Links, Buttons, Formulare, Toggles) _muss_ via
  Tabulator (`Tab`) erreichbar sein.
- **Fokus-Styles**: Deaktiviere **niemals** den Fokus-Ring, ohne eine deutlich
  sichtbare Alternative anzubieten. Nutze in Tailwind CSS
  `focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-brand-primary`
  auf allen klickbaren Elementen!
- **Tabindex**: Nutze `tabindex="0"` für benutzerdefinierte Komponenten, die
  Fokus erhalten sollen. Vermeide `tabindex` Werte größer als 0 (zerstört
  logische Tab-Ordnung).

## 4. Farben, Kontraste & Visuelle Barrierefreiheit

- **Kontrast (WCAG AA)**: Stelle sicher, dass Text gegenüber seinem Hintergrund
  ein Kontrastverhältnis von mindestens **4.5:1** (für normalen Text) und
  **3:1** (für großen Text) aufweist. Dies gilt auch für Hover-States und im
  Dark Mode!
- **Color-only Meaning**: Informationen dürfen nie ausschließlich durch Farbe
  vermittelt werden (z.B. rot für Fehler). Nutze immer zusätzlich Text, Formen,
  dicke Rahmen oder Icons (z.B.
  `<i class="fa-solid fa-triangle-exclamation"></i>`).
- **Responsive & Zoom**: Die UI muss bei 200% Zoom im Browser voll
  funktionsfähig und bedienbar bleiben, ohne dass horizontales Scrollen oder
  Überschneidungen erzwungen werden (`text-balance`, Vermeidung fixer Höhen
  `h-12` beim Haupttext).

## 5. Formulare & Error Handling

- Verknüpfe jedes `<label>` zwingend über `for="IdDesInputs"` mit dem
  zugehörigen `<input>`.
- Wenn ein Required-Feld falsch befüllt wird, weise in Validation-Summaries via
  `role="alert"` auf den Fehler hin.
- Kennzeichne ungültige Felder mit `aria-invalid="true"` (in ASP.NET Tag Helpers
  oft automatisch unterstützt, aber bei manuellen Feldern zwingend
  hinzuzufügen).

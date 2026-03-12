# Skill: Visual Excellence Workflow (Premium UI)

Schritt-für-Schritt Anleitung zur Erstellung von "Wow"-Interfaces mit Tailwind
CSS 4.2.

## 🏁 Phase 1: Struktur & Layout

- **Semantik**: Nutze HTML5 Elemente (`main`, `section`, `article`, `header`).
- **Layout**: Bevorzuge `grid` für komplexe Layouts und `flex` für
  Komponenten-Innereien.
- **Spacing**: Nutze konsistente Abstände (z.B. `p-6`, `gap-4`).

## 🎨 Phase 2: Design-Sprache

- **Farben**: Vermeide harte Kontraste (reines Schwarz). Nutze `slate-900` für
  Text und `slate-50` für Hintergründe.
- **Gradients**: Setze dezente Verläufe ein:
  `bg-linear-to-br from-white to-slate-100`.
- **Glassmorphism**: Für Overlays oder Karten:
  `bg-white/70 backdrop-blur-xl border border-white/20`.

## ✨ Phase 3: Premium Polishing

- **Shadows**: Nutze `shadow-xl` oder `shadow-2xl` für Tiefe.
- **Borders**: Subtile Ränder (`border-slate-200/50`) lassen Boxen hochwertiger
  wirken.
- **Typography**: Setze `tracking-tight` für Überschriften und `leading-relaxed`
  für Fließtext.

## 🚀 Phase 4: Interaktion (Micro-Animations)

- **Transitions**: Jede Zustandsänderung braucht
  `transition-all duration-300 ease-in-out`.
- **Hover-Effekte**: Nutze `hover:scale-[1.02]`, `hover:shadow-indigo-500/10`
  oder leichte Farbverschiebungen.
- **Buttons**: Verwende die standardisierten utilities `btn-primary` und
  `btn-danger` für konsistente Abstände, Schatten und Fokus-States.
- **Focus Rings**: Alle interaktiven Elemente müssen den violetten Standard-Ring
  besitzen (`focus-visible:outline-brand-primary`).

## ♿ Phase 5: Barrierefreiheit (BTHG/ARIA) & Datenschutz (DSGVO)

- **A11y (ARIA)**: Nutze `aria-hidden="true"` für dekorative Icons und
  `aria-label` für Icon-Buttons. Jeder Button braucht einen Fokus-State (z.B.
  `focus-visible:outline-brand-primary`).
- **Kontraste**: Halte dich an WCAG 2.1 AA Kontraste (mindestens 4.5:1), auch im
  Dark Mode.
- **Privacy by Design**: Binde externe Scripts/Fonts nie über CDNs ein (lokal
  via LibMan) und erhebe nur absolut notwendige Nutzerdaten.

## 🛡️ Excellence Checklist

- [ ] Wirkt die UI "atmet" (genug Whitespace)?
- [ ] Sind alle Abstände symmetrisch?
- [ ] Funktionieren alle Hover-Zustände flüssig?
- [ ] Funktioniert alles responsiv (Mobile-First Check)?
- [ ] Ist das Design im Dark Mode ebenfalls exzellent?
- [ ] Sind alle klickbaren Elemente per Tastatur erreichbar und haben klare
      Fokus-Styles (A11y)?
- [ ] Sind alle Schriften und Icons datenschutzkonform lokal eingebunden
      (DSGVO)?

> **Tipp:** Konsultiere für verbindliche Styling-Regeln immer den detaillierten
> `tailwind_css_styleguide.md`, `accessibility_a11y.md`, `privacy_dsgvo.md` und
> `font_awesome_styleguide.md` unter `.agents/rules/`.

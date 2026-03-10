# Skill: Visual Excellence Workflow (Premium UI)

Schritt-für-Schritt Anleitung zur Erstellung von "Wow"-Interfaces mit Tailwind CSS 4.2.

## 🏁 Phase 1: Struktur & Layout
- **Semantik**: Nutze HTML5 Elemente (`main`, `section`, `article`, `header`).
- **Layout**: Bevorzuge `grid` für komplexe Layouts und `flex` für Komponenten-Innereien.
- **Spacing**: Nutze konsistente Abstände (z.B. `p-6`, `gap-4`).

## 🎨 Phase 2: Design-Sprache
- **Farben**: Vermeide harte Kontraste (reines Schwarz). Nutze `slate-900` für Text und `slate-50` für Hintergründe.
- **Gradients**: Setze dezente Verläufe ein: `bg-linear-to-br from-white to-slate-100`.
- **Glassmorphism**: Für Overlays oder Karten: `bg-white/70 backdrop-blur-xl border border-white/20`.

## ✨ Phase 3: Premium Polishing
- **Shadows**: Nutze `shadow-xl` oder `shadow-2xl` für Tiefe.
- **Borders**: Subtile Ränder (`border-slate-200/50`) lassen Boxen hochwertiger wirken.
- **Typography**: Setze `tracking-tight` für Überschriften und `leading-relaxed` für Fließtext.

## 🚀 Phase 4: Interaktion (Micro-Animations)
- **Transitions**: Jede Zustandsänderung braucht `transition-all duration-300 ease-in-out`.
- **Hover-Effekte**: Nutze `hover:scale-[1.02]`, `hover:shadow-indigo-500/10` oder leichte Farbverschiebungen.
- **Buttons**: Ein Premium-Button hat einen leichten `inner-shadow` und eine subtile Animation beim Klicken.

## 🛡️ Excellence Checklist
- [ ] Wirkt die UI "atmet" (genug Whitespace)?
- [ ] Sind alle Abstände symmetrisch?
- [ ] Funktionieren alle Hover-Zustände flüssig?
- [ ] Ist das Design im Dark Mode ebenfalls exzellent?

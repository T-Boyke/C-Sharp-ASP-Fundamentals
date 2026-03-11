# IHK Excellence Checklist

Die ultimative Checkliste für jedes Feature und jeden Commit.

## 🛡️ Architektur & Clean Code
- [ ] Logik ist in der Domain/Service Layer, nicht im Controller.
- [ ] Jede Klasse hat eine einzige Verantwortung (SRP).
- [ ] SOLID-Prinzipien werden konsequent angewendet.
- [ ] "Code Smells" wurden durch Refactoring (Boy Scout Rule) minimiert.

## 📝 Dokumentation & Standards
- [ ] JEDER öffentliche Member hat ein deutsches XML-Summary.
- [ ] Fachbegriffe werden präzise und konsistent verwendet.
- [ ] Die `README.md` der Unit ist aktuell und aussagekräftig.

## 🧪 Testing & Qualität
- [ ] Unit-Tests decken alle logischen Pfade ab (100% Coverage Ziel).
- [ ] Alle Tests folgen dem AAA-Pattern.
- [ ] Benennung der Tests ist sprechend (`Unit_State_Expected`).

- [ ] UI nutzt Tailwind 4.2 Best Practices (Gradients, Glassmorphism).
- [ ] Alle Assets (Fonts, Icons, CSS) werden **lokal** (ohne CDN) ausgeliefert.
- [ ] Asset-Management erfolgt **Node-frei** (via LibMan & Tailwind.MSBuild).
- [ ] Interaktionen sind durch Micro-Animations (Transitions) flüssig.
- [ ] Das Layout ist responsiv und barrierefrei.

## ⚙️ Git & Workflow
- [ ] Commit ist atomar und folgt den Convention-Regeln.
- [ ] Alle Tests sind grün, bevor der Commit erfolgt.

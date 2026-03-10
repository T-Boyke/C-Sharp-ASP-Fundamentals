# Skill: Refactoring (Boy Scout Rule)

"Hinterlasse den Code immer ein Stück besser, als du ihn vorgefunden hast."

## 🕵️ Code-Smells erkennen
- **Long Method**: Methoden über 20-30 Zeilen sollten gesplittet werden.
- **Large Class**: Eine Klasse mit zu vielen Verantwortlichkeiten (SRP-Verletzung).
- **Primitive Obsession**: Nutze Value Objects statt einfacher Datentypen (z.B. `Email` statt `string`).
- **Feature Envy**: Eine Methode nutzt fast nur Daten einer anderen Klasse.

## 🛠️ Refactoring-Aktionen
1. **Methoden extrahieren**: Teile Logik in kleine, gut benannte Methoden auf.
2. **Namen verbessern**: Nutze präzise Fachbegriffe aus dem DDD-Kontext.
3. **Komplexität reduzieren**: Nutze Guard-Clauses (`if (item == null) return;`) statt tief verschachtelter If-Blöcke.
4. **Dokumentation ergänzen**: Fehlende XML-Kommentare nach IHK-Standard hinzufügen.

## ⚖️ Die Balance finden
- Refactore nur den Bereich, an dem du gerade arbeitest.
- **Sicherheit geht vor**: Jedes Refactoring wird durch vorhandene Tests abgesichert (TDD).
- Ein Refactoring darf die Funktionalität niemals verändern.

## 🛡️ Boy Scout Checkliste
- [ ] Ist der Code jetzt lesbarer als vorher?
- [ ] Wurden redundante Stellen entfernt (DRY)?
- [ ] Sind alle Tests nach dem Refactoring noch grün?

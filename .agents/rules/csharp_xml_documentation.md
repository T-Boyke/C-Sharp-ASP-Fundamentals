# C# XML-Dokumentationsstandard (IHK-Konform)

Um exzellente Code-Qualität und IHK-Konformität zu erreichen, muss jeder öffentliche Member (Klassen, Methoden, Eigenschaften) detailliert auf **Deutsch** dokumentiert werden.

## Grundprinzipien

- **Sprache**: Deutsch (Präzise, professionell und verständlich).
- **Zielgruppe**: IHK-Prüfer (Technischer Anspruch) und Anfänger (Erklärender Charakter).
- **Vollständigkeit**: Jedes `<summary>`, `<param>` und `<returns>` muss ausgefüllt sein.

## Erforderliche Tags

### `<summary>`

Beschreibt *was* die Komponente macht. Vermeide triviale Aussagen wie "Setzt den Namen". Nutze stattdessen: "Initialisiert den Namen des Patienten und validiert die Eingabe auf Korrektheit."

### `<param name="xxx">`

Beschreibt den Zweck des Parameters, nicht nur den Typ.

*Gut*: `Der eindeutige Identifikator des Benutzers.`
*Schlecht*: `Die ID.`

### `<returns>`

Beschreibt den Rückgabewert und ggf. den Zustand bei Erfolg/Fehler.

*Gut*: `Gibt die Liste aller aktiven Termine zurück. Wenn keine Termine vorhanden sind, wird eine leere Liste zurückgegeben.`

### `<exception cref="xxx">`

Muss dokumentiert werden, wenn die Methode gezielt Exceptions wirft.

## Beispiel für Exzellenz

```csharp
/// <summary>
/// Berechnet den Gesamtpreis der Einkaufsliste inklusive der gesetzlichen Mehrwertsteuer.
/// </summary>
/// <param name="rabattProzent">Der anzuwendende Rabatt in Prozent (0-100).</param>
/// <returns>Der berechnete Brutto-Gesamtbetrag als Dezimalwert.</returns>
/// <exception cref="ArgumentOutOfRangeException">Wird geworfen, wenn der Rabatt kleiner als 0 oder größer als 100 ist.</exception>
public decimal BerechneGesamtpreis(decimal rabattProzent)
{
    // Logik...
}
```

## Best Practices für "IHK-Glück"

1. **Kontext liefern**: Erkläre kurz das *Warum*, wenn es nicht offensichtlich ist.
2. **Fachbegriffe nutzen**: Verwende korrekte deutsche Fachbegriffe (z.B. "Instanziierung", "Kapselung", "Zeichenfolge").
3. **Lesbarkeit**: Nutze `<para>` für längere Beschreibungen, um Absätze zu bilden.
4. **Beispiele**: Nutze `<example>` für komplexe Logik, um Anfängern den Einstieg zu erleichtern.

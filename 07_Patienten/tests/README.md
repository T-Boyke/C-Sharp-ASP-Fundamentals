# 🛡️ MedCare - Qualitätssicherung (/tests)

Willkommen in der Test-Abteilung! Hier stellen wir sicher, dass die App niemals lügt oder abstürzt. Wir nutzen **xUnit** und **Moq**, um alles auf Herz und Nieren zu prüfen.

---

## 🧪 Warum testen wir?
Stell dir vor, du änderst etwas am Speichern-Knopf und merkst erst Wochen später, dass alle neuen Patienten gelöscht werden. Ein Albtraum! 😱
Unsere Tests verhindern das. Sie laufen in Sekunden und sagen dir sofort: "Alles OK" oder "Hier ist ein Fehler!".

---

## 🔍 Was wird genau getestet? (100% Coverage)

1.  **Domain Tests**: Wir prüfen, ob ein Patient z.B. sein Alter richtig berechnet.
2.  **Infrastructure Tests**: Wir simulieren eine Datenbank im Arbeitsspeicher (**InMemory**), um zu schauen, ob das Speichern und Laden wirklich funktioniert.
3.  **Controller Tests**: Wir tun so, als ob wir ein Browser wären, und klicken virtuell alle Buttons durch. Dabei nutzen wir **Moq**, um die echten Datenbanken durch "Dummies" zu ersetzen – so sind die Tests extrem schnell.

---

## 🏃 So startest du die Tests
Du musst kein Profi sein. Öffne einfach ein Terminal in diesem Ordner und tippe:

```bash
dotnet test
```

Visual Studio zeigt dir die Ergebnisse auch im "Test Explorer" mit grünen Häkchen an. Wenn alles grün ist, kannst du ruhig schlafen! ✅

---

> [!TIP]
> Echte Profis schreiben erst den Test und dann den Code (**TDD**). Probier es mal aus!

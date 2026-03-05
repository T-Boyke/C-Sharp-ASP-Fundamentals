# 🏥 Unit 07: Patienten-Management-System (MedCare)

Willkommen zur **Unit 07**. In diesem Modul bauen wir ein professionelles Patienten-Management-System ("MedCare"). Dieses Projekt ist das "Meisterstück" der ersten Module und kombiniert alles, was du bisher gelernt hast, in einer sauberen Architektur.

---

## 👨‍🏫 Was ist das hier? (Für absolute Anfänger)
Stell dir vor, du arbeitest in einer Arztpraxis. Du brauchst ein Programm, um Patienten zu speichern, ihre Daten (Name, Geburtstag, SVNR) zu verwalten und ihre Untersuchungen (Befunde) zu dokumentieren. Genau das macht diese App!

### Die wichtigsten Regeln, die hier genutzt werden:
1. **DDD (Domain Driven Design)**: Das Programm ist wie ein Burger geschichtet. Die Mitte (das Fleisch) sind die Daten (Patienten). Außenrum sind die Schichten, die die Daten speichern (Datenbank) oder anzeigen (Webseite).
2. **SOLID & Clean Code**: Wir schreiben den Code so ordentlich, dass ihn auch ein "Dummy" (oder ein Kollege in 6 Monaten) sofort versteht. Keine Abkürzungen, klare Namen!
3. **100% Tests**: Wir haben für alles automatische Tests geschrieben. Wenn wir etwas kaputt machen, sagt uns das Programm sofort Bescheid.

---

## 🏗️ Wie ist das Projekt aufgebaut?
Das Projekt besteht aus drei großen Teilen:

1.  **[Domäne (Kernelemente)](./src/07_Patienten/Models)**: Hier liegen `Patient.cs` und `Examination.cs`. Das sind die Baupläne für unsere Daten. Ein Patient hat einen Namen, ein Geburtsdatum und viele Untersuchungen.
2.  **[Webseite (UI)](./src/07_Patienten/Views)**: Hier nutzen wir **Tailwind CSS 4**. Das sorgt dafür, dass die Seite modern und "premium" aussieht – wie bei einer teuren Privatpraxis! ✨
3.  **[Datenbank (Speicher)](./src/07_Patienten/Infrastructure)**: Wir nutzen EF Core (Code-First). Das bedeutet: Wir schreiben C#-Code, und das Programm baut die Datenbank von ganz alleine.

---

## 📂 Wo finde ich was?
*   [**/src**](./src/README.md): Hier lebt der eigentliche Programmcode. Wenn du wissen willst, wie die Webseite funktioniert, schau hier rein.
*   [**/tests**](./tests/README.md): Hier sind die "Eichmeister". Sie prüfen, ob alles richtig rechnet und speichert.
*   [**/docs**](./docs/07_Patienten_UML_Architecture.md): Hier gibt es schicke Diagramme (UML), die zeigen, wie alles zusammenhängt.

---

## 🚀 Schnellstart: So startest du die App
Keine Angst vor der Technik! Folge einfach diesen Schritten:

1.  Öffne das Projekt in **Visual Studio 2022** (oder neuer).
2.  Drücke den **grünen Play-Button** oben (oder `F5`).
3.  Die Datenbank wird beim ersten Start automatisch erstellt und mit **23 Test-Patienten** (inkl. Tobias Boyke!) gefüllt.
4.  **Fertig!** Du kannst jetzt Patienten suchen, neue anlegen oder Befunde schreiben.

---

## 🛡️ Qualität & Sicherheit
*   **Asynchroner Code**: Die App "friert" nie ein, weil sie alle schweren Aufgaben im Hintergrund erledigt (`async/await`).
*   **Validierung**: Das Programm passt auf, dass du keine Quatsch-Daten eingibst (z.B. ein Geburtsdatum in der Zukunft oder eine ungültige SVNR).
*   **Premium Design**: Keine langweiligen grauen Buttons. Alles ist animiert, responsive und sieht auf dem Handy super aus.

---
> [!TIP]
> Schau dir die **UML-Diagramme** in `/docs` an, wenn du den Überblick verlierst. Sie sind die Landkarte dieses Projekts! 🗺️

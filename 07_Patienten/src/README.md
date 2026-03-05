# 🛠️ MedCare - Source Code Innenleben (/src)

Dies ist das Herzstück der Anwendung. Hier passiert die ganze Magie! Wenn du verstehen willst, wie die Webseite die Daten verarbeitet, bist du hier richtig.

---

## 🏗️ Die Architektur (Der "Drei-Schichten-Burger")

Wir teilen den Code in drei klare Bereiche auf, damit alles ordentlich bleibt:

### 1. Domain (Der Kern)
Hier liegen die **Entities** (Patient, Untersuchung). Das sind einfache Klassen, die beschreiben, welche Daten wir speichern wollen. 
*   **Wichtig**: Hier gibt es keine Technik, nur Logik!

### 2. Infrastructure (Die Werkzeuge)
Hier kümmert sich EF Core um die Datenbank. 
*   **AppDbContext**: Der Übersetzer zwischen C# und SQL.
*   **DbSeeder**: Unser "Gärtner", der beim ersten Start automatisch 23 Test-Patienten pflanzt, damit die App nicht leer aussieht.

### 3. Presentation (Das Gesicht)
Hier liegen die **Controller** und **Views**.
*   **Controllers**: Die Verkehrspolizisten. Sie nehmen Anfragen vom Browser entgegen und sagen dem System, was zu tun ist.
*   **Views**: Die HTML-Seiten. Dank **Tailwind CSS 4** sehen sie modern aus und passen sich jeder Bildschirmgröße an.

---

## 🎨 Frontend Highlights
*   **SFC (Single File Components)**: Wir nutzen CSS Isolation. Das bedeutet, das Design einer Seite stört niemals das Design einer anderen Seite.
*   **SFC Tailwind**: Wir haben Tailwind direkt in die Komponenten integriert – für maximale Pflegbarkeit.
*   **Icons**: Wir nutzen Font Awesome 7 für intuitive Symbole (z.B. eine Lupe für die Suche).

---

## 🚀 So arbeitest du am Code
Wenn du eine Änderung machst:
1.  Ändere das Model (falls nötig).
2.  Pass den Controller an.
3.  Aktualisiere die View.
4.  **Wichtig**: Nutze `dotnet run` im Terminal, um die App im Browser zu sehen (meist unter `http://localhost:5000`).

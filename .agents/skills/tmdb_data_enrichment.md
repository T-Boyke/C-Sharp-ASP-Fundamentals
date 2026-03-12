# Skill: TMDB Data Enrichment

Pattern für die Echtzeit-Anreicherung von lokalen Film- und Personendaten via TMDB (The Movie Database).

## 🏁 Phase 1: Service Integration

- **ITmdbService**: Nutze das Interface in der Application-Schicht zur Kapselung der API-Aufrufe.
- **TMDbLib**: Nutze diese Bibliothek für typsicheren Zugriff auf Movie, Person und Credit Daten.
- **Caching**: Externe Daten sollten zur Performance-Steigerung kurzzeitig gecacht werden (z.B. `IMemoryCache`).

## 🛠️ Phase 2: Perfect Alignment Schema

- **Entity Mapping**: Lokale Entitäten (`Film`, `Person`) müssen die entsprechenden TMDB-Felder (z.B. `TmdbId`, `VoteAverage`, `BackdropUrl`) besitzen.
- **Property Priority**: Behandle externe Daten als "Vorschläge", die vom Nutzer im Frontend überschrieben werden können.

## 🔄 Phase 3: Smart Sync Workflow

- **Lookup Logic**: Beim Speichern/Anlegen immer zuerst prüfen, ob eine Entität mit der `TmdbId` bereits existiert.
- **Many-to-Many Sync (Cast)**:
  1. Hole Credits für einen Film.
  2. Synchronisiere Personen-Stammblätter (Lookup by `TmdbId`).
  3. Erstelle Verknüpfungen in der Join-Tabelle (`PersonEigenschaftFilm`) mit der Rolle "Actor".

## 🎨 Phase 4: Premium UI Integration

- **Echtzeit-Suche**: Implementiere eine debounced API-Suche (`/api/tmdb/search`), die dem Nutzer Vorschaubilder und Metadaten anzeigt.
- **Auto-Fill**: Nutze Vanilla JS, um Formularfelder nach der Auswahl eines Films automatisch zu füllen.
- **Visual Feedback**: Nutze CSS-Animationen (z.B. `animate-pulse`), wenn Daten geladen oder in das Formular übernommen werden.

## 🛡️ Excellence Checklist

- [ ] Sind alle externen IDs optional (`nullable`) im Schema?
- [ ] Werden Personen-Duplikate durch den Smart Sync verhindert?
- [ ] Ist die API-Key Handhabung sicher (User Secrets/Environment)?
- [ ] Wirkt der Import-Prozess für den Nutzer "magisch" und schnell?

> **Tipp:** Siehe `FilmController.cs` (Create-Action) für eine Referenz-Implementierung des Smart Sync Patterns.

# Blazor Component Rules

Richtlinien für skalierbare und performante Blazor-UIs.

## 🏗️ Struktur & Trennung

- **Partial Classes**: Trenne Markup (`.razor`) von der Logik (`.razor.cs`). Das
  erhöht die Übersichtlichkeit enorm.
- **Komponentengröße**: Halte Komponenten klein und spezialisiert. Nutze
  Unterkomponenten für wiederkehrende Teile.

## 🚀 Parameters & State

- **Parameter-Validierung**: Prüfe Parameter in `OnParametersSet` oder nutze
  `[EditorRequired]`.
- **EventCallback**: Nutze `EventCallback` für die Kommunikation nach oben (vom
  Kind zum Elternteil).
- **CascadingParameters**: Setze sie sparsam ein, bevorzugt für globale Zustände
  (z.B. CurrentUser, Theme).

## 🎨 Rendering & Performance

- **ShouldRender**: Optimiere das Re-Rendering bei komplexen Komponenten.
- **Dispose**: Implementiere `IDisposable`, wenn Abonnements oder Ressourcen
  bereinigt werden müssen.

## 🛡️ Excellence Checklist

- [ ] Ist die Logik sauber vom Markup getrennt?
- [ ] Werden Parameter korrekt validiert?
- [ ] Sind die Komponenten klein und wiederverwendbar?
- [ ] Wurden CSS-Komponenten (OOCSS/Tailwind 4) korrekt angewendet?

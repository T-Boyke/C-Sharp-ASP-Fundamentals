# Tailwind CSS 4 Styleguide - The Ultimate Reference

Dieses Dokument ist die **vollumfängliche, verbindliche Referenz** für den Einsatz von Tailwind CSS v4 in unserem ASP.NET Core MVC-Projekt ("Filmdatenbank"). Es basiert direkt auf der offiziellen Tailwind v4 Dokumentation und ist optimiert für unsere "Node-Free" Architektur mit Visual Studio, LibMan und `Tailwind.MSBuild`.

---

## 1. Core Concepts & Configuration (v4)
Tailwind v4 ist vollständig CSS-gesteuert. Es gibt keine `tailwind.config.js` mehr.
*   **Initialization**: `@import "tailwindcss";` am Anfang der `input.css`.
*   **Theme Customization**: Nutze die `@theme`-Direktive.
    ```css
    @theme {
      --color-brand-primary: #7c3aed; /* Violet 600 */
      --color-brand-bg: #fafafa;
    }
    ```
*   **Preflight**: Automatisch inkludiert (setzt Margins/Paddings zurück, standardisiert Borders etc.).
*   **Custom Utilities**: Statt Plugins nutzt v4 `@utility`.
    ```css
    @utility card-premium { @apply bg-white border border-slate-200 rounded-xl shadow-sm; }
    ```
*   **Detection**: Klassen werden via MSBuild-Integration (`Tailwind.MSBuild`) oder über die Tailwind CLI automatisch im Projekt (`.cshtml` Dateien) erkannt.

---

## 2. Responsive Design (`sm`, `md`, `lg`, `xl`, `2xl`)
*   **Mobile First**: Verwende Basisklassen ohne Prefix für Mobile. Füge Breakpoints für größere Bildschirme hinzu (z.B. `text-center md:text-left`).
*   **Ranges & Max-Breakpoints**: In v4 kannst du Ranges (`md:max-lg:flex-col`) oder exklusive Max-Breakpoints (`max-md:hidden`) nutzen.
*   **Container Queries**: Verwende `@container` auf einem Parent (z.B. Card). Reagiere innen mit `@sm:flex` oder `@md:grid`, basierend auf der *Breite des Containers*, nicht des Viewports!

---

## 3. Hover, Focus & Other States
*   **Pseudo-Klassen**: Kette Modifier logisch: `hover:bg-brand-hover focus-visible:outline focus-visible:outline-2 focus-visible:outline-brand-primary`.
*   **Group States**: Setze `group` auf den Parent und `group-hover:text-white` auf das Child, um Hover-Effekte zu synchronisieren.
*   **Peer States**: Setze `peer` auf ein Input (z.B. Checkbox) und `peer-checked:bg-blue-500` auf ein nachfolgendes Element (Label).
*   **Dark Mode**: Das Theme regelt den Dark Mode (`dark:` Prefix). Standardmäßig OS-basiert, anpassbar auf HTML-Klassen-Basis (`class="dark"`).

---

## 4. Layout
*   **aspect-ratio**: Nutze `aspect-video` (16:9) für Filmtrailer, `aspect-square` (1:1) für Bilder/Avatare.
*   **columns**: Für mehrspaltigen Text (`columns-2`, `columns-3`), nicht für Cards (dafür Grid nutzen). Nutze `break-inside-avoid`.
*   **box-decoration-break**: `box-decoration-clone`, um Backgrounds/Borders über Zeilenumbrüche hinweg zu duplizieren.
*   **box-sizing**: Bleibt `box-border` (Preflight Standard).
*   **display**: `flex`, `grid` strukturieren fast alles. `hidden` (display: none) und `block` regeln Responsivität.
*   **float / clear**: *Modernes CSS vermeidet Float.* Nutze Flexbox!
*   **isolation**: `isolate` kreiert einen neuen Stacking-Context (wichtig für komplexe Überlagerungen ohne z-index-Klimmzüge).
*   **object-fit / position**: Image-Tags immer mit `object-cover` und `object-center` (oder `object-top`), damit Bilder nicht verzerren.
*   **overflow**: `overflow-hidden` für Cards mit Bildern (schneidet Ecken ab). `overflow-x-auto` für horizontale Slider.
*   **overscroll-behavior**: `overscroll-contain` in Scrollboxen (z.B. Modals), damit die Hauptseite im Hintergrund nicht weiterscrollt.
*   **position**: `relative` für Parents, `absolute` für Overlays. `sticky top-0 z-50` für die Navbar.
*   **z-index**: Nutze logische Layer: `z-10` (Floating Bgs), `z-40` (Modals/Backdrops), `z-50` (Header/Navs).

---

## 5. Flexbox & Grid
*   **Flex-Direction / Wrap**: `flex-col` für Card-Flow. `flex-wrap` für Tags/Badges.
*   **Flex Sizing**: `flex-1` (füllt Platz), `shrink-0` (verhindert quetschen von Icons).
*   **Order**: Verändere die Reihenfolge bei Mobile (z.B. `order-last md:order-1`).
*   **Grid Template**: `grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3` ist das Standard-Layout für die `Film/Index.cshtml`.
*   **Grid Placement**: `col-span-2` oder `row-span-2` für markante Hero-Objekte.
*   **Gap**: Immer Spacing via `gap-*` (z.B. `gap-6`), vermeide Margins in Flex/Grid-Containern!
*   **Justify & Align**: `justify-between items-center` ist der Goldstandard für Header/Listen. Bevorzuge bei Grid `place-items-center` für absolute Zentrierung.

---

## 6. Spacing
*   **padding**: `p-4` bis `p-8` für Card-Innenabstände. `px-4 sm:px-6 lg:px-8` für Main-Container (Max-Width-Wrappers).
*   **margin**: Nutze `mx-auto` zum Zentrieren. `mb-8` für Abstände unter Section-Headern.
*   **Logical Properties**: Erlaube LTR/RTL Adaptionen (z.B. `ms-4` statt `ml-4`), im aktuellen Projekt primär auf Standard-Margins beschränkt.

---

## 7. Sizing
*   **width / height**: `w-full` für fließende Layouts. Absolute Werte (`w-12 h-12`) für Avatar-Container.
*   **min/max**: `max-w-7xl` dominiert das Layout. `min-h-screen` erzwingt, dass der Footer immer am unteren Bildschirmrand bleibt.

---

## 8. Typography
*   **font-family**: `font-sans` greift bei uns auf "Noto Sans Variable" zu.
*   **font-size / weight**: `text-sm`, `text-base`, `text-3xl font-black` (für Hero-Titles).
*   **font-smoothing**: Standardmäßig via Preflight in Tailwind aktiv (`antialiased`).
*   **letter-spacing**: `tracking-tight` für fette Headlines. `tracking-widest capitalize` für SUP-Headlines (Metadaten).
*   **line-height**: `leading-relaxed` (Lesetexte), `leading-none` (Kompakte Titles/Icons).
*   **Text Manipulation**: `truncate` (schneidet bei 1 Zeile ab). `line-clamp-3` (schneidet nach 3 Zeilen ab).
*   **Text Wrap**: V4 Highlight! Verwende `text-balance` auf Headlines, um unschöne Einzelwörter am Zeilenende ("Widows") zu verhindern.
*   **hyphens**: `hyphens-auto` (Worttrennung) für Text-Spalten nützlich, erfordert `lang="de"`.

---

## 9. Backgrounds
*   **background-color**: `bg-brand-bg` (`#fafafa`) als Base. `bg-brand-soft` für Layer.
*   **background-image / gradient**: Nutze `bg-linear-to-r` gefolgt von `from-[color] via-[color] to-[color]` für moderne Verläufe.
*   **background-size / position**: `bg-cover bg-center` für Images.

---

## 10. Borders
*   **border-radius**: `rounded-full` für Badges. `rounded-2xl` für große Layer. `rounded-lg` für Inputs.
*   **border-width / color**: Dezent halten! `border border-slate-200`.
*   **outline**: WICHTIG für Accessibility. `focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-brand-primary`. Keine reinen `outline-none`!

---

## 11. Effects (Schatten, Glassmorphism, Masks)
*   **box-shadow**: `shadow-sm` als Baseline, `shadow-lg` bei Hover-Zuständen. Nutze farbige Schatten für CTA-Buttons (`shadow-brand-hover/20`).
*   **opacity**: `opacity-75` für Disabled-States.
*   **mix-blend-mode**: Für kreative Hero-Bilder, wo sich die Bildfarbe mit einem lila Hintergrund mischt (`mix-blend-multiply`).
*   **mask-image**: Einsetzen (via CSS), wenn z.B. eine Hintergrundgrafik in Richtung des Contents "ausfaden" soll.

---

## 12. Filters
*   **backdrop-filter**: Kern der *Glassmorphism* Ästhetik. Nutze `bg-white/70 backdrop-blur-md border-white/20`.
*   **blur / grayscale**: `blur-sm` auf Hintergrund-Images lassen Text poppen. `grayscale` plus `hover:grayscale-0 transition-all` ist ein Premium-Effekt für Film-Kataloge.

---

## 13. Tables
*   Die klassische Tabellendarstellung erfordert `w-full text-left border-collapse`.
*   Rows (`tr`) erhalten `border-b border-slate-100` und oft `hover:bg-slate-50`.
*   *Anmerkung:* Für Enduser-Metriken bevorzugen wir Card-Grids!

---

## 14. Transitions, Animation & Transforms
*   **transition**: Auf interaktiven Elementen *immer* `transition-all duration-300 ease-in-out` einsetzen!
*   **animation**: `animate-pulse` für Skeleton Loaders (falls wir async nachladen), `animate-spin` für Loading-Icons (`fa-spinner`).
*   **Transforms**: `hover:scale-105` (z.B. Film-Card Hover). `hover:-translate-y-1` (Kacheln, die aufpoppen).
*   **origin**: `origin-center` etc., definiert den Ankkerpunkt für Rotationen (z.B. Dropdown Arrows `rotate-180`).

---

## 15. Interactivity
*   **cursor**: `cursor-pointer`, wenn Klicks implementiert sind. `cursor-not-allowed` für Disabled.
*   **pointer-events**: `pointer-events-none` auf Icons (`<i class="...">`), damit sie den Klick an den darunterliegenden Button/Link weitergeben.
*   **accent-color**: `accent-brand-primary` färbt die nativen Browser-Checkboxen Lila ein.
*   **scroll-behavior**: `scroll-smooth` am `html`-Tag global setzen (via `input.css`).
*   **user-select**: `select-none` auf reinen Deko-Badges oder Buttons, um hässliches Blau-Markieren zu verhindern.

---

## 16. SVG
*   **fill / stroke**: Arbeite vorzugsweise mit Font Awesome, dort greift einfach `text-*`. Bei Custom-SVGs `fill-current` / `stroke-current` nutzen, um Textfarben an Pfade zu vererben.

---

## 17. Accessibility (A11y)
*   **Screen Readers**: Blende dekorative Elemente via `aria-hidden="true"` aus. Biete visuell versteckten, aber lesbaren Text für Screenreader an: `sr-only` (z.B. Label eines Icon-only Buttons).
*   **color-adjust**: Tailwind `forced-color-adjust-none` sparsam verwenden, primär wenn OS-High-Contrast Themes das Branding einer Fläche stark zerstören würden.

---

## 18. Tailwind Plus & Tailark (Marketing / Application UI / E-Commerce)
Wenn wir fertige UI-Blöcke (Tailark, TailwindUI) adaptieren:
1.  **Blockauswahl**: Ob "Product Application UI", "Marketing" oder "E-Commerce" - die HTML-Strukturen und Utility-Klassen werden 1:1 in Razor View `.cshtml` Dateien übernommen.
2.  **Node-Free / No-React**:
    *   JS Framework Attribute (`className`, `state`, `{...props}`) entfernen oder in C#/HTML übersetzen.
    *   Lucide Icons durch `<i class="fa-solid fa-[icon]"></i>` ersetzen.
    *   Interaktivität (Mobile Menu Toggle, Tabs) mit winzigen Vanilla-JS Scripten direkt in der View implementieren, da kein React/Vue Backend existiert.
3.  **Color-Mapping**: Achte darauf, dass neutrale Variablen aus Tailark an unsere CSS-Variablen in `theme.css` oder Custom-Klassen (`bg-brand-primary`) angepasst werden.

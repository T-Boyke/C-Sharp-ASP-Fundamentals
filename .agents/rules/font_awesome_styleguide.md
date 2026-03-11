# Font Awesome Styleguide

Dieses Dokument regelt den Einsatz von **Font Awesome (Free)** Icons in unseren
ASP.NET Core MVC-Projekten. Da wir die **Web Fonts + CSS** Methode verwenden
(kein SVG+JS), stehen bestimmte SVG-exklusive Features (wie Masking oder
Layering via JS) nicht zur Verfügung, können aber durch CSS-Tricks oder Tailwind
ersetzt werden.

## 1. Basis-Einsatz

Alle Icons werden über ein `<i>`-Tag mit den entsprechenden Tool-Klassen
eingebunden.

- **Solid Icons (Standard)**: `<i class="fa-solid fa-[icon-name]"></i>`
- **Regular Icons**: `<i class="fa-regular fa-[icon-name]"></i>`
- **Brands Icons**: `<i class="fa-brands fa-[brand-name]"></i>`

## 2. Größe anpassen (Sizing)

Vermeide es, Icon-Größen mit rohem CSS (`font-size`) hart zu codieren. Nutze die
integrierten FA-Klassen oder Tailwind.

- **Relative Größen**: `fa-xs`, `fa-sm`, `fa-lg`, `fa-xl`, `fa-2xl`. Die Icons
  skalieren relativ zum umliegenden Text.
- **Absolute Multiplikatoren**: `fa-1x`, `fa-2x`, `fa-[...]-10x`. Nützlich für
  große Icons in Hero-Headern oder Fallback-Screens.
- _Tipp:_ Wenn du exakte Tailwind-Größen brauchst, kombiniere es:
  `<i class="fa-solid fa-user text-3xl"></i>`.

## 3. Ausrichtung & Layout

- **Fixed Width (`fa-fw`)**: **MUSS** verwendet werden, wenn Icons horizontal
  zentriert in einer vertikalen Liste (z.B. Navigation, Dropdowns) aufgereiht
  sind! `<i class="fa-solid fa-envelope fa-fw"></i>`
- **Listen (`fa-ul` & `fa-li`)**: Ersetze langweilige Listenpunkte durch Icons.

  ```html
  <ul class="fa-ul">
    <li>
      <span class="fa-li"><i class="fa-solid fa-check text-green-500"></i></span
      >Eintrag 1
    </li>
  </ul>
  ```

- **Border & Pull**: Nutze `fa-border`, um einen schnellen Rahmen um das Icon zu
  ziehen (besser: Tailwind `border rounded p-2`). Nutze `fa-pull-left` oder
  `fa-pull-right`, um Quotes oder Teaser mit umfließendem Text zu erstellen.

## 4. Animationen

Nutze Bewegung, um Nutzer-Feedback zu geben (Vermeide Übernutzung!).

- **Lade-Icons**: `<i class="fa-solid fa-spinner fa-spin"></i>`. Alternativ:
  `fa-spin-pulse` für einen schrittweisen Dreheffekt (8 Steps).
- **Wackeln / Schütteln**: `fa-shake` für Fehlermeldungen in Formularen.
- **Hüpfen**: `fa-bounce` für Notifications (z.B. Glocken-Icon bei neuen
  Nachrichten).
- **Flippen**: `fa-flip` für 3D-Karten-Effekte.
- **Klopfen**: `fa-beat` oder `fa-beat-fade` (Herz-Icon favorisieren).

## 5. Drehung & Spiegelung (Rotate / Flip)

- **Rotieren**: `fa-rotate-90`, `fa-rotate-180`, `fa-rotate-270`. Gut, um aus
  einem normalen Pfeil (`fa-arrow-right`) einen nach unten zeigenden Pfeil zu
  machen, ohne ein neues Icon zu suchen.
- **Spiegeln**: `fa-flip-horizontal`, `fa-flip-vertical`, `fa-flip-both`.

## 6. Stacking (Symbole übereinanderlegen)

Nützlich, um z.B. aus einer Kamera und einem Verbots-Schild ein "Keine Fotos"
Schild zu bauen.

```html
<span class="fa-stack fa-lg">
  <i class="fa-solid fa-camera fa-stack-1x"></i>
  <i class="fa-solid fa-ban fa-stack-2x text-red-500"></i>
</span>
```

## 7. Zusammenarbeit mit Tailwind CSS

- **Färben**: Nutze Tailwind Text-Utilities:
  `<i class="fa-solid fa-star text-yellow-400"></i>`.
- **Hover-Effekte**: Setze die Transition auf das `i` oder den Parent:
  `<a class="group"><i class="fa-solid fa-arrow-right group-hover:translate-x-1 transition-transform"></i></a>`.
- **Verwenden ohne i-Tag**: Vermeide den Einsatz via Pseudo-Elementen
  (`::before { content: "\f007" }`), wenn sich das Icon via HTML `<i ...>`
  einfügen lässt. Falls doch nötig, müssen Font-Family
  (`font-family: "Font Awesome 6 Free"; font-weight: 900;`) korrekt per CSS
  geladen werden.

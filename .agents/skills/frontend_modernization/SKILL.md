---
name: Frontend Modernization (Node-Free)
description: Guide for converting CDN-based apps to local Tailwind 4.2, Font Awesome 7.2, and Noto Sans Variable without Node.js.
---

# Skill: Frontend Modernization (Node-Free)

Guide for implementing a professional, local-only frontend stack in .NET 10 without requiring Node.js or npm, achieving visual excellence ("The WOW Factor") without "AI Slop".

## 1. Prerequisites (Tools)

Ensure the following tools are available:

- **LibMan CLI**: `dotnet tool install -g Microsoft.Web.LibraryManager.Cli`
- **Tailwind.MSBuild**: NuGet package (v2.0.2+) for automated CSS compilation.

## 2. Local Asset Setup (LibMan)

Configure `libman.json` to download fonts (Noto Sans Variable) and icons (Font Awesome) from `unpkg`.

```json
{
  "version": "1.0",
  "defaultProvider": "unpkg",
  "libraries": [
    {
      "library": "@fortawesome/fontawesome-free@7.2.0",
      "destination": "wwwroot/lib/font-awesome",
      "files": [
        "css/all.min.css",
        "webfonts/fa-solid-900.woff2",
        "webfonts/fa-brands-400.woff2",
        "webfonts/fa-regular-400.woff2"
      ]
    },
    {
      "library": "@fontsource-variable/noto-sans@5.1.1",
      "destination": "wwwroot/lib/noto-sans/",
      "files": [
        "index.css",
        "files/noto-sans-latin-wght-normal.woff2"
      ]
    }
  ]
}
```

## 3. Tailwind CSS 4 Integration (CSS-First)

1. **Add NuGet Package**:
   ```xml
   <PackageReference Include="Tailwind.MSBuild" Version="2.0.2">
     <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
     <PrivateAssets>all</PrivateAssets>
   </PackageReference>
   ```

2. **Configure Properties in `.csproj`**:
   ```xml
   <PropertyGroup>
     <TailwindCSSInputFile>wwwroot/css/input.css</TailwindCSSInputFile>
     <TailwindCSSOutputFile>wwwroot/css/site.css</TailwindCSSOutputFile>
   </PropertyGroup>
   ```

3. **Single File Component (SFC) CSS Architecture**:
   Organize CSS into components to keep `input.css` clean.

   - `wwwroot/css/components/theme.css`: Core design tokens (`@theme`).
   - `wwwroot/css/components/btn.css`: Button utilities (`@utility`).
   - `wwwroot/css/components/card.css`: Card utilities (`@utility`).

   **Main Input CSS (`wwwroot/css/input.css`)**:
   ```css
   @import "tailwindcss";

   /* Theme and Base */
   @import "./components/theme.css";

   /* Components */
   @import "./components/btn.css";
   @import "./components/card.css";
   ```

   **Theme Component (`wwwroot/css/components/theme.css`)**:
   ```css
   @theme {
     --font-sans: "Noto Sans Variable", ui-sans-serif, system-ui;
     --color-brand-primary: #7c3aed; /* Professional Purple */
     --color-brand-bg: #fafafa;
   }
   ```

## 4. Integration into ASP.NET Core (`_Layout.cshtml`)

Replace CDNs with local references. Ensure Noto Sans is loaded correctly:

```html
<head>
    <!-- Local Fonts -->
    <link rel="stylesheet" href="~/lib/noto-sans/index.css" />
    <!-- Local Icons -->
    <link rel="stylesheet" href="~/lib/font-awesome/css/all.min.css" />
    <!-- Tailwind Generated CSS -->
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
</head>
<body class="bg-brand-bg text-slate-900 font-sans antialiased">
```

## 5. Tailark / UI Blocks Integration

When integrating external premium UI blocks (like Tailark):
- Copy HTML and Tailwind classes verbatim.
- Remove React specific tags (`className`, `{...props}`).
- Translate `Lucide` icons to Font Awesome tags (`<i class="fa-solid fa-*"></i>`).
- Handle interaction states (e.g., mobile menu toggle) with standard Razor View conditionals or Vanilla JS.

> For comprehensive styling rules, always refer to `.agents/rules/tailwind_css_styleguide.md` and `.agents/rules/font_awesome_styleguide.md`.

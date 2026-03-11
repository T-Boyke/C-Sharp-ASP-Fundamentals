---
name: Frontend Modernization (Node-Free)
description: Guide for converting CDN-based apps to local Tailwind 4.2, Font Awesome 7.2, and Google Sans Flex without Node.js.
---

# Skill: Frontend Modernization (Node-Free)

Guide for implementing a professional, local-only frontend stack in .NET 10 without requiring Node.js or npm.

## 1. Prerequisites (Tools)

Ensure the following tools are available:

- **LibMan CLI**: `dotnet tool install -g Microsoft.Web.LibraryManager.Cli`
- **Tailwind.MSBuild**: NuGet package (v2.0.2+) for automated CSS compilation.

## 2. Local Asset Setup (LibMan)

Configure `libman.json` to download fonts and icons from `unpkg`.

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
        "webfonts/fa-brands-400.woff2"
      ]
    },
    {
      "library": "@fontsource-variable/google-sans-flex@latest",
      "destination": "wwwroot/lib/google-sans-flex/",
      "files": [
        "index.css",
        "files/google-sans-flex-latin-wght-normal.woff2"
      ]
    }
  ]
}
```

## 3. Tailwind CSS 4.2 Integration

1. **Add NuGet Package**:

   ```xml
   <PackageReference Include="Tailwind.MSBuild" Version="2.0.2">
     <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
     <PrivateAssets>all</PrivateAssets>
   </PackageReference>
   ```

2. **Configure Properties**:

   ```xml
   <PropertyGroup>
     <TailwindCSSInputFile>wwwroot/css/input.css</TailwindCSSInputFile>
     <TailwindCSSOutputFile>wwwroot/css/site.css</TailwindCSSOutputFile>
   </PropertyGroup>
   ```

3. **Define Input CSS** (`wwwroot/css/input.css`):

   ```css
   @import "tailwindcss";

   @theme {
     --font-sans: "Google Sans Flex", ui-sans-serif, system-ui;
   }

   @font-face {
     font-family: "Google Sans Flex";
     src: url("../lib/google-sans-flex/files/google-sans-flex-latin-wght-normal.woff2") format("woff2");
     font-weight: 100 900;
     font-style: normal;
     font-display: swap;
   }
   ```

## 4. Layout Integration (`_Layout.cshtml`)

Replace CDNs with local references:

```html
<link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
<link rel="stylesheet" href="~/lib/font-awesome/css/all.min.css" />
<link rel="stylesheet" href="~/lib/google-sans-flex/index.css" />
<style>
    body { font-family: "Google Sans Flex", sans-serif; }
</style>
```

## 5. Build & Verify

- Run `dotnet build`.
- Verify `wwwroot/css/site.css` is generated.
- Check browser Network tab: **Zero external requests** (all from localhost).

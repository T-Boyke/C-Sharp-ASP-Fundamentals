# 🛠️ 04_ShoppingList - Source Code (/src)

This directory contains the primary ASP.NET Core 10 MVC application "Einkaufsliste".

## 🏗️ Technical Overview
- **Framework**: .NET 10 MVC
- **Design System**: Tailwind CSS 4.2 (OOCSS, Utility-First) & FontAwesome 7.2
- **Namespace**: `_04_ShoppingList`
- **Frontend Architecture**: Ausgelagerte Tailwind-Styling-Module unter `/wwwroot/css/modules/` für sauberes Separation of Concerns.

> [!TIP]
> **Tailwind v4 CDN Integration:** Wenn Tailwind v4 per Browser-CDN (`@tailwindcss/browser@4`) genutzt wird, müssen externe CSS-Module (`@theme`, `@utility`) als `<link type="text/tailwindcss" href="...">` integriert werden. Razor-`@import` Direktiven im `<style>` Block werden vom Caching-System des CDNs ignoriert.

## 📐 Architektur & Datenfluss
```mermaid
flowchart LR
    %% Styling
    classDef view fill:#eff6ff,stroke:#3b82f6,stroke-width:1px
    classDef action fill:#f5f3ff,stroke:#8b5cf6,stroke-width:1px
    classDef db fill:#ecfdf5,stroke:#10b981,stroke-width:1px

    %% Controller Actions
    idx["[GET] Index()"]:::action
    formGet["[GET] ArtikelForm()"]:::action
    formPost["[POST] ArtikelForm()"]:::action
    sehen["[GET] ArtikelAnsehen()"]:::action

    %% Views
    v_idx["Index.cshtml"]:::view
    v_form["ArtikelForm.cshtml"]:::view
    v_angelegt["Angelegt.cshtml"]:::view
    v_sehen["ArtikelAnsehen.cshtml"]:::view

    %% Database
    Database[("💾 Repository")]:::db

    %% Flow
    idx --> v_idx
    v_idx -- "Neu" --> formGet
    v_idx -- "Liste" --> sehen
    formGet --> v_form
    v_form -- "Speichern" --> formPost
    formPost -- "Save()" --> Database
    formPost --> v_angelegt
    sehen -- "Read()" --> Database
    sehen --> v_sehen
```

> 💡 **Erweiterte Diagramme:** Das detaillierte Layer-Architektur-Zusammenspiel finden Sie in der [Architekturdokumentation](../docs/Architektur_Einkaufsliste.md).

## 🚀 How to Run
From this directory:
```bash
dotnet run
```


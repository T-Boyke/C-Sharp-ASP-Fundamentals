# 🛠️ 04_ShoppingList - Source Code (/src)

This directory contains the primary ASP.NET Core 10 MVC application "Einkaufsliste".

## 🏗️ Technical Overview
- **Framework**: .NET 10 MVC
- **Design System**: Tailwind CSS 4.2 (OOCSS, Utility-First) & FontAwesome 7.2
- **Namespace**: `_04_ShoppingList`
- **Frontend Architecture**: Lokales Hosting via LibMan (`wwwroot/lib/`) für volle Offline-Fähigkeit. Strikte "Single File Component" (SFC) Architektur durch Verwendung von ASP.NET Core CSS Isolation (`.cshtml.css`). Aufgeräumt durch globale Atomic-Komponenten (`btn.css` etc.) und eigene Tag Helper.

> [!TIP]
> **Tailwind v4 Local Architecture:** Da das `@tailwindcss/browser@4` Skript lokal eingebunden ist, werden die Seiten clientseitig blitzschnell gerendert. Komplexe Utility-Ketten wurden in `.cshtml.css`-Dateien (`@apply`) ausgelagert, um den HTML-Code (Separation of Concerns) extrem übersichtlich zu halten.

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
    sehen["[GET] ArtikelAnsehen(search)"]:::action
    editGet["[GET] ArtikelBearbeiten(id)"]:::action
    editPost["[POST] ArtikelBearbeiten(Position)"]:::action
    deletePost["[POST] Loeschen(id)"]:::action

    %% Views
    v_idx["Index.cshtml"]:::view
    v_form["ArtikelForm.cshtml"]:::view
    v_angelegt["Angelegt.cshtml"]:::view
    v_sehen["ArtikelAnsehen.cshtml"]:::view
    v_edit["ArtikelBearbeiten.cshtml"]:::view

    %% Database
    Database[("💾 IShoppingListRepository")]:::db

    %% Flow
    idx --> v_idx
    v_idx -- "Neu" --> formGet
    v_idx -- "Liste" --> sehen
    formGet --> v_form
    v_form -- "Speichern" --> formPost
    formPost -- "Add()" --> Database
    formPost --> v_angelegt
    sehen -- "Read() / Find()" --> Database
    sehen --> v_sehen
    v_sehen -- "Editieren" --> editGet
    editGet --> v_edit
    v_edit -- "Speichern" --> editPost
    editPost -- "Update()" --> Database
    editPost --> sehen
    v_sehen -- "Löschen" --> deletePost
    deletePost -- "Delete()" --> Database
    deletePost --> sehen
```

> 💡 **Erweiterte Diagramme:** Das detaillierte Layer-Architektur-Zusammenspiel finden Sie in der [Architekturdokumentation](../docs/Architektur_Einkaufsliste.md).

## 🚀 How to Run
From this directory:
```bash
dotnet run
```


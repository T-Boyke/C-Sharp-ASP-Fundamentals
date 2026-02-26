# 🛠️ 04_ShoppingList - Source Code (/src)

This directory contains the primary ASP.NET Core 10 MVC application "Einkaufsliste".

## 🏗️ Technical Overview
- **Framework**: .NET 10 MVC
- **Design System**: Tailwind CSS 4.2 (OOCSS, Utility-First) & FontAwesome 7.2
- **Namespace**: `_04_ShoppingList`
- **Frontend Architecture**: Ausgelagerte Tailwind-Styling-Module unter `/wwwroot/css/modules/` für sauberes Separation of Concerns.

## 📐 Architektur & Datenfluss
```mermaid
graph TD
    subgraph HomeController [HomeController]
        IndexAction[Index Action<br/>GET /] 
        ArtikelFormAction[ArtikelForm Action<br/>GET /Home/ArtikelForm]
        ArtikelFormPostAction[ArtikelForm Action<br/>POST /Home/ArtikelForm]
        AngelegtAction[Angelegt Action<br/>GET /Home/Angelegt]
        ArtikelAnsehenAction[ArtikelAnsehen Action<br/>GET /Home/ArtikelAnsehen]
    end

    subgraph Views [Views]
        IndexView[Index.cshtml]
        ArtikelFormView[ArtikelForm.cshtml]
        AngelegtView[Angelegt.cshtml]
        ArtikelAnsehenView[ArtikelAnsehen.cshtml]
    end
    
    subgraph Models [Models]
        PositionModel[Position.cs]
        RepositoryClass[Repository.cs]
    end

    IndexAction --> IndexView
    ArtikelFormAction --> ArtikelFormView
    ArtikelFormPostAction -->|Saves| RepositoryClass
    ArtikelFormPostAction --> AngelegtView
    ArtikelAnsehenAction -->|Reads| RepositoryClass
    ArtikelAnsehenAction --> ArtikelAnsehenView
```

## 🚀 How to Run
From this directory:
```bash
dotnet run
```


# 🚀 04_ShoppingList: ASP.NET Erste eigene App - Einkaufsliste

Welcome to 04_ShoppingList. This unit is part of the C# ASP.NET Core 10 fundamentals series.

## 🎯 Learning Objectives
- [ ] Implement core concepts for this unit (MVC, Model Binding, Routing).
- [ ] Master the architectural patterns demonstrated (Layering, View-Controller decoupling).
- [ ] Maintain 100% test coverage for new logic (TDD with xUnit).
- [ ] Follow SOLID, Clean Architecture, and DRY/KISS guidelines.

## 📈 Projektauftrag (IHK Prüfungsrelevant)
Erstellung einer ASP.NET Core-Anwendung zur Verwaltung einer Einkaufsliste. Die Anwendung speichert zu besorgende Artikel aus verschiedenen Geschäften.

### Kernanforderungen (Features)
- **Model**: Eine Klasse `Position` zur Speicherung von Artikelname, Anzahl und Geschäft.
- **Repository Pattern**: Implementierung eines `IShoppingListRepository` als In-Memory Store via Dependency Injection (Singleton). Dies gewährleistet das Dependency Inversion Principle.
- **Views**: `Index.cshtml`, `ArtikelForm.cshtml`, `Angelegt.cshtml`, `ArtikelAnsehen.cshtml` und `ArtikelBearbeiten.cshtml`. Aufgeräumt durch Partial Views.
- **Controller**: Ein `HomeController`, der den Ablauf inkl. vollständigem CRUD und Suche steuert.
- **Frontend-Architektur**: Local Hosting via LibMan von `Tailwind CSS 4.2` und `FontAwesome 7.2` für volle Offline-Fähigkeit. Strikte "Single File Component" (SFC) Architektur durch Verwendung von ASP.NET Core CSS Isolation (`.cshtml.css`) kombiniert mit globalen Atomic-Komponenten (`btn.css` etc.).
- **Tag Helpers**: Eigener `IconTagHelper` zur Reduzierung von HTML-Boilerplate (`<fa-icon name="plus" />`).

### Architekturübersicht
```mermaid
flowchart TD
    classDef ui fill:#3b82f6,stroke:#1d4ed8,stroke-width:2px,color:#fff
    classDef controller fill:#8b5cf6,stroke:#6d28d9,stroke-width:2px,color:#fff
    classDef data fill:#10b981,stroke:#047857,stroke-width:2px,color:#fff

    User((👤 Client / Browser))

    subgraph Presentation ["Presentation Layer (Views)"]
        UI["CSS Module (Tailwind 4.2)<br>Razor Views (.cshtml)"]:::ui
    end

    subgraph Application ["Application Layer (Controller)"]
        HC["HomeController.cs<br>(Routing & Actions)"]:::controller
    end

    subgraph Domain ["Domain Layer (Models)"]
        PM["Position.cs<br>(Entity)"]:::data
        IRepo<<"IShoppingListRepository.cs<br>(Interface)">>:::data
        Repo[("Repository.cs<br>(In-Memory Store)")]:::data
    end

    User <--> Presentation
    Presentation <--> Application
    Application --> PM
    Application <--> IRepo
    IRepo <.. Repo : Implements
```

> 💡 **Erweiterte Diagramme:** Detaillierte Sequenz- und Datenfluss-Diagramme finden Sie in der [Architekturdokumentation](./docs/Architektur_Einkaufsliste.md).

## 📂 Folder Structure
- [**/src**](./src/README.md): Application source code and implementation details.
- [**/tests**](./tests/README.md): Automated testing suite and validation logic.

---
> [!IMPORTANT]
> Every unit in this repository is designed to be a standalone, fully testable component. Ensure you check both the source and the tests to understand the full context.


# Architektur Einkaufsliste

Hier ist die detaillierte architektonische Dokumentation der MVC-Implementierung für die Einkaufsliste (ASP.NET Core). Um maximale Übersichtlichkeit und Vollständigkeit nach IHK-Standards zu gewährleisten, ist die Dokumentation in vier spezifische UML-Diagramme unterteilt:

- **Blockdiagramm** (Architektur-Schichten vs. Separation of Concerns)
- **Ablauf- und Architekturdiagramm** (Data Flow / Navigation)
- **Klassendiagramm** (Domain Model)
- **Use-Case-Diagramm** (Anwendungsfälle)

## 1. Blockdiagramm (Schichten-Architektur)

Dieses Diagramm zeigt die strikte Trennung (Separation of Concerns) zwischen UI, Steuerung und Datenhaltung.

```mermaid
flowchart TD
    %% Styling
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
    Application --> IRepo
    IRepo <.. Repo : Implements
```

## 2. Ablauf- und Architekturdiagramm (Data Flow)

Dieses Diagramm verdeutlicht den konkreten Datenfluss und die Navigation zwischen den Views und Controller-Actions.

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

## 3. Klassendiagramm (Domain Layer)

Veranschaulicht die zugrundeliegende Datenstruktur (`Position`) und den Singleton-In-Memory-Speicher (`Repository`).

```mermaid
classDiagram
    direction TB
    class Position {
        +Guid Id
        +string Name
        +int Anzahl
        +string Geschaeft
    }
    
    class IShoppingListRepository {
        <<interface>>
        +IEnumerable~Position~ Positions$
        +AddResponse(Position position)$ void
        +GetById(Guid id)$ Position?
        +Update(Position position)$ bool
        +Delete(Guid id)$ bool
        +Clear()$ void
    }

    class Repository {
        -List~Position~ _positions$
        +IEnumerable~Position~ Positions$
        +AddResponse(Position position)$ void
        +GetById(Guid id)$ Position?
        +Update(Position position)$ bool
        +Delete(Guid id)$ bool
        +Clear()$ void
    }
    
    IShoppingListRepository <|.. Repository : Implements
    Repository "1" *-- "*" Position : Contains
```

## 4. Use-Case-Diagramm (Applikationsnutzer)

Definiert die grundlegenden Geschäftsprozesse, die der Nutzer ausführen darf (CRUD auf Item-Ebene).

```mermaid
flowchart LR
    User((👤 Anwender))
    
    subgraph System ["Einkaufsliste System"]
        UC1([Position hinzufügen])
        UC2([Liste ansehen])
        UC3([Position suchen/filtern])
        UC4([Position bearbeiten])
        UC5([Position löschen])
    end
    
    User --> UC1
    User --> UC2
    User --> UC3
    User --> UC4
    User --> UC5
```

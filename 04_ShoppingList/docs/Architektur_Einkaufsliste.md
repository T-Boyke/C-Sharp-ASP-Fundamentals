# Architektur Einkaufsliste

Hier ist die vereinfachte grafische Übersicht der MVC-Architektur für die Einkaufsliste (ASP.NET Core). Um maximale Übersichtlichkeit zu gewährleisten, ist die Darstellung in zwei Diagramme unterteilt: Ein grobes **Blockdiagramm** (Komponenten & Schichten) und ein präzises **Ablaufdiagramm** (Data Flow).

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
        Repo[("Repository.cs<br>(In-Memory Store)")]:::data
    end

    User <--> Presentation
    Presentation <--> Application
    Application --> PM
    Application <--> Repo
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

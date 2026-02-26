# Architektur Einkaufsliste

Hier ist die grafische Übersicht der MVC-Architektur für die Einkaufsliste (ASP.NET Core).

```mermaid
graph TD
    %% Controllers & Actions
    subgraph HomeController [HomeController (Controller)]
        IndexAction[Index Action<br/>GET /] 
        ArtikelFormAction[ArtikelForm Action<br/>GET /Home/ArtikelForm]
        ArtikelFormPostAction[ArtikelForm Action<br/>POST /Home/ArtikelForm]
        AngelegtAction[Angelegt Action<br/>GET /Home/Angelegt]
        ArtikelAnsehenAction[ArtikelAnsehen Action<br/>GET /Home/ArtikelAnsehen]
    end

    %% Views
    subgraph Views [Views (UI)]
        IndexView[Index.cshtml<br/>Startseite]
        ArtikelFormView[ArtikelForm.cshtml<br/>Neuen Artikel anlegen]
        AngelegtView[Angelegt.cshtml<br/>Bestätigung & Anzahl]
        ArtikelAnsehenView[ArtikelAnsehen.cshtml<br/>Alle Artikel Liste]
    end
    
    %% Models
    subgraph Models [Models (Daten)]
        PositionModel[Position.cs<br/>Name, Anzahl, Geschäft]
        RepositoryClass[Repository.cs<br/>static List&lt;Position&gt;]
    end

    %% Flow: User to Controller
    User((User)) -->|Ruft App auf| IndexAction
    
    %% Controller to Views
    IndexAction -->|Gibt View zurück| IndexView
    
    %% View interactions
    IndexView -->|Klick 'Neuen Artikel hinzufügen'| ArtikelFormAction
    IndexView -->|Klick 'Artikel ansehen'| ArtikelAnsehenAction
    
    ArtikelFormAction -->|Gibt View zurück| ArtikelFormView
    
    ArtikelFormView -->|Formular Absenden (Post)| ArtikelFormPostAction
    ArtikelFormView -->|Klick 'Zurück'| IndexAction
    
    ArtikelFormPostAction -->|Speichert neue Position| RepositoryClass
    ArtikelFormPostAction -->|Gibt View zurück| AngelegtView
    
    AngelegtView -->|Klick 'Weitere Artikel anlegen'| ArtikelFormAction
    AngelegtView -->|Klick 'Zurück zur Startseite'| IndexAction
    
    ArtikelAnsehenAction -->|Liest Liste| RepositoryClass
    ArtikelAnsehenAction -->|Gibt View zurück| ArtikelAnsehenView
    
    ArtikelAnsehenView -->|Klick 'Zurück zur Startseite'| IndexAction
```

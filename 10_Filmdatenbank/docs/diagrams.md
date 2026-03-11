# 📊 FilmDB - Technical Documentation (Diagrams)

This document provides a visual overview of the `10_Filmdatenbank` architecture using Mermaid diagrams.

---

## 1. ERD (Entity Relationship Diagram)

The database schema follows the 3rd Normal Form (3NF) to manage movies, persons, and their roles efficiently.

```mermaid
erDiagram
    FILM ||--o{ PERSON_EIGENSCHAFT_FILM : has
    PERSON ||--o{ PERSON_EIGENSCHAFT_FILM : acts_in
    EIGENSCHAFT ||--o{ PERSON_EIGENSCHAFT_FILM : defines_role

    FILM {
        int FilmID PK
        string Titel
        int Erscheinungsjahr
        int Spieldauer
        decimal Preis
    }

    PERSON {
        int PersonID PK
        string Vorname
        string Nachname
    }

    EIGENSCHAFT {
        int EigenschaftID PK
        string Bezeichnung
    }

    PERSON_EIGENSCHAFT_FILM {
        int PEFID PK
        int PersonID FK
        int FilmID FK
        int EigenschaftID FK
    }
```

---

## 2. UML Use Case Diagram

Describes the interactions between different user roles and the system.

```mermaid
graph LR
    subgraph Users
        G((Guest))
        M((Member))
        A((Admin))
    end

    subgraph "Film Management System"
        UC1(Browse Films)
        UC2(View Film Details)
        UC3(Register/Login)
        UC4(Search Films)
        UC5(CRUD Movies)
        UC6(Manage Permissions)
    end

    G --> UC1
    G --> UC3
    M --> UC1
    M --> UC2
    M --> UC3
    M --> UC4
    A --> UC1
    A --> UC2
    A --> UC3
    A --> UC4
    A --> UC5
    A --> UC6
```

---

## 3. UML Class Diagram

Shows the Domain Entities and their relationships within the `_10_Filmdatenbank.Domain` layer.

```mermaid
classDiagram
    class Film {
        +int FilmID
        +string Titel
        +int Erscheinungsjahr
        +int Spieldauer
        +decimal Preis
        +ICollection~PersonEigenschaftFilm~ PersonEigenschaftFilme
    }

    class Person {
        +int PersonID
        +string Vorname
        +string Nachname
        +ICollection~PersonEigenschaftFilm~ PersonEigenschaftFilme
    }

    class Eigenschaft {
        +int EigenschaftID
        +string Bezeichnung
        +ICollection~PersonEigenschaftFilm~ PersonEigenschaftFilme
    }

    class PersonEigenschaftFilm {
        +int PEFID
        +int PersonID
        +Person Person
        +int FilmID
        +Film Film
        +int EigenschaftID
        +Eigenschaft Eigenschaft
    }

    Film "1" --o "*" PersonEigenschaftFilm
    Person "1" --o "*" PersonEigenschaftFilm
    Eigenschaft "1" --o "*" PersonEigenschaftFilm
```

---

## 4. UML Sequence Diagram (Add Movie Flow)

Illustrates the interaction between the Web UI, Controller, and Database when an Admin adds a new film.

```mermaid
sequenceDiagram
    actor Admin
    participant View as Web Browser
    participant Controller as FilmController
    participant DB as ApplicationDbContext

    Admin->>View: Enter Movie Data
    View->>Controller: POST /Film/Create
    Note over Controller: Validate ModelState
    Controller->>DB: Add(film)
    Controller->>DB: SaveChangesAsync()
    DB-->>Controller: Success
    Controller-->>View: Redirect to Index
    View-->>Admin: Show Updated List
```

---

## 5. UML Activity Diagram (Application Flow)

Shows the logical flow of a user navigating the application with authentication checks.

```mermaid
flowchart TD
    Start([Start]) --> OpenApp[User opens App]
    OpenApp --> Auth{Is Authenticated?}
    Auth -- Yes --> Dashboard[Show Dashboard]
    Dashboard --> Browse[User Browses Films]
    Browse --> AdminCheck{Is Admin?}
    AdminCheck -- Yes --> AdminUI[Enable Edit/Delete Buttons]
    AdminCheck -- No --> MemberUI[Show Only Details]
    Auth -- No --> Redirect[Redirect to Login]
    Redirect --> Credentials[User enters Credentials]
    Credentials --> LoginCheck{Login Successful?}
    LoginCheck -- Yes --> Dashboard
    LoginCheck -- No --> Error[Show Error]
    Error --> Credentials
    AdminUI --> Stop([Stop])
    MemberUI --> Stop
```

---

## 6. UML State Diagram (Auth State)

Visualizes the different states of a user session.

```mermaid
stateDiagram-v2
    [*] --> Guest
    Guest --> LoginProcessing : Enter Credentials
    LoginProcessing --> AuthMember : Success (Member)
    LoginProcessing --> AuthAdmin : Success (Admin)
    LoginProcessing --> Guest : Failure
    
    AuthMember --> Guest : Logout
    AuthAdmin --> Guest : Logout
    
    state AuthMember {
        [*] --> Browsing
        Browsing --> DetailedView
    }
    
    state AuthAdmin {
        [*] --> Managing
        Managing --> Creating
        Managing --> Editing
        Managing --> Deleting
    }
```

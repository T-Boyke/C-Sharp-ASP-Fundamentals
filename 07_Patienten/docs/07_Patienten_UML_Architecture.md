# 📊 Unit 07: UML & System-Architektur

Dieses Dokument visualisiert die Architektur und die Abläufe der **MedCare Patientenverwaltung** mittels Mermaid.js.

## 1. Klassendiagramm (Struktur)
Zeigt die Domänen-Entitäten, Repositories und deren Beziehungen.

```mermaid
classDiagram
    class Patient {
        +int Id
        +string Firstname
        +string Lastname
        +DateTime Birthdate
        +string SocialSecurityNumber
        +int Age
        +string Fullname
        +List~Examination~ Examinations
    }

    class Examination {
        +int Id
        +DateTime Date
        +string Findings
        +int PatientId
        +Patient Patient
    }

    class IPatientRepository {
        <<interface>>
        +GetAllAsync() Task~IEnumerable~
        +GetByIdAsync(int id) Task~Patient~
        +AddAsync(Patient p) Task
    }

    class PatientRepository {
        -AppDbContext _context
        +AddAsync(Patient p) Task
    }

    Patient "1" --> "*" Examination : besitzt
    IPatientRepository <|.. PatientRepository : implementiert
    PatientRepository --> Patient : verwaltet
```

## 2. Use Case Diagramm (Funktionalität)
Beschreibt die Interaktion der Praxis-Mitarbeiter mit dem System.

```mermaid
graph LR
    Staff((Arzt / Personal))
    
    subgraph MedCare [MedCare Patientenverwaltung]
        UC1(Patient aufnehmen)
        UC2(Patientenliste einsehen)
        UC3(Befund dokumentieren)
        UC4(Akte löschen)
    end
    
    Staff --> UC1
    Staff --> UC2
    Staff --> UC3
    Staff --> UC4
```

## 3. Sequenzdiagramm (Ablauf: Befund erfassen)
Visualisiert den asynchronen Datenfluss beim Speichern eines neuen Befunds.

```mermaid
sequenceDiagram
    participant User as Browser / Frontend
    participant Ctrl as ExaminationsController
    participant Repo as ExaminationRepository
    participant DB as SQL Server (EF Core)

    User->>Ctrl: POST /Examinations/Create (Data)
    Ctrl->>Ctrl: Validate Model
    alt Valid
        Ctrl->>Repo: AddAsync(examination)
        Repo->>DB: SaveChangesAsync()
        DB-->>Repo: Success
        Repo-->>Ctrl: Task Complete
        Ctrl->>User: Redirect to PatientDetails
    else Invalid
        Ctrl->>User: Return View(Data) with Errors
    end
```

## 4. Zustandsdiagramm (Patienten-Status)
Zeigt den Lebenszyklus eines Patienten im System.

```mermaid
stateDiagram-v2
    [*] --> Neuaufnahme: Register
    Neuaufnahme --> InBehandlung: Erster Befund
    InBehandlung --> InBehandlung: Weiterer Befund
    InBehandlung --> Archiviert: Lösch-Vorbereitung
    Archiviert --> [*]: Endgültig gelöscht
    InBehandlung --> [*]: Schnelle Löschung
```

## 5. Aktivitätsdiagramm (Prozess: Untersuchung)
Detaillierter Prozess für das Personal während einer Untersuchung.

```mermaid
flowchart TD
    Start([Start]) --> Search[Suche Patient in Liste]
    Search --> Found{Gefunden?}
    Found -- Ja --> Open[Akte öffnen]
    Found -- Nein --> Create[Neuen Patienten anlegen]
    
    Open --> Form[Befund-Formular öffnen]
    Create --> Form
    
    Form --> Input[Untersuchung eingeben]
    Input --> Valid{Valide?}
    
    Valid -- Ja --> Save[Speichern & Festschreiben]
    Valid -- Nein --> Error[Fehler korrigieren]
    Error --> Input
    
    Save --> Redirect[Redirect zur Akte]
    Redirect --> End([Ende])
```

## 6. Entity Relationship Diagram (Datenbank)
Das physische Datenmodell (Code-First Schema).

```mermaid
erDiagram
    PATIENT ||--o{ EXAMINATION : "hat viele"
    PATIENT {
        int Id PK
        string Firstname
        string Lastname
        datetime Birthdate
        string SVNR
    }
    EXAMINATION {
        int Id PK
        int PatientId FK
        datetime Date
        string Findings
    }
```

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
useCaseDiagram
    actor "Arzt / Personal" as Staff
    
    package MedCare {
        usecase "Patient aufnehmen" as UC1
        usecase "Patientenliste einsehen" as UC2
        usecase "Befund dokumentieren" as UC3
        usecase "Akte löschen" as UC4
    }
    
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
activityDiagram
    start
    :Suche Patient in Liste;
    if (Patient gefunden?) then (Ja)
        :Akte öffnen;
    else (Nein)
        :Neuen Patienten anlegen;
    endif
    :Befund-Formular öffnen;
    :Ergebnisse der Untersuchung eintragen;
    if (Eingaben valide?) then (Ja)
        :Befund festschreiben (Speichern);
    else (Nein)
        :Fehler korrigieren;
        stop
    endif
    :Redirect zur Akte;
    stop
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

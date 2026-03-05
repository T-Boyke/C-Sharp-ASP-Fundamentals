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
        +bool IsPrivatePatient
        +DateTime? NextAppointmentDate
        +string Symptoms
        +List~Examination~ Examinations
        +List~Medication~ Medications
    }

    class Examination {
        +int Id
        +DateTime Date
        +string Findings
        +int PatientId
        +Patient Patient
    }

    class Medication {
        +int Id
        +string Name
        +string Dosage
        +string Pzn
        +int PatientId
    }

    class IPatientRepository {
        <<interface>>
        +GetAllAsync() Task~IEnumerable~
        +GetByIdAsync(int id) Task~Patient~
        +AddAsync(Patient p) Task
    }

    class IMedicationLookupService {
        <<interface>>
        +SearchByPznAsync(string pzn) Task
    }

    class PatientRepository {
        -AppDbContext _context
        +AddAsync(Patient p) Task
    }

    class MedicationLookupService {
        -HttpClient _httpClient
        +SearchByPznAsync(string pzn) Task
    }

    Patient "1" --> "*" Examination : besitzt
    Patient "1" --> "*" Medication : verschrieben
    IPatientRepository <|.. PatientRepository : implementiert
    IMedicationLookupService <|.. MedicationLookupService : implementiert
    PatientRepository --> Patient : verwaltet
```

## 2. Use Case Diagramm (Funktionalität)
Beschreibt die Interaktion der Praxis-Mitarbeiter mit dem System.

```mermaid
graph LR
    Staff((Arzt / Personal))
    Extern[Shop Apotheke / Live-PZN]
    
    subgraph MedCare [MedCare Patientenverwaltung]
        UC1(Patient aufnehmen)
        UC2(Patientenliste einsehen)
        UC3(Befund dokumentieren)
        UC4(Medikament via PZN suchen)
        UC5(Akte löschen)
    end
    
    Staff --> UC1
    Staff --> UC2
    Staff --> UC3
    Staff --> UC4
    UC4 -.-> Extern
    Staff --> UC5
```

## 3. Sequenzdiagramm (Ablauf: PZN Live-Check)
Visualisiert den asynchronen Datenfluss bei der Medikamenten-Suche.

```mermaid
sequenceDiagram
    participant User as Browser / Frontend
    participant Api as MedicationsApiController
    participant Service as MedicationLookupService
    participant Extern as Externes System (via PZN)

    User->>Api: GET /api/MedicationsApi/lookup/{pzn}
    Api->>Service: SearchByPznAsync(pzn)
    alt Mock found
        Service-->>Api: Return Mock Data
    else Scrape Live
        Service->>Extern: HTTP GET (PZN Search)
        Extern-->>Service: HTML Response
        Service->>Service: Parse Metadata
        Service-->>Api: Return Scraped Data
    end
    Api-->>User: JSON Response (Success/Fail)
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
    PATIENT ||--o{ MEDICATION : "bekommt viele"
    PATIENT {
        int Id PK
        string Firstname
        string Lastname
        datetime Birthdate
        string SVNR
        bool IsPrivatePatient
        datetime NextAppointmentDate
        string Symptoms
    }
    EXAMINATION {
        int Id PK
        int PatientId FK
        datetime Date
        string Findings
    }
    MEDICATION {
        int Id PK
        int PatientId FK
        string Name
        string Dosage
        string Pzn
        datetime PrescribedDate
    }
```

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
        +int? DoctorId
        +int? HealthInsuranceId
        +int? AddressId
        +int? ContactInfoId
        +List~Examination~ Examinations
        +List~Medication~ Medications
    }

    class Doctor {
        +int Id
        +string Title
        +string Firstname
        +string Lastname
        +string Fullname
    }

    class HealthInsurance {
        +int Id
        +string Name
    }

    class Address {
        +int Id
        +string Street
        +string HouseNumber
        +string ZipCode
        +string City
    }

    class ContactInfo {
        +int Id
        +string PhoneNumber
        +string Email
    }

    class Examination {
        +int Id
        +DateTime Date
        +string Findings
        +int PatientId
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
        +GetByIdWithExaminationsAsync(int id) Task~Patient~
        +AddAsync(Patient p) Task
    }

    class IMedicationLookupService {
        <<interface>>
        +SearchAsync(string query) Task~List~
    }

    Patient "*" --> "1" Doctor : behandelt von
    Patient "*" --> "1" HealthInsurance : versichert bei
    Patient "1" --> "1" Address : wohnt in
    Patient "1" --> "1" ContactInfo : erreichbar unter
    Patient "1" --> "*" Examination : besitzt
    Patient "1" --> "*" Medication : verschrieben
    IPatientRepository <|.. PatientRepository : implementiert
    IMedicationLookupService <|.. MedicationLookupService : implementiert
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
    participant DB as Mock-Datenbank (30 Medikamente)

    User->>Api: GET /api/MedicationsApi/search?q=ibupro
    Api->>Service: SearchAsync("ibupro")
    Service->>DB: Suche nach Name/PZN
    DB-->>Service: Treffer-Liste
    Service-->>Api: List~MedicationSearchResult~
    Api-->>User: JSON Response (Treffer)
    User->>User: Live-Preview anzeigen
    User->>User: Medikament auswählen → Formular füllen
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
    DOCTOR ||--o{ PATIENT : "behandelt"
    HEALTH_INSURANCE ||--o{ PATIENT : "versichert"
    ADDRESS ||--o{ PATIENT : "wohnt in"
    CONTACT_INFO ||--o{ PATIENT : "erreichbar unter"
    PATIENT ||--o{ EXAMINATION : "hat viele"
    PATIENT ||--o{ MEDICATION : "bekommt viele"

    DOCTOR {
        int Id PK
        string Title
        string Firstname
        string Lastname
    }
    HEALTH_INSURANCE {
        int Id PK
        string Name
    }
    ADDRESS {
        int Id PK
        string Street
        string HouseNumber
        string ZipCode
        string City
    }
    CONTACT_INFO {
        int Id PK
        string PhoneNumber
        string Email
    }
    PATIENT {
        int Id PK
        string Firstname
        string Lastname
        datetime Birthdate
        string SVNR
        bool IsPrivatePatient
        datetime NextAppointmentDate
        string Symptoms
        int DoctorId FK
        int HealthInsuranceId FK
        int AddressId FK
        int ContactInfoId FK
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

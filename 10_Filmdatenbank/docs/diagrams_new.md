# 🎥 FilmDB - Modernized Architecture (New Diagrams)

This document complements [diagrams.md](diagrams.md) and focuses on the newly implemented systems: **Deep Metadata Scraping**, **Commercial Integration**, and **Community Features**.

---

## 1. Augmented ERD (Metadata & Social)

The updated schema incorporates external scoring, watch providers, and our community/discussion subsystem.

```mermaid
erDiagram
    FILM ||--o{ METACRITIC_REVIEW : "scraped_scores"
    FILM ||--o{ WATCH_PROVIDER : "streamed_on"
    FILM ||--o{ EXTERNAL_RESOURCE : "links_to"
    FILM ||--o{ FILMLISTE : "contained_in"
    
    APPLICATION_USER ||--o{ FAN_GROUP : "belongs_to"
    FAN_GROUP ||--o{ DISCUSSION_THREAD : "contains"
    DISCUSSION_THREAD ||--o{ COMMENT : "has"
    
    FILM {
        int FilmID PK
        string Titel
        decimal Nutzerwertung
        string MetaScore
        string TvdbScore
        string OmdbScore
        long? Budget
        long? Revenue
        string CommercialNote
    }

    WATCH_PROVIDER {
        int ProviderID PK
        int FilmID FK
        string Name
        string Type
        string LogoUrl
    }

    METACRITIC_REVIEW {
        int ReviewID PK
        int FilmID FK
        string Author
        int Score
        string Summary
    }

    EXTERNAL_RESOURCE {
        int ResourceID PK
        int FilmID FK
        string Title
        string Url
        string Type
    }
```

---

## 2. Multi-Source Enrichment Sequence

Illustrates how the `FilmController` orchestrates data from **TMDB**, **TVDB**, **Metacritic**, and **Wikidata** to build a comprehensive profile.

```mermaid
sequenceDiagram
    participant App as Web Server
    participant TMDB as TMDb API
    participant TVDB as TVDB API (Scraped)
    participant MC as Metacritic (Scraped)
    participant WD as Wikidata API
    participant DB as SQL Database

    App->>TMDB: Fetch Basic Info & Credits
    TMDB-->>App: Core Data
    App->>TVDB: Get External IDs & Score
    TVDB-->>App: Secondary Identification
    App->>MC: Deep Scrape Reviews & Scores
    MC-->>App: Critical Reception Data
    App->>App: Normalize Scores (0-100)
    App->>WD: Fetch Awards & Bio
    WD-->>App: Cultural Context
    App->>DB: Persist Enriched Entity
    DB-->>App: Success
```

---

## 3. Scraper Logic Flow

Shows the logic used for the custom web scrapers that bypass standard API limitations.

```mermaid
flowchart TD
    Start([Sync Request]) --> Search[Search External Site]
    Search --> Match{Found Match?}
    Match -- No --> Log[Log Warning]
    Log --> End
    Match -- Yes --> Parse[Parse HTML Content]
    Parse --> Extraction{Extract Fields}
    Extraction --> Score[Normalized Score]
    Extraction --> Review[Top Reviews]
    Extraction --> Link[Commercial Links]
    Score & Review & Link --> Transform[Transform to Entity Model]
    Transform --> Save[(Update Database)]
    Save --> End([Complete])
```

---

## 4. Community Interaction Hierarchy

Visualizes the relationship between users, groups, and content.

```mermaid
graph TD
    User((User)) -- Member --> Group[Fan Group]
    Admin((Admin)) -- Manages --> Group
    Group -- Hosts --> Thread[Discussion Thread]
    Thread -- Contains --> Post[Comment]
    User -- Authors --> Post
    Post -- Replies To --> Post
    System{Notification System} -- Alerts --> User
    Thread -- Triggers --> System
```

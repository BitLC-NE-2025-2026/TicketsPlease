## 🗄️ Datenbankschema & Entities (ERD)

Die Datenbankstruktur (Entity Framework Core - Code First) ist streng relational und spiegelt die Bounded Contexts unseres Domain-Driven Designs (DDD) wider.

### Entity Relationship Diagram (Erster Entwurf)

```mermaid
erDiagram
    %% Identity & User Context
    USER {
        Guid Id PK
        string Username
        string Email
        string PasswordHash
        string FirstName
        string LastName
        string PhoneNumber
        string Street
        string City
        string ZipCode
        string Country
        datetime CreatedAt
    }
    
    ROLE {
        Guid Id PK
        string Name "e.g. Admin, User"
    }
    
    USER_ROLE {
        Guid UserId FK
        Guid RoleId FK
    }

    %% Team Context
    TEAM {
        Guid Id PK
        string Name
        string Description
        datetime CreatedAt
        Guid CreatedByUserId FK
    }

    TEAM_MEMBER {
        Guid TeamId FK
        Guid UserId FK
        datetime JoinedAt
    }

    %% Ticket Management Context
    TICKET {
        Guid Id PK
        string Title
        string Description
        enum Priority "Low, Medium, High, Critical"
        int Difficulty "1-5 Chillies 🌶️"
        datetime StartDate
        datetime Deadline
        Guid WorkflowStateId FK
        Guid CreatorId FK "User"
        Guid AssignedUserId FK "Nullable"
        Guid AssignedTeamId FK "Nullable"
        datetime CreatedAt
        datetime UpdatedAt
    }

    SUBTICKET {
        Guid Id PK
        Guid ParentTicketId FK
        string Title
        boolean IsCompleted
        datetime CreatedAt
    }

    %% Workflow / Kanban Context
    WORKFLOW_STATE {
        Guid Id PK
        string Name "e.g. Backlog, Todo, InProgress, Review, Done"
        int OrderIndex
        string ColorCode
    }

    %% Relationships
    USER ||--o{ USER_ROLE : has
    ROLE ||--o{ USER_ROLE : assigned_to

    USER ||--o{ TEAM_MEMBER : is_in
    TEAM ||--o{ TEAM_MEMBER : contains
    USER ||--o{ TEAM : creates

    USER |o--o{ TICKET : assigned_to
    TEAM |o--o{ TICKET : assigned_to
    USER ||--o{ TICKET : creates

    TICKET ||--o{ SUBTICKET : contains
    WORKFLOW_STATE ||--o{ TICKET : groups
```

### Detaillierte Entity Beschreibung

#### Identity & Access Context
*   **User (Benutzer):** Erweitert das standardmäßige `IdentityUser<Guid>`. Enthält neben Authentifizierungsdaten auch ein vollständiges Profil (Anschrift, Kontakt).
*   **Role (Rolle):** Basis-RBAC (Role-Based Access Control) zur Unterscheidung von System-Administratoren und regulären Nutzern.

#### Team Collaboration Context
*   **Team:** Ein definierter Zusammenschluss von Benutzern. Besitzt einen Namen und eine Beschreibung.
*   **TeamMember:** Die n:m Auflösungstabelle. Ein User kann in vielen Teams sein, ein Team hat viele User.

#### Ticket Management Context
*   **Ticket:** Das Kern-Aggregat (Aggregate Root). Ein Ticket *muss* einen Workflow-State und einen Ersteller haben. Es *kann* einem User **oder** einem Team zugewiesen sein. Es beinhaltet Metadaten wie Zeitraum (Start/Ende), Priorität und Schwierigkeitsgrad (`Difficulty`).
*   **Subticket:** Befindet sich innerhalb der Aggregat-Grenze des Tickets. Einfache Checklisten-Einträge ("Erledigt" / "Offen") für große Tickets.

#### Kanban & Workflow Context
*   **WorkflowState (Spalten im Kanban Board):** Die einzelnen Phasen eines Tickets (z.B. "Todo", "In Progress", "Done"). Beinhaltet die Reihenfolge (`OrderIndex`) für das Board und eine Darstellungsfarbe (`ColorCode`).

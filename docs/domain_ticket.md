# Core Domain: The Ticket Entity

Die `Ticket`-Entität ist das absolut zentrale Objekt dieser Applikation. Sie unterliegt strikten Design-Regeln aus dem *Domain-Driven Design (DDD)*.

## Eigenschaften (Properties)

Eine vollständige Ausbaustufe eines Tickets umfasst:

*   **Id:** `Guid` (Global Unique Identifier)
*   **Title/Name:** `string` (Max. 150 Zeichen)
*   **Description:** `string` (Markdown wird unterstützt)
*   **State / Status:** `TicketStatus` Enum
    *   `ToDo`
    *   `InProgress`
    *   `InReview`
    *   `Done`
*   **Priority:** `TicketPriority` Enum (`Low`, `Medium`, `High`, `Critical/Blocker`)
*   **Complexity / Difficulty (Chillischoten 🌶️):** `byte` (1 bis 5). Dies soll rein visuell die Komplexität abbilden, ohne klassische "Story Points" zu nutzen.
*   **Timeframes:**
    *   `CreatedAt`: `DateTimeOffset` (Systemgesteuert)
    *   `UpdatedAt`: `DateTimeOffset?` (Bei Modifikation)
    *   `StartDate`: `DateTimeOffset?` (Optional, wann die Arbeit beginnt)
    *   `Deadline`: `DateTimeOffset?` (Optional, wann die Arbeit beendet sein muss)
    *   `EstimatedHours`: `decimal?` (Geschätzter Aufwand)
    *   `LoggedHours`: `decimal` (Tatsächlich erfasste Zeit)
*   **Assignees:**
    *   `AssignedUserId`: `Guid?` (Referenz zum bearbeitenden Nutzer)
    *   Fazit: Kann an eine Person oder ein ganzes `Team` delegiert werden (via Join-Table oder eigener Property).
*   **Hierarchy:**
    *   `ParentTicketId`: `Guid?`
    *   `SubTickets`: `IReadOnlyList<Ticket>` (Hiermit können große Epics in Tasks zerlegt werden).

## Domain Logic & Encapsulation

Wir nutzen im Domain Layer **Rich Models** anstelle von anämischen (datengetriebenen) Modellen. Dies bedeutet:
1.  **Properties besitzen `private set`:** Sie können nicht einfach von außen manipuliert werden (`ticket.Status = TicketStatus.Done;` ist verboten!).
2.  **Verhaltens-Methoden:** Zustandsänderungen geschehen ausschließlich über definierte Methoden der Klasse (z.B. `ticket.MoveToReview(Guid userId)`), die intern sicherstellen, dass alle Business-Regeln eingehalten werden (z.B. dass beim Verschieben auf `Review` ein Assignee vorhanden ist).
3.  **Konstruktoren:** Es gibt keinen leeren Parameterlosen Konstruktor für die Erstellung. Ein Ticket **muss** immer mit den minimalen Pflichtfeldern (Title, Context) initialisiert werden.

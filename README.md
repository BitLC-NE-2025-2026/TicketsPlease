# TicketsPlease 🎫

Ein hochmodernes, skalierbares Ticket- und Kanban-System, entwickelt mit modernsten Architektur-Prinzipien und einem extrem hohen Qualitätsstandard.

## 🚀 Vision & Projektziele
Dieses Projekt realisiert ein umfassendes Ticketverwaltungssystem mit Kanban-Boards. Der Fokus liegt dabei nicht nur auf reiner Funktionalität, sondern vor allem auf **exzellenter Softwarearchitektur**, **hoher Code-Qualität** und **strikter Testabdeckung**.

---

## 🛠️ Technologie-Stack

- **IDEs:** Visual Studio 2026 & JetBrains Rider 3.2026 (Exklusiver Support)
- **Backend:** C# 14, ASP.NET Core 10.3
- **Datenbank:** Entity Framework Core (Code-First), MSSQL Server
  - *Siehe detailliertes [Datenbankschema (ERD)](docs/database_schema.md)*
- **Frontend & UI:** 
  - Lokales TailwindCSS 4.2
  - Lokales FontAwesome 7.2
  - Paketverwaltung für statische Client-Bibliotheken via **libman** (`libman.json`)
- **Architektur-Pattern:** Clean Architecture, Domain-Driven Design (DDD)
- **Testing:** xUnit/NUnit, Playwright, Vitest (Integration & E2E)

---

## 📐 Architektur & Design

### Domain-Driven Design (DDD)
Das System nutzt striktes DDD zur Trennung der Fachlichkeit. 
- **Bounded Contexts:** Eindeutige Abgrenzung der Domänen (z.B. *Identity & Access*, *Ticket Management*, *Team Collaboration*).
- **Ubiquitous Language:** Ein klares, gemeinsames Vokabular für Entwickler und Fachexperten.

### Clean Architecture (Onion / Hexagonal)
Strikte Trennung von Zuständigkeiten (Separation of Concerns):
- **Domain Layer:** Beinhaltet Core-Geschäftsregeln, Entities (Ticket, User, Team) und Value Objects. Vollständig unabhängig von Datenbank oder Web-Technologien.
- **Application Layer:** Use Cases, Commands/Queries (CQRS), DTOs und Interfaces für die Domäne.
- **Infrastructure Layer:** EF Core Repositories, Datenbankschemata und Implementierung von externen Diensten.
- **Web/Presentation Layer:** ASP.NET Core Komponenten, API-Endpoints und Benutzeroberfläche.

### SOLID, DRY, KISS & YAGNI
- Höchste Code-Disziplin: Wir etablieren eine fixe **"1 Class pro File"**-Regel.
- Vermeidung jeglicher Code-Duplikate (DRY - Don't Repeat Yourself).
- Einfachste funktionierende und lesbare Lösung wählen (KISS - Keep It Simple, Stupid & YAGNI - You Aren't Gonna Need It).

---

## 🎨 UI & Frontend Spezifikation

### Single File Components (SFC)
Modulare UI-Architektur innerhalb von ASP.NET Core (mittels Razor CSS-Isolation oder als dedizierte ViewComponents), bei der Template, Logik und domänenspezifisches Styling konsequent pro Komponente gebündelt sind.

### Lokale Assets (No CDN)
Sämtliche Libraries (Tailwind, FontAwesome) werden vollständig lokal in das Projekt integriert über den Library Manager (`libman`).

### Atomic CSS & Utility First
- Nutzung von **TailwindCSS 4.2** für pfeilschnelles, utility-basiertes Design.
- **Komponenten-CSS:** Benutzerdefinierte, stark wiederverwendbare CSS-Klassen für grundlegende UI-Bausteine liegen logisch getrennt und aufgeräumt im Frontend-Verzeichnis:
  - `/css/components/btn.css`
  - `/css/components/theme.css`
  - `/css/components/cards.css`
  - `/css/components/form.css` usw.

---

## 🧩 Features & Funktionalitäten

### 🔐 Benutzer & Rollen (IAM)
- **Registrierung & Login:** Sicheres Authentifizierungs-System.
- **Admin-Bereich:** Separates Admin-Dashboard zur System- und Benutzerverwaltung.
- **Benutzerprofile:** Ausführliche Profile inkl. sämtlicher profil relevanter Daten, Anschriften und Kontaktinformationen.

### 👥 Teams
- Nutzer können neue Teams erstellen.
- Nutzer können bestehenden Teams beitreten.
- Team-basierte Ticket-Zuweisung fördert die Zusammenarbeit.

### 🎫 Tickets & Subtickets
Umfassendes Ticket-Objekt mit spezifischen Eigenschaften:
- Titel (Name) & Beschreibung
- Zeitraum (Startdatum / Deadline)
- Priorität
- **Schwierigkeit:** Anschaulich bewertet und visualisiert in "Chillischoten" 🌶️ (z.B. 1 bis 5).
- **Zuweisung:** Tickets können direkt an einzelne Nutzer oder komplette Teams zugewiesen werden.
- **Subtickets:** Große Aufgaben (Tickets) können granular in kleinere Subtickets unterteilt werden.

### 📋 Kanban Dashboard
- Interaktives, visuelles Board.
- Anpassbare Workflows.
- Elegante **Drag & Drop**-Funktionalität, mit der Tickets reibungslos durch verschiedene Workflow-Stadien gezogen werden können.

---

## 🛡️ Code-Qualität & Workflows

### Test-Driven Development (TDD)
- Konsequenter **Red-Green-Refactor**-Zyklus von Beginn an.
- **100% Test Coverage-Ziel:** Zero Compromise. 
  - *Unit Tests* für alle Systemdienste und Domainlogik.
  - *Integration Tests* für Repositories und API/Controllers.
  - *E2E Tests* mittels Playwright & Vitest für die Frontend- und UI-Qualitätssicherung.

### Continuous Integration / Continuous Deployment (CI/CD)
- Automatisierte Pipelines sichern jeden Commit ab.
- **Quality Gates:** Der Code-Build erzwingt striktes Linting, Code-Formatting und das Bestehen *aller* Tests.

### Extensive Dokumentation
- **Self-Documenting Code:** Präzise und selbsterklärende Namensgebung für absolute Lesbarkeit.
- **XML-Kommentare:** Exzessive und vollständige Nutzung der [C# Dokumentationskommentare](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments) für Klassen, Methoden und Properties.
- **Architectural Decision Records (ADR):** Wichtige Architektur- und Technologieentscheidungen (z.B. Wahl des CSS-Aufbaus) werden nachhaltig in Markdown dokumentiert.

---

## 🗺️ Implementierungs Roadmap

1. **Phase 1: Setup & Groundwork**
   - Initiale Solution & Clean Architecture Strukturierung.
   - Setup EF Core, MSSQL und Libman für Frontend Assets.
2. **Phase 2: Domain Modeling & IAM**
   - User, Profil und Team Models erstellen sowie Identity & Auth implementieren.
3. **Phase 3: UI Foundation**
   - Erstellen der `btn.css`, `cards.css`, `theme.css` und des SFC Layout-Frames.
4. **Phase 4: Ticket Engine**
   - Ticket & Subticket Domains, Routing und Repositories integrieren (TDD First!).
5. **Phase 5: Kanban & Interaktivität**
   - Drag & Drop Dashboard, visuelle "Chillischoten"-Anzeige und Workflow Funktionalität aufbereiten.

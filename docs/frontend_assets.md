# Frontend Asset Management Strategy

Dieses Dokument (ADR) beschreibt, wie wir in *TicketsPlease* mit statischen Web-Assets (CSS, JavaScript, Fonts) umgehen.

## Kontext
Moderne Webanwendungen hängen von externen Frameworks ab (hier: TailwindCSS und FontAwesome). Oftmals werden diese einfach über externe Content Delivery Networks (CDNs) wie *cdnjs* oder *jsdelivr* in die HTML-Köpfe importiert. 

Wir mussten entscheiden, wie wir diese externen Abhängigkeiten in unser ASP.NET Core MVC Projekt laden, versionieren und ausliefern.

## Entscheidung
Wir verzichten **komplett** auf externe CDNs im produktiven HTML-Code. 
Sämtliche Front-End-Bibliotheken werden zwingend lokal gehostet.

Wir nutzen den in Visual Studio integrierten **Library Manager (LibMan)** (`libman.json`), um die Pakete aus Providern wie *unpkg* zur Entwicklungszeit herunterzuladen und im Projektverzeichnis (`wwwroot/lib/`) abzulegen.

## Gründe & Design-Treiber

1. **DSGVO / Datenschutz (Kritisch):** Beim Einbinden von Assets über externe CDNs (insb. US-Server wie Google Fonts) wird die IP-Adresse des Nutzers unabdingbar an den Drittanbieter übertragen. Dies erfordert laut DSGVO eine explizite Zustimmung (Cookie-Banner-Zwang) und birgt hohe Abmahnrisiken. Durch lokales Hosting entfällt dieses Problem vollständig.
2. **Ausfallsicherheit (Resilience):** Wenn das Google-CDN oder cdnjs ausfällt (oder im Firmennetzwerk des Kunden geblockt wird), sieht unsere Seite "kaputt" aus (keine CSS-Styles, keine Icons). Lokale Assets garantieren, dass die UI *immer* erreichbar ist, solange unser eigener Server erreichbar ist.
3. **Reproduzierbarkeit (Offline-Entwicklung):** Entwickler können im Zug oder Flugzeug ohne Internetverbindung an der UI arbeiten, da alle Abhängigkeiten im Projekt liegen. Versionen (z.B. Tailwind 4.2) sind hart festgeschrieben und ändern sich nicht magisch unter der Haube.

## Negative Konsequenzen
- Unser Repository wird geringfügig größer, da Assets mit verwaltet werden.
- Die Ladezeiten beim initialen Seitenaufruf können um Bruchteile von Millisekunden höher sein als bei verteilten, extrem optimierten globalen CDNs. Für unsere Enterprise-Kanban-Lösung ist dies jedoch ein akzeptabler Trade-off zugunsten des Datenschutzes.

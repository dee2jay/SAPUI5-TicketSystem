# TicketSystem Mobile PoC

Testbare mobile-first PWA für den TicketSystem-Prototyp.

## Was du testen kannst

- Ticketliste mit Beispiel-Tickets
- Neues Ticket anlegen
- Titel, Beschreibung und Priorität erfassen
- Kamera direkt öffnen
- Mehrere Fotos aus Kamera/Galerie auswählen
- Foto-Vorschau und Entfernen einzelner Fotos
- Admin/User-Modus umschalten
- Admin-Dashboard mit KPI-Karten und Statusübersicht
- Responsive Darstellung auf Smartphone und Desktop
- PWA-Manifest / Standalone-Installation

**Wichtig:** Diese Version ist bewusst ein UX-/Frontend-PoC. Tickets und Fotos werden noch nicht an die bestehende API übertragen und nicht dauerhaft gespeichert.

## Direkt testen

Der Branch ist:

`poc/mobile-ticket-app`

Die GitHub-Pages-Deployment-Pipeline liegt unter:

`.github/workflows/mobile-poc-pages.yml`

Nach erfolgreichem GitHub-Actions-Lauf kann die PWA über die GitHub-Pages-URL des Repositorys geöffnet werden.

## Lokal testen

Im Repository:

```powershell
cd poc/mobile-ticket-app
python -m http.server 8080
```

Danach im Browser:

`http://localhost:8080`

Für die Kamera auf einem Smartphone sollte die Anwendung über **HTTPS** aufgerufen werden. `localhost` ist für lokale Tests ebenfalls ein sicherer Kontext.

## Testablauf

### 1. Ticket erstellen
1. Auf **+ Ticket** gehen.
2. Titel und Beschreibung eingeben.
3. Priorität auswählen.
4. **Foto aufnehmen** bzw. Bilder aus der Galerie auswählen.
5. Fotos prüfen.
6. **Ticket erstellen** drücken.
7. Zur Ticketliste wechseln und das neue Ticket prüfen.

### 2. Admin testen
1. Oben rechts auf **Admin** wechseln.
2. **Dashboard** öffnen.
3. KPI-Karten und Statusverteilung prüfen.

### 3. Mobile testen
Auf einem Smartphone insbesondere prüfen:
- Touch-Bedienung
- Kamera-Dialog
- mehrere Fotos
- Hoch-/Querformat
- kleine Bildschirmbreiten
- Navigation über die untere Tab-Leiste

## Nächster technischer Schritt

Nach der UX-Abnahme wird der PoC an die bestehende ASP.NET-Core-API angebunden:

1. Authentication/Login
2. Ticket-Liste und Ticket-Details
3. Ticket-Erstellung
4. Multipart-Foto-Upload
5. Attachment-Galerie
6. Admin-Dashboard-API
7. Autorisierung und Integrationstests


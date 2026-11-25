Testcase 1: Opslaan & scheiden van voorspelling en werkelijke meting (Integratietest | Risicoklasse A | Functionality & Compatibility)

Situatie: De achtergrondservice synchroniseert zowel voorspellingen als actuele metingen voor dezelfde stad en hetzelfde uur in SQLite.

Verwacht resultaat: Beide records worden naast elkaar bewaard zonder elkaar te overschrijven; bij voorspellingen blijven fijnstofwaarden (PM2.5, PM10, NO₂) leeg, terwijl bij werkelijke metingen alle waarden gevuld zijn.

Testcase 2: Grafiekweergave en periodeselectie (Systeemtest | Risicoklasse B | Functionality & Usability)

Situatie: De gebruiker kiest in het dropdownmenu achtereenvolgens Vandaag, Afgelopen 2 dagen, Afgelopen 7 dagen en Voorspelling.

Verwacht resultaat: De historische keuzes tonen data tot aan het huidige uur (inclusief vergelijking tussen voorspelde en werkelijke AQI), terwijl Voorspelling uitsluitend de paarse stippellijn voor de komende dagen toont.

Testcase 3: Steden vergelijken en conclusiebepaling (Unittest / Systeemtest | Risicoklasse A | Functionality)

Situatie: De gebruiker selecteert op de vergelijkingspagina twee steden met verschillende actuele AQI-waarden (bijvoorbeeld Amsterdam en Parijs).

Verwacht resultaat: De pagina toont de meest recente werkelijke metingen (niet de toekomstige voorspelling), vult de voortgangsbalken van de fijnstoffen en markeert de stad met de laagste AQI als betere luchtkwaliteit met het exacte puntenverschil.

Testcase 4: Foutafhandeling bij onbereikbare backend-API (Systeemtest – unhappy path | Risicoklasse B | Reliability)

Situatie: De backend-API (Opdracht 2) is uitgeschakeld terwijl de gebruiker de webapplicatie (Opdracht 1) opent of van stad wisselt.

Verwacht resultaat: De applicatie crasht niet, maar vangt de fout netjes op en toont een duidelijke, begrijpelijke foutmelding aan de gebruiker.

Testcase 5: End-to-end keten en smoke-check na opstarten (E2E / Smoke-test | Risicoklasse C | Compatibility & Portability)

Situatie: Beide projecten (API en MVC-webapplicatie) worden lokaal gestart en een gebruiker selecteert een andere stad uit de stedenlijst.

Verwacht resultaat: De stedenlijst laadt zonder fouten uit de API, de AQI-badge, statuskleur, kaartjes en grafiek verversen direct met de gegevens van de gekozen stad.
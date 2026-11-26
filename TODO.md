Start
- [x]Github aanmaken en repo delen
- [x]Kijken of repo correct staat en te lezen is in xampp
- [x]Readme met toelichting
- [x]TODO toevoegen
- [x]voortganglog toevoegen
- [x]Structuur bedenken voor de classes 
- [x]Readme uitbreiden

Opdracht #1
Model
- [x]CityLocation lijst van 8 steden maken - verwerkt ook deels in appsettings.json
- [x]DTO aanmaken van json
- [x]OpenMeteoAirClient implementeren met HttClient en DI

Logica
- [x]IAirQualityService interface maken
- [x]AirQualityService voor ophalen mappen en naar view klaar model
- [x]Advieslabel (extra opdracht) 

Controller & view
- [x]HomeController opzetten met index
- [x]Dropdown menu voor steden wisselen
- [x]Herladen voor nieuwe gegevens
- [x]AQI badge tonen met verschillende kleuren
- [x]Cards voor verschillende stoffen
- [x]Foutmelding wanneer API niet bereikbaar is
- [x]Progress bars of gauge bar toevoegen
- [ ]Vergelijk 2 steden (extra opdracht)

-[x]Data opslaan
-[x]Grafiek maken
-[x]Grafiek een dropdown menu geven voor verschillende opties

Vervolgstappen
- [x]Grafieken toevoegen bij vergelijken
- [x]Nadenken over visualisatie van de grafiek, EAQI heeft bovenwaarde in plaats van grenswaarde. Nu kan EAQI onder de andere waarden liggen. 

-[x] Opdracht 2 backend maken
-[x] backend koppelen met frontend
-[x] Opschonen bestanden zodat niks dubbel staat
-[x] Check alle visualisaties en of alle data kloppend is
-[x] Edge cases nalopen en kijken of alles wordt afgehangen in geval van error - lastig wat ik nog moet checken
-[x] Bestanden zo inrichten dat frontend los van backend kan draaien .shared (bestaand uit: AirSample, CityLocation, AirQualitySample)
-[x] Forecast waardes opslaan 
-[x] Forecast waardes vergelijken met nieuwe data - enige kanttekening is dat elke keer bij nieuwe ophaling er weer nieuwe waardes zijn dus forecast is best nauwkeurig als ik het  laat draaien de hele dag. 
-[x] Vergelijking maken tussen 2 steden met AQI waarde en welke dan beter is 
-[x] in weergave de optie geven voor forecast of andere data. 
-[x] denk na over test mogelijkheden en schrijf dit in een los bestandje op
-[x] shared ook opsplitsen in model/services
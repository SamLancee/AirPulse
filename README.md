# AirPulse

Waarom deze case?
Ik vind dingen over de luchtvervuiling in het algemeen interessant. De visualisatie klinkt ook erg interessant om deze uit te werken. 

Welke viz/UX?
Hier moet ik nog beter over nadenken want niet zeker hoe AQI werkt. Iets met goede AQI weergave en dynamische kaarten.

Extra's
Advieslabel
Vergelijk 2 steden

Opzet van de verschillende classes:
Model
    DTO
        AirQualityDto.cs // mapping op basis van json
    CityLocation.cs //config voor dropdownmenu
    AirQualityData.cs //domeinmodel 
    AirQualityview.cs //model voor view

Services
    IAirQualityService.cs // Interface voor DI
    AirQualitySerivce.cs //Logica (berekeningen/advieslabel/mapping)
    OpenMeteoclient.cs //HTTP client voor Open-Meteo

Controller
    AirQualityController.cs //Controller

Views
    Home
        index.cs //view


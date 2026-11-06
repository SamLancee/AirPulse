# AirPulse

Waarom deze case?
Ik vind dingen over de luchtvervuiling in het algemeen interessant. Verschillende waardes vergelijken en kijken wat de waardes doen over het verloop van de dag lijkt mij leuk. Ik ben al gelijk benieuwd naar een data set met getallen om te zien of er bijvoorbeeld tijdens spitsuren veel meer luchtvervuiling is ten opzichte van de nacht/ rest van de dag. 

Welke viz/UX?
Algeheel layout scherm met bovenaan de Luchtkwaliteit voor die specifieke stad. Direct in dezelfde regel de optie om een andere stad aan te klikken of de huidige gegevens te verversen of 2 steden te vergelijken. Dan een grid layout met eerst de grote AQI badge in het midden die veranderd van kleur op basis van de waarde. Daaronder de 3 stoffen met elk hun getal. Daaronder nog een grafiek met tijdsmeeting afgelopen 24uur.

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


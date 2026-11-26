using System.Diagnostics;
using AirPulse.Models;
using AirPulse.Shared;

namespace TestProject1;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

    [Test]
    public void CheckBestand()
    {
        string dbPad= @"C:\xampp\htdocs\AirPulse\Opdracht2\airpulse.db";

        Assert.That(File.Exists(dbPad), Is.True, "Geen db gevonden");
    }

    [Test]
    public void NietLeeg()
    {
        string dbPad= @"C:\xampp\htdocs\AirPulse\Opdracht2\airpulse.db";
        var fileInfo = new FileInfo(dbPad);

        Assert.That(fileInfo.Length, Is.GreaterThan(0), "Database is leeg");

    }

    [Test]

    public void Vergelijken_lager()
    {
        var view = new CompareAirQualityViewModel
        {
            CityA = "Amsterdam",
            CityB = "Parijs",
            LatestCityA = new AirPulse.Shared.AirSample {EuropeanAqi = 50},
            LatestCityB = new AirPulse.Shared.AirSample {EuropeanAqi = 35}
        };

        Assert.That(view.IsCityBBetter, Is.True);
        Assert.That(view.IsCityABetter, Is.False);
    } 

    [Test]
     public void Vergelijken_gelijk()
    {
        var view = new CompareAirQualityViewModel
        {
            CityA = "Amsterdam",
            CityB = "Parijs",
            LatestCityA = new AirPulse.Shared.AirSample {EuropeanAqi = 35},
            LatestCityB = new AirPulse.Shared.AirSample {EuropeanAqi = 35}
        };

        Assert.That(view.IsCityBBetter, Is.False);
        Assert.That(view.IsCityABetter, Is.False);
    } 

    [Test]
    public void aqi_blauw()
    {
        var badge = new AirSample {Pm25=12};
        var kleur = badge.GetPollutantColor(badge.Pm25, 25);
        Assert.That(kleur, Is.EqualTo("info"));
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather
{
    public enum WeatherType {
        ClearSkies,
        Cloudy,
        StrongWinds,
        HeavyRain,
        LightRain,
        Null
    }

    public bool isNight = false;
    public WeatherType wType = WeatherType.Null;
    public int temperatureF = 0;

    public Weather() {
        isNight = false;
        wType = WeatherType.Null;
        temperatureF = 0;
    }

    public Weather(bool isNight, WeatherType weatherType, int tempF) {
        this.isNight = isNight;
        this.wType = weatherType;
        this.temperatureF = tempF;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private const string CLEARSKIES = "Clear Skies";
    private const string CLOUDY = "Cloudy";
    private const string STRONGWINDS = "Strong Winds";
    private const string HEAVYRAIN = "Heavy Rain";
    private const string LIGHTRAIN = "Light Rain";

    [SerializeField] private bool runDefault = true;
    [SerializeField] private List<Vector2> treePositions;
    private static List<Weather> dayWeather = new List<Weather>();
    private static List<Weather> nightWeather = new List<Weather>();
    private static Vector2 treePosition = Vector2.zero;

    private static float probabilityAbandoned = 0.0f;
    private int currentDay = 0;

    void Awake(){
        if (runDefault) {
            RunDefaultData();
        }
        //probabilityAbandoned = 0.0f;
        probabilityAbandoned = 0.9f;        // April probability
    }

    private void OnEnable()
    {
        SceneHandler.OnSunrise += ChangeWeatherMorning;
        SceneHandler.OnSunset += ChangeWeatherNight;
    }

    private void OnDisable()
    {
        SceneHandler.OnSunrise -= ChangeWeatherMorning;
        SceneHandler.OnSunset -= ChangeWeatherNight;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetTreePositionAtIndex(int index) {
        if (index >= 0 && index < treePositions.Count) { 
            return treePositions[index];
        }
        Debug.Log("The index provided (" + index + ") does not exist.");
        return Vector3.zero;
    }

    public void SetWeather(bool isNight, string weatherType, int temperature) {
        Weather.WeatherType wt;
        switch (weatherType) {
            case CLEARSKIES:
                wt = Weather.WeatherType.ClearSkies;
                break;
            case CLOUDY:
                wt = Weather.WeatherType.Cloudy;
                break;
            case STRONGWINDS:
                wt = Weather.WeatherType.StrongWinds;
                break;
            case HEAVYRAIN:
                wt = Weather.WeatherType.HeavyRain;
                break;
            case LIGHTRAIN:
                wt = Weather.WeatherType.LightRain;
                break;
            default:
                wt = Weather.WeatherType.Null;
                break;
        }
        if (!isNight)
            dayWeather.Add(new Weather(isNight, wt, temperature));
        else
            nightWeather.Add(new Weather(isNight, wt, temperature));
    }

    public void SetTreePosition(int index) {
        treePosition = treePositions[index];
    }

    public static Vector2 GetTreePosition() {
        return treePosition;
    }

    public void RunDefaultData() {
        SetTreePosition(0);
        // Data from April 11th - April 18th
        // Reference Link: https://www.wunderground.com/history/daily/us/ca/san-jose/KSJC/date/2022-4-11
        SetWeather(false, CLOUDY, 59);      // 4/11 day
        SetWeather(true, CLOUDY, 50);       // 4/12 night
        SetWeather(false, CLOUDY, 59);      // 4/12 day
        SetWeather(true, CLEARSKIES, 42);   // 4/13 night
        SetWeather(false, CLOUDY, 62);      // 4/13 day
        SetWeather(true, CLOUDY, 53);       // 4/14 night
        SetWeather(false, CLOUDY, 50);      // 4/14 day
        SetWeather(true, LIGHTRAIN, 42);    // 4/15 night
        SetWeather(false, CLEARSKIES, 70);  // 4/15 day
        SetWeather(true, LIGHTRAIN, 55);    // 4/16 night
        SetWeather(false, CLEARSKIES, 54);  // 4/16 day
        SetWeather(true, CLEARSKIES, 42);   // 4/17 night
        SetWeather(false, CLEARSKIES, 64);  // 4/17 day
        SetWeather(true, CLEARSKIES, 43);   // 4/18 night
    }

    public static string GetProbabilityString() {
        int prob = (int)(probabilityAbandoned * 100);
        return prob.ToString() + "%";
    }

    public static void ChangeProbability(float delta) {
        probabilityAbandoned += delta;
    }

    private void ChangeWeatherMorning() {
        AbandonWeatherCalc(dayWeather);
    }

    private void ChangeWeatherNight() {
        AbandonWeatherCalc(nightWeather);
        currentDay++;
        if (currentDay >= nightWeather.Count) {
            Debug.Log("End day: " + currentDay);
            currentDay = 0;
            SceneHandler.endOfSim = true;
        }
    }

    private void AbandonWeatherCalc(List<Weather> wl) { 
        if(wl[currentDay].wType == Weather.WeatherType.LightRain || wl[currentDay].wType == Weather.WeatherType.HeavyRain || wl[currentDay].wType == Weather.WeatherType.StrongWinds){
            ChangeProbability(0.01f);
        }
        if (wl[currentDay].temperatureF < 45) {
            ChangeProbability(0.005f);
        }
    }
}

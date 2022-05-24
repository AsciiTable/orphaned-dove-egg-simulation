using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private List<Vector2> treePositions;
    private static List<Weather> dayWeather = new List<Weather>();
    private static List<Weather> nightWeather = new List<Weather>();

    // Start is called before the first frame update
    void Start(){
        
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
            case "Clear Skies":
                wt = Weather.WeatherType.ClearSkies;
                break;
            case "Cloudy":
                wt = Weather.WeatherType.Cloudy;
                break;
            case "Strong Winds":
                wt = Weather.WeatherType.StrongWinds;
                break;
            case "Heavy Rain":
                wt = Weather.WeatherType.HeavyRain;
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
}

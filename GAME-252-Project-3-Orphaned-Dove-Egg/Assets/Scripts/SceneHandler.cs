using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    private bool useSpecificSeed = false;          // If we have a specific seed to test...
    private int seed = 0;                              // Define this seed here

    [SerializeField] private static float timeScale = 1.0f;
    [SerializeField] private Text currentSeedText;
    [SerializeField] private InputField enteredText;
    [SerializeField] private Animator optionMenuAnimation;
    [SerializeField] private Text timeText;
    [SerializeField] private Animator dayNightFilter;
    [SerializeField] private Text probabilityText;
    [SerializeField] private static int openingHour = 5;
    [SerializeField] private static int openingMinute = 0;
    [SerializeField] private static int closingHour = 20;
    [SerializeField] private static int closingMinute = 0;
    [SerializeField] private static int startMonth = 4;
    [SerializeField] private static int endMonth = 4;
    private bool sunRose = true;
    private static int timeHour = 0;
    private static int timeMinute = 0;
    private float nextUpdateTime;
    private static int enteredSeedValue;
    private static bool valueRequested;

    public delegate void Sunrise();
    public static Sunrise OnSunrise;
    public delegate void Sunset();
    public static Sunset OnSunset;
    public delegate void Tick();
    public static Tick OnTick;

    public static bool endOfSim = false;

    private void Awake()
    {
        if (useSpecificSeed){
            Random.InitState(seed);  // Will initiate Random using the given seed if requested for reproducing results
        }
        Debug.Log("Initiating scene with seed: " + Random.seed);
        if (currentSeedText != null) {
            currentSeedText.text = "Current: " + Random.seed.ToString();
        }
    }

    void Start()
    {
        timeHour = openingHour;
        timeMinute = openingMinute;
        nextUpdateTime = Time.time + (1.0f / timeScale);
        timeText.text = TimeToString();
        sunRose = true;
        endOfSim = false;
    }

    void Update()
    {
        if (endOfSim) {
            Time.timeScale = 0.0f;
            return;
        }
        if (Time.time >= nextUpdateTime) {
            if(OnTick != null)
                OnTick();
            timeMinute += 60;
            if (timeMinute >= 60) {
                timeMinute = 0;
                timeHour += 1;
                if (timeHour >= 24)
                    timeHour = 0;
            }
            nextUpdateTime = Time.time + (1.0f / timeScale);
            timeText.text = TimeToString();

            if (timeHour >= closingHour && sunRose){
                if(OnSunrise != null)
                    OnSunrise();
                sunRose = false;
                dayNightFilter.SetBool("isDay", sunRose);
            }
            else if (timeHour >= openingHour && timeHour < closingHour && !sunRose) {
                if(OnSunset != null)
                    OnSunset();
                sunRose = true;
                dayNightFilter.SetBool("isDay", sunRose);
            }
        }
        probabilityText.text = DataManager.GetProbabilityString();
    }

    public static float GetTimeScale() {
        return timeScale;
    }

    public static int SetTimeScale() {
        switch (timeScale) {
            case 1.0f:
                timeScale = 2.0f;
                break;
            case 2.0f:
                timeScale = 5.0f;
                break;
            case 5.0f:
                timeScale = 10.0f;
                break;
            case 10.0f:
                timeScale = 1.0f;
                break;
        }
        return (int)timeScale;
    }

    public static void ReloadScene() {
        SceneManager.LoadScene(0);
        if (valueRequested) {
            Random.InitState(enteredSeedValue);
        }
        else {
            Random.InitState((int)System.DateTime.Now.Ticks);
        }
        Time.timeScale = 1.0f;
        valueRequested = false;
    }

    public void SetEnteredSeedValue() {
        if (enteredText.text != ""){
            enteredSeedValue = int.Parse(enteredText.text);
            valueRequested = true;
        }
        else {
            valueRequested = false;
        }
    }

    public void ToggleOptionsMenu() {
        optionMenuAnimation.SetBool("open", !optionMenuAnimation.GetBool("open"));
    }

    public string TimeToString() {
        string hstr = timeHour.ToString();
        string mstr = timeMinute.ToString();
        if (timeHour < 10)
            hstr = "0" + hstr;
        if (timeMinute < 10)
            mstr = "0" + mstr;
        return hstr + ":" + mstr;
    }

    public static int GetCurrentHour() {
        return timeHour;
    }

    public static int GetCurrentMinute() {
        return timeMinute;
    }

    public static int GetOpeningHour() {
        return openingHour;
    }
    public static int GetOpeningMinute() {
        return openingMinute;
    }

    public static int GetEndMonth() {
        return endMonth;
    }

    public static int GetStartMonth() {
        return startMonth;
    }
}

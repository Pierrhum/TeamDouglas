using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SecondsController : MonoBehaviour
{
    public float CurrentHour;
    private float oldHour;
    
    public Light SunLight;
    public float SunriseHour;
    public float SunsetHour;
    
    private DateTime currentTime;
    private TimeSpan sunriseTime;
    private TimeSpan sunsetTime;

    public Color dayAmbientLight;
    public Color nightAmbientLight;
    public AnimationCurve lightChangeCurve;
    public float maxSunLightIntensity;

    public bool TimeDebug = false;

    public List<Color> colorHistory;
    public int currentDay = 0;
    private bool isNewDay = false;

    private void Awake()
    {
        colorHistory = new List<Color>();
        oldHour = CurrentHour;
    }

    private void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(CurrentHour);
        
        sunriseTime = TimeSpan.FromHours(SunriseHour);
        sunsetTime = TimeSpan.FromHours(SunsetHour);
    }

    private void Update()
    {
        if (TimeDebug)
            CurrentHour += Time.deltaTime;
        
        UpdateController(CurrentHour - oldHour);
        oldHour = CurrentHour;
    }

    public void UpdateController(float value)
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(CurrentHour += value); // CurrentHour += value
        
        RotateSun( value < 0);
        UpdateLights();
    }

    private void RotateSun(bool clockwise)
    {
        float SunRotation;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            if(isNewDay)
                if (clockwise) PreviousDay(); else NextDay();
            
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;
            SunRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            isNewDay = true;
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;
            SunRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        SunLight.transform.rotation = Quaternion.AngleAxis(SunRotation, Vector3.right);
    }

    private void UpdateLights()
    {
        float dot = Vector3.Dot(SunLight.transform.forward, Vector3.down);

        SunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dot)); 
        RenderSettings.ambientSkyColor = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dot)); 
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan diff = toTime - fromTime;

        if (diff.TotalSeconds < 0)
            diff += TimeSpan.FromHours(24);

        return diff;
    }

    private void NextDay()
    {
        if (currentDay < 10)
        {
            if(currentDay < colorHistory.Count - 1)
                SunLight.color = colorHistory[currentDay + 1];
            else
            {
                SunLight.color = new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f));
                colorHistory.Add(SunLight.color);
            }
            currentDay++;
        }
        else
        {
            colorHistory.RemoveAt(0);
            SunLight.color = new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f));
            colorHistory.Add(SunLight.color);
        }
        
        isNewDay = false;
    }

    private void PreviousDay()
    {
        if (currentDay > 0)
        {
            SunLight.color = colorHistory[currentDay - 1];
            currentDay--;
        }
        else
        {
            if(colorHistory.Count > 10)
                colorHistory.RemoveAt(colorHistory.Count-1);
            
            SunLight.color = new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f));
            colorHistory.Insert(0, SunLight.color);
        }
        
        isNewDay = false;
    }
}

using System;
using UnityEngine;

public class SecondsController : MonoBehaviour
{
    public float TimeMultiplier;
    public float startHour;
    
    public Light SunLight;
    public Light MoonLight;
    public float SunriseHour;
    public float SunsetHour;
    
    private DateTime currentTime;
    private TimeSpan sunriseTime;
    private TimeSpan sunsetTime;

    public Color dayAmbientLight;
    public Color nightAmbientLight;
    public AnimationCurve lightChangeCurve;
    public float maxSunLightIntensity;
    public float maxMoonLightIntensity;
    
    private void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
        
        sunriseTime = TimeSpan.FromHours(SunriseHour);
        sunsetTime = TimeSpan.FromHours(SunsetHour);
    }

    private void Update()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * TimeMultiplier);
        Debug.Log(currentTime.ToString("HH:mm"));
        
        RotateSun();
        UpdateLights();
    }

    private void RotateSun()
    {
        float SunRotation;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;
            SunRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
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
        MoonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dot));

        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dot));
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan diff = toTime - fromTime;

        if (diff.TotalSeconds < 0)
            diff += TimeSpan.FromHours(24);

        return diff;
    }
}

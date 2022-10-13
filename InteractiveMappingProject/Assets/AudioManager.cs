using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public float GravityValue = 0f;
    public bool Geometry = false;
    public bool Buildings = false;
    public bool Trees = false;
    
    private float oldGravityValue;

    private FMODUnity.StudioEventEmitter musicEvent;


    private void Awake()
    {
        musicEvent = GetComponent<FMODUnity.StudioEventEmitter>();
        oldGravityValue = GravityValue;
    }

    void Update()
    {
        if (GravityValue != oldGravityValue)
        {
            musicEvent.SetParameter("Gravity", GravityValue);
            oldGravityValue = GravityValue;
        }
        
        musicEvent.SetParameter("Geometry", Geometry ? 1.0f : 0.0f);
        musicEvent.SetParameter("Buildings", Buildings ? 1.0f : 0.0f);
        musicEvent.SetParameter("Trees", Trees ? 1.0f : 0.0f);
    }
}

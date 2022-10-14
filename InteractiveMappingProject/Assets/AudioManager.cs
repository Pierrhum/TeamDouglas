using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public float GravityValue = 0f;
    
    private float oldGravityValue;

    private FMODUnity.StudioEventEmitter musicEvent;

    public static AudioManager instance;


    private void Awake()
    {
        musicEvent = GetComponent<FMODUnity.StudioEventEmitter>();
        oldGravityValue = GravityValue;

        instance = this;
    }

    void Update()
    {
        if (GravityValue != oldGravityValue)
        {
            musicEvent.SetParameter("Gravity", GravityValue);
            oldGravityValue = GravityValue;
        }
    }

    public void PlayMusic(string name, bool ShouldPlay)
    {
        musicEvent.SetParameter(name, ShouldPlay ? 1.0f : 0.0f);
    }
}

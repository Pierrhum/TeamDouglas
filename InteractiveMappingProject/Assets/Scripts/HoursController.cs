using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoursController : MonoBehaviour
{
    [SerializeField]
    private Material terrainMaterial;
    [SerializeField]
    private AudioManager audioManager;

    public List<HoursObject> controlledObjects;

    private float offset = 0f;
    private float terrainOffset = 0f;
    private float bufferSFX = 0f;

    private void Update() {
        bufferSFX = Mathf.Lerp(bufferSFX, 0f, 0.2f);
        audioManager.GravityValue = bufferSFX;
    }

    public void OnEncoderRotate(bool clockwise, float value){
        
    }
    
    public void OnEncoderStep(float value){
        foreach (HoursObject obj in controlledObjects) {
            if (obj != null) {
                obj.UpdateObject(value * 4f);
            }
        }
        
        bufferSFX = Mathf.Max(bufferSFX + Mathf.Abs(value) * 8f, 0f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Heures");

        offset += value;
        terrainOffset = Mathf.Sin(offset);
        terrainMaterial.SetFloat("Vector1_a171f6ff504e4aa592e9dabd4d1bac94", terrainOffset);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class HoursController : MonoBehaviour
{
    [SerializeField]
    private Material terrainMaterial;

    public List<HoursObject> controlledObjects;

    private float offset = 0f;
    private float terrainOffset = 0f;
    private float bufferSFX = 0f;

    private void Update() {
        bufferSFX = Mathf.Lerp(bufferSFX, 0f, 0.2f);

    }

    public void OnEncoderRotate(bool clockwise, float value){
        
    }
    
    public void OnEncoderStep(float value){
        foreach (HoursObject obj in controlledObjects) {
            if (obj != null) {
                obj.UpdateObject(value);
            }
        }
        
        offset += value;
        terrainOffset = Mathf.Sin(offset);
        terrainMaterial.SetFloat("Vector1_a171f6ff504e4aa592e9dabd4d1bac94", terrainOffset);
    }
}

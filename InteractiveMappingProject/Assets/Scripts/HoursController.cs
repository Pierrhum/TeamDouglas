using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoursController : MonoBehaviour
{
    [SerializeField]
    private Material terrainMaterial;

    public List<HoursObject> controlledObjects;

    public void OnEncoderRotate(bool clockwise, float value){
        Debug.Log("Change Terrain: " + value / 2f);
        terrainMaterial.SetFloat("terrainOffset", value / 2f);
    }
    
    public void OnEncoderStep(float value){
        foreach (HoursObject obj in controlledObjects) {
            if (obj != null) {
                obj.UpdateObject(value);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoursController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody[] gravityObjects;
    [SerializeField]
    private float[] gravityHeights;

    [SerializeField]
    private Material terrainMaterial;
    
    public void OnEncoderRotate(bool clockwise, float encoderValue) {
        
    }
}

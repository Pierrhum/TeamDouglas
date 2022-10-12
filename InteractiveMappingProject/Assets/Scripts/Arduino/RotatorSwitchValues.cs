using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorSwitchValues : MonoBehaviour
{
    const int STEPS_BY_TURN = 40;
    
    [SerializeField]
    [Range(0, 5)]
    private int encoderID = 0;
    [SerializeField]
    private float turns = 4f;
    
    [SerializeField]
    [Range(0,1)]
    private float value = 0f;

    [Space]
    [SerializeField]
    private Gradient test;
    private Quaternion target_rot;

    void Awake() {
        
    }

    void Update() {
        Vector3 rot = transform.localRotation.eulerAngles;
        rot.y = value * 360f;
        target_rot.eulerAngles = rot;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rot, 4 * Time.deltaTime);
    
        GetComponent<MeshRenderer>().material.color = test.Evaluate(value);
    }

    void OnSerialValues(string[] values){
        int id = int.Parse(values[0]);
        int direction = int.Parse(values[1]);
        int position = int.Parse(values[2]);

        if (id == encoderID) {
            value = Mathf.Repeat((float)position / (STEPS_BY_TURN * turns), 1f);
        }

        Debug.Log(string.Format("{0}, {1}, {2}", id, direction, position));
    }

    void OnSerialData(string data) {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RotatorSwitchValues : MonoBehaviour
{
    const int STEPS_BY_TURN = 40;
    
    [Header("Parameters")]
    [SerializeField]
    [Range(0, 5)]
    private int encoderID = 0;
    [SerializeField]
    private float turns = 4f;

    [Header("Data")]
    [SerializeField]
    [Range(0,1)]
    public float encoderValue = 0f;
    private float lastEncoderValue = 0f;

    public UnityEvent<bool, float> OnEncoderRotate = new UnityEvent<bool, float>();
    public UnityEvent<float> OnEncoderStep = new UnityEvent<float>();

    private void Update() {
        if (encoderValue != lastEncoderValue) {
            if (OnEncoderRotate != null) OnEncoderRotate.Invoke(encoderValue > lastEncoderValue, encoderValue);
            if (OnEncoderStep != null) OnEncoderStep.Invoke(encoderValue - lastEncoderValue);

            lastEncoderValue = encoderValue;
        }
    }
    
    void OnSerialValues(string[] values){
        int id = int.Parse(values[0]);
        if (id == encoderID) {
            int direction = int.Parse(values[1]);
            int position = int.Parse(values[2]);

            encoderValue = Mathf.Repeat((float)position / (STEPS_BY_TURN * turns), 1f);

            //Debug.Log(string.Format("{0}, {1}", direction > 0, value));
        }
    }
}

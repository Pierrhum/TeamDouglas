using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUsingValues : MonoBehaviour
{
    public void OnEncoderRotate(bool clockwise, float encoderValue){
        Debug.Log(string.Format("{0}, {1}", clockwise, encoderValue));
    }

    public void OnEncoderStep(float encoderStep){
        Debug.Log(encoderStep);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinutesObject : MonoBehaviour
{
    private float _SpawnTime;
    private float _LifeTime;
    public void Setup(float SpawnTime, float LifeTime)
    {
        _SpawnTime = SpawnTime;
        _LifeTime = LifeTime;
    }

    public bool ShouldDestroy(float Time)
    {
        return Time > _SpawnTime + _LifeTime || Time < _SpawnTime;
    }
}

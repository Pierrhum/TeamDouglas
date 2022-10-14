using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinutesObject : MonoBehaviour
{
    private float _SpawnTime;
    private float _LifeTime;

    private float currentValue = 0f;
    private bool isBeingDestroyed = false;
    public void Setup(float SpawnTime, float LifeTime)
    {
        _SpawnTime = SpawnTime;
        _LifeTime = LifeTime;

        StartCoroutine(FadeMaterial(true));
    }

    public bool ShouldDestroy(float Time)
    {
        return Time > _SpawnTime + _LifeTime || Time < _SpawnTime;
    }

    public IEnumerator FadeMaterial(bool FadeIn)
    {
        if (GetComponent<Renderer>() != null)
        {
            if (FadeIn)
            {
                while (!isBeingDestroyed && currentValue < 1.0f)
                {
                    GetComponent<Renderer>().material.SetFloat("_AppearFactor", currentValue);
                    currentValue += Time.deltaTime * 2;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
            }
            else
            {
                isBeingDestroyed = true;
                while (currentValue > 0.0f)
                {
                    GetComponent<Renderer>().material.SetFloat("_AppearFactor", currentValue);
                    currentValue -= Time.deltaTime * 2;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
            }
        }

        yield return null;
    }
    
}

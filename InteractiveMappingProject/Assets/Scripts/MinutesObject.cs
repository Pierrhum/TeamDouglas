using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    TREE,
    BUILDING,
    GEOMETRY
};
public class MinutesObject : MonoBehaviour
{
    private float _SpawnTime;
    private float _LifeTime;
    private Type _Type;

    private float currentValue = 0f;
    private bool isBeingDestroyed = false;
    public void Setup(float SpawnTime, float LifeTime, Type Type)
    {
        _SpawnTime = SpawnTime;
        _LifeTime = LifeTime;
        _Type = Type;

        StartCoroutine(FadeMaterial(true));
    }

    public bool ShouldDestroy(float Time)
    {
        return Time > _SpawnTime + _LifeTime || Time < _SpawnTime;
    }

    public IEnumerator FadeMaterial(bool FadeIn)
    {
            if (FadeIn)
            {
                while (!isBeingDestroyed && currentValue < 1.0f)
                {
                    if(_Type == Type.TREE)
                        GetComponent<Renderer>().material.SetFloat("_AppearFactor", currentValue);
                    else if(_Type == Type.BUILDING)
                        foreach (var renderer in GetComponentsInChildren<Renderer>())
                        {
                            renderer.material.SetFloat("_DissolveFactor", 1-currentValue);
                        }
                    else if(_Type == Type.GEOMETRY)
                        GetComponent<Renderer>().material.SetFloat("_DissolveFactor", 1-currentValue);
                    currentValue += Time.deltaTime;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
            }
            else
            {
                isBeingDestroyed = true;
                while (currentValue > 0.0f)
                {
                    if(_Type == Type.TREE)
                        GetComponent<Renderer>().material.SetFloat("_AppearFactor", currentValue);
                    else if(_Type == Type.BUILDING)
                        foreach (var renderer in GetComponentsInChildren<Renderer>())
                        {
                            renderer.material.SetFloat("_DissolveFactor", 1-currentValue);
                        }
                    else if(_Type == Type.GEOMETRY)
                        GetComponent<Renderer>().material.SetFloat("_DissolveFactor", 1-currentValue);
                    
                    currentValue -= Time.deltaTime;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
            }

        yield return null;
    }
    
}

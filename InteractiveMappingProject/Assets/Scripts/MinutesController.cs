using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MinutesController : MonoBehaviour
{
    public float CurrentHour;
    private float oldHour;
    private DateTime currentTime;

    public GameObject SpawnerPlane;
    public GameObject TreePrefab;
    public GameObject BuildingPrefab;
    
    private bool TreePhase = true;
    public List<GameObject> Trees;
    private List<GameObject> Buildings;

    public float TreeDuration;
    public float BuildingDuration;

    private float StartPhaseTime;
    private void Awake()
    {
        Trees = new List<GameObject>();
        Buildings = new List<GameObject>();
        oldHour = CurrentHour;
    }

    private void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(CurrentHour);
        TreeDuration = Random.Range(5.0f, 9.0f);
        BuildingDuration = Random.Range(3.0f, 5.0f);
        StartPhaseTime = CurrentHour;
    }

    // Update is called once per frame
    void Update()
    {
        if (oldHour != CurrentHour)
        {
            UpdateController(CurrentHour - oldHour);
            oldHour = CurrentHour;
        }
    }
    
    public void UpdateController(float value)
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(CurrentHour += value);

        // Gestion de changement des phases
        if (StartPhaseTime + (TreePhase ? TreeDuration : BuildingDuration) < CurrentHour)
        {
            // On clear les anciennes listes
            if (TreePhase)
            {
                Trees.ForEach(t => Destroy(t));
                Trees.Clear();
            }
            else
            {
                Buildings.ForEach(t => Destroy(t));
                Buildings.Clear();
            }
            
            // On update la phase
            TreePhase = !TreePhase;
            StartPhaseTime = CurrentHour;
        }
        
        if (TreePhase)
        {
            Trees.Add(SpawnTree(GetRandomPointInPlane()));
            
            // Remove Destroyed objects
            Trees.ForEach(t => {
                if (t.GetComponent<MinutesObject>().ShouldDestroy(CurrentHour))
                    Destroy(t);
            });
            Trees.RemoveAll(t => t.GetComponent<MinutesObject>().ShouldDestroy(CurrentHour));
        }
        else
        {
            Buildings.Add(SpawnBuilding(GetRandomPointInPlane()));
            
            // Remove Destroyed objects
            Buildings.ForEach(t => {
                if (t.GetComponent<MinutesObject>().ShouldDestroy(CurrentHour))
                    Destroy(t);
            });
            Buildings.RemoveAll(t => t.GetComponent<MinutesObject>().ShouldDestroy(CurrentHour));
        }
    }

    private Vector3 GetRandomPointInPlane()
    {
        List<Vector3> VerticeList = new List<Vector3>(SpawnerPlane.GetComponent<MeshFilter>().sharedMesh.vertices);
        
        Vector3 leftTop = SpawnerPlane.transform.TransformPoint(VerticeList[0]);
        Vector3 rightTop = SpawnerPlane.transform.TransformPoint(VerticeList[10]);
        Vector3 leftBottom = SpawnerPlane.transform.TransformPoint(VerticeList[110]);
        
        Vector3 XAxis = rightTop - leftTop;
        Vector3 ZAxis = leftBottom - leftTop;
        
        return leftTop + XAxis * Random.value + ZAxis * Random.value;
    }

    private GameObject SpawnTree(Vector3 position)
    {
        GameObject go = Instantiate(TreePrefab, position, Quaternion.Euler(-90,0,0));
        
        MinutesObject mobj = go.AddComponent<MinutesObject>();
        mobj.Setup(CurrentHour, Random.Range(1.0f,3.0f));

        return go;
    }

    private GameObject SpawnBuilding(Vector3 position)
    {
        GameObject go = Instantiate(BuildingPrefab, position, Quaternion.Euler(0,0,0));
        
        MinutesObject mobj = go.AddComponent<MinutesObject>();
        mobj.Setup(CurrentHour, Random.Range(1.0f,3.0f));

        return go;
    }
}

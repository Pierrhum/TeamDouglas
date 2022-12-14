using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deform;

public class HoursObject : MonoBehaviour
{
    [SerializeField]
    private MeltDeformer meltPrefab;
    
    private MeltDeformer deformer;
    private Deformable deformable;
    private HoursController controller;
    private Rigidbody body;

    private Vector3 startPosition;
    private Vector3 startAngularVelocity;

    private float floatHeight = 0f;
    private float progress = 0f;
    private float targetProgress = 0f;
    private float sinProgress = 0f;
    
    private void Awake() {
        Vector3 pos = transform.position;
        Quaternion rot = meltPrefab.transform.rotation;

        pos.y = 0.1f;

        deformer = Instantiate(meltPrefab, pos, rot);
        deformable = gameObject.AddComponent<Deformable>();
        deformable.AddDeformer(deformer);

        controller = FindObjectOfType<HoursController>();
        controller.controlledObjects.Add(this);

        floatHeight = Random.Range(10f, 20f);
        startPosition = transform.position;
        startAngularVelocity = Random.onUnitSphere;

        body = GetComponent<Rigidbody>();
        body.useGravity = false;
    }

    private void Update() {
        progress = Mathf.Lerp(progress, targetProgress, 0.2f);
        sinProgress = Mathf.Sin(progress);

        transform.position = startPosition + Vector3.up * floatHeight * Mathf.Max(sinProgress, -0.5f);
        body.angularVelocity = startAngularVelocity * Mathf.Max(sinProgress, 0f);
        deformer.Factor = Mathf.Max(1f - sinProgress * 10f, 0f);
    }

    public void UpdateObject(float value) {
        targetProgress += value;
        
    }

    private void OnDestroy() {
        controller.controlledObjects.Remove(this);
        Destroy(deformer.gameObject);
    }
}

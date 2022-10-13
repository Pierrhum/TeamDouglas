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
    private Vector3 targetPosition;

    private float floatHeight = 0f;
    private int state = 0;
    private float stateProgress = 0f;
    
    private void Awake() {
        Vector3 pos = transform.position;
        Quaternion rot = meltPrefab.transform.rotation;

        pos.y = 0;

        deformer = Instantiate(meltPrefab, pos, rot);
        deformable = gameObject.AddComponent<Deformable>();
        deformable.AddDeformer(deformer);

        controller = FindObjectOfType<HoursController>();
        controller.controlledObjects.Add(this);

        floatHeight = Random.Range(2f, 5f);
        startPosition = transform.position;

        body = GetComponent<Rigidbody>();
        body.useGravity = false;
    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, targetPosition, stateProgress);
    }

    public void UpdateObject(float value) {
        stateProgress += value;
        if (stateProgress > 1f) {
            state += 1;
            stateProgress = 0f;
        } 
        if (stateProgress < 0f) {
            state -= 1;
            stateProgress = 1f;
        }

        Debug.Log(stateProgress);

        switch (state)
        {
            case -1:
                targetPosition = startPosition;
                targetPosition.y = 0;
                body.angularVelocity *= 0.5f;

                deformer.Factor = stateProgress * 2f;
                break;

            case 0:
                targetPosition = startPosition + Vector3.up * floatHeight;
                body.angularVelocity = Random.onUnitSphere * stateProgress * 2f;
                break;

            case 1:
                targetPosition = startPosition;
                targetPosition.y = 0;
                body.angularVelocity *= 0.5f;

                deformer.Factor = stateProgress * 2f;
                break;

            default:
                Destroy(gameObject);
                break;

        }
    }

    private void OnDestroy() {
        controller.controlledObjects.Remove(this);
        Destroy(deformer.gameObject);
    }
}

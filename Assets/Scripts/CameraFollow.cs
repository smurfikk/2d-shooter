using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    public float followSpeed = 2.0f;

    public Vector3 cameraOffset = new Vector3(0.0f, 5.0f, -10.0f);

    private Vector3 targetPosition;

    void Start()
    {
        transform.position = player.transform.position + cameraOffset;
    }

    void Update()
    {
        targetPosition = player.transform.position + cameraOffset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}


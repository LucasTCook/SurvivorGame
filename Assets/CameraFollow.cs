using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target to follow
    public Vector3 offset; // The offset from the target position

    void LateUpdate()
    {
        // Set the camera position to the target position plus the offset
        transform.position = target.position + offset;
    }
}
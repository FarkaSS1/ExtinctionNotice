using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateTitleScreen : MonoBehaviour
{
    public float rotationSpeed = 5f; // Adjust the speed of rotation
    public Vector3 rotationAxis = new Vector3(1, 1, 0); // Diagonal rotation (X and Y)

    void Update()
    {
        // Normalize the axis to ensure smooth rotation at any speed
        transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime);
    }
}

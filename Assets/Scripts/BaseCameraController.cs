using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform hub; // Assign the hub GameObject here in the Inspector
    public float moveSpeed = 2f;
    public float zoomSpeed = 3f;
    public float minZoom = 10f;
    public float maxZoom = 30f;
    public float moveLimit = 50f; // Limit how far the camera can move from the hub

    private Vector3 startPosition;

    private void Start()
    {
        if (hub != null)
        {
            startPosition = hub.position;
            transform.position = startPosition;
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float v = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Vector3 newPos = transform.position + new Vector3(h, 0, v);

        // Clamp movement within a circular range around the hub
        Vector3 offset = newPos - startPosition;
        if (offset.magnitude > moveLimit)
        {
            newPos = startPosition + offset.normalized * moveLimit;
        }

        transform.position = newPos;
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        float newY = Mathf.Clamp(transform.position.y - scroll, minZoom, maxZoom);

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void ResetToHub()
    {
        transform.position = startPosition;
    }
}

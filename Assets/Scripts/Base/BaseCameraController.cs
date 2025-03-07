using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform hub;
    public float moveSpeed = 5f;
    public float zoomSpeed = 5f;
    public float minZoom = 10f;
    public float maxZoom = 30f;
    public float moveLimit = 50f;

    private Vector3 startPosition;
    private float targetZoom;

    private void Start()
    {
        if (hub != null)
        {
            startPosition = hub.position - new Vector3(0, 0, 200);
            transform.position = startPosition + new Vector3(0, maxZoom, 0); // Start at max zoom
            transform.rotation = Quaternion.Euler(60, 0, 0); // Tilt the camera to a 45-degree angle
        }
        targetZoom = maxZoom;
    }


    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float zoomFactor = Mathf.InverseLerp(minZoom, maxZoom, transform.position.y);
        float dynamicSpeed = moveSpeed + (moveSpeed * zoomFactor * 2); // Increase speed when zoomed out

        float h = Input.GetAxis("Horizontal") * dynamicSpeed;
        float v = Input.GetAxis("Vertical") * dynamicSpeed;

        Vector3 targetPos = transform.position + new Vector3(h, 0, v) * Time.deltaTime;

        Vector3 offset = targetPos - startPosition;
        if (offset.magnitude > moveLimit)
        {
            targetPos = startPosition + offset.normalized * moveLimit;
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        if (scroll == 0) return;

        // Get the mouse position in world space
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        Vector3 targetPoint = transform.position;

        if (groundPlane.Raycast(ray, out float enter))
        {
            targetPoint = ray.GetPoint(enter);
        }

        // Calculate direction towards the zoom target
        Vector3 direction = (targetPoint - transform.position).normalized;

        // Adjust zoom target
        targetZoom = Mathf.Clamp(targetZoom - scroll, minZoom, maxZoom);
        Vector3 zoomTargetPosition = transform.position + direction * scroll * 10f; // Adjust multiplier for speed

        // Lerp for smooth movement
        transform.position = Vector3.Lerp(transform.position, zoomTargetPosition, 0.1f);
    }


    public void ResetToHub()
    {
        transform.position = startPosition + new Vector3(0, maxZoom, 0);
        targetZoom = maxZoom;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class GameHandler : MonoBehaviour
{
    public GameObject minePrefab; // Assign a mine prefab in the inspector
    public CameraModeManager cameraModeManager; // Assign the CameraModeManager in the inspector

    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0) && cameraModeManager.IsBaseViewCamActive())
    //     {
    //         Vector3 mouseWorldPosition = GetMouseWorldPosition();
    //         Quaternion rotation = Quaternion.Euler(-90, 90, 0);
    //         new Mine(mouseWorldPosition, minePrefab, rotation);
    //     }
    // }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.point;
        }

        return Vector3.zero; // Return zero if no hit
    }
}

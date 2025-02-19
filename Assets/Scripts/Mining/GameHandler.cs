using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class GameHandler : MonoBehaviour
{
    public GameObject minePrefab; // Assign a mine prefab in the inspector
    public float mineDistanceFromCamera = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // TimeTickSystem.OnTick += delegate(object sender, TimeTickSystem.OnTickEventArgs e){
        //     Debug.Log("Tick: " + e.tick);
        // };
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            new Mine(mouseWorldPosition);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = mineDistanceFromCamera; // Set this to the fixed distance from the camera
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }
}

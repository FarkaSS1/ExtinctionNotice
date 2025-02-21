using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Mine : SelectableObject
{   
    private GameObject mineGameObject;
    public GameStateManager GSM;
    private bool isGenerating = true;

    public Mine(Vector3 position, GameObject minePrefab, quaternion rotation)
    {   
        if (minePrefab != null)
        {
            mineGameObject = GameObject.Instantiate(minePrefab, position, rotation);
        }
        else
        {
            Debug.LogError("Mine prefab is not assigned.");
            return;
        }

        // Ensure GSM is assigned
        if (GSM == null)
        {
            GSM = GameObject.FindObjectOfType<GameStateManager>();
        }

        TimeTickSystem.OnTick += delegate(object sender, TimeTickSystem.OnTickEventArgs e){
            if (isGenerating && e.tick % 10 == 0)
            { 
                GSM.AddResource("elementX", 10);
                Debug.Log("Generating elementX" + GSM.ReturnResources("elementX"));
            }
        };
    }
}
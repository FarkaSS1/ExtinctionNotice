using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Mine : SelectableObject
{   
    private GameObject mineGameObject;
    public GameStateManager GSM;
    private bool isGenerating = true;

    public Mine(Vector3 position, GameObject minePrefab)
    {   
        if (minePrefab != null)
        {
            mineGameObject = GameObject.Instantiate(minePrefab, position, Quaternion.identity);
            
            // Ensure the mineGameObject has a BoxCollider
            if (mineGameObject.GetComponent<BoxCollider>() == null)
            {
                mineGameObject.AddComponent<BoxCollider>();
            }

            // Ensure the mineGameObject has the SelectableObject component
            if (mineGameObject.GetComponent<SelectableObject>() == null)
            {
                mineGameObject.AddComponent<SelectableObject>();
            }

            // Initialize the SelectableObject component
            SelectableObject selectable = mineGameObject.GetComponent<SelectableObject>();
            if (selectable != null)
            {
                selectable.selectionManager = this.selectionManager;
                selectable.selectedColor = this.selectedColor;
                selectable.cost = this.cost;
                selectable.costType = this.costType;
            }
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
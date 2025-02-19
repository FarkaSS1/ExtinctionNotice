using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class Mine : SelectableObject
{   
    private GameObject mineGameObject;
    private GameStateManager GSM;
    private bool isGenerating = true;

    public Mine(Vector3 position)
    {   
        mineGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Renderer rend = mineGameObject.GetComponent<Renderer>();
        rend.material.color = Color.yellow;
        mineGameObject.transform.position = position;
        //mineGameObject = new GameObject("Mine");
        //  mineGameObject.transform.position = position;
        mineGameObject.AddComponent<Rigidbody>();
        

        TimeTickSystem.OnTick += delegate(object sender, TimeTickSystem.OnTickEventArgs e){
            if (isGenerating)
            { 
                GSM.AddResource("elementX", 1);
                Debug.Log("Generating elementX" + GSM.ReturnResources("elementX"));
            }
        };
    }
}

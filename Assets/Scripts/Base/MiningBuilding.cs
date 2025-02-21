using UnityEngine;

public class Mining : SelectableObject
{
    public int miningSpeed = 10;

    public override void Start()
    {
        base.Start(); //  Ensure parent logic runs
        cost = 300; // Mining-specific cost
        costType = "elementX";
        Debug.Log("Mining object initialized!");
    }
}

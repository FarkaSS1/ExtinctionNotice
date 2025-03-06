using UnityEngine;

public class Mine : SelectableObject
{   
    private GameStateManager GSM;
    private bool isGenerating = false;

    internal override int GetCost() {
        return 200;
    }
    internal override string GetCostType() {
        return "elementX";
    }

    private void OnEnable()
    {
        FindGSM();
    }

    private void FindGSM()
    {
        if (GSM == null)
        {
            GSM = FindObjectOfType<GameStateManager>();
            if (GSM != null)
            {
                Debug.Log($"[{gameObject.name}] Successfully connected to Miner");
            }
        }
    }

    void Start()
    {
        base.Start();
        // Subscribe to the tick event
        TimeTickSystem.OnTick += HandleTick;

    }

    private void HandleTick(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        if (isGenerating && e.tick % 10 == 0)
        { 
            GSM.AddResource("elementX", 10);
            Debug.Log("Generating elementX: " + GSM.ReturnResources("elementX"));
        }
    }

    private void OnDestroy()
    {
        // Always unsubscribe from events when the object is destroyed
        TimeTickSystem.OnTick -= HandleTick;
    }

    public void PlacedMine()
    {
        SetActiveState(true);
    }

    private void SetActiveState(bool state)
    {
        isGenerating = state;
    }
}
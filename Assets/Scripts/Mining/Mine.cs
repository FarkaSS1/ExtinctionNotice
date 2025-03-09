using UnityEngine;

public class Mine : SelectableObject
{   
    private GameStateManager GSM;
    private bool isGenerating = false;

    private static int cost = 500;
    private static int productionModifier = 1;
    private int production = 10;


    internal override int GetCost() {
        return cost;
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
            GSM.AddResource("elementX", production * productionModifier);
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

    public static void UpgradeLowerCost()
    {
        cost -= 50;
    }

    public static void UpgradeProduction()
    {
        productionModifier += 1;
    }
}
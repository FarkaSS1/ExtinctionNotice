// TowerOne.cs
using UnityEngine;

public class TowerOne : SelectableObject
{
    public int attackDamage = 25;
    public float attackRange = 10f;
    private Transform target;

    protected void Awake() // Changed from Start() to Awake()
    {
        cost = 600; // Tower-specific cost
        costType = "elementX";
        Debug.Log("TowerOne object initialized in Awake! Cost: " + cost);
    }


private void Update()
    {
        FindTarget();
    }

    private void FindTarget()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider enemy in enemies)
        {
            if (enemy.CompareTag("Enemy")) // Adjust this tag for your enemies
            {
                target = enemy.transform;
                Debug.Log($"Tower targeting enemy: {target.name}");
                return;
            }
        }
        target = null; // No enemy found
    }

    // Override GetCost method to return the correct cost from the child
    public override int GetCost()
    {
        return cost; // Return the TowerOne cost
    }

    public override string GetCostType()
    {
        return costType; // Return the TowerOne cost type
    }
}

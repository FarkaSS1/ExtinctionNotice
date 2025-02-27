using UnityEngine;

public class BuildingHealth : Health
{
    async protected override void Die()
    {
        Debug.Log($"{gameObject.name} has been destroyed!");
        await System.Threading.Tasks.Task.Delay(500); // Small delay before destruction
        Destroy(gameObject);
    }
}

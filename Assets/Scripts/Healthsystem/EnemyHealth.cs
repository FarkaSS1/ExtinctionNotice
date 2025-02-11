using UnityEngine;

public class EnemyHealth : Health
{
    protected override void Die()
    {
        Debug.Log("Enemy Died. Award XP!");
        // Add XP reward, loot drops, etc.
        base.Die(); // Calls the original Die() to destroy the object
    }
}

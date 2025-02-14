using UnityEngine;

public class PlayerHealth : Health
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))  // Press "D" to take damage
        {
            TakeDamage(10);  // Damage the player by 10
        }

        if (Input.GetKeyDown(KeyCode.H))  // Press "H" to heal
        {
            Heal(10);  // Heal the player by 10
        }
    }

    protected override void Die()
    {
        Debug.Log("Player Died. Game Over!");
        // Add respawn or game-over logic here
    }
}

using UnityEngine;
using System.Threading.Tasks;

public class PlayerHealth : Health
{
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.D))  // Press "D" to take damage (for testing purposes)
        // {
        //     TakeDamage(10);  // Damage the player by 10
        // }

        if (Input.GetKeyDown(KeyCode.H))  // Press "H" to heal
        {
            Heal(10);  // Heal the player by 10
        }

        if (Input.GetKeyDown(KeyCode.O))  // Press "H" to heal
        {
            Time.timeScale = 1;  // Heal the player by 10
        }
    }

    async protected override void Die()
    {
        Debug.Log("Player Died. Game Over!");
        await Task.Delay(500);
        Time.timeScale = 0;
        // Add respawn or game-over logic here
    }
}

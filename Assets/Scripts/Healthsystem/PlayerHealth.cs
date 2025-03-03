using UnityEngine;
using System.Threading.Tasks;

public class PlayerHealth : Health
{
    private VignetteOnHit vignetteOnHit;

    void Start()
    {
        base.Awake();
        // Find the VignetteOnHit component in the scene
        vignetteOnHit = FindObjectOfType<VignetteOnHit>();
        if (vignetteOnHit == null)
        {
            Debug.LogError("VignetteOnHit component not found in the scene");
        }
    }

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

        if (Input.GetKeyDown(KeyCode.O))  // Press "O" to resume time
        {
            Time.timeScale = 1;
        }
    }

    public override void TakeDamage(float damage, Transform attacker = null)
    {
        base.TakeDamage(damage, attacker);

        // Show vignette effect when taking damage
        if (vignetteOnHit != null)
        {
            VignetteOnHit.ShowVignetteOnHit();
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

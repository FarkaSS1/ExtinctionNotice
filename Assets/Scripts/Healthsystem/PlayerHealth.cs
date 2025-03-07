using UnityEngine;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

public class PlayerHealth : Health
{
    public UIManagerBot uiManagerBot; // Reference to the UIManagerBot
    public Transform respawnPoint; // Reference to the respawn point
    public CameraModeManager cameraModeManager; // Reference to the CameraModeManager
    public GameStateManager gameStateManager; // Reference to the GameStateManager
    public int respawnCost = 750;
    private string resourceType = "elementX";
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
        // Ensure the buy back button is hidden initially
        if (uiManagerBot != null)
        {
            uiManagerBot.HideBuyBackButton();
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
        Debug.Log("Player Died.");
        Time.timeScale = 1;
        await Task.Delay(500);
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        gameObject.SetActive(false);
        if (cameraModeManager != null)
        {
            cameraModeManager.SwitchMode(CameraModeManager.GameMode.BaseViewMode); // Switch to base view camera
        }
        if (uiManagerBot != null)
        {
            uiManagerBot.ShowBuyBackButton();
        }
        if (cameraModeManager != null)
        {
            cameraModeManager.SwitchMode(CameraModeManager.GameMode.BaseViewMode); // Switch to base view camera
        }
    }

    public void RespawnOmega()
    {
        if (gameStateManager != null && gameStateManager.CanAfford(resourceType, respawnCost))
        {   
                gameStateManager.RemoveResources(resourceType, respawnCost);
                Debug.Log("O.M.E.G.A is brought back!");
                Time.timeScale = 1;

                transform.position = respawnPoint.position;
                transform.rotation = respawnPoint.rotation;
            
                gameObject.SetActive(true);
                Heal(100);

                foreach (var enemy in FindObjectsOfType<EnemyAI>())
                {
                enemy.UpdatePlayerReference();
                }
                if (uiManagerBot != null)
                {
                    uiManagerBot.HideBuyBackButton();
                }
                if (cameraModeManager != null)
                {
                    cameraModeManager.SwitchMode(CameraModeManager.GameMode.PlayerMode); // Switch to main camera
                }
            }
        }
        
}
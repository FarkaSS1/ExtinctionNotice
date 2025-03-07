using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class BaseHealth : Health
{
    public GameObject gameOverPanel; // Assign the game-over panel in the Inspector
    private VignetteOnHit vignetteOnHit;

    protected override void Awake()
    {
        base.Awake();
        // Find the VignetteOnHit component in the scene
        vignetteOnHit = FindObjectOfType<VignetteOnHit>();
        if (vignetteOnHit == null)
        {
            Debug.LogError("VignetteOnHit component not found in the scene");
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
        Debug.Log("Base got destroyed. Game Over!");
        await Task.Delay(500);
        Time.timeScale = 0;
        ShowGameOverUI();
    }

    private void ShowGameOverUI()
    {
        if (gameOverPanel != null)
        {
            // Disable other UI elements
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            foreach (Canvas canvas in canvases)
            {
                foreach (Transform child in canvas.transform)
                {
                    if (child.gameObject != gameOverPanel)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }

            gameOverPanel.SetActive(true);
            Cursor.visible = true; // Make the cursor visible
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        }
    }
}

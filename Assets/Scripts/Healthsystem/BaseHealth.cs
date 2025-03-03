using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class BaseHealth : Health
{
    public GameObject gameOverPanel; // Assign the game-over panel in the Inspector

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

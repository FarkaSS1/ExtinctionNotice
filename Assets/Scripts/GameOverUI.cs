using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void Start()
    {
        // Find the GameObject with the BaseHealth script and assign the GameOverPanel
        BaseHealth baseHealth = FindObjectOfType<BaseHealth>();
        if (baseHealth != null)
        {
            baseHealth.gameOverPanel = gameOverPanel;
        }
        else
        {
            Debug.LogError("BaseHealth script not found in the scene.");
        }
    }

    private void Update()
    {
        // Check if the Enter key is pressed and the game-over panel is active
        if (gameOverPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Resume the game
        Cursor.visible = false; // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}

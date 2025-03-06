using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePauseMenu : MonoBehaviour
{
    public CanvasGroup pauseScreen; // Assign this in the Inspector
    public Button resumeButton; // Assign in Inspector
    public Button controlsButton; // Assign in Inspector
    public Button gameInfoButton; // Assign in Inspector
    public Button resetButton; // Assign in Inspector
    public Button quitButton; // Assign in Inspector
    public RawImage controlsImage; // Assign in Inspector
    public RawImage gameInfoImage; // Assign in Inspector

    private bool isPaused = false;

    void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        controlsButton.onClick.AddListener(ShowControls);
        gameInfoButton.onClick.AddListener(ShowGameInfo);
        resetButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);

        // Ensure pause screen is hidden at start
        pauseScreen.alpha = 0f;
        pauseScreen.gameObject.SetActive(false);
        pauseScreen.interactable = false;
        pauseScreen.blocksRaycasts = false;

        // Hide both images initially
        controlsImage.gameObject.SetActive(false);
        gameInfoImage.gameObject.SetActive(false);
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0; // Freeze the game
        StartCoroutine(FadeIn());

        // Unlock and show cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ResumeGame()
    {
        StartCoroutine(FadeOut());
    }

    private void ShowControls()
    {
        controlsImage.gameObject.SetActive(true);
        gameInfoImage.gameObject.SetActive(false);
    }

    private void ShowGameInfo()
    {
        controlsImage.gameObject.SetActive(false);
        gameInfoImage.gameObject.SetActive(true);
    }

    private void RestartGame()
    {
        Time.timeScale = 1; // Resume the game
        Cursor.visible = false; // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        SceneManager.LoadScene("MainScene"); // Reload the current scene
    }
    private void QuitGame()
    {
        #if UNITY_EDITOR
        // Stop play mode in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Quit the application when built (.exr)
        Application.Quit();
        #endif
    }



    IEnumerator FadeIn()
    {
        float fadeDuration = 0.5f;
        float time = 0f;
        pauseScreen.gameObject.SetActive(true);

        // Ensure the Canvas Group is interactable and blocks raycasts
        pauseScreen.interactable = true;
        pauseScreen.blocksRaycasts = true;

        // Ensure buttons are rendered on top
        Canvas canvas = pauseScreen.GetComponent<Canvas>();
        if (canvas)
        {
            canvas.sortingOrder = 10;  // Set a higher sorting order if necessary
        }

        while (time < fadeDuration)
        {
            pauseScreen.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        pauseScreen.alpha = 1f;

        // Debugging: Check if buttons are interactable
        Debug.Log("Pause screen buttons should now be interactable.");
    }

    IEnumerator FadeOut()
    {
        float fadeDuration = 0.5f;
        float time = 0f;

        while (time < fadeDuration)
        {
            pauseScreen.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        pauseScreen.alpha = 0f;
        pauseScreen.gameObject.SetActive(false);

        // Disable interaction and raycasting
        pauseScreen.interactable = false;
        pauseScreen.blocksRaycasts = false;

        Time.timeScale = 1; // Resume the game
        isPaused = false;

        // Lock and hide cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

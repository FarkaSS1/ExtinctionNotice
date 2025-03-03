using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeScreen : MonoBehaviour
{
    public AudioClip audioClip;  // Assign this in the Inspector
    public AudioClip audioClipAmbience;  // Assign this in the Inspector
    private AudioSource audioSource;
    private CanvasGroup canvasGroup;  // Reference to the CanvasGroup

    async Task Start() {
        // Get the components
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();

        // Initially set the canvas to be invisible (alpha 0)
        canvasGroup.alpha = 0;

        // Wait for 2 seconds before freezing the game
        await Task.Delay(2000);  // Wait for 2000 milliseconds (2 seconds)

        // Play the ambient audio
        if (audioClipAmbience != null && audioSource != null) {
            audioSource.PlayOneShot(audioClipAmbience);
        }

        // Fade in the canvas
        StartCoroutine(FadeIn());

        

        // Freeze the game after waiting
        Time.timeScale = 0;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartGame();
        }
    }

    IEnumerator FadeIn() {
        float fadeDuration = 1f; // Duration of the fade in
        float time = 0f;

        while (time < fadeDuration) {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            time += Time.unscaledDeltaTime; // Use unscaled delta time for fade duration
            yield return null;
        }

        canvasGroup.alpha = 1f;  // Ensure it ends with full opacity
    }

    private void StartGame()
    {
        Debug.Log("Game Starting...");

        // Play the main audio clip
        if (audioClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(audioClip);
        }

        // Fade out the canvas before destroying it
        StartCoroutine(FadeOut());
        // Fade out the audio (if needed)
        StartCoroutine(FadeOutAudio(1f));  // Fade out over 1 second

        // Resume the game
        Time.timeScale = 1;

        // Enable HUD
        GameObject hud = GameObject.FindWithTag("HUD");
        if (hud != null)
        {
            hud.SetActive(true);
        }

        // Unlock and show the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Ensure the game starts in BaseViewMode
        CameraModeManager camManager = FindObjectOfType<CameraModeManager>();
        if (camManager != null)
        {
            camManager.SwitchMode(CameraModeManager.GameMode.BaseViewMode);
        }
    }


    IEnumerator FadeOut() {
        float fadeDuration = 1f; // Duration of the fade out
        float time = 0f;

        while (time < fadeDuration) {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            time += Time.unscaledDeltaTime; // Use unscaled delta time for fade duration
            yield return null;
        }

        canvasGroup.alpha = 0f;  // Ensure it ends with full transparency

        // Destroy the canvas (or welcome screen)
        Destroy(gameObject);
    }

    // Coroutine to fade out the audio
    IEnumerator FadeOutAudio(float fadeDuration) {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0f) {
            audioSource.volume -= startVolume * Time.unscaledDeltaTime / fadeDuration;
            yield return null;
        }

        audioSource.volume = 0f;  // Ensure it ends at zero volume
    }
}

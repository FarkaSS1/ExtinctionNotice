using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public CanvasGroup titleScreen; // Assign this in the Inspector
    public Button startButton; // Assign in Inspector
    public Button controlsButton; // Assign in Inspector
    public Button gameInfoButton; // Assign in Inspector
    public Button quitButton; // Assign in Inspector
    public RawImage controlsImage; // Assign in Inspector
    public RawImage gameInfoImage; // Assign in Inspector

    public AudioSource audioSource;
    public AudioClip titleScreenMusic;
    public AudioClip startButtonSound;


    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        controlsButton.onClick.AddListener(ShowControls);
        gameInfoButton.onClick.AddListener(ShowGameInfo);
        quitButton.onClick.AddListener(QuitGame);


        controlsImage.gameObject.SetActive(false);
        gameInfoImage.gameObject.SetActive(false);

        audioSource.clip = titleScreenMusic;
        audioSource.loop = true;
        audioSource.Play();
    }


    private void StartGame()
    {
        audioSource.PlayOneShot(startButtonSound);
        Time.timeScale = 1; // Resume the game
        Cursor.visible = false; // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        SceneManager.LoadScene("IntroCinematic"); 
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
}

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class RotatePlanet : MonoBehaviour
{
    [SerializeField] private float rotationSpeedY = -5f; // Y-axis rotation speed
    [SerializeField] private float rotationSpeedZ = 3f;  // Z-axis rotation speed
    private PlayableDirector playableDirector;

    void Start()
    {
        playableDirector = FindObjectOfType<PlayableDirector>(); // Find the timeline ONCE

        if (!playableDirector)
        {
            Debug.LogError("IntroScene > RotatePlanet.cs: PlayableDirector object not found");
            return;
        }

        // Subscribe to stopped event only once
        playableDirector.stopped += OnTimelineFinished;
    }

    void Update()
    {
        // Rotate around both Y-axis and Z-axis
        transform.Rotate(new Vector3(0, rotationSpeedY, rotationSpeedZ) * Time.deltaTime);

        // If Z key is pressed, load the main scene
        if (Input.GetKeyDown(KeyCode.Z))
        {
            LoadMainScene();
        }
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        LoadMainScene();
    }

    private void LoadMainScene()
    {
        Time.timeScale = 1; // Resume the game
        Cursor.visible = false; // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        SceneManager.LoadScene("MainScene");
    }

    void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks (good)
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnTimelineFinished;
        }
    }
}

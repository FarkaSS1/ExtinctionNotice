using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModeManager : MonoBehaviour
{
    public enum GameMode { PlayerMode, BaseViewMode }
    public GameMode currentMode = GameMode.PlayerMode;

    [Header("References")]
    public GameObject thirdPersonCam;
    public GameObject baseViewCam;
    public GameObject player;
    public PlayerController2 playerMovementScript; // The script handling player movement (e.g., ThirdPersonController)
    public float baseViewMoveSpeed = 10f;
    public float zoomSpeed = 5f;
    public Transform baseCameraTransform; // Drag your BaseViewCam here in the inspector

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Make sure we start in Player Mode
        SwitchMode(GameMode.PlayerMode);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) // Press Tab to switch modes
        {
            if (currentMode == GameMode.PlayerMode)
                SwitchMode(GameMode.BaseViewMode);
            else
                SwitchMode(GameMode.PlayerMode);
        }
        if (currentMode == GameMode.BaseViewMode)
        {
            float h = Input.GetAxis("Horizontal") * baseViewMoveSpeed * Time.deltaTime;
            float v = Input.GetAxis("Vertical") * baseViewMoveSpeed * Time.deltaTime;
            float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

            baseCameraTransform.position += new Vector3(h, -scroll, v);
        }

    }

    private void SwitchMode(GameMode newMode)
    {
        currentMode = newMode;

        if (newMode == GameMode.PlayerMode)
        {
            thirdPersonCam.SetActive(true);
            baseViewCam.SetActive(false);
            if (playerMovementScript != null) playerMovementScript.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (newMode == GameMode.BaseViewMode)
        {
            thirdPersonCam.SetActive(false);
            baseViewCam.SetActive(true);
            if (playerMovementScript != null) playerMovementScript.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            baseViewCam.GetComponent<BaseCameraController>().ResetToHub();

        }
    }


}

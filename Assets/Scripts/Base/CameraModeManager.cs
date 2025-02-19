using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModeManager : MonoBehaviour
{
    public enum GameMode { PlayerMode, BaseViewMode }
    public GameMode currentMode = GameMode.PlayerMode;

    [Header("References")]
    public GameObject mainCamera;
    public GameObject baseViewCam;
    public GameObject player;
    public PlayerController playerMovementScript; // The script handling player movement 
    public float baseViewMoveSpeed = 10f;
    public float zoomSpeed = 5f;
    public Transform baseCameraTransform; // Drag your BaseViewCam here in the inspector
    public GameObject HUD; // Drag your HUD here in the inspector
    public SelectionManager selectionManager; // Reference to SelectionManager
    public GameObject weaponHolder; // Drag the Weapon Holder object here in the Inspector
    public GameObject health; 



    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
            mainCamera.SetActive(true);
            baseViewCam.SetActive(false);
            if (playerMovementScript != null)
            {
                playerMovementScript.enabled = true; // Enable player movement
            }

            if (weaponHolder != null)
            {
                weaponHolder.SetActive(true);
            }

            if (health != null)
            {
                health.SetActive(true);
            }

            // Disable Base View Camera's AudioListener & Enable Player Camera's
            mainCamera.GetComponent<AudioListener>().enabled = true;
            baseViewCam.GetComponent<AudioListener>().enabled = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            HUD.SetActive(false);  // Hide HUD when in Player Mode

            if (selectionManager != null) selectionManager.enabled = false; // Disable selection
                                                                            // Update Camera Tags
            mainCamera.tag = "MainCamera";
            baseViewCam.tag = "Untagged"; // Remove MainCamera tag
    
            selectionManager.Deselect(); // Deselect any selected objects
        }
        else if (newMode == GameMode.BaseViewMode)
        {
            mainCamera.SetActive(false);
            baseViewCam.SetActive(true);
            if (playerMovementScript != null)
            {
                playerMovementScript.enabled = false; // Disable player movement!

            }

            if (weaponHolder != null)
            {
                weaponHolder.SetActive(false);
            }

            if (health != null)
            {
                health.SetActive(false);
            }

            // Disable Player Camera's AudioListener & Enable Base View Camera's
            mainCamera.GetComponent<AudioListener>().enabled = false;
            baseViewCam.GetComponent<AudioListener>().enabled = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            baseViewCam.GetComponent<BaseCameraController>().ResetToHub();
            HUD.SetActive(true);  // Hide HUD when in Player Mode

            if (selectionManager != null) selectionManager.enabled = true; // Enable selection

            // Update Camera Tags
            mainCamera.tag = "Untagged"; // Remove MainCamera tag
            baseViewCam.tag = "MainCamera";



        }
    }


}

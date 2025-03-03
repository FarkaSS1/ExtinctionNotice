using System.Collections;
using UnityEngine;
using UnityEngine.Events; // Add this for UnityEvent support

public class CameraModeManager : MonoBehaviour
{
    public enum GameMode { PlayerMode, BaseViewMode }
    public GameMode currentMode = GameMode.PlayerMode;

    [Header("References")]
    public GameObject mainCamera;
    public GameObject baseViewCam;
    public GameObject player;
    public PlayerController playerMovementScript;
    public float baseViewMoveSpeed = 10f;
    public float zoomSpeed = 5f;
    public Transform baseCameraTransform;
    public GameObject HUD;
    public SelectionManager selectionManager;
    public GameObject weaponHolder;
    public GameObject health;

    // Events for mode changes
    public UnityEvent OnPlayerModeActivated;
    public UnityEvent OnBaseViewModeActivated;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Start in BaseViewMode, but keep HUD disabled initially
        SwitchMode(GameMode.BaseViewMode);
        HUD.SetActive(false);
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

    public void SwitchMode(GameMode newMode)
    {
        currentMode = newMode;

        if (newMode == GameMode.PlayerMode)
        {
            mainCamera.SetActive(true);
            baseViewCam.SetActive(false);
            if (playerMovementScript != null)
            {
                playerMovementScript.enabled = true;
            }

            if (weaponHolder != null)
            {
                weaponHolder.SetActive(true);
            }

            if (health != null)
            {
                health.SetActive(true);
            }

            mainCamera.GetComponent<AudioListener>().enabled = true;
            baseViewCam.GetComponent<AudioListener>().enabled = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Canvas[] canvases = HUD.GetComponentsInChildren<Canvas>(true);
            foreach (var canvas in canvases)
            {
                if (canvas.gameObject.name == "Canvas")
                {
                    canvas.gameObject.SetActive(false);
                }
                else if (canvas.gameObject.name == "BottomCanvas")
                {
                    canvas.gameObject.SetActive(false);
                }
            }

            if (selectionManager != null) selectionManager.enabled = false;
            selectionManager.Deselect();

            mainCamera.tag = "MainCamera";
            baseViewCam.tag = "Untagged";

            // Trigger event for Player Mode
            OnPlayerModeActivated?.Invoke();
        }
        else if (newMode == GameMode.BaseViewMode)
        {
            mainCamera.SetActive(false);
            baseViewCam.SetActive(true);
            if (playerMovementScript != null)
            {
                playerMovementScript.enabled = false;
            }

            if (weaponHolder != null)
            {
                weaponHolder.SetActive(false);
            }

            if (health != null)
            {
                health.SetActive(false);
            }

            mainCamera.GetComponent<AudioListener>().enabled = false;
            baseViewCam.GetComponent<AudioListener>().enabled = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            baseViewCam.GetComponent<BaseCameraController>().ResetToHub();
            HUD.SetActive(true);

            Canvas[] canvases = HUD.GetComponentsInChildren<Canvas>(true);
            foreach (var canvas in canvases)
            {
                if (canvas.gameObject.name == "Canvas")
                {
                    canvas.gameObject.SetActive(true);
                }
                else if (canvas.gameObject.name == "BottomCanvas")
                {
                    canvas.gameObject.SetActive(true);
                }
            }

            if (selectionManager != null) selectionManager.enabled = true;

            mainCamera.tag = "Untagged";
            baseViewCam.tag = "MainCamera";

            // Trigger event for Base View Mode
            OnBaseViewModeActivated?.Invoke();
        }
    }

    public bool IsBaseViewCamActive()
    {
        return currentMode == GameMode.BaseViewMode;
    }
}

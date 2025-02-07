using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam2 : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;

    public GameObject thirdPersonCam;
    public GameObject combatCam;
    public GameObject topDownCam;

    public CameraStyle currentStyle;

    public enum CameraStyle
    {
        Basic,
        Combat,
        Topdown
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Auto-find the player if not assigned in the Inspector
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                orientation = player.Find("Orientation"); // Find the Orientation inside the Player
                this.playerObj = player.Find("PlayerObj"); // Find the PlayerObj inside the Player
                rb = this.playerObj.GetComponent<Rigidbody>(); // Get Rigidbody of the player
            }
        }

        // Auto-find the cameras if not assigned
        if (thirdPersonCam == null) thirdPersonCam = GameObject.Find("ThirdPersonCam");
        if (combatCam == null) combatCam = GameObject.Find("CombatCam");
        if (topDownCam == null) topDownCam = GameObject.Find("TopDownCam");

        // Ensure we start with the correct camera
        SwitchCameraStyle(CameraStyle.Basic);
    }


    private void Update()
    {
        // Switch styles
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCameraStyle(CameraStyle.Topdown);

        if (player == null || orientation == null) return; // Prevent errors if the player is missing

        // Rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        if (currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.Topdown)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
        else if (currentStyle == CameraStyle.Combat)
        {
            if (combatLookAt != null)
            {
                Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
                orientation.forward = dirToCombatLookAt.normalized;
                playerObj.forward = dirToCombatLookAt.normalized;
            }
        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        if (thirdPersonCam != null) thirdPersonCam.SetActive(false);
        if (combatCam != null) combatCam.SetActive(false);
        if (topDownCam != null) topDownCam.SetActive(false);

        if (newStyle == CameraStyle.Basic && thirdPersonCam != null) thirdPersonCam.SetActive(true);
        if (newStyle == CameraStyle.Combat && combatCam != null) combatCam.SetActive(true);
        if (newStyle == CameraStyle.Topdown && topDownCam != null) topDownCam.SetActive(true);

        currentStyle = newStyle;
    }
}

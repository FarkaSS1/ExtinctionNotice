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
    private CameraStyle[] cameraModes = { CameraStyle.Basic, CameraStyle.Combat, CameraStyle.Topdown };
    private int cameraIndex = 0;

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

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                orientation = player.Find("Orientation");
                this.playerObj = player.Find("PlayerObj");
                rb = this.playerObj.GetComponent<Rigidbody>();
            }
        }

        if (thirdPersonCam == null) thirdPersonCam = GameObject.Find("ThirdPersonCam");
        if (combatCam == null) combatCam = GameObject.Find("CombatCam");
        if (topDownCam == null) topDownCam = GameObject.Find("TopDownCam");

        // Ensure we start with the correct camera
        SwitchCameraStyle(cameraModes[cameraIndex]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCameraStyle(CameraStyle.Topdown);

        // **New feature: Press "C" to cycle through cameras**
        if (Input.GetKeyDown(KeyCode.C))
        {
            cameraIndex = (cameraIndex + 1) % cameraModes.Length;
            SwitchCameraStyle(cameraModes[cameraIndex]);
        }

        if (player == null || orientation == null) return;

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

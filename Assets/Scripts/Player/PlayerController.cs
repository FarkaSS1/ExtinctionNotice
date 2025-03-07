using Cinemachine;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    public CinemachineVirtualCamera virtualCamera;
    [SerializeField] private AudioSource footstepSound;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeedMultiplier = 2f;
    [SerializeField] private float sprintTransitSpeed = 5f;
    //[SerializeField] private float turningSpeed = 2f;
    [SerializeField] private float gravity = 0;
    [SerializeField] private float jumpHeight = 2f;

    private float verticalVelocity;
    private float currentSpeed;
    private float currentSpeedMultiplier;
    private float xRotation;

    [Header("Camera Bob Settings")]
    [SerializeField] private float bobFrequency = 1.5f;
    [SerializeField] private float bobAmplitude = 0.07f;
    //private float bobTimer = 0f;
    private readonly float bobSmoothing = 5.0f; // Higher = smoother stop
    private readonly float idleBobAmplitude = 0.1f;
    private readonly float idleBobFrequency = 0.2f;

    private CinemachineBasicMultiChannelPerlin noiseComponent;
    

    [Header("Recoil")]
    private Vector3 targetRecoil = Vector3.zero;
    private Vector3 currentRecoil = Vector3.zero;

    [Header("Footstep Settings")]
    [SerializeField] private LayerMask terrainLayerMask;
    [SerializeField] private float stepInterval = 1f;

    private float nextStepTimer = 0;

    [Header("SFX")]
    [SerializeField] private AudioClip[] groundFootsteps;
    [SerializeField] private AudioClip[] grassFootsteps;
    [SerializeField] private AudioClip[] gravelFootsteps;

    [Header("Input")]
    [SerializeField] private float mouseSensitivity;
    private float moveInput;
    private float turnInput;
    private float mouseX;
    private float mouseY;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        noiseComponent = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        InputManagement();
        Movement();

        PlayFootstepSound();
    }

    private void LateUpdate()
    {
        CameraBob();
    }

    private void Movement()
    {
        GroundMovement();
        Turn();
    }

    private void GroundMovement()
    {
        Vector3 move = new Vector3(turnInput, 0, moveInput);
        move = virtualCamera.transform.TransformDirection(move);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeedMultiplier = sprintSpeedMultiplier;
        }
        else
        {
            currentSpeedMultiplier = 1f;
        }

        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed * currentSpeedMultiplier, sprintTransitSpeed * Time.deltaTime);

        move *= currentSpeed;

        move.y = VerticalForceCalculation();

        controller.Move(move * Time.deltaTime);
    }

    private void Turn()
    {
        mouseX *= mouseSensitivity * Time.deltaTime;
        mouseY *= mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        virtualCamera.transform.localRotation = Quaternion.Euler(xRotation + currentRecoil.y, currentRecoil.x, 0);

        transform.Rotate(Vector3.up * mouseX);
    }

    public void ApplyRecoil(GunData gunData) {
        float recoilX = Random.Range(-gunData.maxRecoil.x, gunData.maxRecoil.x) * gunData.recoilAmount;
        float recoilY = Random.Range(-gunData.maxRecoil.y, gunData.maxRecoil.y) * gunData.recoilAmount;

        targetRecoil += new Vector3(recoilX, recoilY, 0);

        currentRecoil = Vector3.MoveTowards(currentRecoil, targetRecoil, Time.deltaTime * gunData.recoilSpeed); 
    }

    public void ResetRecoil(GunData gunData) {
        currentRecoil = Vector3.MoveTowards(currentRecoil, Vector3.zero, Time.deltaTime * gunData.resetRecoilSpeed); 
        targetRecoil = Vector3.MoveTowards(targetRecoil, Vector3.zero, Time.deltaTime * gunData.resetRecoilSpeed); 
    }

    private void CameraBob() {
        bool isMoving = controller.isGrounded && controller.velocity.magnitude > 0.1f;

        float targetAmplitude = isMoving 
            ? bobAmplitude * currentSpeedMultiplier 
            : idleBobAmplitude;

        float targetFrequency = isMoving 
            ? bobFrequency * currentSpeedMultiplier 
            : idleBobFrequency;

        noiseComponent.m_AmplitudeGain = Mathf.Lerp(noiseComponent.m_AmplitudeGain, targetAmplitude, Time.deltaTime * bobSmoothing);
        noiseComponent.m_FrequencyGain = Mathf.Lerp(noiseComponent.m_FrequencyGain, targetFrequency, Time.deltaTime * bobSmoothing);
    }


    private void PlayFootstepSound()
    {
        if (controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            if (Time.time >= nextStepTimer)
            {
                AudioClip[] footstepClips = DetermineAudioClips();

                if (footstepClips.Length > 0)
                {
                    AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

                    footstepSound.PlayOneShot(clip);
                }

                nextStepTimer = Time.time + (stepInterval / currentSpeedMultiplier);
            }
        }
    }

    private AudioClip[] DetermineAudioClips()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, 1.5f, terrainLayerMask))
        {
            string tag = hit.collider.tag;

            switch (tag)
            {
                case "Ground":
                    return groundFootsteps;
                case "Grass":
                    return grassFootsteps;
                case "Gravel":
                    return gravelFootsteps;
                default:
                    return groundFootsteps;
            }
        }
        return groundFootsteps;
    }

    private float VerticalForceCalculation()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -1;

            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * gravity * 2);
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        return verticalVelocity;
    }

    private void InputManagement()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }
}

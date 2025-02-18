using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public AudioSource audioSource;
    public GunData gunData;

    public Transform gunMuzzle;
    public GameObject bulletHolePrefab;
    public GameObject bulletHitParticlePrefab;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public Transform cameraTransform;
    
    [Header("Aiming")]
    private Vector3 defaultWeaponPosition;
    private Quaternion defaultWeaponRotation;
    public Transform weaponPosition; // Hip Fire Position
    public Transform adsPosition;    // Aim Down Sights Position
    private bool isAiming = false;
    private float adsSpeed = 15f; // Speed of movement
    
    [Header("Reload Config")]
    private float currentAmmo = 0f;
    private float nextTimeToFire = 0f;
    private bool isReloading = false;
    

private void Start() {
    currentAmmo = gunData.magazineSize;

    playerController = transform.parent.GetComponentInParent<PlayerController>();

    if (playerController == null) {
        Debug.LogError("PlayerController is null! Check if it is attached to " + transform.root.name);
        return;
    }

    if (playerController.virtualCamera == null) {
        Debug.LogError("virtualCamera is null in PlayerController!");
        return;
    }

    cameraTransform = playerController.virtualCamera.transform;
    defaultWeaponPosition = transform.localPosition;
    defaultWeaponRotation = transform.localRotation;
    audioSource = GetComponent<AudioSource>();
}


    public virtual void Update() {
        playerController.ResetRecoil(gunData);
        HandleAimDownSights();
    }

    public void TryReload() {
        if (!isReloading && currentAmmo < gunData.magazineSize) {
            StartCoroutine(Reload());
        }
    }

    public IEnumerator Reload() {
        isReloading = true;

        Debug.Log(gunData.gunName + " is reloading....");

        yield return new WaitForSeconds(gunData.reloadTime);

        currentAmmo = gunData.magazineSize;
        isReloading = false;

        Debug.Log(gunData.gunName + " is reloaded");
    }

    public void TryShoot() {
        if (isReloading) {
            Debug.Log(gunData.gunName + " is reloading....");
            return;
        }
        if ( currentAmmo < 1f) {
            Debug.Log(gunData.gunName + " has no bullets lefet, Please reload");
            return;
        }
        if (Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + (1 / gunData.fireRate);
            HandleShoot();
        }
    }
    
    private void HandleShoot() {
        currentAmmo--;
        Debug.Log(gunData.gunName + " Shot!, Bullets Left: " + currentAmmo);
        Shoot();

        playerController.ApplyRecoil(gunData);

        PlayFireSound();
    }

    private void PlayFireSound() {
        if(gunData.fireSound != null && audioSource != null) {
            audioSource.PlayOneShot(gunData.fireSound);
        }
    }


    void HandleAimDownSights() {
        bool aimingInput = Input.GetMouseButton(1); // Right-click for ADS

        // Toggle aiming state based on input
        isAiming = aimingInput;

        // Choose target position and rotation based on aiming state
        Vector3 targetPosition = isAiming ? adsPosition.localPosition : weaponPosition.localPosition;
        Quaternion targetRotation = isAiming ? adsPosition.localRotation : weaponPosition.localRotation;

        // Smoothly transition to the target position and rotation
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * adsSpeed);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * adsSpeed);

        // FOV change
        float targetFOV = isAiming ? 40f : 60f; // 40 FOV when aiming, 60 FOV for normal
        playerController.virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(playerController.virtualCamera.m_Lens.FieldOfView, targetFOV, Time.deltaTime * adsSpeed);
    }

    public abstract void Shoot();
}

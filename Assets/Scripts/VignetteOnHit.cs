using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteOnHit : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VolumeProfile volumeProfile;
    private static VignetteOnHit Instance;
    private Volume volume;

    [Header("Vignette Settings")]
    [SerializeField] private float vignetteIntensity = 0.4f;
    [SerializeField] private float vignetteFadeTime = 0.65f;
    private Vignette vignette;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        // Find references make sure to only have 1 volume in the scene
        volume = FindObjectOfType<Volume>();
        if (volume == null)
        {
            Debug.LogError("Volume component not found");
            return;
        }

        volume.profile = volumeProfile;
        if (volumeProfile == null)
        {
            Debug.LogError("VolumeProfile not assigned");
            return;
        }

        if (!volumeProfile.TryGet(out vignette))
        {
            Debug.LogError("Vignette effect not found in profile");
            return;
        }

        // Disable the vignette at the start
        vignette.intensity.value = 0;
    }

    public static void ShowVignetteOnHit()
    {
        Instance.StopAllCoroutines();
        Instance.StartCoroutine(Instance.ShowVignetteOnHitCoroutine());
    }

    private IEnumerator ShowVignetteOnHitCoroutine()
    {
        // Set the vignette intensity to the desired value
        vignette.intensity.value = vignetteIntensity;

        // Wait for a short time
        yield return new WaitForSeconds(0.1f);

        // Gradually fade out the vignette
        float elapsedTime = 0f;
        while (elapsedTime < vignetteFadeTime)
        {
            vignette.intensity.value = Mathf.Lerp(vignetteIntensity, 0, elapsedTime / vignetteFadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the vignette is fully disabled
        vignette.intensity.value = 0;
    }
}
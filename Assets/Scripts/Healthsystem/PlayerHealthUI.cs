using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Health playerHealth;  // Reference to the player's health component
    [SerializeField] private Slider healthSlider;  // Reference to the health slider
    private Coroutine healthLerpCoroutine;

    private void Start()
    {
        // Ensure healthSlider and playerHealth are set
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthBar;
        }
        else
        {
            Debug.LogError("PlayerHealth is not assigned in the HealthUI script!");
        }
    }


    private void UpdateHealthBar(float healthPercentage) {
        if (healthLerpCoroutine != null)
        {
            StopCoroutine(healthLerpCoroutine);
        }
        healthLerpCoroutine = StartCoroutine(SmoothHealthChange(healthPercentage));
    }

    private IEnumerator SmoothHealthChange(float targetValue) {
        float startValue = healthSlider.value;
        float elapsedTime = 0f;
        float duration = 0.5f; // Adjust for desired smoothness

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            healthSlider.value = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            yield return null;
        }

        healthSlider.value = targetValue; // Ensure final value is set
    }


    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;  // Unsubscribe to prevent memory leaks
        }
    }
}

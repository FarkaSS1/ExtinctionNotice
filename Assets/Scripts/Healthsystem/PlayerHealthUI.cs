using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Health playerHealth;  // Reference to the player's health component
    [SerializeField] private Slider healthSlider;  // Reference to the health slider

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

    private void UpdateHealthBar(float healthPercentage)
    {
        // Update the slider based on health percentage
        healthSlider.value = healthPercentage;
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;  // Unsubscribe to prevent memory leaks
        }
    }
}

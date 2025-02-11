using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health
{
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Transform healthBarOffset;

    private GameObject healthBarInstance;
    private Slider healthSlider;

    protected override void Awake()
    {
        base.Awake(); // Initialize health from base class

        // Instantiate the health bar prefab as a child of the enemy
        if (healthBarPrefab != null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, transform);
            healthBarInstance.transform.position = healthBarOffset.position;
            healthSlider = healthBarInstance.GetComponentInChildren<Slider>();

            if (healthSlider != null)
            {
                healthSlider.maxValue = 1;
                healthSlider.value = 1;
            }
            else
            {
                Debug.LogError("Health bar prefab is missing a Slider component!");
            }
        }

        // Subscribe to health change events using the base class
        this.OnHealthChanged += UpdateHealthBar;
        this.OnDeath += DestroyHealthBar;

        Debug.Log("Successfully subscribed to OnHealthChanged");
    }

    private void UpdateHealthBar(float healthPercentage)
    {
        Debug.Log($"Updating health bar: {healthPercentage}");
        if (healthSlider != null)
        {
            healthSlider.value = healthPercentage;
        }
    }

    private void DestroyHealthBar()
    {
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance);
        }
    }

    protected override void Die()
    {
        base.Die();
        DestroyHealthBar();
    }

    private void LateUpdate()
    {
        if (healthBarInstance != null && Camera.main != null)
        {
            healthBarInstance.transform.LookAt(Camera.main.transform);
        }
    }
}

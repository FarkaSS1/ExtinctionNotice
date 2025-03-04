using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealthUI : MonoBehaviour
{
    [SerializeField] private Health towerHealth;  // Reference to the Buildings' health component
    [SerializeField] private CanvasGroup canvasGroupPrefab;  // Reference to the Buildings' health component
    [SerializeField] private float halthBarHeightAboveBuilding = 8f;
    private Slider healthSlider;  // Reference to the health slider
    private Coroutine healthLerpCoroutine;
    private Transform buildingTransform;
    private CanvasGroup healthBarInstance;

    private void Start()
    {
        if (canvasGroupPrefab == null)
        {
            Debug.LogError("BuildingHealthUI.cs: No CanvasGroup prefab assigned! Assign it in the Inspector.", this);
            return;
        }

        buildingTransform = transform; 

        healthBarInstance = Instantiate(canvasGroupPrefab);
        if (healthBarInstance == null)
        {
            Debug.LogError("Failed to instantiate health bar prefab!");
            return;
        }

        Transform healthBarTransform = healthBarInstance.transform;
        if (healthBarTransform == null)
        {
            Debug.LogError("Health bar instance has no transform!");
            return;
        }

        healthBarTransform.position = buildingTransform.position + Vector3.up * 2f;
        healthBarTransform.SetParent(null); 

        FindHealthSlider();
        FindBuildingHealthScript();
    }
 



private void LateUpdate()
{
    if (healthBarInstance != null && buildingTransform != null)
    {
        // Keep health bar above the Building
        healthBarInstance.transform.position = buildingTransform.position + Vector3.up * halthBarHeightAboveBuilding;

        // Make the health bar face the camera
        if (Camera.main != null)
        {
            healthBarInstance.transform.LookAt(Camera.main.transform);
            healthBarInstance.transform.Rotate(0, 180, 0);  // Correct rotation to face player
        }
    }
}

    private void UpdateHealthBar(float healthPercentage)
    {
        if (healthPercentage <= 0) { OnDestroy(); }
        ;
        if (healthLerpCoroutine != null)
        {
            StopCoroutine(healthLerpCoroutine);
        }
        healthLerpCoroutine = StartCoroutine(SmoothHealthChange(healthPercentage));
    }

    private IEnumerator SmoothHealthChange(float targetValue)
    {
        float startValue = healthSlider.value;
        float elapsedTime = 0f;
        float duration = 0.5f; 

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            healthSlider.value = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            yield return null;
        }

        healthSlider.value = targetValue; 
    }


    private void OnDestroy()
    {
        if (towerHealth != null)
        {
            towerHealth.OnHealthChanged -= UpdateHealthBar;  // Unsubscribe to prevent memory leaks
        }

        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance.gameObject); 
        }
    }


    private void FindHealthSlider()
    {
        if (healthBarInstance != null)
        {
            healthSlider = healthBarInstance.GetComponentInChildren<Slider>();
        }

        if (healthSlider == null)
        {
            Debug.LogError("BuildingHealthUI.cs: No healthSlider component found in the instantiated health bar prefab!");
        }
    }

    private void FindBuildingHealthScript()
    {
        if (towerHealth != null)
        {
            towerHealth.OnHealthChanged += UpdateHealthBar;
        }
        else
        {
            Debug.LogError("PlayerHealth is not assigned in the HealthUI script!");
        }
    }
}

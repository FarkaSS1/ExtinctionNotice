using TMPro;
using UnityEngine;

public class UIManagerTop : MonoBehaviour
{
    public TMP_Text elementXText;
    public TMP_Text timeText;
    public TMP_Text baseHealthText;
    public TMP_Text towerCountText;
    public TMP_Text quarryCountText;

    private BaseHealth baseHealth;

    private void Start()
    {
        baseHealth = FindObjectOfType<BaseHealth>();
        if (baseHealth == null)
        {
            Debug.LogError("BaseHealth component not found in the scene");
        }
        UpdateUI();
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(UpdateUI), 0f, 1f);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(UpdateUI));
    }

    public void UpdateUI()
    {
        elementXText.text = "Element X: " + GameStateManager.Instance.ElementX;
        timeText.text = GameStateManager.Instance.GetGameTimeString();

        if (baseHealth != null)
        {
            baseHealthText.text = "Base Health: " + baseHealth.GetCurrentHealth();
        }
        towerCountText.text = $"Towers: {GameStateManager.Instance.GetTowerCount()}/{GameStateManager.Instance.GetMaxTowers()}";
        quarryCountText.text = $"Quarries: {GameStateManager.Instance.GetQuarryCount()}/{GameStateManager.Instance.GetMaxQuarries()}";
    }
}

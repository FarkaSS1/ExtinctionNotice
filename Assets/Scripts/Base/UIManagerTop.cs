using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerTop : MonoBehaviour
{
    public TMP_Text goldText;
    public TMP_Text powerText;
    public TMP_Text elementXText;
    public TMP_Text timeText; 

    private void Start()
    {
        UpdateResourceUI();
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(UpdateResourceUI), 0f, 1f);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(UpdateResourceUI));
    }

    public void UpdateResourceUI()
    {
        goldText.text = "Gold: " + GameStateManager.Instance.Gold;
        powerText.text = "Power: " + GameStateManager.Instance.Power;
        elementXText.text = "Element X: " + GameStateManager.Instance.ElementX;
        timeText.text = GameStateManager.Instance.GetGameTimeString(); 
    }
}

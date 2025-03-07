using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (UIManagerBot.Instance != null)
            {
                UIManagerBot.Instance.HandleBuildTowerButtonClick();
            }
            else
            {
                Debug.LogError("UIManagerBot instance is not set.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            if (UIManagerBot.Instance != null)
            {
                UIManagerBot.Instance.HandleBuildMineButtonClick();
            }
            else
            {
                Debug.LogError("UIManagerBot instance is not set.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (UIManagerBot.Instance != null)
            {
                UIManagerBot.Instance.HandleSellButtonClick();
            }
            else
            {
                Debug.LogError("UIManagerBot instance is not set.");
            }
        }

    }
}

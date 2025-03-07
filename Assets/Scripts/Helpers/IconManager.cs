using UnityEngine;
using System.Collections.Generic;

public static class IconManager
{
    private static Dictionary<string, Sprite> iconDictionary = new Dictionary<string, Sprite>();

    public static void InitializeIcons()
    {
        if (iconDictionary.Count > 0) return; // Avoid reloading if already initialized

        Sprite[] loadedIcons = Resources.LoadAll<Sprite>("simple/images");
        if (loadedIcons.Length == 0)
        {
            Debug.LogError("No icons found in Resources/simple/images!");
        }

        foreach (Sprite icon in loadedIcons)
        {
            iconDictionary[icon.name.ToLower()] = icon;
            Debug.Log($"Loaded icon: {icon.name}");
        }
    }

    public static Sprite GetIcon(string costType)
    {
        if (string.IsNullOrEmpty(costType)) return null;

        costType = costType.ToLower();
        if (iconDictionary.TryGetValue(costType, out Sprite icon))
        {
            return icon;
        }

        Debug.LogWarning($"Icon not found for cost type: {costType}");
        return null;
    }
}

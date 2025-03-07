using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    public List<Image> stripes; 
    private int activeStripes = 0;

    private void Start()
    {
        // Ensure all stripes are turned off at the start
        foreach (var stripe in stripes)
        {
            stripe.color = new Color(stripe.color.r, stripe.color.g, stripe.color.b, 0.3f); // Dimmed
        }
    }

    public void Upgrade()
    {
        if (activeStripes < stripes.Count)
        {
            stripes[activeStripes].color = new Color(stripes[activeStripes].color.r, stripes[activeStripes].color.g, stripes[activeStripes].color.b, 1f); // Brighten
            activeStripes++;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeType
{
    IncreaseTowerDamage,
    IncreaseTowerCapacity,
    LowerTowerCost,
    IncreaseMineCapacity,
    IncreaseMineProduction,
    LowerMineCost,
    IncreaseBaseHealth,
    LowerOMEGARespawnCost,
    IncreaseOMEGADamage,
    IncreaseOMEGAHealth

}

public class UpgradeButtonUI : MonoBehaviour
{
    public List<Image> stripes;
    public UpgradeType upgradeType;
    private int activeStripes = 0;
    public int upgradeCost = 100; // default cost per upgrade
    private string costType = "elementX";

    private void Start()
    {
        // all stripes are dimmed initially
        foreach (var stripe in stripes)
        {
            stripe.color = new Color(stripe.color.r, stripe.color.g, stripe.color.b, 0.3f);
        }
    }

    public void TryUpgrade()
    {
        if (activeStripes < stripes.Count && GameStateManager.Instance.CanAfford(costType, upgradeCost))
        {
            GameStateManager.Instance.RemoveResources(costType, upgradeCost);
            ApplyUpgrade();
            stripes[activeStripes].color = new Color(stripes[activeStripes].color.r, stripes[activeStripes].color.g, stripes[activeStripes].color.b, 1f);
            activeStripes++;
        }
        else
        {
            Debug.Log("Not enough resources or max upgrades reached.");
        }
    }

    private void ApplyUpgrade()
    {
        switch (upgradeType)
        {
            case UpgradeType.IncreaseTowerDamage:
                BasicTower.UpgradeTowerDamage();
                break;

            case UpgradeType.IncreaseTowerCapacity:
                GameStateManager.Instance.IncreaseMaxTowers();
                break;

            case UpgradeType.LowerTowerCost:
                BasicTower.UpgradeLowerCost();
                break;

            case UpgradeType.IncreaseMineCapacity:
                GameStateManager.Instance.IncreaseMaxQuarries();
                break;

            case UpgradeType.IncreaseMineProduction:
                Mine.UpgradeProduction();
                break;

            case UpgradeType.LowerMineCost:
                Mine.UpgradeLowerCost();
                break;

            case UpgradeType.LowerOMEGARespawnCost:
                PlayerHealth.UpgradeRespawnCost();
                break;

            case UpgradeType.IncreaseOMEGAHealth:
                Health.UpgradeMaxHealth();
                break;
            case UpgradeType.IncreaseOMEGADamage:
                Pistol.UpgradeFlatDamage();
                break;



            default:
                Debug.LogWarning("Unhandled upgrade type: " + upgradeType);
                break;
        }
    }

}

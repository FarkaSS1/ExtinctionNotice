using System;
using System.Collections;
using TMPro;
using UnityEngine;

class UpgradePistol : MonoBehaviour
{
    private Pistol pistolObject;
    private GameObject pistolSkin;
    private MeshRenderer laser;
    void OnEnable()
    {
        // Reference to object with Pistol script attached
        pistolObject = FindObjectOfType<Pistol>();
        if (!pistolObject) { Debug.LogError("Upgradepistol.cs: Object of type Pistol not found"); }


        // Reference the "PT-9M Pistol_skin02" GameObject
        pistolSkin = GameObject.Find("PT-9M Pistol_skin02");
        if (!pistolSkin) { Debug.LogError("UpgradePistol.cs: PT-9M Pistol_skin02 GameObject not found"); }
        
        // Reference to Laser 
        laser = pistolObject.GetComponentInChildren<MeshRenderer>();
        if (!laser) { Debug.LogError("UpgradePistol.cs: Laser MeshRenderer not found"); }
    }

    public void UpgradeWeapon()
    {
        OnEnable();
        pistolObject.PlayUpgradeSound();
        ChangeColorLaser();
        pistolObject.SetDamage( pistolObject.GetDamage() * 2 ); // Double the damage
        pistolObject.EnableSpotLight();
    }

    private void ChangeColorLaser()
    {
        if (laser != null)
        {
            // Load the material from the Resources folder
            Material newLaserMaterial = Resources.Load<Material>("VFX/LaserRed");

            if (newLaserMaterial != null)
            {
                laser.material = newLaserMaterial;
            }
            else
            {
                Debug.LogError("Laser material not found at the specified path.");
            }
        }
        else
        {
            Debug.LogError("Laser MeshRenderer is not assigned");
        }
    }
}

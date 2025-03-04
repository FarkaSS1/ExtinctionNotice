using System;
using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

class UpgradePistol : MonoBehaviour
{
    private Pistol pistolObject;
    private GameObject pistolSkin;

    private GameObject pistolSkinBody;
    private MeshRenderer laser;
    void OnEnable()
    {
        // Reference to object with Pistol script attached
        pistolObject = FindObjectOfType<Pistol>();
        if (!pistolObject) { Debug.LogError("Upgradepistol.cs: Object of type Pistol not found"); }


        // Reference the "PT-9M Pistol_skin02" GameObject
        pistolSkin = GameObject.Find("PT-9M Pistol_skin02");
        if (!pistolSkin) { Debug.LogError("UpgradePistol.cs: PT-9M Pistol_skin02 GameObject not found"); }

        // Reference the "Detail.001" GameObject in "PT-9M Pistol_skin02" (pistol body)
        pistolSkinBody = GameObject.Find("Detail.001");
        if (!pistolSkinBody) { Debug.LogError("UpgradePistol.cs: Detail.001 GameObject not found"); }

        // Reference to Laser 
        laser = pistolObject.GetComponentInChildren<MeshRenderer>();
        if (!laser) { Debug.LogError("UpgradePistol.cs: Laser MeshRenderer not found"); }
    }

    public void UpgradeWeapon()
    {
        OnEnable();
        pistolObject.PlayUpgradeSound();
        ChangeMaterialPistolSkinBody();
        ChangeColorLaser();
        pistolObject.SetDamage(pistolObject.GetDamage() * 2); // Double the damage
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


    private void ChangeMaterialPistolSkinBody()
    {
        if (pistolSkinBody != null)
        {
            MeshRenderer pistolSkinBodyRenderer = pistolSkinBody.GetComponentInChildren<MeshRenderer>();

            if (pistolSkinBodyRenderer != null)
            {
                // Load the material from the Resources folder
                Material newMaterial = Resources.Load<Material>("VFX/pistolUpgradeMaterial");

                if (newMaterial != null)
                {
                    // Apply the texture to the material's main texture property (_MainTex or _BaseMap)
                    pistolSkinBodyRenderer.material = newMaterial;
                }
                else
                {
                    Debug.LogError("Failed to load the new base map texture.");
                }
            }
            else
            {
                Debug.LogError("pistolSkinBody MeshRenderer not found.");
            }
        }
        else
        {
            Debug.LogError("pistolSkinBody is not assigned.");
        }
    }

}

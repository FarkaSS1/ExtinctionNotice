using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu (fileName = "NewTowerTurretData", menuName = "TowerTurret/TowerTurretData")]
public class TurretData : ScriptableObject
{
    public string turretName;
    public LayerMask targetLayerMask;
    public float rotationSpeed;

    [Header("Weapon Damage")]
    public float damage;

    [Header("Fire Config")]
    public float shootingRange;
    public float fireRate;


    [Header("VFX")]
    public GameObject bulletTrailPrefab;
    public float bulletSpeed;

    [Header("SFX")]
    public AudioClip fireSound;

}

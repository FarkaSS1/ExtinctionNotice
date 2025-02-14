using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pistol : Gun
{

    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButton(0)) {
            TryShoot();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            TryReload();
        }
        
    }

    public override void Shoot()
{
    RaycastHit hit;
    Vector3 target = Vector3.zero;

    // Use the camera's forward direction, adjusted for sway
    Vector3 shootDirection = Camera.main.transform.forward;

    // Raycast to determine where the bullet will hit
    if (Physics.Raycast(cameraTransform.position, shootDirection, out hit, gunData.shootingRange, gunData.targetLayerMask)) {
        Debug.Log(gunData.gunName + " hit " + hit.collider.name);
        target = hit.point;
        if (hit.collider.TryGetComponent<IHealth>(out var health)) {
            health.TakeDamage(gunData.damage); // Deal damage to the enemy
        }
    } else {
        target = cameraTransform.position + (shootDirection * gunData.shootingRange);
    }

    // Start bullet firing coroutine with the calculated target
    StartCoroutine(BulletFire(target, hit));
}


    private IEnumerator BulletFire(Vector3 target, RaycastHit hit) {
        GameObject bulletTrail = Instantiate(gunData.bulletTrailPrefab, gunMuzzle.position, Quaternion.identity);  
        while(bulletTrail != null && Vector3.Distance(bulletTrail.transform.position, target) > 0.1f) {
            bulletTrail.transform.position = Vector3.MoveTowards(bulletTrail.transform.position, target, Time.deltaTime * gunData.bulletSpeed);
            yield return null;
        }
        Destroy(bulletTrail);

        if (hit.collider != null) {
            BulletHitFX(hit);
        }
    }

    private void BulletHitFX(RaycastHit hit) {
        Vector3 hitPosition = hit.point + hit.normal * 0.1f;

        GameObject bulletHole = Instantiate(bulletHolePrefab, hitPosition, Quaternion.LookRotation(hit.normal));
        GameObject hitParticle = Instantiate(bulletHitParticlePrefab, hit.point, Quaternion.LookRotation(hit.normal));

        bulletHole.transform.parent = hit.collider.transform;
        hitParticle.transform.parent = hit.collider.transform;

        Destroy(bulletHole, 1f);
        Destroy(hitParticle, 1f);
    }
}

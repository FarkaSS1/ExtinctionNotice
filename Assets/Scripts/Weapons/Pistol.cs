using System.Collections;
using UnityEngine;

public class Pistol : Gun, IAttacker
{
    private Shooter shooter;

    public void Awake()
    {
        shooter = GetComponent<Shooter>();

        if (shooter == null)
        {
            Debug.LogError("Shooter component missing on Pistol!");
        }
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButton(0))
        {
            TryShoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
        }
    }

    public override void Shoot()
    {
        if (shooter == null) return;

        Transform playerTransform = GameObject.FindWithTag("Player")?.transform;

        if (playerTransform == null)
        {
            Debug.LogError("Pistol.Shoot(): Player transform is NULL! Make sure the Player has the 'Player' tag.");
            return;
        }

        RaycastHit hit;
        Vector3 target = shooter.Shoot(
            cameraTransform.position,  // Start position
            cameraTransform.forward,   // Shooting direction
            gunData.shootingRange,
            gunData.damage,
            gunData.targetLayerMask,
            playerTransform,                 // Pass the player as the attacker
            out hit
        );

        if (hit.collider != null)
        {
            EnemyAI enemyAI = hit.collider.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.AggroEnemy(transform); // Pass player as the attacker
            }

            BulletHitFX(hit);
        }

        StartCoroutine(BulletFire(target, hit));
    }


    private IEnumerator BulletFire(Vector3 target, RaycastHit hit)
    {
        GameObject bulletTrail = Instantiate(gunData.bulletTrailPrefab, gunMuzzle.position, Quaternion.identity);
        while (bulletTrail != null && Vector3.Distance(bulletTrail.transform.position, target) > 0.1f)
        {
            bulletTrail.transform.position = Vector3.MoveTowards(bulletTrail.transform.position, target, Time.deltaTime * gunData.bulletSpeed);
            yield return null;
        }
        Destroy(bulletTrail);

        if (hit.collider != null)
        {
            BulletHitFX(hit);
        }
    }

    private void BulletHitFX(RaycastHit hit)
    {
        Vector3 hitPosition = hit.point + hit.normal * 0.1f;

        GameObject bulletHole = Instantiate(bulletHolePrefab, hitPosition, Quaternion.LookRotation(hit.normal));
        GameObject hitParticle = Instantiate(bulletHitParticlePrefab, hit.point, Quaternion.LookRotation(hit.normal));

        bulletHole.transform.parent = hit.collider.transform;
        hitParticle.transform.parent = hit.collider.transform;

        Destroy(bulletHole, 1f);
        Destroy(hitParticle, 1f);
    }

    public float GetDamage()
    {
        return gunData.damage;
    }
}

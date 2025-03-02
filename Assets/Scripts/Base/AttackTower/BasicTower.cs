using System.Collections;
using UnityEngine;

class BasicTower : AttackTower, IAttacker
{
    private Shooter shooter;

    private void Awake()
    {
        shooter = GetComponent<Shooter>();
        if (shooter == null)
        {
            shooter = gameObject.AddComponent<Shooter>(); // Ensure it exists
        }
    }

    internal override int GetCost()
    {
        return 200;
    }

    internal override string GetCostType()
    {
        return "elementX";
    }

    public override void Shoot()
    {
        Transform target = GetCurrentTarget();
        if (target == null) return;

        RaycastHit hit;
        Vector3 shootDirection = (target.position - turretMuzzle.position).normalized;

        Vector3 targetPoint = shooter.Shoot(
            turretMuzzle.position,   // Tower's shooting position
            shootDirection,          // Direction toward the enemy
            turretData.shootingRange,
            turretData.damage,
            LayerMask.GetMask("Enemy"),
            transform,               // Pass the tower as the attacker!
            out hit
        );

        if (hit.collider != null)
        {
            EnemyAI enemyAI = hit.collider.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.AggroEnemy(transform); // Pass tower as the attacker
            }

            BulletHitFX(hit);
        }

        StartCoroutine(BulletFire(targetPoint, hit));
    }



    private IEnumerator BulletFire(Vector3 target, RaycastHit hit)
    {
        GameObject bulletTrail = Instantiate(turretData.bulletTrailPrefab, turretMuzzle.position, Quaternion.identity);

        while (bulletTrail != null && Vector3.Distance(bulletTrail.transform.position, target) > 0.1f)
        {
            bulletTrail.transform.position = Vector3.MoveTowards(bulletTrail.transform.position, target, Time.deltaTime * turretData.bulletSpeed);
            yield return null;
        }
        Destroy(bulletTrail);
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
        return turretData.damage;
    }
}

using System.Collections;
using UnityEngine;

class BasicTower : AttackTower
{
    public override void Shoot()
    {
        Transform target = GetCurrentTarget();
        if (target == null) return;

        RaycastHit hit;
        Vector3 shootDirection = (target.position - turretMuzzle.position).normalized;
        Vector3 targetPoint = target.position;

        if (Physics.Raycast(turretMuzzle.position, shootDirection, out hit, turretData.range, LayerMask.GetMask("Enemy")))
        {
            targetPoint = hit.point;
            if (hit.collider.TryGetComponent<IHealth>(out var health))
            {
                health.TakeDamage(turretData.damage);
            }
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
}

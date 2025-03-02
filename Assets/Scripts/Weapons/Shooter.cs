using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Vector3 Shoot(Vector3 shootFrom, Vector3 shootDirection, float range, float damage, LayerMask targetLayer, Transform attacker, out RaycastHit hit)
    {
        Vector3 targetPoint = shootFrom + shootDirection * range;

        if (Physics.Raycast(shootFrom, shootDirection, out hit, range, targetLayer))
        {
            targetPoint = hit.point;

            if (hit.collider.TryGetComponent<IHealth>(out var targetHealth))
            {
                targetHealth.TakeDamage(damage, attacker);
            }
        }

        return targetPoint;
    }
}

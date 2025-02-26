using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Vector3 Shoot(Transform cameraTransform, float range, float damage, LayerMask targetLayerMask, out RaycastHit hit)
    {
        Vector3 shootDirection = cameraTransform.forward;
        Vector3 target = cameraTransform.position + (shootDirection * range);

        if (Physics.Raycast(cameraTransform.position, shootDirection, out hit, range, targetLayerMask))
        {
            Debug.Log("Shot hit: " + hit.collider.name);
            target = hit.point;

            if (hit.collider.TryGetComponent<IHealth>(out var health))
            {
                health.TakeDamage(damage);
            }
        }

        return target;
    }
}

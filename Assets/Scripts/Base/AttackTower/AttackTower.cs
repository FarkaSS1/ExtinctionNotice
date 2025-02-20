using UnityEngine;
using System.Collections;
using System.Linq;

abstract class AttackTower : SelectableObject
{
    public TurretData turretData;
    public Transform turretMuzzle;
    private float nextTimeToFire = 0f;
    private Transform currentTarget; // The enemy currently being targeted

    [Header("VFX")]
    public GameObject bulletHolePrefab;
    public GameObject bulletHitParticlePrefab;

    [Header("SFX")]
    public AudioSource audioSource;

    public override void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating(nameof(FindTarget), 0f, 0.5f); // Check for targets every 0.5s
    }

    private void Update()
    {
        if (currentTarget != null)
        {
            RotateTowardsTarget();
            TryShoot();
        }
    }

    private void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, turretData.shootingRange, LayerMask.GetMask("Enemy"));

        if (colliders.Length == 0)
        {
            currentTarget = null;
            return;
        }

        // Find the closest enemy
        currentTarget = colliders
            .Select(c => c.transform)
            .OrderBy(t => Vector3.Distance(transform.position, t.position))
            .FirstOrDefault();
    }

    private void RotateTowardsTarget()
    {
        if (currentTarget == null) return;

        Vector3 direction = (currentTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Rotate only on Y-axis
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turretData.rotationSpeed);
    }

    public void TryShoot()
    {
        if (currentTarget != null && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1 / turretData.fireRate);
            HandleShoot();
        }
    }

    private void HandleShoot()
    {
        Debug.Log("AttackTower Fired: " + turretData.turretName);
        PlayFireSound();
        Shoot();
    }

    public Transform GetCurrentTarget() => currentTarget; // Allow subclass access to target

    private void PlayFireSound() {
        if(turretData.fireSound != null && audioSource != null) {
            audioSource.PlayOneShot(turretData.fireSound);
        }
    }

    public abstract void Shoot();
}

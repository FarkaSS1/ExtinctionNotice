using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackRange = 15f;
    public float fireRate = 2f;
    private float nextFireTime = 0f;

    private Transform player;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // Rotate toward the player
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // Stop moving while attacking
            if (Time.time >= nextFireTime)
            {
                if (GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
                {
                    GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
                }

                Attack();
                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            if (GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
            {
                GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
            }
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack"); // Triggers attack animation
    }

    // This function will be called by the animation event
    public void ShootProjectile()
    {
        if (firePoint == null || projectilePrefab == null || player == null)
        {
            Debug.LogError("Missing FirePoint, Projectile Prefab, or Player reference!");
            return;
        }

        // Calculate direction toward the player's exact position
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Rotate the Spitter to face the player before shooting
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = lookRotation; // Instantly face the player

        // Instantiate the projectile
        GameObject spit = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = spit.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = directionToPlayer * 10f;  // Shoots directly at player
        }

        Debug.Log("Projectile fired at the correct animation frame!");
    }
}

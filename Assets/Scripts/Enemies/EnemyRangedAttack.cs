using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedAttack : MonoBehaviour, IAttacker
{
    [SerializeField] private float attackRange = 15f;
    [SerializeField] private float enemyDamage = 8f;
    [SerializeField] private float attackCooldown = 2f;
    private float nextAttackTime = 0f;

    private Transform currentTarget;
    private Animator animator;
    public GameObject projectilePrefab;
    public Transform firePoint;
    private NavMeshAgent agent;
    private EnemyAI enemyAI;

    public float AttackRange => attackRange;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyAI = GetComponent<EnemyAI>();

        if (enemyAI != null)
        {
            currentTarget = enemyAI.GetCurrentTarget();
        }
    }

    void Update()
    {
        if (enemyAI != null)
        {
            currentTarget = enemyAI.GetCurrentTarget();
        }

        if (currentTarget == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

        if (distanceToTarget <= attackRange && Time.time >= nextAttackTime)
        {
            if (agent != null)
            {
                agent.isStopped = true; // Stop movement before attacking
                agent.velocity = Vector3.zero; // Ensure they fully stop
            }

            RotateTowardsTarget();
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
        else
        {
            if (agent != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                agent.isStopped = false;
            }
        }
    }

    void RotateTowardsTarget()
    {
        if (currentTarget == null) return;

        Vector3 direction = (currentTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Attack()
    {
        animator.SetTrigger("Attack");  // Triggers the attack animation
    }

    // Called by animation event
    public void TriggerShoot()
    {
        ShootProjectile();  // This ensures the animation calls the function correctly
    }

    public void ShootProjectile()
    {
        if (firePoint == null || projectilePrefab == null || currentTarget == null)
        {
            Debug.LogError("Missing FirePoint, Projectile Prefab, or Target reference!");
            return;
        }

        Vector3 directionToTarget = (currentTarget.position - firePoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = directionToTarget * 10f;  // Shoots toward the current target
        }

        Debug.Log(gameObject.name + " fired at " + currentTarget.name);

        // Resume movement after shooting
        if (agent != null)
        {
            agent.isStopped = false;
        }
    }

    public float GetDamage()
    {
        return enemyDamage;
    }
}

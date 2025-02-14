using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Transform baseTarget;
    private Transform player;
    public float playerDetectionRange = 10f;
    public bool isRangedEnemy = false;
    public float attackRange = 2f; // Melee attack range
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Find the base using the "Base" tag
        GameObject baseObject = GameObject.FindWithTag("Base");
        if (baseObject != null) baseTarget = baseObject.transform;

        // Find the player using the "Player" tag
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) player = playerObject.transform;

        // Set initial destination to base
        if (baseTarget != null) agent.SetDestination(baseTarget.position);
    }

    void Update()
    {
        if (baseTarget == null || agent == null) return;

        float distanceToPlayer = player != null ? Vector3.Distance(transform.position, player.position) : Mathf.Infinity;

        //  Check if the enemy should attack (melee or ranged)
        if (player != null && distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
        else
        {
            MoveTowardsTarget(distanceToPlayer);
        }

        // ?? Update Animator Speed
        float moveSpeed = agent.velocity.magnitude;
        animator.SetFloat("Speed", moveSpeed);
    }

    void MoveTowardsTarget(float distanceToPlayer)
    {
        if (player != null && distanceToPlayer <= playerDetectionRange)
        {
            if (isRangedEnemy)
            {
                // Keep distance if ranged
                if (distanceToPlayer > attackRange + 3f)
                {
                    agent.SetDestination(player.position);  // Move closer
                }
                else
                {
                    agent.SetDestination(transform.position);  // Stop moving and shoot
                }
            }
            else
            {
                // Melee enemy chases player
                agent.SetDestination(player.position);
            }
        }
        else
        {
            agent.SetDestination(baseTarget.position);
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack"); // Play Attack Animation
        Debug.Log("Enemy is attacking!");

        if (!isRangedEnemy)
        {
            if (player.GetComponent<HealthSystem>())
            {
                player.GetComponent<HealthSystem>().TakeDamage(15f); // Melee attack damage
            }
        }
    }
}

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
    public float attackRange = 6f; // Melee attack range
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;

    // Aggro System
    public float aggroDuration = 5f; // Time before losing aggro
    private bool isAggroed = false;

    public bool IsAggroed => isAggroed; // read-only property
    private float aggroTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Find the base using the "Base" tag
        GameObject baseObject = GameObject.FindWithTag("Base");
        if (baseObject != null) baseTarget = baseObject.transform;

        // Find the player using the "Player" tag
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No Player found! Is it missing in the scene?");
        }


        // Set initial destination to base
        if (baseTarget != null) agent.SetDestination(baseTarget.position);
    }

    void Update()
    {
        if (baseTarget == null || agent == null) return;

        float distanceToPlayer = player != null ? Vector3.Distance(transform.position, player.position) : Mathf.Infinity;

        // If enemy is aggroed, update aggro timer
        if (isAggroed)
        {
            aggroTimer -= Time.deltaTime;
            if (aggroTimer <= 0)
            {
                isAggroed = false; // Lose aggro after time
            }
        }

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

        // Update Animator Speed
        float moveSpeed = agent.velocity.magnitude;
        animator.SetFloat("Speed", moveSpeed);
    }

    void MoveTowardsTarget(float distanceToPlayer)
    {
        if ((player != null && distanceToPlayer <= playerDetectionRange) || isAggroed)
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
    // Called when the enemy gets attacked
    public void AggroEnemy()
    {
        if (agent == null || !agent.isOnNavMesh) // 
        {
            Debug.LogWarning(gameObject.name + " tried to aggro but is either dead or not on a NavMesh!");
            return;
        }

        isAggroed = true;
        aggroTimer = aggroDuration;
        agent.SetDestination(player.position);
        Debug.Log(gameObject.name + " is now aggroed!");

        AlertNearbyEnemies(); // Aggroes closeby enemies
    }


    // Notifies nearby enemies to aggro
    private void AlertNearbyEnemies()
    {
        float alertRadius = 10f; // Adjust for how far the aggro spreads
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, alertRadius);

        foreach (Collider col in nearbyEnemies)
        {
            EnemyAI enemy = col.GetComponent<EnemyAI>();
            if (enemy != null && enemy != this && !enemy.IsAggroed)
            {
                enemy.AggroEnemy();
                enemy.ResetAggroTimer(); // Reset timer only for enemies that were aggroed
            }
        }



    }

    public void ResetAggroTimer()
    {
        if (isAggroed)
        {
            aggroTimer = aggroDuration; // Reset aggro timer so enemy doesn't lose interest
        }
    }


}

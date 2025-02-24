using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Transform hive;
    private Transform player;
    public float detectionRange = 15f;
    public float attackRange = 10f;
    public float patrolRadius = 7f;
    public float attackCooldown = 3f;
    private float nextAttackTime = 0f;
    public bool isAggroed = false;
    private Vector3 patrolTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject hiveObject = GameObject.FindWithTag("Hive");
        if (hiveObject != null) hive = hiveObject.transform;

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) player = playerObject.transform;

        if (hive != null) SetNewPatrolTarget();
    }

    void Update()
    {
        if (player == null || hive == null || agent == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If player enters detection range or boss is aggroed
        if (distanceToPlayer <= detectionRange || isAggroed)
        {
            isAggroed = true; // Ensure aggro is set

            if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
            else
            {
                agent.SetDestination(player.position);  // Chase player
            }
        }
        else
        {
            PatrolAroundHive();
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void PatrolAroundHive()
    {
        if (Vector3.Distance(transform.position, patrolTarget) < 1f)
        {
            SetNewPatrolTarget();
        }
        agent.SetDestination(patrolTarget);
    }

    void SetNewPatrolTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += hive.position;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = navHit.position;
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        Debug.Log("Boss is attacking the player!");

        if (player.GetComponent<Health>())
        {
            player.GetComponent<Health>().TakeDamage(30f);
        }
    }

    //  This function will be called when the boss is shot
    public void AggroOnHit()
    {
        isAggroed = true;
        agent.SetDestination(player.position);
        Debug.Log("Boss has been shot and is now attacking the player!");
    }
}

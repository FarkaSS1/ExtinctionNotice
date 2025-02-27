using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Animator animator;
    private Transform baseTarget;
    private Transform player;
    private Transform currentTarget;
    private Transform towerTarget;
    private EnemyMeleeAttack meleeAttack; // Reference to attack script (if melee)
    private EnemyRangedAttack rangedAttack; // Reference to attack script (if ranged)

    [Header("Detection & Aggro")]
    public float playerDetectionRange = 10f;
    private bool isAggroed = false;
    private float aggroTimer = 0f;
    public float aggroDuration = 5f;

    public bool IsAggroed => isAggroed;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Assign attack components
        meleeAttack = GetComponent<EnemyMeleeAttack>();
        rangedAttack = GetComponent<EnemyRangedAttack>();

        // Find base
        GameObject baseObject = GameObject.FindWithTag("Base");
        if (baseObject != null) baseTarget = baseObject.transform;

        // Find player
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) player = playerObject.transform;
        else Debug.LogError("No Player found! Is it missing in the scene?");

        // Set default target to base
        currentTarget = baseTarget;
        if (baseTarget != null) agent.SetDestination(baseTarget.position);
    }

    protected virtual void Update()
    {
        if (baseTarget == null || agent == null) return;

        // If enemy is aggroed, update aggro timer
        if (isAggroed)
        {
            aggroTimer -= Time.deltaTime;
            if (aggroTimer <= 0)
            {
                isAggroed = false;
                currentTarget = baseTarget; // Lose aggro, reset to base
            }
        }

        MoveTowardsTarget();

        // Update Animator Speed
        float moveSpeed = agent.velocity.magnitude;
        animator.SetFloat("Speed", moveSpeed);
    }

    void MoveTowardsTarget()
    {
        if (currentTarget == null)
        {
            currentTarget = baseTarget; // Default to attacking base
        }

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

        // Adjust attack range if target is a tower
        float adjustedAttackRange = meleeAttack != null ? meleeAttack.AttackRange : 2f;
        if (currentTarget.CompareTag("Tower")) adjustedAttackRange += 2f;

        if (distanceToTarget <= adjustedAttackRange)
        {
            agent.isStopped = true; // Stop moving when close enough

            // Tell the attack component to attack
            if (meleeAttack != null) meleeAttack.TryAttack(currentTarget);
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(currentTarget.position);
        }
    }

    public void AggroEnemy(Transform attacker)
    {
        if (agent == null || !agent.isOnNavMesh)
        {
            Debug.LogWarning(gameObject.name + " tried to aggro but is not on a NavMesh or is dead!");
            return;
        }

        isAggroed = true;
        aggroTimer = aggroDuration;

        if (attacker.CompareTag("Tower"))
        {
            currentTarget = attacker;
        }
        else if (attacker.CompareTag("Player"))
        {
            currentTarget = player;
        }

        Debug.Log(gameObject.name + " is now aggroed onto " + currentTarget.name);
        agent.SetDestination(currentTarget.position);
        AlertNearbyEnemies(attacker);
    }

    private void AlertNearbyEnemies(Transform attacker)
    {
        float alertRadius = 10f;
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, alertRadius);

        foreach (Collider col in nearbyEnemies)
        {
            EnemyAI enemy = col.GetComponent<EnemyAI>();
            if (enemy != null && enemy != this && !enemy.IsAggroed)
            {
                enemy.AggroEnemy(attacker);
            }
        }
    }

    public void ResetAggroTimer()
    {
        if (isAggroed) aggroTimer = aggroDuration;
    }

    public Transform GetCurrentTarget()
    {
        return currentTarget ?? baseTarget;
    }
}

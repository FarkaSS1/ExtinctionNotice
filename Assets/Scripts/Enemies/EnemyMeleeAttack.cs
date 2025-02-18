using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackDamage = 20f;
    public float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    private Transform player;
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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
            if (Time.time >= nextAttackTime)
            {
                if (agent != null)
                {
                    agent.isStopped = true;
                }

                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            if (agent != null)
            {
                agent.isStopped = false;
            }
        }
    }

    void Attack()
    {
        Debug.Log("Enemy is attempting to attack!"); // Check if attack is triggered
        animator.SetTrigger("Attack"); // Triggers attack animation
    }


    // This function will be called by the animation event
    public void ApplyMeleeDamage()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Melee Enemy hit the player at the correct animation frame!");
            }
        }
    }
}

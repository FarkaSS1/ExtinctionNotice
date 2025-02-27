using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IAttacker
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float enemyDamage = 10f;
    [SerializeField] private float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    private Animator animator;
    private UnityEngine.AI.NavMeshAgent agent;

    public float AttackRange => attackRange;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void TryAttack(Transform target)
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= attackRange && Time.time >= nextAttackTime)
        {
            if (agent != null) agent.isStopped = true; // Stop moving when attacking
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void ApplyMeleeDamage(Transform target)
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget > attackRange) return;

        if (target.TryGetComponent<IHealth>(out var targetHealth))
        {
            targetHealth.TakeDamage(enemyDamage, transform);
            Debug.Log("Enemy dealt " + enemyDamage + " damage to " + target.name);
        }
    }

    public float GetDamage()
    {
        return enemyDamage;
    }
}

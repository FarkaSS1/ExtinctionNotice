using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IAttacker
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float enemyDamage = 10f;
    [SerializeField] private float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    private Animator animator;
    private UnityEngine.AI.NavMeshAgent agent;
    private AudioSource audioSource;
    private Transform player;

    [Header("Attack Sounds")]
    [SerializeField] private AudioClip[] attackSounds;
    [SerializeField] private float soundRange = 15f;

    public float AttackRange => attackRange;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
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

        if (attackSounds.Length > 0 && audioSource != null && player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= soundRange)
            {
                AudioClip randomClip = attackSounds[Random.Range(0, attackSounds.Length)];
                audioSource.PlayOneShot(randomClip);
            }
        }
        else
        {
            Debug.LogError("Missing AudioSource or AttackSounds not assigned!");
        }
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

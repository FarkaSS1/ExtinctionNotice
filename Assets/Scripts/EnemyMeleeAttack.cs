using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackDamage = 20f;
    public float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

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

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack"); // Play attack animation

        if (player.GetComponent<HealthSystem>())
        {
            player.GetComponent<HealthSystem>().TakeDamage(attackDamage);
        }

        Debug.Log("Chomper attacked the player!");
    }
}

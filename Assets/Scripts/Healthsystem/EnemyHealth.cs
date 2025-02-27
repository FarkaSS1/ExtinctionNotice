using UnityEngine;

public class EnemyHealth : Health
{
    private EnemyAI enemyAI;

    protected override void Awake()
    {
        base.Awake();
        enemyAI = GetComponent<EnemyAI>(); // Get AI script
    }

    public override void TakeDamage(float damage, Transform attacker)
    {
        base.TakeDamage(damage, attacker); // Call base damage function

        if (enemyAI != null) // Only aggro if not already aggroed
        {
            enemyAI.AggroEnemy(attacker);
        }
    }

    protected override void Die()
    {
        base.Die();
    }
}

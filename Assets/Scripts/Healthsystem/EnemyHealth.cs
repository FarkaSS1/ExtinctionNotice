public class EnemyHealth : Health
{
    private EnemyAI enemyAI;

    protected override void Awake()
    {
        base.Awake();
        enemyAI = GetComponent<EnemyAI>(); // Get AI script
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage); // Call base damage function

        if (enemyAI != null) // Only aggro if not already aggroed
        {
            enemyAI.AggroEnemy();
            enemyAI.ResetAggroTimer();
        }
    }

    protected override void Die()
    {
        base.Die();
    }
}

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
        // Check if enemy has a drop
        IDroppable dropInfo = GetComponent<IDroppable>();
        if (dropInfo != null)
        {
            SpawnDrop(dropInfo);
        }

        base.Die();
    }

    private void SpawnDrop(IDroppable dropInfo)
    {
        GameObject dropPrefab = dropInfo.GetDropPrefab();
        if (dropPrefab == null) return;

        GameObject drop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        DropItem dropItem = drop.AddComponent<DropItem>();
        dropItem.resourceType = dropInfo.GetResourceType();
        dropItem.amount = dropInfo.GetDropAmount();
    }

}

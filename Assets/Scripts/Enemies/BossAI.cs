using UnityEngine;
using UnityEngine.AI;

public class BossAI : EnemyAI
{
    private Vector3 spawnPoint; // Store the spawn position
    public float patrolRadius = 7f;
    private Vector3 patrolTarget;

    protected override void Start()
    {
        base.Start();

        spawnPoint = transform.position; // Save the position it spawned at
        SetNewPatrolTarget();
    }

    protected override void Update()
    {
        base.Update(); // Keep EnemyAI movement & attack logic

        if (!IsAggroed) // Only patrol when not in combat
        {
            PatrolAroundSpawn(); // Patrol around spawn position
        }
    }

    private void PatrolAroundSpawn()
    {
        if (Vector3.Distance(transform.position, patrolTarget) < 1f)
        {
            SetNewPatrolTarget();
        }
        agent.SetDestination(patrolTarget);
    }

    private void SetNewPatrolTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += spawnPoint; // Patrol around spawn location
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = navHit.position;
        }
    }
}

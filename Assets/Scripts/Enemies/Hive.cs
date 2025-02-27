using UnityEngine;

public class Hive : MonoBehaviour
{
    public GameObject bossPrefab;
    public float detectionRange = 10f; // How close the player needs to be
    private bool hasSpawnedBoss = false; // Ensures it only spawns once

    private Transform player;

    [SerializeField] private Transform bossSpawnPoint; // Assign in Inspector

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (!hasSpawnedBoss && player != null && Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        if (bossSpawnPoint == null)
        {
            Debug.LogError("Boss spawn point is not set! Assign a Transform in the Inspector.");
            return;
        }

        Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);

        hasSpawnedBoss = true; // Ensure it only spawns once
        Debug.Log("Boss spawned at: " + bossSpawnPoint.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Show designated spawn point
        if (bossSpawnPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(bossSpawnPoint.position, 1f);
        }
    }
}

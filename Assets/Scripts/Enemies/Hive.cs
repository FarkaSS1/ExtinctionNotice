using UnityEngine;
using UnityEngine.Events;

public class Hive : MonoBehaviour
{
    public GameObject bossPrefab;
    public float detectionRange = 10f; // How close the player needs to be
    private bool hasSpawnedBoss = false; // Ensures it only spawns once

    private Transform player;

    [SerializeField] private Transform bossSpawnPoint; // Assign in Inspector

    // Events 
    public UnityEvent OnBossSpawned;

    private void Start()
    {
        UpdatePlayerReference();
    }

    private void Update()
    {
        if (!hasSpawnedBoss && player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= detectionRange)
            {
                Debug.Log("Player is within detection range. Spawning boss!");
                SpawnBoss();
            }
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
         OnBossSpawned?.Invoke();
        Debug.Log("Boss spawned at: " + bossSpawnPoint.position);
    }

    private void UpdatePlayerReference()
    {
        GameObject omegaObject = GameObject.Find("O.M.E.G.A");
        if (omegaObject != null)
        {
            Transform playerTransform = omegaObject.transform.Find("Player");
            if (playerTransform != null)
            {
                player = playerTransform;
                Debug.Log("Player found: " + player.name);
            }
            else
            {
                Debug.LogError("Player object not found inside O.M.E.G.A!");
            }
        }
        else
        {
            Debug.LogError("O.M.E.G.A object not found!");
        }
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

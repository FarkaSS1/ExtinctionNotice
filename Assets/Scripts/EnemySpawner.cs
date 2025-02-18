using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your enemy prefab in Inspector
    public Transform[] spawnPoints; // Assign spawn points in Inspector
    public float spawnInterval = 5f; // Time between spawns

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 2f, spawnInterval); // Starts enemy spawning
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null) return;

        // Pick a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Spawn the enemy
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Spawned enemy at: " + spawnPoint.position);
    }
}

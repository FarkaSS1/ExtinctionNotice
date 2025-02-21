using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your enemy prefab in Inspector
    public Transform[] spawnPoints; // Assign spawn points in Inspector
    public float spawnInterval = 15f; // Time between waves
    public int enemiesPerWave = 3; // Number of enemies per wave

    private void Start()
    {
        InvokeRepeating("SpawnEnemyWave", 2f, spawnInterval); // Start enemy wave spawning
    }

    private void SpawnEnemyWave()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null) return;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("Spawned enemy at: " + spawnPoint.position);
    }
}

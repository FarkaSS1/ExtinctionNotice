using UnityEngine;
using System.Collections;
using System;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your enemy prefab in Inspector
    public Transform[] spawnPoints; // Assign spawn points in Inspector
    [SerializeField] private float initialSpawnInterval = 20f; // Start slow
    [SerializeField] private float minSpawnInterval = 10f; // Prevent spawning too fast
    [SerializeField] private float spawnDecayRate = 0.95f; // Reduce interval per wave
    private float currentSpawnInterval;

    public int maxEnemiesPerWave = 5; // Maximum enemies per wave
    private int currentEnemiesPerWave = 1; // Start with 1 enemy in wave
    private GameObject newEnemy;
    
    // Event
    public event Action<EnemyHealth> OnEnemySpawned;

    private void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(currentSpawnInterval); // Delay first spawn

        while (true)
        {
            // Spawn enemies according to current wave size
            for (int i = 0; i < currentEnemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f); // Small delay between spawns in a wave
            }

            // Increase enemies per wave, up to the max limit
            if (currentEnemiesPerWave < maxEnemiesPerWave)
            {
                currentEnemiesPerWave++;
            }

            // Reduce spawn interval over time
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval * spawnDecayRate);

            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null) return;

        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyHealth enemyHealth = newEnemy.GetComponent<EnemyHealth>();
        OnEnemySpawned?.Invoke(enemyHealth);
        Debug.Log("Spawned enemy at: " + spawnPoint.position);
    }
}

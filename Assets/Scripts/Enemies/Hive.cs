using UnityEngine;

public class Hive : MonoBehaviour
{
    public GameObject bossPrefab;
    public float detectionRange = 10f; // How close the player needs to be
    private bool hasSpawnedBoss = false; // Ensures it only spawns once

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!hasSpawnedBoss && Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        Instantiate(bossPrefab, transform.position, transform.rotation); // Spawn at Hive's position
        hasSpawnedBoss = true; // Ensure it only spawns once
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection range in Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

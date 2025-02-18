# Prefabs: Enemy & Spawner

## 📌 Enemy Prefab (`Spitter.prefab`)
- **Automatically spawns via Enemy Spawner**
- **Contains:**
  - NavMeshAgent for AI movement
  - Animator for animations
- **Usage:**
  - Drag the prefab into the scene if testing manually.
  - Assigned to the EnemySpawner script for automatic spawning.

## 📌 Enemy Spawner Prefab (`EnemySpawner.prefab`)
- **Automatically spawns enemies at designated spawn points**
- **Adjustable Parameters in Inspector:**
  - `Enemy Prefab` → Assign different enemy types.
  - `Spawn Points` → Define where enemies appear.
  - `Spawn Interval` → Time between enemy spawns.
- **Usage:**
  - Drag & drop into the scene.
  - Place GameObjects named SpawnPoint1 2 3 and so on where the enemies should spawn
  - Assign Spawn Points in Enemy Spawner
  - Assign different enemy prefabs for variation.
  - Place multiple spawners to control enemy waves.

## 🛠 Setup Instructions
1. Ensure the **NavMeshSurface** is baked in the scene.
2. Place spawners in strategic locations.
3. Assign the correct **Enemy Prefab** in the spawner's Inspector.
4. Adjust spawn rates for game balance.

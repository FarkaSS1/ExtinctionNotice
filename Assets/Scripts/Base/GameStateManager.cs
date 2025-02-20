using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    private int gold = 1000;
    private int power = 500;
    private int elementX = 2000;
    private List<GameObject> builtStructures = new List<GameObject>();

    // Time Variables
    private float elapsedTime = 0f;
    public int gameDays = 1;
    public int gameHours = 0;

    public float timeMultiplier = 10f; // 1 real second = 10 in-game minutes

    public event Action<int, int> OnTimeUpdated; // UI Hook

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        UpdateGameTime();
    }

    private void UpdateGameTime()
    {
        elapsedTime += Time.deltaTime * timeMultiplier;

        // Convert elapsed time into hours
        int newGameHours = (int)(elapsedTime / 60f) % 24;
        int newGameDays = (int)(elapsedTime / 1440f) + 1;

        // Only update if time has changed
        if (newGameHours != gameHours || newGameDays != gameDays)
        {
            gameHours = newGameHours;
            gameDays = newGameDays;

            OnTimeUpdated?.Invoke(gameDays, gameHours); // Notify UI
        }
    }

    public string GetGameTimeString()
    {
        return $"Day {gameDays}, {gameHours}:00";
    }

public void AddResource(string type, int amount)
    {
        if (type == "gold") gold += amount;
        else if (type == "power") power += amount;
        else if (type == "elementX") elementX += amount;
        else throw new ArgumentException("Invalid resource type", nameof(type));
    }

    public void RemoveResources(string type, int amount)
    {
        if (type == "gold") gold -= amount;
        else if (type == "power") power -= amount;
        else if (type == "elementX") elementX -= amount;
        else throw new ArgumentException("Invalid resource type", nameof(type));
    }

    public bool CanAfford(string type, int cost)
    {
        if (type == "gold") return gold >= cost;
        if (type == "power") return power >= cost;
        if (type == "elementX") return elementX >= cost;
        throw new ArgumentException("Invalid resource type", nameof(type));
    }
    public int ReturnResources(string type)
    {
        if (type == "gold") return gold;
        if (type == "power") return power;
        if (type == "elementX") return elementX;

        throw new ArgumentException("Invalid resource type", nameof(type));
    }

    // Public Getters for UIManager
    public int Gold => gold;
    public int Power => power;
    public int ElementX => elementX;

    public void AddStructure(GameObject structure)
    {
        builtStructures.Add(structure);
    }

    public void RemoveStructure(GameObject structure)
    {
        builtStructures.Remove(structure);
    }

    public void returnStructures()
    {
        foreach (GameObject structure in builtStructures)
        {
            Debug.Log(structure.name);
        }
    }
}

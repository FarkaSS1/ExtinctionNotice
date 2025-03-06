using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    private int gold = 1000;
    private int power = 500;
    private int elementX = 1200;
    private List<GameObject> builtStructures = new List<GameObject>();

    private int towerCount = 0;
    private int mineCount = 0;
    private int maxTowers = 10;
    private int maxMines = 3;


    // Time Variables
    private float elapsedTime = 0f;
    public int gameDays = 1;
    public int gameHours = 0;

    public float timeMultiplier = 10f; // 1 real second = 10 in-game minutes

    public event Action<int, int> OnTimeUpdated; // UI Hook
    public event Action<int> OnElementXUpdated; // Event ElementX change

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
            else if (type == "elementX") {elementX += amount; OnElementXUpdated?.Invoke(elementX);}
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

    // dont use this one
    public void AddStructure(GameObject structure)
    {
        if (structure.CompareTag("Tower"))
        {
            if (towerCount >= maxTowers)
            {
                Debug.Log("Max tower limit reached!");
                return;
            }
            towerCount++;
        }
        else if (structure.CompareTag("Mine"))
        {
            if (mineCount >= maxMines)
            {
                Debug.Log("Max mine limit reached!");
                return;
            }
            mineCount++;
        }
    }

    public void RemoveStructure(GameObject structure)
    {
        if (structure.CompareTag("Tower")) towerCount--;
        else if (structure.CompareTag("Base")) mineCount--;
    }

    public void BuildStructure(GameObject structure)
    {
        Debug.Log("Building structure");
        Debug.Log(structure.name);
        if (structure.CompareTag("Tower")) towerCount++;
        else if (structure.CompareTag("Base")) mineCount++;
    }


    public void returnStructures()
    {
        foreach (GameObject structure in builtStructures)
        {
            Debug.Log(structure.name);
        }
    }
    public bool MaximumCapacityReached(GameObject building)
    {
        Debug.Log("Checking max capacity");
        Debug.Log("Tower count: " + towerCount);
        Debug.Log("Mine count: " + mineCount);
        if (building.CompareTag("Tower")) return towerCount >= maxTowers;
        if (building.CompareTag("Base")) return mineCount >= maxMines;
        return false;
    }

}

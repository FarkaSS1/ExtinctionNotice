using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance; 

    private int gold = 1000;
    private int power = 500;
    private int elementX = 2000;
   // public List<GameObject> activeUnits = new List<GameObject>();
    private List<GameObject> builtStructures = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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

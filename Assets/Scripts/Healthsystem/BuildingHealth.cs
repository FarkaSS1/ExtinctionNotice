using UnityEngine;
using System;

public class BuildingHealth : Health
{
    public event Action OnDestroyed; // Notify when destroyed

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} has been destroyed!");
        OnDestroyed?.Invoke();
        base.Die(); // Calls the base Die() method to handle destruction
    }
}
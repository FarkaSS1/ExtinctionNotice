using System;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] private static float maxHealth = 100f;
    protected float currentHealth;

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        TakeDamage(damage, null); // Default attacker is null
    }

    public virtual void TakeDamage(float damage, Transform attacker)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth / maxHealth);
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
            currentHealth = 0;
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth / maxHealth);
    }

    protected virtual void Die()
    {
        GameStateManager.Instance.RemoveStructure(gameObject);
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    public static void UpgradeMaxHealth()
    {
        maxHealth += 100;
    }
    public static float GetBaseHealth() => maxHealth;

}

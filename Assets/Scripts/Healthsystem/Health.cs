using System;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    public event Action<float> OnHealthChanged; 
    public event Action OnDeath; 

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth / maxHealth);
        float healthPercentage = currentHealth / maxHealth;
         Debug.Log($"Health changed! Current: {currentHealth}, Percentage: {healthPercentage}");
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
        OnDeath?.Invoke();
        Destroy(gameObject); // Change this in Player or Enemy-specific logic
    }
}

using UnityEngine;

public class TowerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 1000f;
    private float currentHealth;

    public event System.Action<float> OnHealthChanged; // Update UI
    public event System.Action OnDestroyed; // Notify when destroyed

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth / maxHealth); // Send UI update

        Debug.Log(gameObject.name + " took " + damage + " damage. Remaining health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void TakeDamage(float damage, Transform attacker)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth / maxHealth);

        Debug.Log(gameObject.name + " took " + damage + " damage from " + (attacker != null ? attacker.name : "Unknown"));

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth / maxHealth);
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has been destroyed!");
        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }
}

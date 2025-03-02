using UnityEngine;

public interface IHealth
{
    void TakeDamage(float damage);
    void TakeDamage(float damage, Transform attacker); 
    void Heal(float amount);
}

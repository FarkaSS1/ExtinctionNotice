using UnityEngine;

public class HealingZone : MonoBehaviour
{
    public int healAmount = 5;
    public float healInterval = 1.0f; // Heal every second
    private bool playerInside = false;
    private PlayerHealth playerHealth;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                InvokeRepeating(nameof(HealPlayer), 0f, healInterval);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            CancelInvoke(nameof(HealPlayer));
        }
    }

    private void HealPlayer()
    {
        if (playerInside && playerHealth != null)
        {
            playerHealth.Heal(healAmount);
            Debug.Log("Player healed: " + healAmount);
        }
    }
}
